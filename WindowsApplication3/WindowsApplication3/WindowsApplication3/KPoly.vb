Public Class KPoly : Implements IHasRect
    Public points As List(Of Point)
    Public center As Point
    Public AABB As Rectangle
    Public id As Short
    Public ReadOnly Property Rect As Rectangle Implements IHasRect.Rect
        Get
            Return AABB
        End Get
    End Property
    Public Sub New()
        points = New List(Of Point)
        center = New Point
    End Sub
    Public Sub New(p As KPoly)
        Init(p.points, True)
    End Sub
    Public Sub New(ByVal list As List(Of Point), ByVal copy As Boolean)
        Init(list, copy)
    End Sub
    Public Sub New(ByVal r As Rectangle)
        AABB = New Rectangle(r.X, r.Y, r.Width, r.Height)
        points = New List(Of Point)(5)
        points.Add(New Point(r.Location))
        points.Add(New Point(r.X, r.Y + r.Height))
        points.Add(New Point(r.X + r.Width, r.Y + r.Height))
        points.Add(New Point(r.X + r.Width, r.Y))
        points.Add(New Point(r.Location))
        center = New Point(r.X + r.Width / 2, r.Y + r.Height / 2)
    End Sub
    Private Sub Init(ByVal list As List(Of Point), ByVal copy As Boolean)
        Dim minx As Integer = list(0).X, miny As Integer = list(0).Y, maxx As Integer = minx, maxy As Integer = miny
        points = New List(Of Point)(list.Count)
        For Each pt As Point In list
            If copy Then
                points.Add(New Point(pt.X, pt.Y))
            Else
                points.Add(pt)
            End If
            If pt.X < minx Then minx = pt.X
            If pt.X > maxx Then maxx = pt.X
            If pt.Y < miny Then miny = pt.Y
            If pt.Y > maxy Then maxy = pt.Y
        Next
        maxx -= minx : maxy -= miny
        AABB = New Rectangle(minx, miny, maxx, maxy)
        center = New Point(minx + maxx / 2, miny + maxy / 2)
    End Sub
    Private Function isLeft(ByVal p0 As Point, ByVal p1 As Point, ByVal p2 As Point) As Double
        Return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y)
    End Function
    Private Function above(ByVal p0 As Point, ByVal p1 As Point, ByVal p2 As Point) As Boolean
        Return (isLeft(p0, p1, p2) > 0)
    End Function
    Private Function below(ByVal p0 As Point, ByVal p1 As Point, ByVal p2 As Point) As Boolean
        Return (isLeft(p0, p1, p2) < 0)
    End Function
    Public Sub getTangent(ByVal p As Point, ByRef l As Point, ByRef r As Point)
        Dim eprev As Double, enext As Double ' aa(i) previous and next edge turn direction
        Dim Rtan As Short = 0, Ltan As Short = 0 ' initially assume aa(0) = both tangents
        eprev = isLeft(points(0), points(1), p)
        For i As Short = 1 To points.Count - 2
            enext = isLeft(points(i), points(i + 1), p)
            If eprev <= 0 And enext > 0 Then
                If Not below(p, points(i), points(Rtan)) Then
                    Rtan = i
                End If
            ElseIf eprev > 0 And enext <= 0 Then
                If Not above(p, points(i), points(Ltan)) Then Ltan = i
            End If
            eprev = enext
        Next
        l = points(Ltan) : r = points(Rtan)
    End Sub
    Public Sub Fill(ByVal g As Graphics, ByVal b As Brush, Optional ByVal d As Boolean = True)
        If d Then g.DrawPolygon(Pens.Black, points.ToArray)
        g.FillPolygon(b, points.ToArray)
    End Sub
    Public Sub Draw(ByVal g As Graphics, ByVal p As Pen, Optional ByVal nodes As Boolean = False)
        If points.Count = 2 Then
            g.DrawLine(p, points(0), points(1))
        ElseIf points.Count > 2 Then
            g.DrawPolygon(p, points.ToArray)
        End If
        If nodes Then
            For Each pt As Point In points
                g.DrawEllipse(Pens.Green, pt.X - 5, pt.Y - 5, 10, 10)
            Next
        End If
    End Sub
    Private Sub calc()
        Dim minx As Integer = points(0).X, miny As Integer = points(0).Y, maxx As Integer = minx, maxy As Integer = miny
        For Each pt As Point In points
            If pt.X < minx Then minx = pt.X
            If pt.X > maxx Then maxx = pt.X
            If pt.Y < miny Then miny = pt.Y
            If pt.Y > maxy Then maxy = pt.Y
        Next
        maxx -= minx : maxy -= miny
        AABB = New Rectangle(minx, miny, maxx, maxy)
        center = New Point(minx + maxx / 2, miny + maxy / 2)
    End Sub
    Public Sub Expand(ByVal v As Integer, ByRef lst As List(Of MeshPoint))
        For i As Integer = 0 To points.Count - 2
            'Dim l As Double = LineDist(center, points(i)) + v
            'Dim a As Double = Angle(points(i), center)
            Dim px As Integer = points(i).X - center.X, py As Integer = points(i).Y - center.Y
            If px < 0 Then px -= v Else px += v
            If py < 0 Then py -= v Else py += v
            Dim p As Point = New Point(px + center.X, py + center.Y) 'ldir(center, a, l)
            If InPolys(p) < 0 Then lst.Add(New MeshPoint(p, id, v))
        Next
    End Sub
    Public Function FindClosestNodeAndDistance(ByVal x As Integer, ByVal y As Integer, ByRef inode As Integer) As Double
        Dim closestDistanceSq As Double = LineDistSq(points(0).X, points(0).Y, x, y)
        Dim closestIndex As Integer = 0
        For i As Integer = 1 To points.Count - 2
            Dim ptSegDistSq As Double = LineDistSq(points(i).X, points(i).Y, x, y)
            If ptSegDistSq < closestDistanceSq Then
                closestDistanceSq = ptSegDistSq
                closestIndex = i
            End If
        Next
        inode = closestIndex
        Return Math.Sqrt(closestDistanceSq)
    End Function
    Public Function FindNodeClosest(ByVal x As Integer, ByVal y As Integer, Optional ByVal delta As Integer = 0, Optional ByVal fInd As Boolean = False) As Integer
        Dim closestDistanceSq As Double = Double.MaxValue
        Dim closestIndex As Integer = -1
        Dim lInd As Integer
        If fInd Then lInd = points.Count - 1 Else lInd = points.Count - 2
        For i As Integer = 0 To lInd
            Dim ptSegDistSq As Double = LineDistSq(points(i).X, points(i).Y, x, y)
            If ptSegDistSq < closestDistanceSq Then
                closestDistanceSq = ptSegDistSq
                closestIndex = i
            End If
        Next
        If closestDistanceSq > (delta * delta) Then closestIndex = -1
        Return closestIndex
    End Function
    Public ReadOnly Property Count As Integer
        Get
            Return points.Count
        End Get
    End Property
    Public Sub Save(ByVal file As Integer)
        FilePut(file, CShort(points.Count))
        For Each p As Point In points
            FilePut(file, CInt(p.X))
            FilePut(file, CInt(p.Y))
        Next
    End Sub
    Public Function Contains(ByVal p As Point) As Boolean
        Return Contains(p.X, p.Y)
    End Function
    Public Function Contains(ByVal x As Integer, ByVal y As Integer) As Boolean
        Dim pointIBefore As New Point
        If points.Count > 2 Then pointIBefore = points(points.Count - 2)
        Dim crossings As Integer = 0
        For i As Integer = 0 To points.Count - 2
            Dim pointI As Point = points(i)
            If (((pointIBefore.Y <= y And y < pointI.Y) Or (pointI.Y <= y And y < pointIBefore.Y)) And x < ((pointI.X - pointIBefore.X) / (pointI.Y - pointIBefore.Y) * (y - pointIBefore.Y) + pointIBefore.X)) Then
                crossings += 1
            End If
            pointIBefore = pointI
        Next
        Return (crossings Mod 2 <> 0)
    End Function
    Public Sub Finish()
        points.Add(New Point(points(0)))
        calc()
    End Sub
    Public Sub AddPt(ByVal p As Point)
        points.Add(New Point(p)) : calc()
    End Sub
    Public Sub AddPt(ByVal x As Integer, ByVal y As Integer)
        points.Add(New Point(x, y)) : calc()
    End Sub
    Public Sub MovePt(ByVal i As Integer, ByVal x As Integer, ByVal y As Integer, Optional ByVal dif As Boolean = False)
        If (i = 0 Or i = points.Count - 1) And dif = False Then
            Dim l As Integer = points.Count - 1
            points(0) = New Point(points(0).X + x, points(0).Y + y)
            points(l) = New Point(points(l).X + x, points(l).Y + y)
        Else
            points(i) = New Point(points(i).X + x, points(i).Y + y)
        End If
        calc()
    End Sub
    Public Sub MoveRel(ByVal x As Integer, ByVal y As Integer)
        For i As Integer = 0 To points.Count - 1
            points(i) = New Point(points(i).X + x, points(i).Y + y)
        Next
        calc()
    End Sub
    Public Function GetBoundaryPtClosest(ByVal x As Integer, ByVal y As Integer) As Point
        Dim closestDistanceSq As Double = Double.MaxValue
        Dim closestIndex As Integer = -1
        Dim closestNextIndex As Integer = -1
        Dim pNext As Point, p As Point
        Dim nextI As Integer
        For i As Integer = 0 To points.Count - 1
            nextI = (i + 1) Mod points.Count
            p = points(i)
            pNext = points(nextI)
            Dim ptSegDistSq As Double = ptLineDistSq(p.X, p.Y, pNext.X, pNext.Y, x, y)
            If ptSegDistSq < closestDistanceSq Then
                closestDistanceSq = ptSegDistSq
                closestIndex = i
                closestNextIndex = nextI
            End If
        Next
        p = points(closestIndex)
        pNext = points(closestNextIndex)
        Return getClosestPtOnLine(p.X, p.Y, pNext.X, pNext.Y, x, y)
    End Function
    Public Function LineIntersects(ByVal p1 As Point, ByVal p2 As Point) As Boolean
        Return LineIntersects(p1.X, p1.Y, p2.X, p2.Y)
    End Function
    Public Function LineIntersects(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Boolean
        If x1 = x2 AndAlso y1 = y2 Then Return False
        Dim ax As Double = x2 - x1, ay As Double = y2 - y1
        Dim pointIBefore As Point = points(points.Count - 1)
        For Each pointI As Point In points
            Dim x3 As Integer = pointIBefore.X, y3 As Integer = pointIBefore.Y, x4 As Integer = pointI.X
            Dim y4 As Integer = pointI.Y, bx As Integer = x3 - x4, by As Integer = y3 - y4, cx As Integer = x1 - x3
            Dim cy As Integer = y1 - y3
            Dim alphaNumerator As Double = by * cx - bx * cy, commonDenominator As Double = ay * bx - ax * by
            If commonDenominator > 0 Then
                If alphaNumerator < 0 OrElse alphaNumerator > commonDenominator Then
                    pointIBefore = pointI
                    Continue For
                End If
            ElseIf commonDenominator < 0 Then
                If alphaNumerator > 0 OrElse alphaNumerator < commonDenominator Then
                    pointIBefore = pointI
                    Continue For
                End If
            End If
            Dim betaNumerator As Double = ax * cy - ay * cx
            If commonDenominator > 0 Then
                If betaNumerator < 0 OrElse betaNumerator > commonDenominator Then
                    pointIBefore = pointI
                    Continue For
                End If
            ElseIf commonDenominator < 0 Then
                If betaNumerator > 0 OrElse betaNumerator < commonDenominator Then
                    pointIBefore = pointI
                    Continue For
                End If
            End If
            If commonDenominator = 0 Then
                ' This code wasn't in Franklin Antonio's method. It was added by Keith Woodward.
                ' The lines are parallel.
                ' Check if they're collinear.
                Dim collinearityTestForP3 As Double = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2) ' see http://mathworld.wolfram.com/Collinear.html
                ' If p3 is collinear with p1 and p2 then p4 will also be collinear, since p1-p2 is parallel with p3-p4
                If collinearityTestForP3 = 0 Then
                    ' The lines are collinear. Now check if they overlap.
                    If x1 >= x3 AndAlso x1 <= x4 OrElse x1 <= x3 AndAlso x1 >= x4 OrElse _
                            x2 >= x3 AndAlso x2 <= x4 OrElse x2 <= x3 AndAlso x2 >= x4 OrElse _
                            x3 >= x1 AndAlso x3 <= x2 OrElse x3 <= x1 AndAlso x3 >= x2 Then
                        If y1 >= y3 AndAlso y1 <= y4 OrElse y1 <= y3 AndAlso y1 >= y4 OrElse _
                                y2 >= y3 AndAlso y2 <= y4 OrElse y2 <= y3 AndAlso y2 >= y4 OrElse _
                                y3 >= y1 AndAlso y3 <= y2 OrElse y3 <= y1 AndAlso y3 >= y2 Then _
                            Return True
                    End If
                End If
                pointIBefore = pointI
                Continue For
            End If
            Return True
        Next
        Return False
    End Function
    Public Function IntersectPerimeter(foreign As KPoly) As Boolean
        Dim pointIBefore As Point = points(points.Count - 1) '(points.size() != 0 ? points.get(points.size()-1) : null);
        Dim pointJBefore As Point = foreign.points(foreign.points.Count - 1) '(foreign.points.size() != 0 ? foreign.points.get(foreign.points.size()-1) : null);
        For Each pointI As Point In points
            For Each pointJ As Point In foreign.points
                ' The below linesIntersect could be sped up slightly since many things are recalc'ed over and over again.
                If linesIntersect(pointI, pointIBefore, pointJ, pointJBefore) Then Return True
                pointJBefore = pointJ
            Next
            pointIBefore = pointI
        Next
        Return False
    End Function
    Public Function Intersects(foreign As KPoly) As Boolean
        If IntersectPerimeter(foreign) Then Return True
        If Contains(foreign.points(0)) OrElse foreign.Contains(points(0)) Then Return True
        Return False
    End Function
    Public Function Intersects(ByVal r As Rectangle) As Boolean
        Dim pointIBefore As Point = points(points.Count - 1) '(points.size() != 0 ? points.get(points.size()-1) : null);
        Dim foreign As Point() = {New Point(r.X, r.Y), New Point(r.X, r.Y + r.Height), New Point(r.X + r.Width, r.Y + r.Height), New Point(r.X + r.Width, r.Y)}
        Dim pointJBefore As Point = foreign(3) '(foreign.points.size() != 0 ? foreign.points.get(foreign.points.size()-1) : null);
        For Each pointI As Point In points
            For i As Integer = 0 To 3
                ' The below linesIntersect could be sped up slightly since many things are recalc'ed over and over again.
                If linesIntersect(pointI, pointIBefore, foreign(i), pointJBefore) Then Return True
                pointJBefore = foreign(i)
            Next
            pointIBefore = pointI
        Next
        Return False
    End Function
End Class

