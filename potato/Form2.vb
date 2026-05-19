Imports System.Net.Sockets
Imports System.Text

Public Class Form2
    Dim client As TcpClient
    Dim myName As String = ""
    Dim IsJoined As Boolean = False
    Dim IsPlaying As Boolean = False
    Dim blockList As New List(Of String)()
    Dim canbet As Boolean = False

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
                    ' TextBox5 ではなく ComboBox2 に名前をセットする
                    ComboBox2.Invoke(Sub()

                                         ComboBox2.Items.Clear()
                                         For Each n In names
                                             ' 自分以外のプレイヤーだけをリストに追加（自分をブロックしないように）
                                             If n <> myName Then
                                                 ComboBox2.Items.Add(n)
                                             End If
                                         Next
                                         ' 最初の人を自動で選択状態にする（空白対策）
                                         If ComboBox2.Items.Count > 0 Then
                                             ComboBox2.SelectedIndex = 0
                                         End If
                                     End Sub)

                ElseIf m.StartsWith("SYSTEM:") Then
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText(m & vbCrLf)
                                    End Sub)
                    Continue While
                ElseIf m.StartsWith("CHIPS:") Then
                    Dim chip = m.Substring(6)
                    TextBox7.Invoke(Sub()
                                        TextBox7.Text = chip
                                    End Sub)
                    Continue While
                ElseIf m.StartsWith("BET_OK:") Then
                    Dim amount = m.Substring(7)
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText(amount & "チップをベットしました" & vbCrLf)
                                    End Sub)
                    canbet = False
                    Button10.Invoke(Sub()
                                        Button10.Enabled = False
                                    End Sub)
                    Continue While
                ElseIf m.StartsWith("BET_FAIL") Then
                    Dim reason = m.Substring(8)
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("チップが不足しています！" & vbCrLf)
                                    End Sub)
                    Continue While
                ElseIf m.StartsWith("HAND:") Then
                    Dim hand = m.Substring(5)
                    Dim cards = hand.Split(","c)
                    PictureBox15.Image = Image.FromFile(cards(0) & ".bmp")
                    PictureBox14.Image = Image.FromFile(cards(1) & ".bmp")
                    Continue While
                ElseIf m = "BET_START" Then
                    TextBox1.Invoke(Sub()
                                        TextBox1.AppendText("ベットフェーズが始まりました！" & vbCrLf)
                                    End Sub)
                    canbet = True
                    Button10.Invoke(Sub()
                                        Button10.Enabled = True
                                    End Sub)
                    Continue While
                Else
                    ' ★最後のElse（その他の一般チャット：名前:メッセージ の処理）
                    If m.Contains(":") Then
                        Dim speaker As String = m.Split(":"c)(0) ' 送信者の名前を抜き出す

                        ' もしブロックリストに入っている名前なら、表示せずにスルーして次のループへ
                        If blockList.Contains(speaker) Then
                            Continue For
                        End If
                    End If

                    ' ブロックされていなければ普通に表示
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

End Class