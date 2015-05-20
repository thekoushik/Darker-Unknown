Public Class Form2

    Dim mouse As Point, LastNPC As Short = -1
    Private kl As Boolean, kr As Boolean, ku As Boolean, kd As Boolean, kS As Boolean
    Private mShoot As Boolean, mMove As Boolean = False
    Private explode As New Anim
    Dim lights As List(Of tLight)
    Dim GlobalLightBuffer As Bitmap
    Dim shadowBrush As SolidBrush
    Dim npcCarryLight As List(Of Boolean)

    Private Class tLight
        Public loc As Point
        Public rad As Short
        Public sweep As Short
        Public ang As Double
        Public rect As Rectangle
        Public img As Bitmap
        Private imgData As Bitmap
        'Private col As Color 'SolidBrush
        Private wid As Short
        Public moveWith As Short = -1

        Public Sub New(ByVal p As Point, ByVal s As Short, ByVal r As Short)
            loc = p : sweep = s : ang = 0 : rad = r
            wid = r + r
            rect = New Rectangle(p.X - r, p.Y - r, wid, wid)
            img = New Bitmap(wid, wid, Imaging.PixelFormat.Format32bppArgb)
        End Sub
        Public Sub setCol(ByVal r As Short, ByVal g As Short, ByVal b As Short, Optional ByVal a As Short = 200)
            'col = Color.FromArgb(a, r, g, b)
            imgData = New Bitmap(wid, wid, Imaging.PixelFormat.Format32bppArgb)
            Dim aa As Double = a / rad
            Dim aaa As Double = 0, ww = wid
            Dim gg As Graphics = Graphics.FromImage(imgData)
            gg.CompositingMode = Drawing2D.CompositingMode.SourceCopy
            For i As Short = 0 To rad
                gg.FillEllipse(New SolidBrush(Color.FromArgb(CInt(aaa), r, g, b)), i, i, ww, ww)
                aaa += aa : ww -= 2
            Next
            gg.Dispose()
        End Sub
        Public Sub Move(ByVal p As Point)
            loc = p
            rect = New Rectangle(p.X - rad, p.Y - rad, wid, wid)
        End Sub
        Public Sub FillEye(ByVal gg As Graphics)
            'gg.FillPie(col, New Rectangle(0, 0, wid, wid), -ang, sweep)
            gg.DrawImage(imgData, 0, 0)
        End Sub
        Public Sub DrawEye(ByVal g As Graphics)
            g.DrawImage(img, loc.X - rad, loc.Y - rad)
        End Sub
    End Class

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        tmrSimu.Enabled = False
        EndSimu()
    End Sub

    Private Sub Form2_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        DoKey(e.KeyCode, True)
    End Sub
    Private Sub DoKey(ByVal k As Keys, ByVal b As Boolean)
        Select Case k
            Case Keys.Left
                kl = b
            Case Keys.Right
                kr = b
            Case Keys.Up
                ku = b
            Case Keys.Down
                kd = b
            Case Keys.ShiftKey
                kS = b
            Case Keys.Escape
                If b = False Then
                    EndSimu()
                End If
        End Select
    End Sub
    Private Sub EndSimu()
        Me.Text = "AI Simulator"
        tmrSimu.Enabled = False
        For i As Integer = 1 To npcs.Count - 1
            npcs(i).Deactivate()
        Next
        TableLayoutPanel1.Visible = True
    End Sub

    Private Sub Form2_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        DoKey(e.KeyCode, False)
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.Location = Form1.Location
        Me.Width = Form1.Width
        Me.Height = Form1.Height
        Me.BackColor = Color.White
        npcs = New List(Of NPC)
        lights = New List(Of tLight)
        npcCarryLight = New List(Of Boolean)
        chkPlayerLight.Enabled = shadowMode : mnuCarryLight.Enabled = shadowMode : mnuAddLight.Enabled = shadowMode
        npcs.Add(New NPC(New Point(10, 20), -1, Color.Black, "Player"))
        npcs(0).id = 0
        mnuAddAvoidable.DropDownItems.Clear()
        mnuAddReachable.DropDownItems.Clear()
        Dim m As New ToolStripMenuItem() With {.Text = "Player", .Name = "mnuNPCA0", .Tag = "0"}
        AddHandler m.Click, AddressOf mnuNPCItems_Click
        mnuAddAvoidable.DropDownItems.Add(m)
        Dim m2 As New ToolStripMenuItem() With {.Text = "Player", .Name = "mnuNPCR0", .Tag = "0"}
        AddHandler m2.Click, AddressOf mnuNPCItems_Click
        mnuAddReachable.DropDownItems.Add(m2)
        canvas.Invalidate()
    End Sub

    Private Sub canvas_MouseDown(sender As Object, e As MouseEventArgs) Handles canvas.MouseDown
        mouse = e.Location
        If Not tmrSimu.Enabled And e.Button = Windows.Forms.MouseButtons.Right Then
            Dim ii As Short = GetNearestNPC(mouse.X, mouse.Y, 6)
            If ii > 0 AndAlso npcs(ii).myAI.clever > 0 Then LastNPC = ii : mnuNPC.Show(canvas, mouse) Else LastNPC = -1 : mnuCanvas.Show(canvas, mouse)
        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
            Dim ii As Short = GetNearestNPC(mouse.X, mouse.Y, 6)
            If ii > 0 Then mMove = True : LastNPC = ii Else mMove = False : LastNPC = -1
            canvas.Invalidate()
        End If
    End Sub

    Private Sub canvas_MouseMove(sender As Object, e As MouseEventArgs) Handles canvas.MouseMove
        mouse = e.Location
        If mMove Then
            npcs(LastNPC).x = mouse.X
            npcs(LastNPC).y = mouse.Y
            canvas.Invalidate()
        End If
    End Sub

    Private Sub canvas_Paint(sender As Object, e As PaintEventArgs) Handles canvas.Paint
        Dim g As Graphics = e.Graphics
        If polys.Count = 0 Then Exit Sub
        For i As Integer = 0 To polys.Count - 1
            polys(i).Fill(g, Brushes.Gray)
        Next
        For i As Integer = 1 To npcs.Count - 1
            npcs(i).Draw(g)
            If npcs(i).alive AndAlso npcs(i).myAI.clever > 0 Then npcs(i).myObjective.DrawDesign(g)
        Next
        If shadowMode Then
            For i As Short = 0 To lights.Count - 1
                g.DrawEllipse(Pens.Purple, lights(i).loc.X - 3, lights(i).loc.Y - 3, 6, 6)
            Next
        End If
        g.DrawEllipse(Pens.Blue, CInt(npcs(0).x - 5), CInt(npcs(0).y - 5), 10, 10)
    End Sub

    Private Sub tmrSimu_Tick(sender As Object, e As EventArgs) Handles tmrSimu.Tick
        If gameOver Then
            EndSimu()
            If cDOCs = pDOCs AndAlso cDOCs > 0 Then
                MessageBox.Show("Win!", "Darker Unknown", MessageBoxButtons.OK)
            Else
                MessageBox.Show("Game Over!", "Darker Unknown", MessageBoxButtons.OK)
            End If
            canvas.Invalidate() : Exit Sub
        End If
        npcs(0).myangle = Angle(mouse, npcs(0).Loc)
        'If mShoot Then
        'mShoot = False
        'Dim p As Point = HitToWorldRayCast(npcs(0).x, npcs(0).y, npcs(0).myangle,0)
        'If p <> Nothing Then explode.Start(3, p.X, p.Y)
        'End If
        Dim spd As Short = 5, b As Boolean = False, h As Short = 0, v As Short = 0
        If kS Then spd = 7
        If kl Then h = -spd : b = True
        If kr Then h = spd : b = True
        If ku Then v = -spd : b = True
        If kd Then v = spd : b = True
        npcs(0).SetSpeeds(h, v)
        If b Then npcs(0).Go(True)
        If DOCS.Count > 0 Then
            For i As Short = 0 To DOCS.Count - 1
                If npcs(0).DistanceTo(npcs(DOCS(i))) < 8 Then
                    Dim j As Short = DOCS(i)
                    npcs(j).alive = False
                    DOCS.RemoveAt(i)
                    pDOCs += 1
                    Me.Text = "AI Simulator - Keys(" & pDOCs & "/" & cDOCs & ")"
                    If pDOCs = cDOCs Then gameOver = True
                    Exit For
                End If
            Next
        End If
        'If explode.Active Then explode.Update()
        Me.Invalidate()
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        'If tmrSimu.Enabled = False Then
        'Debug.Flush()
        worldWidth = canvas.Width : worldHeight = canvas.Height
        RegisterQuadF()
        GenNavMesh()
        cDOCs = DOCS.Count
        'btnStart.Text = "Stop"
        If shadowMode Then
            If chkPlayerLight.Checked Then
                lights.Add(New tLight(npcs(0).Loc, 360, 250))
                With  lights(lights.Count - 1)
                    .setCol(255, 255, 0)
                    .moveWith = 0
                End With
            End If
            shadowBrush = New SolidBrush(Color.Transparent)
            Me.BackColor = Color.Black
        End If
        gameOver = False
        TableLayoutPanel1.Visible = False
        Me.Focus()
        Dim r As New Random
        tmrSimu.Enabled = True
        For i As Integer = 1 To npcs.Count - 1
            If shadowMode AndAlso npcCarryLight(i - 1) Then
                lights.Add(New tLight(npcs(i).Loc, 360, 250))
                With lights(lights.Count - 1)
                    .setCol(r.Next(128, 255), r.Next(128, 255), r.Next(128, 255))
                    .moveWith = i
                End With
            End If
            npcs(i).InitMyMesh(cMesh)
            npcs(i).Activate()
        Next
        If cDOCs > 0 Then Me.Text = "AI Simulator - Keys(0/" & cDOCs & ")"
        'lblStatus.Text = "Simulation started"
        'Else
        'btnStart.Text = "Start"
        'tmrSimu.Enabled = False
        'lblStatus.Text = "Simulation stopped"
        'End If
    End Sub

    Private Sub mnuSetPlayer_Click(sender As Object, e As EventArgs) Handles mnuSetPlayer.Click
        If InPolys(mouse) >= 0 Then
            MessageBox.Show("Point inside obstracles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        npcs(0).Go(mouse)
        canvas.Invalidate()
    End Sub

    Private Sub canvas_MouseUp(sender As Object, e As MouseEventArgs) Handles canvas.MouseUp
        mouse = e.Location
        If mMove Then
            mMove = False
            npcs(LastNPC).x = mouse.X
            npcs(LastNPC).y = mouse.Y
            LastNPC = -1
            canvas.Invalidate()
        End If
    End Sub

    Private Sub Form2_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        mShoot = True
    End Sub

    Private Sub Form2_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover

    End Sub

    Private Sub Form2_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        mouse = e.Location
    End Sub

    Private Sub Form2_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        mShoot = False
    End Sub

    Private Sub Form2_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics
        If polys.Count = 0 Then Exit Sub
        If shadowMode Then
            For ii As Short = 0 To lights.Count - 1
                Dim here As Point, np As Short = lights(ii).moveWith
                If np >= 0 Then
                    If npcs(np).alive = False Then Continue For
                    here = npcs(np).Loc
                    lights(ii).Move(here)
                Else
                    here = lights(ii).loc
                End If
                Dim bg As Graphics = Graphics.FromImage(lights(ii).img)
                bg.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                bg.Clear(Color.Transparent)
                lights(ii).FillEye(bg)
                For i As Short = 0 To polys.Count - 1
                    Dim k As KPoly = makeShadowPoly(here.X, here.Y, lights(ii).rad, polys(i))
                    k.Fill(bg, shadowBrush, False)
                Next
                bg.Dispose()
                lights(ii).DrawEye(g)
            Next
        End If
        For i As Integer = 0 To polys.Count - 1
            polys(i).Fill(g, Brushes.Black)
        Next
        For i As Integer = 1 To npcs.Count - 1
            If expertMode Then
                If npcs(0).CanSee(npcs(i)) AndAlso npcs(i).alive Then
                    If shadowMode Then
                        If LineDistSq(npcs(0).x, npcs(0).y, npcs(i).x, npcs(i).y) < 62500 Then
                            npcs(i).Draw(g, False)
                        End If
                    Else
                        npcs(i).Draw(g, False)
                    End If
                End If
            Else
                If npcs(i).alive Then npcs(i).Draw(g, False)
            End If
        Next
        If showMeshpt Then
            For i As Short = 0 To cMesh - 1
                g.DrawEllipse(Pens.Purple, mesh(i).x - 3, mesh(i).y - 3, 6, 6)
            Next
        End If
        'g.DrawEllipse(Pens.Blue, npcs(0).x - 5, npcs(0).y - 5, 10, 10)
        npcs(0).Draw(g, True)
        'explode.Draw(g)
    End Sub
    Private Sub mnuNPCItems_Click(ByVal sender As Object, ByVal e As EventArgs)
        'ContextMenuStrip1.Hide() 'Sometimes the menu items can remain open.  May not be necessary for you.
        Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If item IsNot Nothing Then
            Dim i As Char = item.Name.Chars(6), j As Short = Short.Parse(item.Tag)
            If LastNPC = j Then
                MessageBox.Show("Can't Perform on self!" & vbNewLine & "Try on others.", "How can it be possible?", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Else
                Select Case i
                    Case "A" 'avoid
                        'If MessageBox.Show("Avoid " & npcs(j).name, "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        If npcs(LastNPC).myObjective.hasAvoidable AndAlso npcs(LastNPC).myObjective.avoidTarget.Contains(npcs(j)) Then
                            MessageBox.Show("Already added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                        Else
                            npcs(LastNPC).myObjective.hasAvoidable = True
                            npcs(LastNPC).myObjective.avoidTarget.Add(npcs(j))
                        End If
                        'End If
                    Case "R" 'reach
                        'If MessageBox.Show("Reach " & npcs(j).name, "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        If npcs(LastNPC).myObjective.hasReachable AndAlso npcs(LastNPC).myObjective.reachTarget.Contains(npcs(j)) Then
                            MessageBox.Show("Already added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                        Else
                            npcs(LastNPC).myObjective.hasReachable = True
                            npcs(LastNPC).myObjective.reachTarget.Add(npcs(j))
                        End If
                        'End If
                End Select
                canvas.Invalidate()
            End If
            LastNPC = -1
        End If
    End Sub

    Private Sub mnuAddNPC_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddNPC.Click
        Dim r As New Random
        Dim ii As Short = npcs.Count
        npcs.Add(New NPC(mouse, 8, Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)), "NPC" & ii))
        npcs(ii).id = ii
        npcCarryLight.Add(False)
        Dim m As New ToolStripMenuItem() With {.Text = "NPC" & ii, .Name = "mnuNPCA" & ii, .Tag = ii}
        AddHandler m.Click, AddressOf mnuNPCItems_Click
        mnuAddAvoidable.DropDownItems.Add(m)
        Dim m2 As New ToolStripMenuItem() With {.Text = "NPC" & ii, .Name = "mnuNPCR" & ii, .Tag = ii}
        AddHandler m2.Click, AddressOf mnuNPCItems_Click
        mnuAddReachable.DropDownItems.Add(m2)
        canvas.Invalidate()
    End Sub

    Private Sub mnuNPC_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mnuNPC.Opening
        mnuCarryLight.Checked = npcCarryLight(LastNPC - 1)
        mnuAddCollectable.Enabled = IIf(DOCS.Count > 0, True, False)
        mnuAddCollectable.Checked = npcs(LastNPC).myObjective.hasCollectable
    End Sub

    Private Sub mnuCarryLight_Click(sender As Object, e As EventArgs) Handles mnuCarryLight.Click
        mnuCarryLight.Checked = Not mnuCarryLight.Checked
        npcCarryLight(LastNPC - 1) = mnuCarryLight.Checked
        'If  Then
        'mnuCarryLight.Checked = False
        '    = False
        'Else
        'mnuCarryLight.Checked = True
        'npcCarryLight(LastNPC - 1) = True
        'npcCarryLight(LastNPC - 1) = lights.Count
        'lights.Add(New tLight(npcs(LastNPC).Loc, 360, 250))
        'Dim r As New Random
        'lights(lights.Count - 1).setCol(r.Next(128, 255), r.Next(128, 255), r.Next(128, 255), r.Next(150, 200))
        'End If
    End Sub

    Private Sub mnuAddLight_Click(sender As Object, e As EventArgs) Handles mnuAddLight.Click
        Dim r As New Random
        lights.Add(New tLight(mouse, 360, 250))
        With lights(lights.Count - 1)
            .setCol(r.Next(128, 255), r.Next(128, 255), r.Next(128, 255))
            .moveWith = -1
        End With
        canvas.Invalidate()
    End Sub

    Private Sub mnuAddKey_Click(sender As Object, e As EventArgs) Handles mnuAddKey.Click
        Dim ii As Short = npcs.Count
        DOCS.Add(ii)
        Dim i As Short = DOCS.Count
        npcCarryLight.Add(False)
        npcs.Add(New NPC(mouse, 0, Color.Black, "Key" & i))
        npcs(ii).id = ii
        canvas.Invalidate()
    End Sub

    Private Sub mnuAddCollectable_Click(sender As Object, e As EventArgs) Handles mnuAddCollectable.Click
        'If npcs(LastNPC).myObjective.hasCollectable Then
        'MessageBox.Show("Already added!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        'Else
        npcs(LastNPC).myObjective.hasCollectable = Not npcs(LastNPC).myObjective.hasCollectable
        'End If
    End Sub
End Class