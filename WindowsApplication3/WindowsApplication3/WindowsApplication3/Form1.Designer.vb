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
        Me.components = New System.ComponentModel.Container()
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNewMap = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOpenMap = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSaveMap = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.SimulateToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSimulate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEditMove = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuShowMeshPoints = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuShadowMode = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExpertMode = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HowToUseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Canvas = New System.Windows.Forms.PictureBox()
        Me.lstPoly = New System.Windows.Forms.ListBox()
        Me.mnuList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuDeletePoly = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.btn_poly = New System.Windows.Forms.Button()
        Me.CanvasMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddPt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDelPt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFinishShape = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCancel = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
        Me.OpenDlg = New System.Windows.Forms.OpenFileDialog()
        Me.MenuStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.Canvas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuList.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.CanvasMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.SettingsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(551, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewMap, Me.mnuOpenMap, Me.mnuSaveMap, Me.mnuSaveAs, Me.SimulateToolStripMenuItem, Me.mnuSimulate, Me.ToolStripSeparator1, Me.mnuExit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'mnuNewMap
        '
        Me.mnuNewMap.Name = "mnuNewMap"
        Me.mnuNewMap.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNewMap.Size = New System.Drawing.Size(174, 22)
        Me.mnuNewMap.Text = "New"
        '
        'mnuOpenMap
        '
        Me.mnuOpenMap.Name = "mnuOpenMap"
        Me.mnuOpenMap.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuOpenMap.Size = New System.Drawing.Size(174, 22)
        Me.mnuOpenMap.Text = "Open"
        '
        'mnuSaveMap
        '
        Me.mnuSaveMap.Name = "mnuSaveMap"
        Me.mnuSaveMap.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSaveMap.Size = New System.Drawing.Size(174, 22)
        Me.mnuSaveMap.Text = "Save"
        '
        'mnuSaveAs
        '
        Me.mnuSaveAs.Name = "mnuSaveAs"
        Me.mnuSaveAs.Size = New System.Drawing.Size(174, 22)
        Me.mnuSaveAs.Text = "Save As"
        '
        'SimulateToolStripMenuItem
        '
        Me.SimulateToolStripMenuItem.Name = "SimulateToolStripMenuItem"
        Me.SimulateToolStripMenuItem.Size = New System.Drawing.Size(171, 6)
        '
        'mnuSimulate
        '
        Me.mnuSimulate.Name = "mnuSimulate"
        Me.mnuSimulate.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuSimulate.Size = New System.Drawing.Size(174, 22)
        Me.mnuSimulate.Text = "Run Simulation"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(171, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(174, 22)
        Me.mnuExit.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEditMove})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'mnuEditMove
        '
        Me.mnuEditMove.Checked = True
        Me.mnuEditMove.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuEditMove.Name = "mnuEditMove"
        Me.mnuEditMove.Size = New System.Drawing.Size(171, 22)
        Me.mnuEditMove.Text = "Edit Move(Design)"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowMeshPoints, Me.mnuShadowMode, Me.mnuExpertMode})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'mnuShowMeshPoints
        '
        Me.mnuShowMeshPoints.Name = "mnuShowMeshPoints"
        Me.mnuShowMeshPoints.Size = New System.Drawing.Size(171, 22)
        Me.mnuShowMeshPoints.Text = "Show Mesh Points"
        '
        'mnuShadowMode
        '
        Me.mnuShadowMode.Checked = True
        Me.mnuShadowMode.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuShadowMode.Name = "mnuShadowMode"
        Me.mnuShadowMode.Size = New System.Drawing.Size(171, 22)
        Me.mnuShadowMode.Text = "Shadow Mode"
        '
        'mnuExpertMode
        '
        Me.mnuExpertMode.Name = "mnuExpertMode"
        Me.mnuExpertMode.Size = New System.Drawing.Size(171, 22)
        Me.mnuExpertMode.Text = "Expert Mode"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HowToUseToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'HowToUseToolStripMenuItem
        '
        Me.HowToUseToolStripMenuItem.Name = "HowToUseToolStripMenuItem"
        Me.HowToUseToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.HowToUseToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HowToUseToolStripMenuItem.Text = "How to use"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.Padding = New System.Windows.Forms.Padding(0, 0, 100, 0)
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.AutoScroll = True
        Me.ContentPanel.Size = New System.Drawing.Size(520, 418)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 24)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(551, 390)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Canvas, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lstPoly, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 42)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(545, 345)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Canvas
        '
        Me.Canvas.BackColor = System.Drawing.Color.White
        Me.Canvas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Canvas.Location = New System.Drawing.Point(3, 3)
        Me.Canvas.Name = "Canvas"
        Me.Canvas.Size = New System.Drawing.Size(489, 339)
        Me.Canvas.TabIndex = 0
        Me.Canvas.TabStop = False
        '
        'lstPoly
        '
        Me.lstPoly.ContextMenuStrip = Me.mnuList
        Me.lstPoly.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstPoly.FormattingEnabled = True
        Me.lstPoly.Location = New System.Drawing.Point(498, 3)
        Me.lstPoly.Name = "lstPoly"
        Me.lstPoly.Size = New System.Drawing.Size(44, 339)
        Me.lstPoly.TabIndex = 1
        '
        'mnuList
        '
        Me.mnuList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDeletePoly})
        Me.mnuList.Name = "mnuList"
        Me.mnuList.ShowImageMargin = False
        Me.mnuList.Size = New System.Drawing.Size(109, 26)
        '
        'mnuDeletePoly
        '
        Me.mnuDeletePoly.Name = "mnuDeletePoly"
        Me.mnuDeletePoly.Size = New System.Drawing.Size(108, 22)
        Me.mnuDeletePoly.Text = "Delete poly"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 4
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel3.Controls.Add(Me.btn_poly, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(545, 33)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'btn_poly
        '
        Me.btn_poly.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn_poly.Location = New System.Drawing.Point(3, 3)
        Me.btn_poly.Name = "btn_poly"
        Me.btn_poly.Size = New System.Drawing.Size(130, 27)
        Me.btn_poly.TabIndex = 2
        Me.btn_poly.Text = "Start Shape"
        Me.btn_poly.UseVisualStyleBackColor = True
        '
        'CanvasMenu
        '
        Me.CanvasMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddPt, Me.mnuDelPt, Me.mnuFinishShape, Me.mnuCancel})
        Me.CanvasMenu.Name = "CanvasMenu"
        Me.CanvasMenu.ShowImageMargin = False
        Me.CanvasMenu.Size = New System.Drawing.Size(124, 92)
        '
        'mnuAddPt
        '
        Me.mnuAddPt.Name = "mnuAddPt"
        Me.mnuAddPt.Size = New System.Drawing.Size(123, 22)
        Me.mnuAddPt.Text = "Add Point"
        '
        'mnuDelPt
        '
        Me.mnuDelPt.Name = "mnuDelPt"
        Me.mnuDelPt.Size = New System.Drawing.Size(123, 22)
        Me.mnuDelPt.Text = "Remove Point"
        '
        'mnuFinishShape
        '
        Me.mnuFinishShape.Name = "mnuFinishShape"
        Me.mnuFinishShape.Size = New System.Drawing.Size(123, 22)
        Me.mnuFinishShape.Text = "Finish Shape"
        '
        'mnuCancel
        '
        Me.mnuCancel.Name = "mnuCancel"
        Me.mnuCancel.Size = New System.Drawing.Size(123, 22)
        Me.mnuCancel.Text = "Cancel Shape"
        '
        'SaveDlg
        '
        Me.SaveDlg.Filter = "Map file|*.jkmap"
        '
        'OpenDlg
        '
        Me.OpenDlg.Filter = "Map file|*.jkmap"
        Me.OpenDlg.Title = "Open map"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(551, 414)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Map Maker & Simulator"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        CType(Me.Canvas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuList.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.CanvasMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewMap As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpenMap As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSaveMap As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HowToUseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents SimulateToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSimulate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Canvas As System.Windows.Forms.PictureBox
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lstPoly As System.Windows.Forms.ListBox
    Friend WithEvents CanvasMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddPt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDelPt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCancel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditMove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFinishShape As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents mnuList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuDeletePoly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuShowMeshPoints As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuShadowMode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExpertMode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btn_poly As System.Windows.Forms.Button

End Class
