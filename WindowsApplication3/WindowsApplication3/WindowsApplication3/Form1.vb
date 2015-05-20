Public Class Form1
    Enum ActionType
        None = 0
        Create = 1
        Move = 2
    End Enum
    Dim mMove As Boolean = False
    Dim mousex As Integer = 0, mousey As Integer = 0
    Dim action As ActionType = ActionType.None
    Dim titleE As String
    Dim cpoly As New KPoly
    Dim ipoly As Integer = -1
    Dim inode As Integer = -1
    Dim changes As Boolean = False
    Private Sub SetTitle(Optional ByVal s As String = "Map Maker")
        Me.Text = s + " - " + titleE
    End Sub
    Private Function FindIndex(ByVal x As Integer, ByVal y As Integer) As Integer
        If polys.Count = 0 Then Return -1
        For i As Integer = 0 To polys.Count - 1
            If mnuEditMove.Checked Then
                Dim ii As Integer = polys(i).FindNodeClosest(x, y, 6)
                If ii >= 0 Then inode = ii : Return i
            Else
                If polys(i).Contains(x, y) Then Return i
            End If
        Next
        Return -1
    End Function

    Private Sub Canvas_DoubleClick(sender As Object, e As EventArgs) Handles Canvas.DoubleClick
        If action = ActionType.Create Then finishPoly() 'Exit Sub
        'cpoly.AddPt(mousex, mousey)
        changes = True
        Canvas.Invalidate()
    End Sub
    Private Sub Canvas_MouseDown(sender As Object, e As MouseEventArgs) Handles Canvas.MouseDown
        mousex = e.X
        mousey = e.Y
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If action = ActionType.None Then Exit Sub
            ipoly = FindIndex(mousex, mousey)
            CanvasMenu.Show(Canvas, e.Location)
        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
            If action = ActionType.Create Then
                inode = cpoly.FindNodeClosest(mousex, mousey, 6, True)
                If inode < 0 Then
                    If cpoly.Count < 3 AndAlso cpoly.Contains(mousex, mousey) = False Then Exit Sub
                    ipoly = -2
                Else
                    ipoly = -1
                End If
            Else
                ipoly = FindIndex(mousex, mousey)
                If ipoly < 0 Then Exit Sub
            End If
            mMove = True
            Canvas.Invalidate()
        End If
    End Sub

    Private Sub Canvas_MouseMove(sender As Object, e As MouseEventArgs) Handles Canvas.MouseMove
        If mMove Then
            Dim dx As Integer = e.X - mousex, dy As Integer = e.Y - mousey
            If action = ActionType.Create Then
                If ipoly = -2 Then
                    cpoly.MoveRel(dx, dy)
                Else
                    cpoly.MovePt(inode, dx, dy, True)
                End If
            Else
                If inode < 0 Then
                    polys(ipoly).MoveRel(dx, dy)
                Else
                    polys(ipoly).MovePt(inode, dx, dy)
                End If
            End If
            mousex = e.X
            mousey = e.Y
            Canvas.Invalidate()
        End If
    End Sub

    Private Sub Canvas_MouseUp(sender As Object, e As MouseEventArgs) Handles Canvas.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If mMove = False Then Exit Sub
            mMove = False
            inode = -1
            SetTitle()
            Canvas.Invalidate()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        action = ActionType.None
        titleE = ""
        polys.Add(New KPoly(New Rectangle(60, 50, 100, 150)))
        cpoly = New KPoly
        lstPoly.Items.Add(polys.Count)
        Canvas.Invalidate()
    End Sub


    Private Sub Canvas_Paint(sender As Object, e As PaintEventArgs) Handles Canvas.Paint
        Dim g As Graphics = e.Graphics
        If action = ActionType.Create Then
            cpoly.Draw(g, Pens.Blue, True)
        End If
        If polys.Count = 0 Then Exit Sub
        For i As Integer = 0 To polys.Count - 1
            If ipoly = i Then
                polys(i).Draw(g, Pens.Blue, mnuEditMove.Checked)
            Else
                polys(i).Draw(g, Pens.Black)
            End If
        Next
    End Sub

    
    Private Sub btn_poly_Click(sender As Object, e As EventArgs) Handles btn_poly.Click
        If action = ActionType.Create Then
            MessageBox.Show("Already creating..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            action = ActionType.Create
            titleE = "Poly"
            SetTitle()
        End If
    End Sub

    Private Sub mnuCancel_Click(sender As Object, e As EventArgs) Handles mnuCancel.Click
        cpoly = New KPoly
        action = ActionType.None
        titleE = ""
        Me.Text = titleE
        Canvas.Invalidate()
    End Sub

    Private Sub mnuEditMove_Click(sender As Object, e As EventArgs) Handles mnuEditMove.Click
        mnuEditMove.Checked = Not mnuEditMove.Checked
        If ipoly >= 0 Then Canvas.Invalidate()
    End Sub

    Private Sub mnuAddPt_Click(sender As Object, e As EventArgs) Handles mnuAddPt.Click
        cpoly.AddPt(mousex, mousey)
        changes = True
        Canvas.Invalidate()
    End Sub
    Private Sub finishPoly()
        If cpoly.Count < 3 Then MessageBox.Show("Polygon must have atleast 3 points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) : Exit Sub
        action = ActionType.None
        titleE = ""
        cpoly.Finish()
        polys.Add(New KPoly(cpoly))
        cpoly = New KPoly
        lstPoly.Items.Add(polys.Count)
        Canvas.Invalidate()
    End Sub
    Private Sub mnuFinishShape_Click(sender As Object, e As EventArgs) Handles mnuFinishShape.Click
        finishPoly()
    End Sub

    Private Sub lstPoly_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstPoly.SelectedIndexChanged
        If lstPoly.SelectedIndex < 0 Then Exit Sub
        ipoly = lstPoly.SelectedItem - 1
        Canvas.Invalidate()
    End Sub

    Private Sub mnuDelPt_Click(sender As Object, e As EventArgs) Handles mnuDelPt.Click
        If inode < 0 Then MessageBox.Show("Check Edit Point option from Edit Menu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub

    End Sub

    Private Sub mnuSaveMap_Click(sender As Object, e As EventArgs) Handles mnuSaveMap.Click
        If cpoly.Count > 0 Then MessageBox.Show("Finish the current shape.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) : Exit Sub
        DoSave("Save Map")
        MessageBox.Show("Saved")
    End Sub
    Private Sub DoSave(ByVal s As String)
        SaveDlg.Title = s
        Dim i As DialogResult = SaveDlg.ShowDialog
        If i = Windows.Forms.DialogResult.OK Then
            If polys.Count = 0 Then MessageBox.Show("No shapes to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
            If Dir(SaveDlg.FileName).Length <> 0 Then Kill(SaveDlg.FileName)
            Dim f As Integer = FreeFile()
            FileOpen(f, SaveDlg.FileName, OpenMode.Binary, OpenAccess.Write)
            FilePut(f, CShort(polys.Count))
            For Each p As KPoly In polys
                p.Save(f)
            Next
            FileClose(f)
        End If
        changes = False
    End Sub
    Private Sub mnuSaveAs_Click(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
        If cpoly.Count > 0 Then MessageBox.Show("Finish the current shape.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) : Exit Sub
        DoSave("Save As..")
        MessageBox.Show("Saved")
    End Sub

    Private Sub mnuOpenMap_Click(sender As Object, e As EventArgs) Handles mnuOpenMap.Click
        Dim i As DialogResult = OpenDlg.ShowDialog
        If i = Windows.Forms.DialogResult.OK Then
            If Dir(OpenDlg.FileName).Length = 0 Then MessageBox.Show("File not found") : Exit Sub
            Dim f As Integer = FreeFile()
            FileOpen(f, OpenDlg.FileName, OpenMode.Binary, OpenAccess.Read)
            Dim c As Short 'FilePut(f, CShort(polys.Count))
            FileGet(f, c)
            polys.Clear() : lstPoly.Items.Clear()
            For ia As Short = 1 To c
                Dim cc As Short, pp As New List(Of Point)
                FileGet(f, cc)
                For ii As Short = 1 To cc
                    Dim x As Integer, y As Integer
                    FileGet(f, x) : FileGet(f, y)
                    pp.Add(New Point(x, y))
                Next
                polys.Add(New KPoly(pp, True))
                lstPoly.Items.Add(polys.Count)
            Next
            FileClose(f)
            MessageBox.Show(polys.Count & " shape(s).")
            titleE = ""
            SetTitle()
            Canvas.Invalidate()
        End If
    End Sub

    Private Sub mnuSimulate_Click(sender As Object, e As EventArgs) Handles mnuSimulate.Click
        Form2.ShowDialog()
        MessageBox.Show("Simulation End")
    End Sub

    Private Sub mnuNewMap_Click(sender As Object, e As EventArgs) Handles mnuNewMap.Click
        If polys.Count > 0 Or cpoly.Count > 0 Then
            Dim i As DialogResult = MessageBox.Show("Unsaved changes will be lost." + vbCrLf + "Save?", "New Map", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)
            If i = Windows.Forms.DialogResult.Cancel Then Exit Sub
            If i = Windows.Forms.DialogResult.Yes Then DoSave("Save map")
        End If
        lstPoly.Items.Clear()
        polys.Clear()
        cpoly = New KPoly
        inode = -1
        ipoly = -1
        titleE = ""
        SetTitle()
        action = ActionType.None
        Canvas.Invalidate()
    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        If changes Then
            Dim i As DialogResult = MessageBox.Show("Unsaved changes will be lost." + vbCrLf + "Save?", "New Map", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)
            If i = Windows.Forms.DialogResult.Cancel Then Exit Sub
            If i = Windows.Forms.DialogResult.Yes Then DoSave("Save map")
        End If
        End
    End Sub

    Private Sub mnuList_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mnuList.Opening
        If lstPoly.Items.Count = 0 OrElse lstPoly.SelectedIndex < 0 Then
            mnuDeletePoly.Enabled = False
        Else
            mnuDeletePoly.Enabled = True
        End If
    End Sub

    Private Sub mnuDeletePoly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeletePoly.Click
        polys.RemoveAt(lstPoly.SelectedIndex)
        lstPoly.Items.RemoveAt(lstPoly.SelectedIndex)
        Canvas.Invalidate()
    End Sub

    Private Sub mnuShowMeshPoints_Click(sender As Object, e As EventArgs) Handles mnuShowMeshPoints.Click
        showMeshpt = Not showMeshpt
        mnuShowMeshPoints.Checked = showMeshpt
    End Sub

    Private Sub mnuShadowMode_Click(sender As Object, e As EventArgs) Handles mnuShadowMode.Click
        shadowMode = Not shadowMode
        mnuShadowMode.Checked = shadowMode
    End Sub

    Private Sub mnuExpertMode_Click(sender As Object, e As EventArgs) Handles mnuExpertMode.Click
        expertMode = Not expertMode
        mnuExpertMode.Checked = expertMode
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Map Maker and AI Simulator for 'Darker Unknown'" & vbNewLine & _
                        "Created By: Koushik Seal and Jhelum Das", "About Us", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub HowToUseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HowToUseToolStripMenuItem.Click
        MessageBox.Show("Please check the documentation for usage and detail.", "Darker Unknown", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Canvas_Click(sender As Object, e As EventArgs) Handles Canvas.Click

    End Sub
End Class
