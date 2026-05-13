<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Label1 = New Label()
        ComboBox1 = New ComboBox()
        TextBox1 = New TextBox()
        Button1 = New Button()
        Button2 = New Button()
        Label2 = New Label()
        TextBox2 = New TextBox()
        Label3 = New Label()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        Button7 = New Button()
        TextBox3 = New TextBox()
        PictureBox1 = New PictureBox()
        PictureBox2 = New PictureBox()
        PictureBox3 = New PictureBox()
        Label4 = New Label()
        TextBox4 = New TextBox()
        Button8 = New Button()
        PictureBox4 = New PictureBox()
        TextBox5 = New TextBox()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox4, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(37, 128)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(92, 25)
        Label1.TabIndex = 0
        Label1.Text = "ニックネーム"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(160, 125)
        ComboBox1.Margin = New Padding(4, 3, 4, 3)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(188, 33)
        ComboBox1.TabIndex = 1
        ' 
        ' TextBox1
        ' 
        TextBox1.Cursor = Cursors.IBeam
        TextBox1.Location = New Point(50, 410)
        TextBox1.Margin = New Padding(4, 3, 4, 3)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.ScrollBars = ScrollBars.Vertical
        TextBox1.Size = New Size(445, 351)
        TextBox1.TabIndex = 2
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(380, 127)
        Button1.Margin = New Padding(4, 3, 4, 3)
        Button1.Name = "Button1"
        Button1.Size = New Size(117, 37)
        Button1.TabIndex = 3
        Button1.Text = "ユーザー設定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(380, 203)
        Button2.Margin = New Padding(4, 3, 4, 3)
        Button2.Name = "Button2"
        Button2.Size = New Size(117, 37)
        Button2.TabIndex = 4
        Button2.Text = "ブロック設定"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(37, 209)
        Label2.Margin = New Padding(4, 0, 4, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(114, 25)
        Label2.TabIndex = 5
        Label2.Text = "プレイヤー一覧"
        ' 
        ' TextBox2
        ' 
        TextBox2.Cursor = Cursors.IBeam
        TextBox2.Location = New Point(50, 807)
        TextBox2.Margin = New Padding(4, 3, 4, 3)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.ScrollBars = ScrollBars.Vertical
        TextBox2.Size = New Size(351, 71)
        TextBox2.TabIndex = 6
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(50, 382)
        Label3.Margin = New Padding(4, 0, 4, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(62, 25)
        Label3.TabIndex = 7
        Label3.Text = "チャット"
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(426, 807)
        Button3.Margin = New Padding(4, 3, 4, 3)
        Button3.Name = "Button3"
        Button3.Size = New Size(87, 72)
        Button3.TabIndex = 8
        Button3.Text = "送信"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(576, 710)
        Button4.Margin = New Padding(4, 3, 4, 3)
        Button4.Name = "Button4"
        Button4.Size = New Size(150, 67)
        Button4.TabIndex = 11
        Button4.Text = "Hit"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(734, 710)
        Button5.Margin = New Padding(4, 3, 4, 3)
        Button5.Name = "Button5"
        Button5.Size = New Size(153, 67)
        Button5.TabIndex = 12
        Button5.Text = "Stand"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(1179, 709)
        Button6.Margin = New Padding(4, 3, 4, 3)
        Button6.Name = "Button6"
        Button6.Size = New Size(132, 67)
        Button6.TabIndex = 13
        Button6.Text = "Bet"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(1370, 754)
        Button7.Margin = New Padding(4, 3, 4, 3)
        Button7.Name = "Button7"
        Button7.Size = New Size(69, 50)
        Button7.TabIndex = 14
        Button7.Text = "いーと"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(1016, 727)
        TextBox3.Margin = New Padding(4, 3, 4, 3)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(155, 31)
        TextBox3.TabIndex = 15
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(571, 53)
        PictureBox1.Margin = New Padding(4, 3, 4, 3)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(914, 513)
        PictureBox1.TabIndex = 16
        PictureBox1.TabStop = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), Image)
        PictureBox2.Location = New Point(571, 573)
        PictureBox2.Margin = New Padding(4, 3, 4, 3)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(914, 128)
        PictureBox2.TabIndex = 17
        PictureBox2.TabStop = False
        ' 
        ' PictureBox3
        ' 
        PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), Image)
        PictureBox3.Location = New Point(707, 810)
        PictureBox3.Margin = New Padding(4, 3, 4, 3)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(669, 83)
        PictureBox3.TabIndex = 18
        PictureBox3.TabStop = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(37, 54)
        Label4.Margin = New Padding(4, 0, 4, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(96, 25)
        Label4.TabIndex = 19
        Label4.Text = "サーバーのIP"
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(160, 48)
        TextBox4.Margin = New Padding(4, 5, 4, 5)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(188, 31)
        TextBox4.TabIndex = 20
        ' 
        ' Button8
        ' 
        Button8.Location = New Point(380, 48)
        Button8.Margin = New Padding(4, 5, 4, 5)
        Button8.Name = "Button8"
        Button8.Size = New Size(117, 38)
        Button8.TabIndex = 21
        Button8.Text = "接続"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' PictureBox4
        ' 
        PictureBox4.Image = My.Resources.Resources.potatochips_girl
        PictureBox4.Location = New Point(1446, 707)
        PictureBox4.Name = "PictureBox4"
        PictureBox4.Size = New Size(88, 103)
        PictureBox4.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox4.TabIndex = 22
        PictureBox4.TabStop = False
        ' 
        ' TextBox5
        ' 
        TextBox5.BackColor = SystemColors.ButtonHighlight
        TextBox5.Location = New Point(160, 209)
        TextBox5.Multiline = True
        TextBox5.Name = "TextBox5"
        TextBox5.ReadOnly = True
        TextBox5.Size = New Size(188, 140)
        TextBox5.TabIndex = 23
        ' 
        ' Form2
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1537, 905)
        Controls.Add(TextBox5)
        Controls.Add(PictureBox4)
        Controls.Add(Button8)
        Controls.Add(TextBox4)
        Controls.Add(Label4)
        Controls.Add(PictureBox3)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox1)
        Controls.Add(TextBox3)
        Controls.Add(Button7)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Label3)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        Controls.Add(ComboBox1)
        Controls.Add(Label1)
        Margin = New Padding(4, 3, 4, 3)
        Name = "Form2"
        Text = "Form2"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox4, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button8 As Button
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents TextBox5 As TextBox
End Class
