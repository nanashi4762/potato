Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
'Imports System.Threading

Public Class Form3
    Dim listener As TcpListener
    Dim clients As New List(Of TcpClient)
    Dim playerNames As New List(Of String)
    Dim players As New Dictionary(Of TcpClient, Player)
    Dim count As Integer = 30
    Dim timer As New System.Windows.Forms.Timer()
    'Dim timer As New Timer()
    Dim gameState As String = "WAITING" ' ゲーム状態: WAITING, BETTING, PLAYING
    Dim deck As New List(Of String) 'カードデッキ
    Dim dealerHand As New List(Of String) 'ディーラーの手札
    Dim turnOrder As New List(Of TcpClient) 'プレイヤーのターン順
    Dim currentTurn As Integer = 0 '現在のターンインデックス

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            listener = New TcpListener(IPAddress.Any, Network.port)
            listener.Start()
            WriteMessage("サーバー起動")
            While True
                Dim client = Await listener.AcceptTcpClientAsync()
                clients.Add(client)
                WriteMessage("クライアント接続: " & client.Client.RemoteEndPoint.ToString())
                Receive(client)
            End While
        Catch ex As Exception ' 例外がスローされたとき
            ' エラーメッセージを出力
            WriteMessage("Error: " & ex.Message)
        End Try
    End Sub

    Private Async Sub Receive(client As TcpClient)
        Dim stream = client.GetStream()
        Dim buffer(1024) As Byte

        While True
            Dim len = Await stream.ReadAsync(buffer, 0, buffer.Length)
            If len = 0 Then Exit While

            Dim msg = Encoding.UTF8.GetString(buffer, 0, len)
            Dim msgs = msg.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)

            For Each m In msgs
                If m.StartsWith("JOIN:") Then
                    Dim name = m.Substring(5)

                    If playerNames.Contains(name) Then
                        Dim res = Encoding.UTF8.GetBytes("NAME_TAKEN")
                        Await client.GetStream().WriteAsync(res, 0, res.Length)
                    Else
                        playerNames.Add(name)
                        players(client) = New Player With {.Name = name}

                        Dim res = Encoding.UTF8.GetBytes("NAME_OK:" & name)
                        Await client.GetStream().WriteAsync(res, 0, res.Length)

                        Dim joinMsg As String = "SYSTEM:" & name & " が参加しました" & vbCrLf
                        Dim joindata = Encoding.UTF8.GetBytes(joinMsg)
                        WriteMessage("プレイヤー数: " & players.Count)
                        WriteMessage(joinMsg)

                        Dim chipMsg As String = "CHIPS:" & players(client).Chips & vbCrLf
                        Dim chipData = Encoding.UTF8.GetBytes(chipMsg)
                        Await client.GetStream().WriteAsync(chipData, 0, chipData.Length)

                        For Each c In clients.ToList()
                            Try
                                Dim s = c.GetStream()
                                Await s.WriteAsync(joindata, 0, joindata.Length)
                            Catch
                                clients.Remove(c)
                            End Try
                        Next
                        ' プレイヤー一覧を作る
                        Dim listMsg As String = "PLAYERS:" & String.Join(",", playerNames) & vbCrLf
                        Dim listData = Encoding.UTF8.GetBytes(listMsg)

                        For Each c In clients.ToList()
                            Try
                                Dim s = c.GetStream()
                                Await s.WriteAsync(listData, 0, listData.Length)
                            Catch
                                clients.Remove(c)
                            End Try
                        Next
                        Dim playerListMsg As String = "PLAYER_LIST:" & String.Join(",", playerNames) & vbCrLf
                        Dim playerListData = Encoding.UTF8.GetBytes(playerListMsg)

                        For Each c In clients.ToList()
                            Try
                                Dim s = c.GetStream()
                                Await s.WriteAsync(playerListData, 0, playerListData.Length)
                            Catch
                                clients.Remove(c)
                            End Try
                        Next

                    End If
                    Continue While
                ElseIf m.StartsWith("MODE:") Then
                    Dim mode = m.Substring(5)

                    If players.ContainsKey(client) Then
                        If mode = "参加" Then
                            players(client).IsPlaying = True
                        Else
                            players(client).IsPlaying = False
                        End If
                        WriteMessage(players(client).Name & " は " & mode & " を選択")
                    End If
                    Continue While
                ElseIf m.StartsWith("BET:") Then
                    Dim value = Integer.Parse(m.Substring(4))
                    Dim bet As Integer
                    If Integer.TryParse(value, bet) Then
                        Dim player = players(client)
                        If bet <= player.Chips AndAlso bet > 0 Then
                            player.Bet = bet
                            player.Chips -= bet
                            WriteMessage(player.Name & "が" & bet & " チップをベット")
                            Dim res = Encoding.UTF8.GetBytes("BET_OK:" & bet & vbCrLf)
                            Await client.GetStream().WriteAsync(res, 0, res.Length)
                            Dim resChip = Encoding.UTF8.GetBytes("CHIPS:" & player.Chips & vbCrLf)
                            Await client.GetStream().WriteAsync(resChip, 0, resChip.Length)
                        Else
                            Dim res = Encoding.UTF8.GetBytes("BET_FAIL" & vbCrLf)
                            Await client.GetStream().WriteAsync(res, 0, res.Length)
                        End If
                    End If
                    Continue While
                ElseIf m.StartsWith("TSUMAMI:") Then
                    Dim player = players(client)

                    ' ちゃんとゲーム中、かつチップが1枚以上あるかチェック
                    If gameState = "PLAYING" AndAlso player.IsPlaying AndAlso player.Chips >= 1 Then
                        ' チップを3枚減らす
                        player.Chips -= 3

                        ' 手札をクリアして現在の山札から2枚引き直す
                        player.Hand.Clear()
                        player.Hand.Add(DrawCard())
                        player.Hand.Add(DrawCard())

                        ' 1. 本人に減った後のチップ数を通知
                        Dim chipMsg As String = "CHIPS:" & player.Chips & vbCrLf
                        Dim chipData = Encoding.UTF8.GetBytes(chipMsg)
                        Await client.GetStream().WriteAsync(chipData, 0, chipData.Length)

                        ' 2. 本人に新しい手札を通知（例: HAND:01_02,11_00）
                        Dim handMsg As String = "HAND:" & String.Join(",", player.Hand) & vbCrLf
                        Dim handData = Encoding.UTF8.GetBytes(handMsg)
                        Await client.GetStream().WriteAsync(handData, 0, handData.Length)

                        ' 3. 全員にシステムメッセージで実況
                        WriteMessage(player.Name & " がつまみ食いして手札を引き直した！")
                        Dim sysMsg As String = "SYSTEM:" & player.Name & " がつまみ食いして手札を引き直しました（チップ-3）" & vbCrLf
                        Dim sysData = Encoding.UTF8.GetBytes(sysMsg)

                        ' 全員にブロードキャスト
                        For Each c In clients.ToList()
                            Try
                                Dim s = c.GetStream()
                                Await s.WriteAsync(sysData, 0, sysData.Length)
                            Catch
                                clients.Remove(c)
                            End Try
                        Next
                    End If
                    Continue While ' チャットとして処理されないようにループをスキップ！
                ElseIf m = "HIT" Then
                    Dim player = players(client)
                    player.Hand.Add(DrawCard())
                    Dim hitmsg As String = "HAND:" & String.Join(",", player.Hand) & vbCrLf
                    Dim hitdata = Encoding.UTF8.GetBytes(hitmsg)
                    Await client.GetStream().WriteAsync(hitdata, 0, hitdata.Length)

                    Dim score = GetScore(player.Hand)
                    If score > 21 Then
                        WriteMessage(player.Name & " はバーストしました")

                        Dim bustMsg = Encoding.UTF8.GetBytes("SYSTEM:" & player.Name & " はバーストしました！" & vbCrLf)
                        For Each c In clients
                            Await c.GetStream().WriteAsync(bustMsg, 0, bustMsg.Length)
                        Next

                        Await NextTurnOrDealer()
                    End If
                    Continue While
                ElseIf m = "STAND" Then
                    players(client).IsStand = True
                    WriteMessage(players(client).Name & " はスタンド")

                        Await NextTurnOrDealer()
                    Continue While
                End If

                WriteMessage(m)
                ' 全員に送信（ブロードキャスト）
                Dim data = Encoding.UTF8.GetBytes(m)

                For Each c In clients.ToList()
                    Try
                        Dim s = c.GetStream()
                        Await s.WriteAsync(data, 0, data.Length)
                    Catch ex As Exception
                        clients.Remove(c) ' 送信に失敗したクライアントをリストから削除
                    End Try
                Next
            Next
        End While
    End Sub

    Public Sub WriteMessage(ByVal msg As String)
        TextBox1.Invoke(Sub() TextBox1.Text = TextBox1.Text & msg & vbCrLf) 'TextBox1にメッセージ表示
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim host = Dns.GetHostEntry(Dns.GetHostName())

        timer.Interval = 1000
        AddHandler timer.Tick, AddressOf TimerTick
        timer.Start()

        For Each ip In host.AddressList '鯖のIPアドレスを表示(Wifiのはず)
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                TextBox2.Text = ip.ToString()
                Exit For
            End If
        Next
    End Sub

    Private Async Sub TimerTick(sender As Object, e As EventArgs)
        If count > 0 Then
            count -= 1
            TextBox3.Text = count

            Dim timeMsg As String = "TIME:" & count & vbCrLf
            Dim timeData = Encoding.UTF8.GetBytes(timeMsg)

            For Each c In clients.ToList()
                Try
                    Dim s = c.GetStream()
                    Await s.WriteAsync(timeData, 0, timeData.Length)
                Catch
                    clients.Remove(c)
                End Try
            Next

        Else
            If gameState = "BETTING" Then
                WriteMessage("ベット終了&ゲーム開始")
                gameState = "PLAYING"
                Await StartGame()
                timer.Stop()
                count = 30
            ElseIf gameState = "WAITING" Then
                CheckStart()
            End If
        End If
    End Sub

    Private Async Sub CheckStart()
        Dim clearMsg As String = "CLEAR_HAND" & vbCrLf
        Dim clearData = Encoding.UTF8.GetBytes(clearMsg)

        For Each c In clients.ToList()
            Await c.GetStream().WriteAsync(clearData, 0, clearData.Length)
        Next
        If players.Values.Any(Function(p) p.IsPlaying) Then
            WriteMessage("ベット開始")
            gameState = "BETTING"
            Dim msg = "BET_START" & vbCrLf
            Dim data = Encoding.UTF8.GetBytes(msg)
            For Each c In clients.ToList()
                Try
                    Dim s = c.GetStream()
                    Await s.WriteAsync(data, 0, data.Length)
                Catch ex As Exception
                    clients.Remove(c)
                End Try
            Next
            count = 15
        Else
            WriteMessage("参加者なし")
            count = 30
        End If
    End Sub

    Private Async Function StartGame() As Task
        Dim rnd As New Random()
        turnOrder.Clear()
        For Each pair In players
            If pair.Value.IsPlaying Then
                turnOrder.Add(pair.Key)
            End If
        Next
        currentTurn = 0
        InitDeck() '山札初期化

        For Each pair In players
            Dim client = pair.Key
            Dim player = pair.Value

            If player.IsPlaying Then

                player.Hand.Clear()
                player.Hand.Add(DrawCard())
                player.Hand.Add(DrawCard())

                Dim msg As String = "HAND:" & String.Join(",", player.Hand) & vbCrLf
                Dim data = Encoding.UTF8.GetBytes(msg)

                Await client.GetStream().WriteAsync(data, 0, data.Length)
            End If
        Next

        dealerHand.Clear()
        dealerHand.Add(DrawCard())
        dealerHand.Add(DrawCard())

        Dim dmsg As String = "DEALER_HAND:" & dealerHand(0) & ",BACK" & vbCrLf
        Dim ddata = Encoding.UTF8.GetBytes(dmsg)
        For Each c In clients
            Try
                Dim s = c.GetStream()
                Await s.WriteAsync(ddata, 0, ddata.Length)
            Catch ex As Exception
                clients.Remove(c)
            End Try
        Next
        WriteMessage("ゲーム開始（カード配布完了）")

        ' ★【追加】ディーラーが最初に引いた時点でブラックジャック（BJ）だった場合の特殊ルート
        If IsBlackjack(dealerHand) Then
            WriteMessage("【特殊】ディーラーがナチュラルブラックジャックです！")

            ' 全員にディーラーがBJだったことをアナウンス
            Dim bjNoticeMsg As String = "SYSTEM:ディーラーはブラックジャックです！" & vbCrLf
            Dim bjNoticeData = Encoding.UTF8.GetBytes(bjNoticeMsg)
            For Each c In clients
                Try
                    Await c.GetStream().WriteAsync(bjNoticeData, 0, bjNoticeData.Length)
                Catch
                End Try
            Next

            ' ※この時点で、各プレイヤーが最初に配られた2枚でBJかどうかも判定しておく必要があります
            ' なぜなら、SendTurnを通らないため、ここでフラグを立てておかないとResultで引き分けにできないからです
            For Each pair In players
                If pair.Value.IsPlaying AndAlso IsBlackjack(pair.Value.Hand) Then
                    pair.Value.IsBJ = True
                End If
            Next

            ' プレイヤーのターン(SendTurn)やディーラーの追加ドロー(DealerPlay)を全てスキップして、即リザルトへ！
            Await Result()
            Return
        End If

        ' ↓ ディーラーがBJじゃなければ、通常通りプレイヤーのターンへ移行
        Await SendTurn()
    End Function

    Private Sub InitDeck() '山札初期化
        deck.Clear()
        For num = 0 To 12
            For suit = 0 To 3
                deck.Add(num.ToString("00") & "_" & suit.ToString("00"))
            Next
        Next
    End Sub

    Private Function DrawCard() As String 'カードを引く
        Dim rnd As New Random()
        Dim index = rnd.Next(deck.Count)
        Dim card = deck(index)
        deck.RemoveAt(index)
        Return card
    End Function

    Private Async Function SendTurn() As Task
        If turnOrder.Count = 0 Then
            WriteMessage("ターン対象なし")
            Return
        End If

        Dim currentClient = turnOrder(currentTurn)
        Dim player = players(currentClient)
        Dim name = player.Name

        ' ブラックジャック判定
        If IsBlackjack(player.Hand) Then

            WriteMessage(name & " はブラックジャック！")

            ' 全員に通知
            Dim msgBJ As String = "SYSTEM:" & name & " はブラックジャック！" & vbCrLf
            Dim dataBJ = Encoding.UTF8.GetBytes(msgBJ)

            For Each c In clients
                Await c.GetStream().WriteAsync(dataBJ, 0, dataBJ.Length)
            Next
            player.IsBJ = True

            ' 即ターン終了
            currentTurn += 1

            If currentTurn < turnOrder.Count Then
                Await SendTurn()
            Else
                Await DealerPlay()
            End If

            Return ' ← 重要（ここで終わる）
        End If
        players(currentClient).IsStand = False

        Dim msg As String = "TURN:" & name & vbCrLf
        Dim data = Encoding.UTF8.GetBytes(msg)

        For Each c In clients.ToList()
            Try
                Dim s = c.GetStream()
                Await s.WriteAsync(data, 0, data.Length)
            Catch
                WriteMessage("TURN送信失敗: " & c.Client.RemoteEndPoint.ToString())
            End Try
        Next
    End Function

    Private Function GetScore(hand As List(Of String)) As Integer
        Dim total As Integer = 0
        Dim aceCount As Integer = 0

        For Each card In hand
            Dim num As Integer = Integer.Parse(card.Substring(0, 2))

            If num = 0 Then
                total += 11
                aceCount += 1
            ElseIf num >= 10 Then
                total += 10
            Else
                total += num + 1
            End If
        Next

        ' 21超えてたらAを1に変換
        While total > 21 AndAlso aceCount > 0
            total -= 10
            aceCount -= 1
        End While

        Return total
    End Function

    Private Async Function NextTurnOrDealer() As Task
        currentTurn += 1

        If currentTurn >= turnOrder.Count Then
            WriteMessage("全員終了 → ディーラーのターン")

            Await DealerPlay()

        Else
            Await SendTurn()
        End If
    End Function

    Private Async Function DealerPlay() As Task
        Dim msg As String = "DEALER_HAND:" & String.Join(",", dealerHand) & vbCrLf
        Dim data = Encoding.UTF8.GetBytes(msg)
        For Each c In clients
            Await c.GetStream().WriteAsync(data, 0, data.Length)
        Next
        Await Task.Delay(2000) ' ディーラーの手札を見せるために少し待つ
        ' 17以上になるまで引く
        While GetScore(dealerHand) < 17
            dealerHand.Add(DrawCard())
            Dim msg2 As String = "DEALER_HAND:" & String.Join(",", dealerHand) & vbCrLf
            Dim data2 = Encoding.UTF8.GetBytes(msg2)
            For Each c In clients
                Await c.GetStream().WriteAsync(data2, 0, data2.Length)
            Next
            Await Task.Delay(2000)
            If dealerHand.Count > 3 Then
                Exit While
            End If
            If GetScore(dealerHand) > 21 Then
                WriteMessage("ディーラーはバーストしました")
                Dim bastMsg = Encoding.UTF8.GetBytes("SYSTEM:ディーラーはバーストしました！" & vbCrLf)
                For Each c In clients
                    Await c.GetStream().WriteAsync(bastMsg, 0, bastMsg.Length)
                Next
                Exit While
            End If
        End While

        WriteMessage("ディーラー終了")
        Await Result()
    End Function

    Private Async Function Result() As Task

        Dim dealerScore = GetScore(dealerHand)
        Dim dealerIsBJ As Boolean = IsBlackjack(dealerHand) ' ★ディーラーがBJかどうかを変数に持つ

        For Each pair In players
            Dim player = pair.Value

            ' 参加していないプレイヤーはスキップ
            If Not player.IsPlaying Then Continue For

            Dim score = GetScore(player.Hand)
            Dim res As String = ""

            ' ★勝敗判定のロジックを正確にルート分け
            If dealerIsBJ Then
                ' ① ディーラーがBJのとき
                If player.IsBJ Then
                    res = "DRAW"        ' 両方BJなら引き分け（チップはそのまま戻る）
                    player.Chips += player.Bet
                Else
                    res = "LOSE"        ' それ以外はプレイヤーの負け
                End If
            Else
                ' ② ディーラーがBJではないとき（通常ルール）
                If player.IsBJ Then
                    res = "WIN"         ' プレイヤーだけがBJなら文句なしの勝ち（2.5倍）
                    player.Chips += player.Bet * 2.5
                ElseIf score > 21 OrElse (score < dealerScore AndAlso dealerScore <= 21) Then
                    res = "LOSE"        ' バーストした、またはディーラーよりスコアが低い
                ElseIf score > dealerScore OrElse dealerScore > 21 Then
                    res = "WIN"         ' ディーラーよりスコアが高い、またはディーラーがバースト
                    player.Chips += player.Bet * 2
                Else
                    res = "DRAW"        ' 通常の数字での引き分け
                    player.Chips += player.Bet
                End If
            End If

            Dim resMsg As String = "RESULT:" & res & vbCrLf & "CHIPS:" & player.Chips & vbCrLf
            Dim resData = Encoding.UTF8.GetBytes(resMsg)
            Try
                Await pair.Key.GetStream().WriteAsync(resData, 0, resData.Length)
            Catch
                clients.Remove(pair.Key)
            End Try

            ' ベットをリセット
            player.Bet = 0
            ' 次のためにBJフラグもリセット
            player.IsBJ = False
        Next

        gameState = "WAITING"
        Await Task.Delay(3000)
        timer.Start()
    End Function

    Private Function IsBlackjack(hand As List(Of String)) As Boolean
        If hand.Count = 2 AndAlso GetScore(hand) = 21 Then
            Return True
        End If
        Return False
    End Function
End Class

Class Player
    Public Name As String
    Public Hand As New List(Of String)
    Public IsStand As Boolean = False
    Public IsPlaying As Boolean = False
    Public IsBJ As Boolean = False
    Public Chips As Integer = 10
    Public Bet As Integer = 0
End Class