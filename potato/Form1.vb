Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim moveScreen As New Form2
        moveScreen.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim moveScreen As New Form3
        moveScreen.Show()
    End Sub
End Class
Public Class Network
    Public Const port As Integer = 20000 ' ポート番号
    Public Shared enc As System.Text.Encoding = System.Text.Encoding.Default ' 文字コードに「Shift-JIS」を指定
    Public Shared sHandle As Long = -1 ' サーバハンドル
    Public Shared cHandle As Long = -1 ' 自分のクライアントハンドル
    Public Shared clients As New Dictionary(Of Long, String)
End Class
