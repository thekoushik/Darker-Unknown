Public Class Matrix
    'Private Class MatrixNode
    'Public ngbrs As SortedSet(Of MatrixNode)
    'Public h As Double, g As Double
    'Public parent As MatrixNode, node As Point
    'Public Sub New(ByVal n As Point)
    'ngbrs = New SortedSet(Of MatrixNode) : node = n
    'h = 0 : g = Double.MinValue
    'End Sub
    'End Class
    Private Class MatrixElement ': Inherits Object
        Public visNgbrs As SortedList(Of Double, MatrixElement)
        Public h As Double, g As Double, f As Double
        Public parent As MatrixElement, node As Point
        Public id As Integer
        Public Overloads Function Equals(obj As MatrixElement) As Boolean
            If obj Is Nothing Then Return False
            Return node.Equals(obj.node)
        End Function
        Public Sub Clear()
            h = 0 : g = 0 : f = 0
        End Sub
        Public Sub New(ByVal n As Point, ByVal i As Integer)
            visNgbrs = New SortedList(Of Double, MatrixElement)
            node = n : h = 0 : g = 0
            parent = Nothing
            id = i
        End Sub
        Public Sub calcH(ByVal e As Point)
            h = LineDist(node, e)
        End Sub
        Public Function calcG() As Double
            If parent Is Nothing OrElse Me.Equals(parent) Then g = 0 : Return 0
            g = parent.g + LineDist(node, parent.node)
            Return g
        End Function
        Public Function calcF() As Double
            f = g + h
            Return f
        End Function
        Public Function calcF(ByVal e As Point) As Double
            calcG() : calcH(e)
            Return calcF()
        End Function
        Public Function CanSee(ByVal o As MatrixElement) As Boolean
            If o Is Nothing Then Return False
            If o.node = Nothing Then Return False
            Return LineOfSight(node, o.node)
        End Function
        Public Function SeeParent() As Boolean
            If parent Is Nothing Then Return False
            If parent.node.Equals(node) Then Return False
            Return LineOfSight(node, parent.node)
        End Function
        Public Sub AddNgbr(ByVal p As MatrixElement, ByVal d As Double)
            If Not visNgbrs.ContainsValue(p) Then visNgbrs.Add(d, p)
        End Sub
        Public Sub RemoveNgbr(ByVal p As MatrixElement)
            Dim i As Integer = visNgbrs.IndexOfValue(p)
            If i >= 0 Then visNgbrs.RemoveAt(i)
        End Sub
        Public Function GetDist(ByVal p As MatrixElement) As Double
            Dim i As Integer = visNgbrs.IndexOfValue(p)
            If i >= 0 Then Return visNgbrs.Keys(i)
            Throw New KeyNotFoundException
        End Function
    End Class
    Private elements As List(Of MatrixElement)
    Public lastPathLength As Double = -1
    Public Sub New()
        elements = New List(Of MatrixElement)
    End Sub
    Private Sub GenNgbrs(ByVal p As MatrixElement, ByRef lst As SortedList(Of Double, MatrixElement))
        For i As Short = 0 To elements.Count - 1
            Dim q As MatrixElement = elements(i)
            If p.node <> q.node Then
                If LineOfSight(p.node, q.node) Then
                    If Not lst.ContainsValue(q) Then
                        Try
                            lst.Add(LineDist(p.node, q.node), q)
                        Catch e As Exception
                        End Try
                    End If
                End If
            End If
        Next
    End Sub
    Public Sub MakeNavMesh() 'ByVal mesh As List(Of Point), ByVal lstPolys As List(Of KPoly), ByVal qt As QuadTree(Of KPoly))
        elements.Clear()
        For i As Integer = 0 To cMesh - 1
            elements.Add(New MatrixElement(mesh(i).Location, i))
        Next
        'Dim l As Integer = elements.Count - 1
        'For i As Integer = 0 To l
        'Dim p As Point = elements(i).node
        'For j As Integer = 0 To l
        ' If i <> j Then
        'Dim q As Point = elements(j).node
        'If LineOfSight(p, q) Then elements(i).AddNgbr(elements(j), LineDist(p, q))
        'End If
        'Next
        'Next
        For i As Short = 0 To cMesh - 1
            Dim l As Short = mesh(i).visibleNgbrs.Count - 1
            For j As Short = 0 To l
                elements(i).AddNgbr(elements(mesh(i).visibleNgbrs.Values(j)), mesh(i).visibleNgbrs.Keys(j))
            Next
        Next
    End Sub
    'Immediate Theta* (By: Jhelum Das & Koushik Seal)
    Public Function JKImmediateThetaStar(ByVal p1 As Point, ByVal p2 As Point, ByRef mylist As List(Of Integer)) As List(Of Point)
        Dim l As New List(Of Point)
        If Not LineOfSight(p1, p2) Then
            Dim s As New MatrixElement(p2, -1)
            GenNgbrs(s, s.visNgbrs)
            Dim mindist As Double = Double.MaxValue, mindx As Short = -1
            For i As Short = 0 To s.visNgbrs.Count - 1
                Dim n As MatrixElement = s.visNgbrs.Values(i)
                If mylist(n.id) = -1 Then Continue For
                Dim d As Double = LineDist(n.node, p1) + s.visNgbrs.Keys(i)
                If d < mindist Then
                    If Not LineOfSight(n.node, p1) Then Continue For
                    mindx = i : mindist = d
                End If
            Next
            If mindx < 0 Then Return Nothing
            l.Add(s.visNgbrs.Values(mindx).node)
        End If
        l.Add(p2)
        Return l
    End Function
    Public Function JKFlee(ByVal p As Point, ByVal targets As List(Of NPC)) As List(Of Point)
        'Debug.WriteLine("cc")
        Dim st As New MatrixElement(p, -1)
        GenNgbrs(st, st.visNgbrs)
        Dim lst As New List(Of Point)
        For i As Short = 0 To st.visNgbrs.Count - 1
            Dim n As MatrixElement = st.visNgbrs.Values(i)
            For j As Short = 0 To n.visNgbrs.Count - 1
                Dim v As Boolean = False, nn As MatrixElement = n.visNgbrs.Values(j)
                Dim pp As New Point(nn.id, n.id)
                If Not lst.Contains(pp) Then
                    For k As Short = 0 To targets.Count - 1
                        If targets(k).alive AndAlso LineOfSight(nn.node, targets(k).Loc) Then
                            v = True : Exit For
                        End If
                    Next
                    If v = False Then
                        lst.Add(pp)
                    End If
                End If
            Next
        Next
        Dim d As Double = -1, ii As Short = -1
        If lst.Count = 1 Then ii = 0
        If lst.Count > 1 Then
            For i As Short = 0 To lst.Count - 1
                Dim far As Double = -1
                For j As Short = 0 To targets.Count - 1
                    If targets(j).alive Then
                        Dim dd As Double = LineDistSq(targets(j).x, targets(j).y, mesh(lst(i).X).x, mesh(lst(i).X).y)
                        If far < dd Then far = dd
                    End If
                Next
                If d < far Then d = far : ii = i
            Next
        End If
        Dim l As New List(Of Point)
        If ii >= 0 Then l.Add(mesh(lst(ii).Y).Location) : l.Add(mesh(lst(ii).X).Location)
        Return l
    End Function
    Public Function FindPath3(ByVal p1 As Point, ByVal p2 As Point, Optional ByRef avoidList As List(Of Integer) = Nothing, Optional ByVal calcDist As Boolean = False) As List(Of Point)
        If LineOfSight(p1, p2) Then
            Dim l As New List(Of Point)
            l.Add(p1) : l.Add(p2)
            If calcDist Then lastPathLength = LineDist(p1, p2)
            Return l
        End If
        'Debug.WriteLine("Not direct")
        Dim open As New SortedList(Of Double, MatrixElement), closed As New SortedList(Of Double, MatrixElement)
        Dim startN As New MatrixElement(p1, -1), endN As New MatrixElement(p2, -1)
        GenNgbrs(startN, startN.visNgbrs)
        GenNgbrs(endN, endN.visNgbrs)
        For i As Short = 0 To cMesh - 1
            elements(i).Clear()
        Next
        startN.parent = Nothing
        open.Add(startN.calcF(p2), startN)
        Dim m As MatrixElement
        While open.Count > 0
            m = open.Values(0)
            'Debug.WriteLine(open.Count)
            open.RemoveAt(0)
            'Debug.WriteLine(open.Count)
            If m.node.Equals(p2) Then
                'Debug.WriteLine("samed")
                If calcDist Then lastPathLength = m.f
                Dim l As New List(Of Point)
                Do
                    l.Add(m.node)
                    If m.parent Is Nothing OrElse m.parent.Equals(m) Then Exit Do
                    m = m.parent
                Loop While True
                If Not l.Contains(p1) Then l.Add(p1) ': Debug.WriteLine("Eoo")
                l.Reverse()
                'Debug.WriteLine("samed end")
                Return l
            ElseIf LineOfSight(m.node, p2) Then 'path found
                'Debug.WriteLine("line")
                If calcDist Then lastPathLength = m.f
                Dim l As New List(Of Point)
                l.Add(p2)
                Do
                    If m.node = Nothing Then Exit Do
                    l.Add(m.node)
                    If m.parent Is Nothing OrElse m.parent.Equals(m) Then Exit Do
                    m = m.parent
                Loop While True
                If Not l.Contains(p1) Then l.Add(p1)
                l.Reverse()
                'Debug.WriteLine("line end")
                Return l
            End If
            m.calcF(p2)
            If closed.ContainsKey(m.f) Then Continue While
            closed.Add(m.f, m)
            'Debug.WriteLine(closed.Values(closed.Count - 1).id)
            For ind As Integer = 0 To m.visNgbrs.Values.Count - 1
                Dim Sdash As MatrixElement = m.visNgbrs.Values(ind)
                If Not avoidList Is Nothing AndAlso avoidList.Contains(Sdash.id) Then Continue For
                If closed.IndexOfValue(Sdash) < 0 Then
                    If open.IndexOfValue(Sdash) < 0 Then
                        Sdash.g = Double.MaxValue
                        Sdash.parent = Nothing
                    End If
                    '''''''update vertex sdash, m
                    Dim gOld As Double = Sdash.g
                    '''''''''''compute cost m, sdash
                    If Sdash.CanSee(m.parent) Then ''''''path2
                        Dim l As Double = LineDist(m.parent.node, Sdash.node)
                        If m.parent.g + l < Sdash.g Then
                            Sdash.parent = m.parent
                            Sdash.g = m.parent.g + l
                        End If
                    Else ''''''''''''''''''''''''''''''''''''''''''''''''path1
                        Dim l As Double = LineDist(Sdash.node, m.node)
                        If m.g + l < Sdash.g Then
                            Sdash.parent = m
                            Sdash.g = m.g + l
                        End If
                    End If
                    '''''''''''end of compute cost
                    If Sdash.g < gOld Then
                        Sdash.calcF(p2)
                        Dim i As Integer = open.IndexOfValue(Sdash)
                        If i >= 0 Then open.RemoveAt(i)
                        If open.ContainsKey(Sdash.f) Then
                            'If 
                        Else
                            open.Add(Sdash.f, Sdash)
                        End If
                    End If
                    ''''''''end of update vertex
                End If
            Next
            'm = New MatrixElement(Nothing, -1)
        End While
        'Debug.WriteLine("NOthing")
        lastPathLength = -1
        Return Nothing
    End Function
    Public Function FindPath(ByVal p1 As Point, ByVal p2 As Point) As List(Of Point)
        If LineOfSight(p1, p2) Then
            Dim l As New List(Of Point)
            l.Add(p1) : l.Add(p2)
            Return l
        End If
        Debug.WriteLine("Not direct")
        Dim open As New SortedList(Of Double, MatrixElement), closed As New SortedList(Of Double, MatrixElement)
        Dim startN As New MatrixElement(p1, -1), endN As New MatrixElement(p2, -1)
        GenNgbrs(startN, startN.visNgbrs)
        GenNgbrs(endN, endN.visNgbrs)
        startN.parent = startN
        open.Add(startN.calcF(p2), startN)
        While open.Count > 0
            Dim m As MatrixElement = open.Values(0)
            open.RemoveAt(0)
            '''''''''''set vertex
            If Not m.SeeParent Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''path1
                Dim si As Short = -1, sd As Double
                For ind As Integer = 0 To m.visNgbrs.Count - 1
                    Dim Sdash As MatrixElement = m.visNgbrs.Values(ind)
                    If closed.IndexOfValue(Sdash) >= 0 Then
                        Dim t As Double = Sdash.g + LineDist(Sdash.node, m.node)
                        Dim i As Integer = closed.IndexOfKey(t)
                        If i >= 0 Then
                            If si = -1 OrElse t < sd Then
                                si = i : sd = t
                            End If
                        End If
                    End If
                Next
                If si >= 0 Then m.parent = closed.Values(si) : m.calcG() : Debug.WriteLine("si")
            End If
            If (m.node.X = p2.X AndAlso m.node.Y = p2.Y) Then
                Debug.WriteLine("samed")
                Dim l As New List(Of Point)
                Do
                    l.Add(m.node)
                    If m.parent.Equals(Nothing) OrElse m.parent.Equals(m) Then Exit Do
                    m = m.parent
                Loop While True
                If Not l.Contains(p1) Then l.Add(p1) ': Debug.WriteLine("Eoo")
                'l.Reverse()
                Return l
            ElseIf LineOfSight(m.node, p2) Then 'path found
                Debug.WriteLine("line")
                Dim l As New List(Of Point)
                l.Add(p2)
                Do
                    l.Add(m.node)
                    If m.parent.Equals(Nothing) OrElse m.parent.Equals(m) Then Exit Do
                    m = m.parent
                Loop While True
                If Not l.Contains(p1) Then l.Add(p1)
                'l.Reverse()
                Return l
            End If
            closed.Add(m.calcF(p2), m)
            Debug.WriteLine("closed.add")
            For ind As Integer = 0 To m.visNgbrs.Values.Count - 1
                Dim Sdash As MatrixElement = m.visNgbrs.Values(ind)
                If closed.IndexOfValue(Sdash) < 0 Then
                    If open.IndexOfValue(Sdash) < 0 Then
                        Sdash.g = Double.MaxValue
                        Sdash.parent = Nothing
                    End If
                    '''''''update vertex sdash, m
                    Dim gOld As Double = Sdash.g
                    '''''''''''compute cost m, sdash'''''''''''''''''''''''''''''path2
                    Dim t As Double = m.parent.g + LineDist(m.parent.node, Sdash.node)
                    If t < Sdash.g Then ' AndAlso LineOfSight(m.parent.node, Sdash.node) Then
                        Sdash.parent = m.parent
                        Sdash.g = t
                        Debug.WriteLine("new parent " & Sdash.g)
                    End If
                    '''''''''''end of compute cost
                    If Sdash.g < gOld Then
                        Dim i As Integer = open.IndexOfValue(Sdash)
                        If i >= 0 Then open.RemoveAt(i)
                        open.Add(Sdash.calcF(), Sdash)
                    End If
                    ''''''''end of update vertex
                End If
            Next
        End While
        Return New List(Of Point)
    End Function
End Class
