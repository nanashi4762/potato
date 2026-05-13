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
            Receive()
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

            TextBox1.Invoke(Sub()
                                TextBox1.AppendText("受信: " & msg & vbCrLf)
                            End Sub)
        End While
    End Sub

    Private Sub PictureBox14_Click(sender As Object, e As EventArgs) Handles PictureBox14.Click
        ' ここにコードがなかったからエラーになっていた
    End Sub
End Class