Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form3
    Dim listener As TcpListener
    Dim clients As New List(Of TcpClient)
    Dim playerNames As New List(Of String)
    Dim players As New Dictionary(Of TcpClient, Player)
    Dim count As Integer = 30
    Dim timer As New Timer()
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
            If msg.StartsWith("JOIN:") Then
                Dim name = msg.Substring(5)

                If playerNames.Contains(name) Then
                    Dim res = Encoding.UTF8.GetBytes("NAME_TAKEN")
                    Await client.GetStream().WriteAsync(res, 0, res.Length)
                Else
                    playerNames.Add(name)
                    players(client) = New Player With {.Name = name}

                    Dim res = Encoding.UTF8.GetBytes("NAME_OK:" & name & vbCrLf)
                    Await client.GetStream().WriteAsync(res, 0, res.Length)

                    Dim joinMsg As String = "SYSTEM:" & name & " が参加しました" & vbCrLf
                    Dim joindata = Encoding.UTF8.GetBytes(joinMsg)
                    WriteMessage("プレイヤー数: " & players.Count)

                    WriteMessage(joinMsg)
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

                End If
                Continue While
            End If

            ' 表示
            TextBox1.Invoke(Sub()
                                TextBox1.AppendText(msg & vbCrLf)
                            End Sub)

            ' 全員に送信（ブロードキャスト）
            Dim data = Encoding.UTF8.GetBytes(msg)

            For Each c In clients
                Try
                    Dim s = c.GetStream()
                    Await s.WriteAsync(data, 0, data.Length)
                Catch ex As Exception
                    clients.Remove(c) ' 送信に失敗したクライアントをリストから削除
                End Try
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

    Private Sub TimerTick(sender As Object, e As EventArgs)
        If count > 0 Then
            count -= 1
            TextBox3.Text = count
        Else
            timer.Stop()
            CheckStart()
            count = 30
            timer.Start()
        End If
    End Sub

    Private Sub CheckStart()
        If players.Values.Any(Function(p) p.IsPlaying) Then
            WriteMessage("ゲーム開始")
            'StartGame()
        Else
            WriteMessage("参加者なし, ゲームを開始できません")
        End If
    End Sub
End Class

Class Player
    Public Name As String
    Public Hand As New List(Of Integer)
    Public IsStand As Boolean = False
    Public IsPlaying As Boolean = False
End Class