Imports System.Net.Sockets
Imports System.Text

Public Class Form2
    Dim client As TcpClient
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Async Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            client = New TcpClient()
            Dim s_ip = TextBox4.Text
            Await client.ConnectAsync(s_ip, Network.port)
            TextBox1.AppendText("接続に成功しました" & vbCrLf)
        Catch ex As Exception
            TextBox1.AppendText("接続に失敗しました: " & ex.Message & vbCrLf)
        End Try
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim msg As String = TextBox2.Text
        Dim data As Byte() = Encoding.UTF8.GetBytes(msg)
            Dim stream = client.GetStream()
            Await stream.WriteAsync(data, 0, data.Length)
        Catch ex As Exception
            TextBox1.AppendText("送信に失敗しました: " & ex.Message & vbCrLf)
        End Try
    End Sub
End Class