<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label1 = New Label()
        Button1 = New Button()
        TextBox1 = New TextBox()
        Button2 = New Button()
        Label2 = New Label()
        TextBox2 = New TextBox()
        TextBox3 = New TextBox()
        Label3 = New Label()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(126, 33)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(190, 25)
        Label1.TabIndex = 0
        Label1.Text = "管理者(サーバー用)ページ"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(229, 75)
        Button1.Margin = New Padding(4)
        Button1.Name = "Button1"
        Button1.Size = New Size(78, 76)
        Button1.TabIndex = 1
        Button1.Text = "サーバー起動"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.ImeMode = ImeMode.NoControl
        TextBox1.Location = New Point(27, 190)
        TextBox1.Margin = New Padding(4)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(372, 249)
        TextBox1.TabIndex = 2
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(316, 75)
        Button2.Margin = New Padding(4)
        Button2.Name = "Button2"
        Button2.Size = New Size(84, 79)
        Button2.TabIndex = 3
        Button2.Text = "切断"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(27, 69)
        Label2.Margin = New Padding(4, 0, 4, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(81, 25)
        Label2.TabIndex = 4
        Label2.Text = "サーバーIP"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(27, 98)
        TextBox2.Margin = New Padding(4)
        TextBox2.Name = "TextBox2"
        TextBox2.ReadOnly = True
        TextBox2.Size = New Size(193, 31)
        TextBox2.TabIndex = 5
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(243, 465)
        TextBox3.Margin = New Padding(4)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(155, 31)
        TextBox3.TabIndex = 6
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(119, 469)
        Label3.Margin = New Padding(4, 0, 4, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(113, 25)
        Label3.TabIndex = 7
        Label3.Text = "次のゲームまで"
        ' 
        ' Form3
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(422, 570)
        Controls.Add(Label3)
        Controls.Add(TextBox3)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(Button2)
        Controls.Add(TextBox1)
        Controls.Add(Button1)
        Controls.Add(Label1)
        Margin = New Padding(4)
        Name = "Form3"
        Text = "サーバーページ"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label3 As Label
End Class
