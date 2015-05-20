Public Class NPC : Implements IHasRect
    Public Enum TargetType
        None = 0
        NPC = 1
        Location = 2
    End Enum
    Public x As Double
    Public y As Double
    Public id As Integer
    Public name As String
    Public alive As Boolean = True
    Public rad As Short
    Public speed As Double
    Public speedX As Double
    Public speedY As Double
    'Public rotSpeed As Double = Math.PI
    Public curTarget As TargetType
    Public curTargetPt As Point
    Public curTargetNPC As NPC
    Private curMesh As Integer = -1
    Public myRange As Double
    Private myRangeSq As Double
    Public myangle As Double
    Public targetAngle As Double
    Public targetDist As Double
    Public myAI As AI
    Private active As Boolean = False
    Private rand As New Random
    Private path As New List(Of Point)
    Public myObjective As Objective
    Public checkPts As List(Of CheckPoint)
    Public myMeshptVisibleFromCheckpt As List(Of Integer)
    Private meshptCovered As Short = 0
    Public iBasicMeshVisited As Integer = 0
    Private col As Pen, colb As Brush
    Public Sub New(ByVal p As Point, ByVal clever As Integer, ByVal c As Color, Optional ByVal nm As String = "")
        x = p.X : y = p.Y
        rad = 10
        If nm <> "" Then name = nm
        speedX = 0 : speedY = 0
        If clever >= 0 Then
            myAI = New AI(clever)
            If clever > 0 Then myObjective = New Objective(Me)
        End If
        myRange = 400 : myRangeSq = myRange * myRange
        myangle = 90
        checkPts = New List(Of CheckPoint)
        myMeshptVisibleFromCheckpt = New List(Of Integer)
        col = New Pen(c) : colb = New SolidBrush(c)
    End Sub
    Public ReadOnly Property Rect() As Rectangle Implements IHasRect.Rect
        Get
            Dim r2 As Short = rad + rad
            Return New Rectangle(x - rad, y - rad, r2, r2)
        End Get
    End Property
    Public Function DistanceTo(ByVal np As NPC) As Double
        Return LineDist(x, y, np.x, np.y)
    End Function
    Public Sub InitMyMesh(ByVal v As Integer)
        If myAI.clever < 1 Then Exit Sub
        myMeshptVisibleFromCheckpt = New List(Of Integer)
        For i As Short = 0 To v - 1
            myMeshptVisibleFromCheckpt.Add(-1)
        Next
    End Sub
    'which mesh points are visible from this checkpoint, tell those mesh points about this checkpoint 
    Public Function PlotCheckpt(ByVal i As Integer) As Boolean
        Dim l As Integer = checkPts.Count
        For ii As Short = 0 To l - 1
            If checkPts(ii).i = i Then Debug.WriteLine("checkpt already" & i) : Return False
        Next
        checkPts.Add(New CheckPoint(i)) ': Debug.WriteLine("=>" & i)
        Dim c As Short = 0
        For ii As Short = 0 To mesh.Count - 1
            If myMeshptVisibleFromCheckpt(ii) = -1 Then
                If ii = i Then
                    myMeshptVisibleFromCheckpt(ii) = l : c += 1
                ElseIf mesh(ii).visibleNgbrs.ContainsValue(i) Then
                    myMeshptVisibleFromCheckpt(ii) = l : c += 1
                End If
            End If
        Next
        meshptCovered += c
        'Debug.WriteLine("+" & c)
        Return IIf(c = 0, False, True)
    End Function
    Public Function IsAllMeshptCovered() As Boolean
        meshptCovered = 0
        For Each v As Integer In myMeshptVisibleFromCheckpt
            If v = -1 Then Return False
        Next
        Return True
    End Function
    Public Sub Activate()
        If myAI.clever < 1 Then Exit Sub
        Dim myThread As New Threading.Thread(AddressOf Update)
        active = True
        myThread.Name = "An NPC" & id
        myThread.Start()
        myAI.State = AI.NPState.SpecialPathfindForDebug
        curTarget = TargetType.None
    End Sub
    Public Function CanSee(ByVal n As NPC) As Boolean
        Return LineOfSight(x, y, n.x, n.y)
    End Function
    Public ReadOnly Property Loc() As Point
        Get
            Return New Point(x, y)
        End Get
    End Property
    Public Sub Deactivate()
        active = False
    End Sub
    Private Sub DoElse()
        speedX = 0
        speedY = 0
        If rand.NextDouble() * 50 > 40 Then myangle = CInt(rand.NextDouble() * 360)
    End Sub
    Private Sub checkTarget()
        If curTarget = TargetType.None Then Exit Sub
        If myObjective.lastObjective = Objective.ObjectiveType.Avoidable Then
            targetAngle = myObjective.closestRunAngle
        Else
            targetAngle = Angle(curTargetPt.X, curTargetPt.Y, x, y)
        End If
        targetDist = LineDist(x, y, curTargetPt.X, curTargetPt.Y)
    End Sub
    Private Function Reached() As Boolean
        If targetDist > speed Then Return False
        x = curTargetPt.X : y = curTargetPt.Y
        Return True
    End Function
    Private Function canSee() As Boolean
        If Ang_Dif(targetAngle, myangle) < 90 AndAlso _
           targetDist <= myRange AndAlso _
           LineOfSight(x, y, curTargetPt.X, curTargetPt.Y) Then _
              Return True
        Return False
    End Function
    Private Sub Kill(np As NPC)
        If np.id = 0 Then gameOver = True : active = False
        myObjective.RemoveObj(np)
        npcs(np.id).Deactivate()
        npcs(np.id).alive = False
    End Sub
    Public Sub SetSpeeds(ByVal hspd As Double, ByVal vspd As Double)
        speedX = hspd : speedY = vspd
    End Sub
    Public ReadOnly Property GetDirection() As Double
        Get
            If id = 0 Then
                If speedY > 0 AndAlso speedX > 0 Then
                    myangle = 315
                ElseIf speedY > 0 AndAlso speedX < 0 Then
                    myangle = 225
                ElseIf speedY > 0 AndAlso speedX = 0 Then
                    myangle = 270
                ElseIf speedY < 0 AndAlso speedX > 0 Then
                    myangle = 45
                ElseIf speedY = 0 AndAlso speedX > 0 Then
                    myangle = 0
                ElseIf speedY < 0 AndAlso speedX < 0 Then
                    myangle = 135
                ElseIf speedY = 0 AndAlso speedX < 0 Then
                    myangle = 180
                Else
                    myangle = 90
                End If
            End If
            Return myangle
        End Get
    End Property
    Public ReadOnly Property GetSpeed() As Double
        Get
            If speed = 0 Then speed = Math.Sqrt(speedX * speedX + speedY * speedY)
            Return speed
        End Get
    End Property
    Public Sub ToMesh(ByVal idd As Integer)
        curTarget = TargetType.Location
        curTargetPt = mesh(idd).Location
        checkTarget()
        speed = myAI.Speed
        myangle = targetAngle
        Force(speed, myangle, speedX, speedY)
    End Sub
    Public Sub Go(ByVal p As Point)
        x = p.X : y = p.Y
    End Sub
    Public Sub Go(ByVal xx As Integer, ByVal yy As Integer, Optional ByVal check As Boolean = False)
        x += xx : y += yy
        If check Then MoveOutside(x, y)
    End Sub
    Public Sub Go(Optional ByVal check As Boolean = False)
        'Dim xx As Double = x + speedX, yy As Double = y + speedY
        x += speedX : y += speedY
        If check Then MoveOutside(x, y)
    End Sub
    Private Sub Update()
        While active
            Dim t As Long = Now.Millisecond

            If myObjective.Running = False Then myObjective.StartEvaluate()
            If myObjective.Done Then
                Select Case myObjective.lastObjective
                    Case Objective.ObjectiveType.Reachable
                        curTarget = TargetType.NPC
                        curTargetNPC = myObjective.reachTarget(myObjective.lastTarget)
                        curTargetPt = curTargetNPC.Loc
                        myAI.State = AI.NPState.Chase
                    Case Objective.ObjectiveType.Collectable
                        curTarget = TargetType.Location
                        curTargetNPC = npcs(myObjective.lastTarget)
                        curTargetPt = curTargetNPC.Loc
                        myAI.State = AI.NPState.Chase
                    Case Objective.ObjectiveType.Avoidable
                        'If myAI.State = AI.NPState.Flee Then

                        'Else
                        curTarget = TargetType.NPC
                        curTargetNPC = myObjective.avoidTarget(myObjective.lastTarget)
                        curTargetPt = curTargetNPC.Loc
                        myAI.State = AI.NPState.Avoid
                        'End If
                    Case Else
                        If meshptCovered < cMesh Then
                            If myAI.State = AI.NPState.Stay Then
                                Dim ii As Integer = GetNearestMeshpt(x, y)
                                If ii < 0 Then
                                    myAI.State = AI.NPState.Stay
                                Else
                                    ToMesh(ii) : myAI.State = AI.NPState.UnknownExplorePhase1Move : curMesh = ii
                                End If
                            End If
                        Else
                            If myAI.State = AI.NPState.Stay Then
                                myAI.State = AI.NPState.VisiblePathMove
                                path = NavMesh.FindPath3(Me.Loc, mesh(checkPts(iBasicMeshVisited).i).Location)
                                iBasicMeshVisited = (iBasicMeshVisited + 1) Mod checkPts.Count
                                curTarget = TargetType.Location
                                curTargetPt = path(0) : path.RemoveAt(0)
                                speed = myAI.Speed
                            End If
                        End If
                End Select
            End If
            checkTarget()
            Select Case myAI.State
                Case AI.NPState.Stay
                    DoElse()
                Case AI.NPState.Chase
                    If Reached() Then
                        Kill(curTargetNPC)
                        If curTarget = TargetType.Location Then
                            If DOCS.Count = 0 Then
                                gameOver = True
                            End If
                        End If
                        Exit Select
                    End If
                    If LineOfSight(x, y, curTargetPt.X, curTargetPt.Y) Then
                        If myangle <> targetAngle Then
                            myangle = targetAngle
                            speed = AI.ChaseSpeed
                            Force(speed, myangle, speedX, speedY)
                        End If
                        Go(True) ' GoToTargetOutside()
                    Else
                        If myObjective.lastObjective = Objective.ObjectiveType.Reachable Then myObjective.lastObjective = Objective.ObjectiveType.None
                        Dim b As Boolean = False
                        speed = myAI.Speed
                        If myAI.LocEstimate Then '' to do
                            Dim pa As Double = curTargetNPC.GetDirection * d2r
                            Dim playertogo As Point = New Point(curTargetPt.X + Math.Cos(pa) * targetDist, curTargetPt.Y + Math.Sin(pa) * targetDist)
                            If playertogo.X < 5 Then playertogo.X = 5
                            If playertogo.X > worldWidth - 5 Then playertogo.X = worldWidth - 5
                            If playertogo.Y < 5 Then playertogo.Y = 5
                            If playertogo.Y > worldHeight - 5 Then playertogo.Y = worldHeight - 5
                            MoveOutside(playertogo.X, playertogo.Y)
                            curTargetPt = playertogo
                            If GetPath() Then
                                Debug.WriteLine("estimated")
                                myAI.State = AI.NPState.SearchAlert
                                curTarget = TargetType.Location : b = True
                                myAI.DoingLocEst = True
                            Else
                                curTargetPt = curTargetNPC.Loc
                            End If
                        End If
                        If Not b Then
                            myAI.State = AI.NPState.Investigate
                            curTarget = TargetType.Location
                        End If
                    End If
                Case AI.NPState.Investigate
                    If Reached() Then
                        myAI.State = AI.NPState.Stay
                        curTarget = TargetType.None
                    ElseIf LineOfSight(x, y, curTargetPt.X, curTargetPt.Y) Then
                        If myangle <> targetAngle Then myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        Go(True)
                    Else 'shortest path
                        myAI.State = AI.NPState.SearchAlert : curTarget = TargetType.Location
                        'GoToTargetOutside()
                        If Not GetPath() Then
                            myAI.State = AI.NPState.Stay
                            curTarget = TargetType.None
                        End If
                    End If
                Case AI.NPState.SearchAlert
                    If CanSee(curTargetNPC) Then
                        myAI.State = AI.NPState.Chase : curTarget = TargetType.NPC
                        If Not path Is Nothing Then path.Clear()
                    ElseIf Reached() Then
                        If path.Count = 0 Then
                            If myAI.DoingLocEst Then myAI.LocEstimate = False
                            myAI.State = AI.NPState.Stay
                            curTarget = TargetType.None
                        Else
                            curTargetPt = New Point(path(0)) : path.RemoveAt(0)
                        End If
                    Else
                        If myangle <> targetAngle Then
                            myangle = targetAngle
                            speed = AI.ChaseSpeed
                            Force(speed, myangle, speedX, speedY)
                        End If
                        'If GoToTargetOutside() = 0 Then myAI.State = AI.NPState.SearchNormal
                        Go(True)
                    End If
                Case AI.NPState.SpecialPathfindForDebug
                    'Dim ii As Integer = GetNearestMeshpt(x, y)
                    'If ii < 0 Then
                    ' myAI.State = AI.NPState.Stay
                    'Else
                    'ToMesh(ii) : myAI.State = AI.NPState.UnknownExplorePhase1Move : curMesh = ii
                    'End If
                    'path = NavMesh.FindPath3(npcs(0).Loc, Loc)
                    myAI.State = AI.NPState.Stay
                    'curTarget = TargetType.Player
                    speed = 4
                    'curTargetPt = npcs(0).Loc
                    'myAI.State = AI.NPState.Avoid
                Case AI.NPState.VisiblePathMove
                    If Reached() Then
                        For i As Short = 0 To cMesh - 1
                            If x = mesh(i).x AndAlso y = mesh(i).y Then curMesh = i : Exit For
                        Next
                        If path.Count > 0 Then
                            curTarget = TargetType.Location
                            curTargetPt = New Point(path(0)) : path.RemoveAt(0)
                        Else
                            Debug.WriteLine("finish path")
                            path = NavMesh.FindPath3(Me.Loc, mesh(checkPts(iBasicMeshVisited).i).Location)
                            curMesh = checkPts(iBasicMeshVisited).i
                            iBasicMeshVisited = (iBasicMeshVisited + 1) Mod checkPts.Count
                            curTarget = TargetType.Location
                            curTargetPt = path(0) : path.RemoveAt(0)
                        End If
                    Else
                        If myangle <> targetAngle Then
                            myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        End If
                        Go()
                    End If
                Case AI.NPState.UnknownExploreMovePath
                    If Reached() Then
                        If path.Count > 0 Then
                            'Debug.WriteLine("Count " & path.Count)
                            curTarget = TargetType.Location
                            curTargetPt = New Point(path(0)) : path.RemoveAt(0)
                        Else
                            'Debug.WriteLine("finish path")
                            myAI.State = AI.NPState.UnknownExplorePhase1
                            curTarget = TargetType.None
                            myAI.memory = 0
                        End If
                    Else
                        If myangle <> targetAngle Then myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        Go()
                    End If
                Case AI.NPState.UnknownExplorePhase1Move
                    If Reached() Then
                        curTarget = TargetType.None
                        myAI.State = AI.NPState.UnknownExplorePhase1
                    Else
                        If myangle <> targetAngle Then myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        Go()
                    End If
                Case AI.NPState.UnknownExplorePhase1
                    If myAI.memory = 0 Then GoTo plot
                    Dim mp As Integer = GetNearestMeshptInvisible(curMesh, myMeshptVisibleFromCheckpt)
                    If mp = -1 Then
                        If meshptCovered < cMesh Then
                            Dim mmp As Integer = -1
                            mp = GetNearestMesptInvisibleFromNgbr(curMesh, myMeshptVisibleFromCheckpt, mmp)
                            If mp = -1 Then
                                path = GetNearestInvisibleMeshpt(curMesh, mp, myMeshptVisibleFromCheckpt)
                                If mp >= 0 Then
                                    curMesh = mp
                                    myAI.State = AI.NPState.UnknownExploreMovePath
                                    curTargetPt = path(0) : path.RemoveAt(0)
                                    curTarget = TargetType.Location
                                Else
                                    Debug.WriteLine("no path! " & (cMesh - meshptCovered))
                                    myAI.State = AI.NPState.Stay
                                End If
                            Else
                                If mmp >= 0 Then
                                    myAI.memory = mmp : ToMesh(mp) : myAI.State = AI.NPState.UnknownExplorePhase2Move : curMesh = mp
                                Else
                                    ToMesh(mp) : myAI.State = AI.NPState.UnknownExplorePhase1Move : curMesh = mp
                                    If myMeshptVisibleFromCheckpt(mp) = -1 Then myAI.memory = 0 Else myAI.memory = -1
                                End If
                            End If
                        Else
                            Debug.WriteLine("all covered")
                            myAI.State = AI.NPState.Stay
                        End If
                    Else
                        If myMeshptVisibleFromCheckpt(curMesh) >= 0 Then
                            ToMesh(mp) : curMesh = mp : myAI.State = AI.NPState.UnknownExplorePhase1Move
                        Else
