<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.picCanvas = New System.Windows.Forms.PictureBox()
        Me.scrHand = New System.Windows.Forms.HScrollBar()
        Me.scrJoint1 = New System.Windows.Forms.HScrollBar()
        Me.scrJoint2 = New System.Windows.Forms.HScrollBar()
        CType(Me.picCanvas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picCanvas
        '
        Me.picCanvas.Location = New System.Drawing.Point(0, 0)
        Me.picCanvas.Name = "picCanvas"
        Me.picCanvas.Size = New System.Drawing.Size(724, 421)
        Me.picCanvas.TabIndex = 0
        Me.picCanvas.TabStop = False
        '
        'scrHand
        '
        Me.scrHand.Location = New System.Drawing.Point(35, 424)
        Me.scrHand.Maximum = 1000
        Me.scrHand.Name = "scrHand"
        Me.scrHand.Size = New System.Drawing.Size(449, 25)
        Me.scrHand.TabIndex = 1
        '
        'scrJoint1
        '
        Me.scrJoint1.Location = New System.Drawing.Point(35, 456)
        Me.scrJoint1.Maximum = 1000
        Me.scrJoint1.Name = "scrJoint1"
        Me.scrJoint1.Size = New System.Drawing.Size(449, 25)
        Me.scrJoint1.TabIndex = 2
        '
        'scrJoint2
        '
        Me.scrJoint2.Location = New System.Drawing.Point(35, 481)
        Me.scrJoint2.Maximum = 1000
        Me.scrJoint2.Name = "scrJoint2"
        Me.scrJoint2.Size = New System.Drawing.Size(449, 25)
        Me.scrJoint2.TabIndex = 3
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(736, 513)
        Me.Controls.Add(Me.scrJoint2)
        Me.Controls.Add(Me.scrJoint1)
        Me.Controls.Add(Me.scrHand)
        Me.Controls.Add(Me.picCanvas)
        Me.Name = "Form2"
        Me.Text = "Form2"
        CType(Me.picCanvas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picCanvas As System.Windows.Forms.PictureBox
    Friend WithEvents scrHand As System.Windows.Forms.HScrollBar
    Friend WithEvents scrJoint1 As System.Windows.Forms.HScrollBar
    Friend WithEvents scrJoint2 As System.Windows.Forms.HScrollBar
End Class
