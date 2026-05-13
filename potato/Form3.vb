Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form3
    Dim listener As TcpListener
    Dim clients As New List(Of TcpClient)
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

            ' 表示
            TextBox1.Invoke(Sub()
                                TextBox1.AppendText(msg & vbCrLf)
                            End Sub)

            ' 全員に送信（ブロードキャスト）
            Dim data = Encoding.UTF8.GetBytes(msg)

            For Each c In clients
                Dim s = c.GetStream()
                Await s.WriteAsync(data, 0, data.Length)
            Next
        End While
    End Sub

    Public Sub WriteMessage(ByVal msg As String)
        TextBox1.Invoke(Sub() TextBox1.Text = TextBox1.Text & msg & vbCrLf) 'TextBox1にメッセージ表示
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim host = Dns.GetHostEntry(Dns.GetHostName())

        For Each ip In host.AddressList '鯖のIPアドレスを表示(Wifiのはず)
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                TextBox2.Text = ip.ToString()
                Exit For
            End If
        Next
    End Sub

End Class