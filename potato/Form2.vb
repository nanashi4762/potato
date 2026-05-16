Imports System.Net.Sockets
Imports System.Text

Public Class Form2
    Dim client As TcpClient
    Dim myName As String = ""
    Dim IsJoined As Boolean = False

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

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
            Dim msg As String = myName & ":" & TextBox2.Text
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
            Dim msgs = msg.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)

            For Each m In msgs
                If m.StartsWith("NAME_OK:") Then
                    Dim name = m.Substring(8)
                    myName = name
                    IsJoined = True
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("参加完了: " & name & vbCrLf)
                                    End Sub)
                ElseIf m.StartsWith("PLAYERS:") Then
                    Dim list = m.Substring(8)
                    Dim names = list.Split(","c)
                    TextBox5.Invoke(Sub()
                                        TextBox5.Clear()
                                        For Each n In names
                                            TextBox5.AppendText(n & vbCrLf)
                                        Next
                                    End Sub)
                ElseIf m.StartsWith("SYSTEM:") Then
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText(m & vbCrLf)
                                    End Sub)
                Else
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

End Class