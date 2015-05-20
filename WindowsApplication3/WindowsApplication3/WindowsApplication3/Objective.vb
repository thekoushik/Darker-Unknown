Imports System.Threading
Public Class Objective
    Public Enum ObjectiveType
        None
        Collectable
        Reachable
        Avoidable
    End Enum
    Private self As NPC
    Public lastObjective As ObjectiveType = ObjectiveType.None
    Public lastTarget As Integer = -1
    Public hasCollectable As Boolean = False
    Public closestCollectable As Short = -1
    Public hasReachable As Boolean = False
    Public reachTarget As List(Of NPC)
    Public closestReachable As Short = -1
    Public hasAvoidable As Boolean = False
    Public avoidTarget As List(Of NPC)
    Public closestAvoidable As Short = -1
    Public closestRunAngle As Double = 0
    Public FleePathFound As Boolean = False
    Public FleePath As List(Of Point)
    Public curAvoiders As New List(Of NPC)
    Public Running As Boolean = False
    Public Done As Boolean = False
    Public Sub New(ByVal np As NPC)
        self = np
        lastObjective = ObjectiveType.None
        reachTarget = New List(Of NPC)
        avoidTarget = New List(Of NPC)
        
    End Sub
    Public Sub RemoveObj(ByVal i As NPC)
        Dim d As Boolean = False
        Dim ii As Short
        If i.id > 0 Then
            If i.myAI.clever = 0 Then
                ii = DOCS.IndexOf(i.id)
                If ii >= 0 Then DOCS.RemoveAt(ii)
                Exit Sub
            End If
        End If
        ii = reachTarget.IndexOf(i)
        If ii >= 0 Then reachTarget.RemoveAt(ii) : d = True
        If reachTarget.Count = 0 Then hasReachable = False
        If d = False Then
            ii = avoidTarget.IndexOf(i)
            If ii >= 0 Then avoidTarget.RemoveAt(ii) : d = True
            If avoidTarget.Count = 0 Then hasAvoidable = False
        End If
        If d Then lastTarget = -1 : lastObjective = ObjectiveType.None
    End Sub
    Public Function ClosestDOCReachable() As Double
        If Not hasCollectable AndAlso DOCS.Count = 0 Then Return -1
        If hasCollectable AndAlso DOCS.Count = 0 Then hasCollectable = False : Return -1
        Dim d As Double = self.DistanceTo(npcs(DOCS(0)))
        Dim cc As Short = 0
        For i As Short = 1 To DOCS.Count - 1
            If self.CanSee(npcs(DOCS(i))) Then
                Dim dd As Double = self.DistanceTo(npcs(DOCS(i)))
                If dd < d Then d = dd : cc = i
            End If
        Next
        If cc = 0 AndAlso Not LineOfSight(self.x, self.y, npcs(DOCS(0)).x, npcs(DOCS(0)).y) Then closestCollectable = -1 : Return -1
        closestCollectable = DOCS(cc)
        Return d
    End Function
    Public Sub DrawDesign(ByVal g As Graphics)
        If hasReachable Then
            Dim pR As New Pen(Color.Red)
            pR.StartCap = Drawing2D.LineCap.ArrowAnchor
            'pR.EndCap = Drawing2D.LineCap.RoundAnchor
            For i As Short = 0 To reachTarget.Count - 1
                g.DrawLine(pR, CInt(reachTarget(i).x), CInt(reachTarget(i).y), CInt(self.x), CInt(self.y))
            Next
        End If
        If hasAvoidable Then
            Dim pR As New Pen(Color.Blue)
            pR.StartCap = Drawing2D.LineCap.ArrowAnchor
            'pR.EndCap = Drawing2D.LineCap.RoundAnchor
            For i As Short = 0 To avoidTarget.Count - 1
                g.DrawLine(pR, CInt(avoidTarget(i).x), CInt(avoidTarget(i).y), CInt(self.x), CInt(self.y))
            Next
        End If

    End Sub
    Public Function ClosestTargetReachable() As Double
        If Not hasReachable AndAlso reachTarget.Count = 0 Then Return -1
        Dim d As Double = self.DistanceTo(reachTarget(0))
        Dim cr As Short = 0
        For i As Short = 1 To reachTarget.Count - 1
            If self.CanSee(reachTarget(i)) Then
                Dim dd As Double = self.DistanceTo(reachTarget(i))
                If dd < d Then d = dd : cr = i
            End If
        Next
        If cr = 0 AndAlso LineOfSight(self.x, self.y, reachTarget(0).x, reachTarget(0).y) = False Then closestReachable = -1 : Return -1
        closestReachable = cr
        Return d
    End Function
    Public Function ClosestTargetAvoidable() As Double
        If Not hasAvoidable AndAlso avoidTarget.Count = 0 Then Return -1
        Dim d As Double = Double.MaxValue 'self.DistanceTo(avoidTarget(0))
        Dim ca As Short = -1
        Dim sl As New SortedList(Of Double, Short) 'angle,id
        Dim l As New List(Of NPC)
        For i As Short = 0 To avoidTarget.Count - 1
            If self.CanSee(avoidTarget(i)) Then
                l.Add(avoidTarget(i))
                sl.Add(Angle(self.Loc, avoidTarget(i).Loc), i)
                Dim dd As Double = self.DistanceTo(avoidTarget(i))
                If ca < 0 OrElse dd < d Then d = dd : ca = i
            End If
        Next
        closestAvoidable = ca
        Dim maxdif As Double = -1, ii As Short = -1
        For i As Short = 0 To sl.Count - 1
            Dim n As Short = (i + 1) Mod sl.Count
            Dim cdif As Double = Ang_Dif(sl.Keys(i), sl.Keys(n))
            If cdif > maxdif Then maxdif = cdif : ii = i
        Next
        'closestAvoidable = mi
        If ii >= 0 Then closestRunAngle = sl.Keys(ii) - (maxdif / 2)
        If IsSame(l) Then Return Double.MaxValue
        'If closestAvoidable = 0 AndAlso LineOfSight(self.x, self.y, avoidTarget(0).x, avoidTarget(0).y) = False Then closestAvoidable = -1 : Return -1
        'If d < 150 Then
        'If FleePathFound = False Then
        'FleePathFound = True
        'FleePath = NavMesh.JKFlee(self.Loc, l)
        'If FleePath.Count = 0 Then FleePathFound = False
        'End If
        'End If
        Return d
    End Function
    Private Function IsSame(ByVal l As List(Of NPC)) As Boolean
        If l.Count <> curAvoiders.Count Then Return False
        Dim i As Short = 0
        For j As Short = 0 To l.Count - 1
            If curAvoiders.Contains(l(j)) Then i += 1
        Next
        If i = curAvoiders.Count Then Return True Else Return False
    End Function
    Public Sub GetFlee()
        Dim l As New List(Of NPC)
        For i As Short = 0 To avoidTarget.Count - 1
            If self.CanSee(avoidTarget(i)) Then l.Add(avoidTarget(i))
        Next
        If IsSame(l) Then Exit Sub
        curAvoiders = l
        FleePathFound = True
        FleePath = NavMesh.JKFlee(self.Loc, l)
        If FleePath.Count = 0 Then FleePathFound = False
    End Sub
    Public Sub StartEvaluate()
        Dim myThread As New Threading.Thread(AddressOf Evaluate)
        myThread.Name = "obj_of_" & self.id
        Running = True
        myThread.Start()
    End Sub
    Private Sub Evaluate()
        Dim closest As Double = Double.MaxValue, co As ObjectiveType = ObjectiveType.None
        Dim d As Double = ClosestDOCReachable()
        If d >= 0 Then closest = d : co = ObjectiveType.Collectable
        d = ClosestTargetReachable()
        If d >= 0 AndAlso d < closest Then closest = d : co = ObjectiveType.Reachable
        d = ClosestTargetAvoidable()
        If d >= 0 AndAlso d < closest Then closest = d : co = ObjectiveType.Avoidable
        If closest < 0 Then Done = True : Running = False : Exit Sub
        lastObjective = co
        Select Case lastObjective
            Case ObjectiveType.Collectable
                lastTarget = closestCollectable
            Case ObjectiveType.Reachable
                lastTarget = closestReachable
            Case ObjectiveType.Avoidable
                lastTarget = closestAvoidable
        End Select
        Done = True
        Running = False
    End Sub
End Class