plot:
                            If Not PlotCheckpt(curMesh) Then
                                If meshptCovered = cMesh Then Debug.WriteLine("all cover done") Else Debug.WriteLine("check point error")
                                myAI.State = AI.NPState.Stay
                            Else
                                'Debug.WriteLine("checkpt, " & (cMesh - meshptCovered))
                                If myAI.memory = 0 Then myAI.memory = -1 : mp = curMesh
                                If cMesh = meshptCovered Then
                                    Debug.WriteLine("All done")
                                    myAI.State = AI.NPState.Stay
                                Else
                                    Dim mmp As Integer = GetNearestMesptInvisibleFromNgbr(mp, myMeshptVisibleFromCheckpt)
                                    If mmp >= 0 Then
                                        If myMeshptVisibleFromCheckpt(mp) = -1 Then
                                            ToMesh(mp) : myAI.State = AI.NPState.UnknownExplorePhase1Move : curMesh = mp
                                            myAI.memory = 0
                                        Else
                                            ToMesh(mp) : myAI.State = AI.NPState.UnknownExplorePhase2Move : myAI.memory = mmp : curMesh = mp
                                        End If
                                    Else
                                        path = GetNearestInvisibleMeshpt(curMesh, mp, myMeshptVisibleFromCheckpt)
                                        If mp >= 0 Then
                                            'Debug.WriteLine("pathhh")
                                            curMesh = mp
                                            myAI.State = AI.NPState.UnknownExploreMovePath
                                            curTargetPt = path(0) : path.RemoveAt(0)
                                            curTarget = TargetType.Location
                                        Else
                                            Debug.WriteLine("no path again")
                                            myAI.State = AI.NPState.Stay
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Case AI.NPState.UnknownExplorePhase2Move
                    If Reached() Then
                        curTarget = TargetType.None
                        myAI.State = AI.NPState.UnknownExplorePhase2
                    Else
                        If myangle <> targetAngle Then myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        Go()
                    End If
                Case AI.NPState.UnknownExplorePhase2
                    ToMesh(myAI.memory) : myAI.State = AI.NPState.UnknownExplorePhase1Move : curMesh = myAI.memory : myAI.memory = -1
                Case AI.NPState.Avoid 'go away
                    If targetDist < 200 Then
                        'myangle = (targetAngle + 180) Mod 360
                        myangle = targetAngle
                        Dim d As Double
                        Dim p As Point = HitToWorldRayCast(x, y, myangle, d)
                        If p <> Nothing Then
                            myObjective.GetFlee()
                            If myObjective.FleePathFound Then
                                'myObjective.FleePathFound = False
                                path = myObjective.FleePath
                                curTargetPt = path(0) : path.RemoveAt(0)
                                curTarget = TargetType.Location
                                speed = AI.ChaseSpeed
                                myAI.State = AI.NPState.Flee
                                myObjective.lastObjective = Objective.ObjectiveType.None
                                myObjective.lastTarget = -1
                            Else
                                speed = AI.ChaseSpeed
                                Force(speed, myangle, speedX, speedY)
                                Go(True)
                            End If
                        Else
                            speed = AI.ChaseSpeed
                            Force(speed, myangle, speedX, speedY)
                            Go(True)
                        End If
                    Else 'not stay but go to covered location
                        'speed = myAI.Speed
                        'myAI.State = AI.NPState.Stay
                        If myObjective.FleePathFound Then
                            myObjective.FleePathFound = False
                            path = myObjective.FleePath
                            curTargetPt = path(0) : path.RemoveAt(0)
                            curTarget = TargetType.Location
                            speed = AI.ChaseSpeed
                            myAI.State = AI.NPState.Flee
                            myObjective.lastObjective = Objective.ObjectiveType.None
                            myObjective.lastTarget = -1
                        Else
                            myAI.State = AI.NPState.Stay
                        End If
                    End If
                Case AI.NPState.Flee
                    If Reached() Then
                        If path.Count > 0 Then
                            curTarget = TargetType.Location
                            curTargetPt = New Point(path(0)) : path.RemoveAt(0)
                            Dim i As Short = GetMeshID(curTargetPt.X, curTargetPt.Y)
                            If i >= 0 Then
                                If myMeshptVisibleFromCheckpt(i) = -1 Then PlotCheckpt(i)
                            End If
                        Else
                            myObjective.curAvoiders.Clear()
                            speed = myAI.Speed
                            myAI.State = AI.NPState.Stay
                        End If
                    Else
                        If myangle <> targetAngle Then
                            myangle = targetAngle : Force(speed, myangle, speedX, speedY)
                        End If
                        Go()
                    End If
            End Select
            t = Now.Millisecond - t
            If t < 60 Then Threading.Thread.Sleep(60 - t)
        End While
    End Sub
    Private Function GetPath() As Boolean
        Try
            path = NavMesh.JKImmediateThetaStar(New Point(x, y), curTargetPt, myMeshptVisibleFromCheckpt)
            If path Is Nothing Then path = New List(Of Point) : Return False
            curTargetPt = path(0) : path.RemoveAt(0)
            Return True
        Catch ex As Exception
            path.Clear()
            Return False
        End Try
    End Function
    Public Sub Draw(ByVal g As Graphics, Optional ByVal st As Boolean = True)
        If id = 0 Then
            'g.DrawPie(Pens.Blue, CInt(x - 80), CInt(y - 80), 160, 160, CInt(-myangle) - 45, 90)
        ElseIf myAI.clever = 0 Then
            g.DrawRectangle(Pens.Green, CInt(x - 8), CInt(y - 2), 16, 4)
            g.DrawRectangle(Pens.Green, CInt(x - 10), CInt(y - 6), 6, 12)
            g.DrawRectangle(Pens.Green, CInt(x + 5), CInt(y + 2), 2, 3)
            Exit Sub
        Else
            g.DrawPie(Pens.Red, CInt(x - 80), CInt(y - 80), 160, 160, CInt(-myangle) - 45, 90)
        End If
        Dim r As Short = rad / 2
        If st Then
            g.DrawEllipse(col, CInt(x - r), CInt(y - r), rad, rad)
            g.DrawString(name, Form2.Font, Brushes.Black, x - 10, y)
        ElseIf id > 0 Then
            g.FillEllipse(colb, CInt(x - r), CInt(y - r), rad, rad)
            g.DrawString(name & "-" & myAI.State, Form2.Font, Brushes.Black, x - 10, y)
        End If
        'If Not curTargetPt = Nothing Then g.DrawEllipse(Pens.Green, curTargetPt.X - 2, curTargetPt.Y - 2, 4, 4)
        If showMeshpt Then
            For Each c As CheckPoint In checkPts
                g.DrawEllipse(col, c.x - 4, c.y - 4, 8, 8)
            Next
            If myMeshptVisibleFromCheckpt.Count > 0 Then
                For i As Short = 0 To mesh.Count - 1
                    If myMeshptVisibleFromCheckpt(i) >= 0 Then g.DrawEllipse(col, mesh(i).x - 1, mesh(i).y - 1, 2, 2)
                Next
            End If
        End If
    End Sub
End Class
