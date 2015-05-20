<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.components = New System.ComponentModel.Container()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.canvas = New System.Windows.Forms.PictureBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.chkPlayerLight = New System.Windows.Forms.CheckBox()
        Me.tmrSimu = New System.Windows.Forms.Timer(Me.components)
        Me.mnuCanvas = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSetPlayer = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddNPC = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddLight = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNPC = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddReachable = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddCollectable = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddAvoidable = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCarryLight = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.canvas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.mnuCanvas.SuspendLayout()
        Me.mnuNPC.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.83892!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblStatus, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.12641!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.87359!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(614, 415)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.canvas, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 35)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(608, 357)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'canvas
        '
        Me.canvas.BackColor = System.Drawing.Color.Linen
        Me.canvas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.canvas.Location = New System.Drawing.Point(3, 3)
        Me.canvas.Name = "canvas"
        Me.canvas.Size = New System.Drawing.Size(602, 351)
        Me.canvas.TabIndex = 1
        Me.canvas.TabStop = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(3, 395)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(151, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "Simulator status message label"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.BackColor = System.Drawing.SystemColors.Control
        Me.TableLayoutPanel4.ColumnCount = 4
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571!))
        Me.TableLayoutPanel4.Controls.Add(Me.btnStart, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.chkPlayerLight, 1, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(608, 26)
        Me.TableLayoutPanel4.TabIndex = 2
        '
        'btnStart
        '
        Me.btnStart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnStart.Location = New System.Drawing.Point(3, 3)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(146, 20)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'chkPlayerLight
        '
        Me.chkPlayerLight.AutoSize = True
        Me.chkPlayerLight.Checked = True
        Me.chkPlayerLight.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPlayerLight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkPlayerLight.Location = New System.Drawing.Point(155, 3)
        Me.chkPlayerLight.Name = "chkPlayerLight"
        Me.chkPlayerLight.Size = New System.Drawing.Size(146, 20)
        Me.chkPlayerLight.TabIndex = 1
        Me.chkPlayerLight.Text = "Player Carry Light"
        Me.chkPlayerLight.UseVisualStyleBackColor = True
        '
        'tmrSimu
        '
        Me.tmrSimu.Interval = 60
        '
        'mnuCanvas
        '
        Me.mnuCanvas.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSetPlayer, Me.mnuAddNPC, Me.mnuAddLight, Me.mnuAddKey})
        Me.mnuCanvas.Name = "mnuCanvas"
        Me.mnuCanvas.ShowImageMargin = False
        Me.mnuCanvas.Size = New System.Drawing.Size(150, 92)
        '
        'mnuSetPlayer
        '
        Me.mnuSetPlayer.Name = "mnuSetPlayer"
        Me.mnuSetPlayer.Size = New System.Drawing.Size(149, 22)
        Me.mnuSetPlayer.Text = "Set Player Location"
        '
        'mnuAddNPC
        '
        Me.mnuAddNPC.Name = "mnuAddNPC"
        Me.mnuAddNPC.Size = New System.Drawing.Size(149, 22)
        Me.mnuAddNPC.Text = "Add NPC"
        '
        'mnuAddLight
        '
        Me.mnuAddLight.Name = "mnuAddLight"
        Me.mnuAddLight.Size = New System.Drawing.Size(149, 22)
        Me.mnuAddLight.Text = "Add Light"
        '
        'mnuNPC
        '
        Me.mnuNPC.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddReachable, Me.mnuAddCollectable, Me.mnuAddAvoidable, Me.mnuCarryLight})
        Me.mnuNPC.Name = "mnuNPC"
        Me.mnuNPC.Size = New System.Drawing.Size(159, 114)
        '
        'mnuAddReachable
        '
        Me.mnuAddReachable.Name = "mnuAddReachable"
        Me.mnuAddReachable.Size = New System.Drawing.Size(158, 22)
        Me.mnuAddReachable.Text = "Add Reachable"
        '
        'mnuAddCollectable
        '
        Me.mnuAddCollectable.Name = "mnuAddCollectable"
        Me.mnuAddCollectable.Size = New System.Drawing.Size(158, 22)
        Me.mnuAddCollectable.Text = "Add Collectable"
        '
        'mnuAddAvoidable
        '
        Me.mnuAddAvoidable.Name = "mnuAddAvoidable"
        Me.mnuAddAvoidable.Size = New System.Drawing.Size(158, 22)
        Me.mnuAddAvoidable.Text = "Add Avoidable"
        '
        'mnuCarryLight
        '
        Me.mnuCarryLight.Name = "mnuCarryLight"
        Me.mnuCarryLight.Size = New System.Drawing.Size(158, 22)
        Me.mnuCarryLight.Text = "Carry Light"
        '
        'mnuAddKey
        '
        Me.mnuAddKey.Name = "mnuAddKey"
        Me.mnuAddKey.Size = New System.Drawing.Size(149, 22)
        Me.mnuAddKey.Text = "Add Key(Pickup)"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Linen
        Me.ClientSize = New System.Drawing.Size(614, 415)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DoubleBuffered = True
        Me.Name = "Form2"
        Me.Text = "AI Simulator"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        CType(Me.canvas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.mnuCanvas.ResumeLayout(False)
        Me.mnuNPC.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents canvas As System.Windows.Forms.PictureBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents tmrSimu As System.Windows.Forms.Timer
    Friend WithEvents mnuCanvas As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSetPlayer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddNPC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNPC As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddReachable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddCollectable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddAvoidable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCarryLight As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkPlayerLight As System.Windows.Forms.CheckBox
    Friend WithEvents mnuAddLight As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddKey As System.Windows.Forms.ToolStripMenuItem
End Class
