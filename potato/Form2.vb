Imports System.Net.Sockets
Imports System.Text

Public Class Form2
    Dim client As TcpClient
    Dim myName As String = ""
    Dim IsJoined As Boolean = False
    Dim IsPlaying As Boolean = False
    Dim blockList As New List(Of String)()
    Dim canbet As Boolean = False
    Dim recvBuffer As String = ""

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' ComboBox2 で選ばれている名前を取得
        Dim targetName As String = ComboBox2.Text

        If targetName = "" Then
            MessageBox.Show("ブロックするプレイヤーを選択してください")
            Return
        End If

        ' すでにブロックリストに入っていないか確認して追加
        If Not blockList.Contains(targetName) Then
            blockList.Add(targetName)
            TextBox1.AppendText("【システム】" & targetName & " をブロックしました" & vbCrLf)
        Else
            TextBox1.AppendText("【システム】" & targetName & " はすでにブロックしています" & vbCrLf)
        End If
    End Sub

    Private Async Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            client = New TcpClient()
            Dim s_ip = TextBox4.Text
            Await client.ConnectAsync(s_ip, Network.port)
            TextBox1.AppendText("接続に成功しました" & vbCrLf)
            Receive()
        Catch ex As Exception
            TextBox1.AppendText("接続に失敗しました: " & ex.Message & vbCrLf)
        End Try
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not IsJoined Then
            TextBox1.AppendText("ニックネームを登録してください" & vbCrLf)
            Return
        End If
        Try
            Dim msg As String = myName & ":" & TextBox2.Text & vbCrLf
            Dim data As Byte() = Encoding.UTF8.GetBytes(msg)
            Dim stream = client.GetStream()
            Await stream.WriteAsync(data, 0, data.Length)
        Catch ex As Exception
            TextBox1.AppendText("送信に失敗しました: " & ex.Message & vbCrLf)
        End Try
    End Sub

    Private Async Sub Receive()
        Dim stream = client.GetStream()
        Dim buffer(1024) As Byte

        While True
            Dim len = Await stream.ReadAsync(buffer, 0, buffer.Length)
            If len = 0 Then Exit While

            Dim msg = Encoding.UTF8.GetString(buffer, 0, len)
            Dim msgs = msg.Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)

            For Each m In msgs
                If m.StartsWith("NAME_OK:") Then
                    Dim name = m.Substring(8)
                    myName = name
                    IsJoined = True
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("参加完了: " & name & vbCrLf)
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("PLAYERS:") Then
                    Dim list = m.Substring(8)
                    Dim names = list.Split(","c)
                    ComboBox2.Invoke(Sub()
                                         ComboBox2.Items.Clear()
                                         For Each n In names
                                             If n <> myName Then
                                                 ComboBox2.Items.Add(n)
                                             End If
                                         Next
                                         If ComboBox2.Items.Count > 0 Then
                                             ComboBox2.SelectedIndex = 0
                                         End If
                                     End Sub)
                    Continue For
                ElseIf m.StartsWith("SYSTEM:") Then
                    ' ★システムメッセージ（つまみ食いなど）をブロック判定して表示
                    Dim showMessage As Boolean = True
                    For Each blockedName In blockList
                        If m.Contains(blockedName) Then
                            showMessage = False
                            Exit For
                        End If
                    Next

                    If showMessage Then
                        TextBox1.Invoke(Sub()
                                            TextBox1.AppendText(m & vbCrLf)
                                        End Sub)
                    End If
                    Continue For
                ElseIf m.StartsWith("TIME:") Then
                    ' ★サーバーから届いた "TIME:29" などの文字列から、数字部分だけを抜き出す
                    Dim t = m.Substring(5)
                    TextBox5.Invoke(Sub()
                                        TextBox5.Text = t ' TextBox5に秒数（数字のみ）を上書き表示
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("CHIPS:") Then
                    Dim chip = m.Substring(6)
                    TextBox7.Invoke(Sub()
                                        TextBox7.Text = chip
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("BET_OK:") Then
                    Dim amountStr = m.Substring(7)
                    Dim amount As Integer = Integer.Parse(amountStr)

                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText(amount & "枚のポテチをベットしました" & vbCrLf)

                                        ' ★画像を表示する処理
                                        Dim imgName As String = GetPotatoImage(amount)
                                        If imgName <> "" Then
                                            ' 念のためファイルが存在するかチェックすると安全です
                                            If System.IO.File.Exists(imgName) Then
                                                PictureBox28.Image = Image.FromFile(imgName)
                                            End If
                                        End If
                                    End Sub)
                    canbet = False
                    Button10.Invoke(Sub()
                                        Button10.Enabled = False
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("BET_FAIL") Then
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("チップが不足しています！" & vbCrLf)
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("HAND:") Then
                    Dim hand = m.Substring(5)
                    Dim cards = hand.Split(","c)
                    PictureBox15.Image = Image.FromFile(cards(0) & ".bmp")
                    PictureBox14.Image = Image.FromFile(cards(1) & ".bmp")
                    Continue For
                ElseIf m = "BET_START" Then
                    PictureBox28.Image = Nothing
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("SYSTEM:ベットフェーズが始まりました！" & vbCrLf)
                                    End Sub)
                    canbet = True
                    Button10.Invoke(Sub()
                                        Button10.Enabled = True
                                    End Sub)
                    Continue For
                ElseIf m.StartsWith("DEALER_HAND:") Then
                    Dim hand = m.Substring(12)
                    Dim cards = hand.Split(","c)
                    PictureBox19.Image = Image.FromFile(cards(0) & ".bmp")
                    PictureBox18.Image = Image.FromFile(cards(1) & ".bmp")
                    Continue For
                ElseIf m.StartsWith("TURN:") Then
                    Dim turnPlayer = m.Substring(5)

                    Me.Invoke(Sub()
                                  If turnPlayer = myName Then
                                      TextBox1.AppendText("SYSTEM:あなたのターンです！" & vbCrLf)
                                      Button4.Enabled = True
                                      Button5.Enabled = True
                                  Else
                                      TextBox1.AppendText("SYSTEM:" & turnPlayer & " のターンです" & vbCrLf)
                                      Button4.Enabled = False
                                      Button5.Enabled = False
                                  End If
                              End Sub)
                    Continue For
                Else
                    If m.Contains(":") Then
                        Dim speaker As String = m.Split(":"c)(0)
                        If blockList.Contains(speaker) Then
                            Continue For
                        End If
                    End If

                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText(m & vbCrLf)
                                    End Sub)
                End If
            Next
        End While
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("うすしお")
        ComboBox1.Items.Add("のりしお")
        ComboBox1.Items.Add("コンソメ")
        ComboBox1.Items.Add("バターしょうゆ")
        ComboBox1.Items.Add("九州しょうゆ")
        ComboBox1.Items.Add("ピザ")
        ComboBox1.Items.Add("バーベキュー")
        ComboBox1.Items.Add("わさび")

    End Sub

    Private Async Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim name As String = ComboBox1.Text
        If name = "" Then
            MessageBox.Show("名前を選択してください")
            Return
        End If

        Dim msg As String = "JOIN:" & name
        Dim data As Byte() = Encoding.UTF8.GetBytes(msg)

        Await client.GetStream().WriteAsync(data, 0, data.Length)
    End Sub

    Private Async Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        IsPlaying = Not IsPlaying
        Dim mode As String
        If IsPlaying Then
            Button9.Text = "参加中"
            mode = "参加"
        Else
            Button9.Text = "観戦中"
            mode = "観戦"
        End If

        Dim msg As String = "MODE:" & mode
        Dim data As Byte() = Encoding.UTF8.GetBytes(msg)
        Await client.GetStream().WriteAsync(data, 0, data.Length)
    End Sub

    Private Async Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim betAmount As Integer
        If Not canbet Then
            Return
        End If
        If Integer.TryParse(TextBox6.Text, betAmount) Then
            Dim msg As String = "BET:" & betAmount
            Dim data As Byte() = Encoding.UTF8.GetBytes(msg)
            Await client.GetStream().WriteAsync(data, 0, data.Length)
        Else
            TextBox1.AppendText("数字を入力してください" & vbCrLf)
        End If
    End Sub

    Private Async Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ' まだゲームに参加していなければ何もしない
        If Not IsJoined Then Return

        Try
            ' サーバーに「つまみ食いするよ！」と伝えるメッセージ（末尾に改行を必ず入れる）
            Dim msg As String = "TSUMAMI:" & vbCrLf
            Dim data As Byte() = Encoding.UTF8.GetBytes(msg)

            Dim stream = client.GetStream()
            Await stream.WriteAsync(data, 0, data.Length)

        Catch ex As Exception
            TextBox1.AppendText("つまみ食いに失敗しました: " & ex.Message & vbCrLf)
        End Try
    End Sub

    ' ベット額に応じた画像名を返す関数
    Private Function GetPotatoImage(betAmount As Integer) As String
        If betAmount = 1 Then
            Return "C:\Users\yoush\source\repos\nanashi4762\potato\potato\Resources\1枚.png"
        ElseIf betAmount >= 2 AndAlso betAmount <= 9 Then
            Return "C:\Users\yoush\source\repos\nanashi4762\potato\potato\Resources\2辛.png"
        ElseIf betAmount >= 10 Then
            Return "C:\Users\yoush\source\repos\nanashi4762\potato\potato\Resources\potatochips.png"
        Else
            Return ""            ' 画像なし
        End If
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim hitMsg As String = "HIT" & vbCrLf
        Dim data As Byte() = Encoding.UTF8.GetBytes(hitMsg)
        client.GetStream().WriteAsync(data, 0, data.Length)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim standMsg As String = "STAND" & vbCrLf
        Dim data As Byte() = Encoding.UTF8.GetBytes(standMsg)
        client.GetStream().WriteAsync(data, 0, data.Length)
    End Sub
End Class