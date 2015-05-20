Module MyMaths
    Public gameOver As Boolean = False
    Public polys As New List(Of KPoly)
    Public qtreeFixed As QuadTree(Of KPoly)
    Public mesh As New List(Of MeshPoint)
    Public cMesh As Integer = 0
    Public worldWidth As Integer, worldHeight As Integer
    Public npcs As List(Of NPC)
    Public DOCS As New List(Of Short)
    Public cDOCs As Short = 0
    Public pDOCs As Short = 0
    Public NavMesh As Matrix

    Public showMeshpt As Boolean = False
    Public shadowMode As Boolean = True
    Public expertMode As Boolean = False

    Public Sub GenNavMesh()
        mesh = New List(Of MeshPoint)
        mesh.Add(New MeshPoint(15, 15)) : ClosestPoly(mesh(0))
        mesh.Add(New MeshPoint(worldWidth - 15, 15)) : ClosestPoly(mesh(1))
        For i As Short = 0 To polys.Count - 1
            polys(i).Expand(5, mesh)
        Next
        mesh.Add(New MeshPoint(15, worldHeight - 15)) : ClosestPoly(mesh(mesh.Count - 1))
        mesh.Add(New MeshPoint(worldWidth - 15, worldHeight - 15)) : ClosestPoly(mesh(mesh.Count - 1))
        Dim l As Integer = mesh.Count - 1
        For i As Integer = 0 To l
            Dim px As Integer = mesh(i).x, py As Integer = mesh(i).y
            For j As Integer = 0 To l
                If i <> j Then
                    Dim qx As Integer = mesh(j).x, qy As Integer = mesh(j).y
                    If LineOfSight(px, py, qx, qy) Then mesh(i).AddNgbr(j, LineDist(px, py, qx, qy))
                End If
            Next
        Next
        cMesh = mesh.Count
        For i As Short = 1 To npcs.Count - 1
            npcs(i).InitMyMesh(cMesh)
        Next
        NavMesh = New Matrix
        NavMesh.MakeNavMesh()
    End Sub
    Public Function GetNgbrs(ByVal x As Integer, ByVal y As Integer) As SortedList(Of Double, Integer)
        Dim lst As New SortedList(Of Double, Integer)
        For i As Short = 0 To cMesh - 1
            Dim xx As Integer = mesh(i).x, yy As Integer = mesh(i).y
            If xx = x AndAlso yy = y Then
                'lst.Add(0, i)
            Else
                If LineOfSight(xx, yy, x, y) Then lst.Add(LineDist(xx, yy, x, y), i)
            End If
        Next
        Return lst
    End Function
    Public Function GetNearestNPC(ByVal xx As Integer, ByVal yy As Integer, ByVal dis As Double) As Short
        If npcs.Count < 2 Then Return -1
        Dim sqdis As Double = dis * dis
        Dim sd As Double = LineDistSq(npcs(1).x, npcs(1).y, xx, yy), si As Short = 1
        For i As Short = 2 To npcs.Count - 1
            Dim d As Double = LineDistSq(npcs(i).x, npcs(i).y, xx, yy)
            If d < sd Then sd = d : si = i
        Next
        If dis = -1 Then
            Return si
        ElseIf sd <= sqdis Then
            Return si
        Else
            Return -1
        End If
    End Function
    Public Function GetNearestInvisibleMeshpt(ByVal id As Integer, ByRef target As Integer, ByRef mylist As List(Of Integer)) As List(Of Point)
        'list visible mesh points with invisible ngbr
        Dim vlst As New List(Of Integer), ilst As New List(Of Integer)
        Dim visited As New List(Of Integer)
        For i As Short = 0 To mylist.Count - 1
            If mylist(i) = -1 Then ilst.Add(i) Else vlst.Add(i)
        Next
        'now we have visible list and invisible list
        Dim si As Short = ilst(0)
        Dim sp As List(Of Point) = NavMesh.FindPath3(mesh(id).Location, mesh(ilst(0)).Location, ilst, True)
        Dim sd As Double = NavMesh.lastPathLength
        For i As Short = 1 To ilst.Count - 1
            Dim cp As List(Of Point) = NavMesh.FindPath3(mesh(id).Location, mesh(ilst(i)).Location, ilst, True)
            If cp Is Nothing Then Continue For
            If sd = -1 OrElse (NavMesh.lastPathLength > 0 AndAlso NavMesh.lastPathLength < sd) Then sp = cp : sd = NavMesh.lastPathLength : si = ilst(i)
        Next
        If Not sp Is Nothing Then target = si : Return sp
        Debug.WriteLine(id & " - No possible path")
        target = -1 : Return Nothing
    End Function
    Public Function GetNearestMeshptInvisibleFromNearestNgbr(ByVal id As Integer, ByRef target As Integer, ByRef mylist As List(Of Integer)) As List(Of Point)
        'list visible mesh points with invisible ngbr
        Dim vlst As New List(Of Integer), ilst As New List(Of Integer)
        Dim visited As New List(Of Integer)
        For i As Short = 0 To mylist.Count - 1
            If mylist(i) = -1 Then
                ilst.Add(i)
                For j As Short = 0 To mesh(i).visibleNgbrs.Count - 1 'i is invisible
                    Dim k As Short = mesh(i).visibleNgbrs.Values(j)
                    If mylist(k) >= 0 Then 'we have k visible
                        If Not vlst.Contains(k) Then vlst.Add(k)
                    End If
                Next
            End If
        Next
        'now we have visible list and invisible list
        Dim si As Short = 0
        Dim sp As List(Of Point) = NavMesh.FindPath3(mesh(id).Location, mesh(vlst(0)).Location, , True)
        Dim sd As Double = NavMesh.lastPathLength
        For i As Short = 1 To vlst.Count - 1
            Dim cp As List(Of Point) = NavMesh.FindPath3(mesh(id).Location, mesh(vlst(i)).Location, , True)
            If cp Is Nothing Then Continue For
            If sp Is Nothing OrElse NavMesh.lastPathLength < sd Then sp = cp : sd = NavMesh.lastPathLength : si = i
        Next
        If Not sp Is Nothing Then target = si : Return sp
        Return Nothing
    End Function
    Public Function GetNearestMesptInvisibleFromNgbr(ByVal id As Integer, ByRef mylist As List(Of Integer), Optional ByRef ingbr As Integer = -1) As Integer
        If mylist(id) = -1 Then Return id
        For i As Short = 0 To mesh(id).visibleNgbrs.Count - 1
            Dim iid As Short = mesh(id).visibleNgbrs.Values(i)
            If mylist(iid) = -1 Then Return iid
            If iid <> id Then
                For j As Short = 0 To mesh(iid).visibleNgbrs.Count - 1
                    Dim iiid As Short = mesh(iid).visibleNgbrs.Values(j)
                    If iiid <> id Then
                        If mylist(iiid) = -1 Then ingbr = iiid : Return iid
                    End If
                Next
            End If
        Next
        Return -1
    End Function
    Public Function GetNearestMeshpt(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim l As SortedList(Of Double, Integer) = GetNgbrs(x, y)
        If l.Count = 0 Then Return -1 Else Return l.Values(0)
    End Function
    Public Function GetMeshID(ByVal x As Short, ByVal y As Short) As Short
        For i As Short = 0 To cMesh - 1
            If mesh(i).x = x AndAlso mesh(i).y = y Then Return i
        Next
        Return -1
    End Function
    Public Function GetNearestMeshptInvisible(ByVal id As Integer, ByRef mylist As List(Of Integer)) As Integer
        'Dim ii As Short = -1, d As Double
        'For i As Short = 0 To mesh(id).visibleNgbrs.Count - 1
        'Dim v As Integer = mesh(id).visibleNgbrs.Values(i)
        'If mylist(v) = -1 Then
        'If ii = -1 Then
        'ii = v : d = mesh(mesh(id).visibleNgbrs.Values(i)).distanceFromPolygon
        'Else
        'Dim dd As Double = mesh(mesh(id).visibleNgbrs.Values(i)).distanceFromPolygon
        'If dd < d Then ii = v : d = dd
        'End If
        'End If
        'Next
        'Return IIf(ii < 0, -1, ii)
        If mylist(id) = -1 Then Return id
        For i As Short = 0 To mesh(id).visibleNgbrs.Count - 1
            Dim v As Integer = mesh(id).visibleNgbrs.Values(i)
            If mylist(v) = -1 Then Return v
        Next
        Return -1
    End Function
    Public Function GetNearestMeshptInvisible(ByVal x As Integer, ByVal y As Integer, ByRef mylist As List(Of Integer)) As Integer
        Dim l As SortedList(Of Double, Integer) = GetNgbrs(x, y)
        For i As Short = 0 To l.Count - 1
            If mylist(l.Values(i)) = -1 Then Return l.Values(i)
        Next
        Return -1
    End Function
    Public Sub RegisterQuadF()
        qtreeFixed = New QuadTree(Of KPoly)(New Rectangle(0, 0, worldWidth, worldHeight))
        For i As Short = 0 To polys.Count - 1
            polys(i).id = i
            qtreeFixed.Insert(polys(i))
        Next
    End Sub
    Public Sub ClosestPoly(ByRef m As MeshPoint)
        If polys.Count = 0 Then Exit Sub
        Dim pClosest As Short = -1
        Dim dClosest As Double = polys(0).FindClosestNodeAndDistance(m.x, m.y, pClosest)
        Dim iClosest As Integer = 0
        For i As Integer = 1 To polys.Count - 1
            Dim pc As Short
            Dim dc As Double = polys(i).FindClosestNodeAndDistance(m.x, m.y, pc)
            If dc < dClosest Then dClosest = dc : pClosest = pc : iClosest = i
        Next
        m.nearestPolygon = iClosest
        m.distanceFromPolygon = dClosest
    End Sub
    Public Function InPolys(ByVal p As Point) As Integer
        If polys.Count = 0 Then Return -1
        For i As Integer = 0 To polys.Count - 1
            If polys(i).Contains(p.X, p.Y) Then Return i
        Next
        Return -1
    End Function
    Public Function HitToWorldRayCast(ByVal cx As Double, ByVal cy As Double, ByVal ang As Double, ByRef dis As Double) As Point
        Dim dx As Double = cx + Math.Cos(ang * d2r) * 1000, dy As Double = cy + Math.Sin(ang * d2r) * 1000
        Dim closestIntersect As Point = Nothing, closestIntersectParam As Double
        Dim minx As Integer = Math.Min(cx, dx), miny As Integer = Math.Min(cy, dy)
        Dim maxx As Integer = Math.Max(cx, dx), maxy As Integer = Math.Max(cy, dy)
        Dim lst As List(Of KPoly) = qtreeFixed.Query(New Rectangle(minx, miny, maxx - minx, maxy - miny))
        For Each p As KPoly In lst
            For i As Short = 0 To p.Count - 2
                Dim px As Integer = p.points(i + 1).X, py As Integer = p.points(i + 1).Y
                Dim param As Double
                Dim pt As Point = RayCast(cx, cy, dx, dy, px, py, p.points(i).X, p.points(i).Y, param)
                If pt <> Nothing Then
                    If closestIntersect = Nothing OrElse param < closestIntersectParam Then
                        closestIntersect = pt : closestIntersectParam = param
                    End If
                End If
            Next
        Next
        dis = closestIntersectParam
        Return closestIntersect
    End Function
    '// Find intersection of RAY & SEGMENT
    Public Function RayCast(ByVal r_px As Double, ByVal r_py As Double, ByVal r_bx As Double, ByVal r_by As Double, _
                            ByVal s_px As Double, ByVal s_py As Double, ByVal s_bx As Double, ByVal s_by As Double, ByRef param As Double) As Point 'function getIntersection(ray,segment){
        ' RAY in parametric: Point + Direction*T1
        Dim r_dx As Double = r_bx - r_px
        Dim r_dy As Double = r_by - r_py

        ' SEGMENT in parametric: Point + Direction*T2
        Dim s_dx As Double = s_bx - s_px
        Dim s_dy As Double = s_by - s_py

        ' Are they parallel? If so, no intersect
        Dim r_mag As Double = Math.Sqrt(r_dx * r_dx + r_dy * r_dy)
        Dim s_mag As Double = Math.Sqrt(s_dx * s_dx + s_dy * s_dy)
        If r_dx / r_mag = s_dx / s_mag AndAlso r_dy / r_mag = s_dy / s_mag Then Return Nothing ' Directions are the same.

        ' SOLVE FOR T1 & T2
        ' r_px+r_dx*T1 = s_px+s_dx*T2 && r_py+r_dy*T1 = s_py+s_dy*T2
        ' ==> T1 = (s_px+s_dx*T2-r_px)/r_dx = (s_py+s_dy*T2-r_py)/r_dy
        ' ==> s_px*r_dy + s_dx*T2*r_dy - r_px*r_dy = s_py*r_dx + s_dy*T2*r_dx - r_py*r_dx
        ' ==> T2 = (r_dx*(s_py-r_py) + r_dy*(r_px-s_px))/(s_dx*r_dy - s_dy*r_dx)
        Dim T2 As Double = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx)
        Dim T1 As Double = (s_px + s_dx * T2 - r_px) / r_dx

        ' Must be within parametic whatevers for RAY/SEGMENT
        If T1 < 0 Then Return Nothing
        If T2 < 0 OrElse T2 > 1 Then Return Nothing

        ' Return the POINT OF INTERSECTION
        param = T1
        Return New Point(r_px + r_dx * T1, r_py + r_dy * T1)
    End Function
    Public Sub Force(ByVal speed As Double, ByVal a As Double, ByRef x As Double, ByRef y As Double)
        x = Math.Cos(a * d2r) * speed
        y = Math.Sin(a * d2r) * speed
    End Sub
    Public Function RectFree(ByVal r As Rectangle) As Boolean
        Dim lst As List(Of KPoly) = qtreeFixed.Query(r)
        For Each p As KPoly In lst
            If p.Intersects(r) Then Return False
        Next
        Return True
    End Function
    Public Function PointFree(ByVal xx As Integer, ByVal yy As Integer) As Boolean
        Dim lst As List(Of KPoly) = qtreeFixed.Query(New Rectangle(xx - 1, yy - 1, 3, 3))
        For Each p As KPoly In lst
            If p.Contains(xx, yy) Then Return False
        Next
        Return True
    End Function
    Public Function MoveOutside(ByRef xx As Integer, ByRef yy As Integer, Optional ByVal rng As Integer = 1) As Boolean ', ByVal wid As Integer, ByVal hgt As Integer) As Point
        Dim r2 As Integer = rng + rng
        Dim lst As List(Of KPoly) = qtreeFixed.Query(New Rectangle(xx - rng, yy - rng, r2, r2))
        Dim ba As Boolean = False
        For Each p As KPoly In lst
            If p.Contains(xx, yy) Then
                Dim pt As Point = p.GetBoundaryPtClosest(xx, yy)
                'Dim ld As Double = LineDist(xx, yy, pt.X, pt.Y) + 4
                'pt.X -= xx : pt.Y -= yy 'subSelf(loc7, loc2)
                'Dim np As New Point(pt.X, pt.Y)  'loc8 = loc7
                'rescaleSelf(np, Math.Max(0, ld - length(pt)))
                'xx += np.X : yy += np.Y : ba = True
                xx = pt.X : yy = pt.Y : ba = True
            End If
        Next
        Return ba
    End Function
    Public Function RectFree(ByVal xx As Integer, ByVal yy As Integer, ByVal wid As Integer, ByVal hgt As Integer) As Boolean
        Return RectFree(New Rectangle(xx, yy, wid, hgt))
    End Function
    Public Function LineOfSight(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Boolean
        Dim minx As Integer = Math.Min(x1, x2), miny As Integer = Math.Min(y1, y2)
        Dim maxx As Integer = Math.Max(x1, x2), maxy As Integer = Math.Max(y1, y2)
        Dim lst As List(Of KPoly) = qtreeFixed.Query(New Rectangle(minx, miny, maxx - minx, maxy - miny))
        For Each p As KPoly In lst
            If p.LineIntersects(x1, y1, x2, y2) Then Return False
        Next
        Return True
    End Function
    Public Function LineOfSight(ByVal p1 As Point, ByVal p2 As Point) As Boolean
        Return LineOfSight(p1.X, p1.Y, p2.X, p2.Y)
    End Function
    Public Const d2r As Double = -Math.PI / 180
    Public Const r2d As Double = 180 / Math.PI
    Public Function Ang_Dif(ByVal ang1 As Double, ByVal ang2 As Double) As Double
        Return ((((ang1 - ang2) Mod 360) + 540) Mod 360) - 180
    End Function
    Public Function Angle(ByVal ptTo As Point, ByVal ptFrom As Point) As Double
        Return Angle(ptTo.X, ptTo.Y, ptFrom.X, ptFrom.Y)
    End Function
    Public Function Angle(ByVal ptToX As Double, ByVal ptToY As Double, ByVal ptFromX As Double, ByVal ptFromY As Double) As Double
        Return ((Math.Atan2(ptFromY - ptToY, ptToX - ptFromX) * r2d) + 360) Mod 360
    End Function
    Public Function ldir(ByVal p As Point, ByVal ang As Double, ByVal d As Double) As Point
        Return New Point(p.X + Math.Cos(ang * d2r) * d, p.Y + Math.Sin(ang * d2r) * d)
    End Function
    Public Function length(ByVal p As Point) As Double
        Return Math.Sqrt(p.X * p.X + p.Y * p.Y)
    End Function
    Private Sub rescaleSelf(ByRef p As Point, ByVal arg1 As Single)
        Try
            Dim loc1 As Double = arg1 / Math.Sqrt(p.X * p.X + p.Y * p.Y)
            p.X *= loc1 : p.Y *= loc1
        Catch e As ArithmeticException
        End Try
    End Sub
    Private Sub addSelf(ByRef p As Point, ByVal p2 As Point)
        p.X += p2.X : p.Y += p2.Y
    End Sub
    Private Function add(ByVal p As Point, ByVal p2 As Point) As Point
        Return New Point(p.X + p2.X, p.Y + p2.Y)
    End Function
    Private Function add(ByVal p As Point, ByVal p2 As Double) As Point
        Return New Point(p.X + p2, p.Y + p2)
    End Function
    Private Sub subSelf(ByRef p As Point, ByVal p2 As Point)
        p.X -= p2.X : p.Y -= p2.Y
    End Sub
    Public Function makeShadowPoly(ByVal lightx As Integer, ByVal lighty As Integer, ByVal lightRad As Integer, ByVal poly As KPoly) As KPoly
        Dim loc2 As New Point(lightx, lighty)
        Dim loc3 As Single = lightRad
        Dim loc6 As Point, loc7 As Point, loc8 As Point, loc9 As Point
        poly.getTangent(loc2, loc6, loc7)
        subSelf(loc6, loc2)
        subSelf(loc7, loc2)
        loc8 = loc7
        rescaleSelf(loc8, Math.Max(0, loc3 - length(loc7)))
        addSelf(loc8, loc7)
        loc9 = loc6
        rescaleSelf(loc9, Math.Max(0, loc3 - length(loc6)))
        addSelf(loc9, loc6)
        Dim list As New KPoly
        list.AddPt(add(loc6, lightRad)) 'start point
        list.AddPt(add(loc7, lightRad))
        Dim l1 As Short = Angle(loc8.X, loc8.Y, 0, 0)
        Dim l2 As Short = l1 - Ang_Dif(l1, Angle(loc9.X, loc9.Y, 0, 0))
        For ll As Short = l1 To l2 Step -1
            list.AddPt(add(ldir(New Point(0, 0), ll, loc3), lightRad))
        Next
        list.Finish()
        Return list
    End Function
    Public Function RotatePt(ByVal angle As Double, ByVal x As Integer, ByVal y As Integer, ByVal xCenter As Integer, ByVal yCenter As Integer) As Point
        Dim currentAngle As Double = Math.Atan2(y - yCenter, x - xCenter)
        currentAngle += angle
        Dim distance As Double = LineDist(x, y, xCenter, yCenter)
        Return New Point(xCenter + (distance * Math.Cos(currentAngle)), xCenter + (distance * Math.Sin(currentAngle)))
    End Function
    Public Function getClosestPtOnLine(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer, ByVal px As Integer, ByVal py As Integer) As Point
        Dim closestPoint As Point = New Point()
        Dim x2LessX1 As Double = x2 - x1, y2LessY1 As Double = y2 - y1
        Dim lNum As Double = x2LessX1 * x2LessX1 + y2LessY1 * y2LessY1
        Dim rNum As Double = ((px - x1) * x2LessX1 + (py - y1) * y2LessY1) / lNum
        If rNum <= 0 Then
            Return New Point(x1, y1)
        ElseIf rNum >= 1 Then
            Return New Point(x2, y2)
        Else
            Return New Point(x1 + rNum * x2LessX1, y1 + rNum * y2LessY1)
        End If
    End Function
    Public Function LineDist(ByVal p1 As Point, ByVal p2 As Point) As Double
        Return LineDist(p1.X, p1.Y, p2.X, p2.Y)
    End Function
    Public Function LineDist(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double) As Double
        Dim xx As Double = x2 - x1, yy As Double = y2 - y1
        If xx = 0 AndAlso yy = 0 Then Return 0
        Return Math.Sqrt(xx * xx + yy * yy)
    End Function
    Public Function LineDistSq(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Double
        Dim xx As Double = x2 - x1, yy As Double = y2 - y1
        Return xx * xx + yy * yy
    End Function
    Public Function linesIntersect(ByVal p1 As Point, ByVal p2 As Point, ByVal p3 As Point, ByVal p4 As Point) As Boolean
        Return linesIntersect(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, p4.X, p4.Y)
    End Function
    Public Function linesIntersect(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer, ByVal x3 As Integer, ByVal y3 As Integer, ByVal x4 As Integer, ByVal y4 As Integer) As Boolean
        ' Return false if either of the lines have zero length
        If (x1 = x2 And y1 = y2 Or x3 = x4 And y3 = y4) Then Return False
        ' Fastest method, based on Franklin Antonio's "Faster Line Segment Intersection" topic "in Graphics Gems III" book (http://www.graphicsgems.org/)
        Dim ax As Double = x2 - x1, ay As Double = y2 - y1, bx As Double = x3 - x4, by As Double = y3 - y4
        Dim cx As Double = x1 - x3, cy As Double = y1 - y3

        Dim alphaNumerator As Double = by * cx - bx * cy, commonDenominator As Double = ay * bx - ax * by
        If commonDenominator > 0 Then
            If alphaNumerator < 0 Or alphaNumerator > commonDenominator Then Return False
        ElseIf commonDenominator < 0 Then
            If alphaNumerator > 0 Or alphaNumerator < commonDenominator Then Return False
        End If
        Dim betaNumerator As Double = ax * cy - ay * cx
        If commonDenominator > 0 Then
            If betaNumerator < 0 Or betaNumerator > commonDenominator Then Return False
        ElseIf commonDenominator < 0 Then
            If betaNumerator > 0 Or betaNumerator < commonDenominator Then Return False
        End If
        ' if commonDenominator == 0 then the lines are parallel.
        If commonDenominator = 0 Then
            ' This code wasn't in Franklin Antonio's method. It was added by Keith Woodward.
            ' The lines are parallel.
            ' Check if they're collinear.
            Dim collinearityTestForP3 As Double = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2) ' see http://mathworld.wolfram.com/Collinear.html
            ' If p3 is collinear with p1 and p2 then p4 will also be collinear, since p1-p2 is parallel with p3-p4
            If collinearityTestForP3 = 0 Then
                ' The lines are collinear. Now check if they overlap.
                If x1 >= x3 And x1 <= x4 Or x1 <= x3 And x1 >= x4 Or _
                        x2 >= x3 And x2 <= x4 Or x2 <= x3 And x2 >= x4 Or _
                        x3 >= x1 And x3 <= x2 Or x3 <= x1 And x3 >= x2 Then
                    If y1 >= y3 And y1 <= y4 Or y1 <= y3 And y1 >= y4 Or _
                            y2 >= y3 And y2 <= y4 Or y2 <= y3 And y2 >= y4 Or _
                            y3 >= y1 And y3 <= y2 Or y3 <= y1 And y3 >= y2 Then Return True
                End If
            End If
            Return False
        End If
        Return True
    End Function
    Public Function ptLineDistSq(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer, ByVal px As Integer, ByVal py As Integer) As Double
        x2 -= x1 : y2 -= y1 : px -= x1 : py -= y1
        Dim dotprod As Double = px * x2 + py * y2, projlenSq As Double
        If dotprod < 0.0 Then
            projlenSq = 0.0
        Else
            px = x2 - px : py = y2 - py
            dotprod = px * x2 + py * y2
            If dotprod < 0.0 Then
                projlenSq = 0.0
            Else
                projlenSq = dotprod * dotprod / (x2 * x2 + y2 * y2)
            End If
        End If
        dotprod = px * px + py * py - projlenSq
        If dotprod < 0 Then
            Return 0
        Else
            Return dotprod
        End If
    End Function
End Module
