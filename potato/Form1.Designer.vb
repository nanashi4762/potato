<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Button1 = New Button()
        Label1 = New Label()
        TextBox1 = New TextBox()
        Label2 = New Label()
        Button2 = New Button()
        ErrorProvider1 = New ErrorProvider(components)
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(286, 257)
        Button1.Name = "Button1"
        Button1.Size = New Size(226, 102)
        Button1.TabIndex = 0
        Button1.Text = "プレイする！"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(286, 103)
        Label1.Name = "Label1"
        Label1.Size = New Size(204, 20)
        Label1.TabIndex = 1
        Label1.Text = "かんたんブラックジャックfeat.ぽてち"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(339, 182)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(173, 27)
        TextBox1.TabIndex = 2
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(278, 185)
        Label2.Name = "Label2"
        Label2.Size = New Size(55, 20)
        Label2.TabIndex = 3
        Label2.Text = "さばのIP"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(607, 255)
        Button2.Name = "Button2"
        Button2.Size = New Size(130, 107)
        Button2.TabIndex = 4
        Button2.Text = "管理者ぺーじ"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' ErrorProvider1
        ' 
        ErrorProvider1.ContainerControl = Me
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(Button2)
        Controls.Add(Label2)
        Controls.Add(TextBox1)
        Controls.Add(Label1)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "Form1"
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents ErrorProvider1 As ErrorProvider

End Class
