Public Class Node(Of T As IHasRect)
    Public bound As Rectangle
    Private contents As New List(Of T)
    Public nodes As New List(Of Node(Of T))(4)
    Public Sub New(ByVal b As Rectangle)
        bound = b
    End Sub
    Public ReadOnly Property Count As Integer
        Get
            Dim cnt As Integer = 0
            For Each n As Node(Of T) In nodes
                cnt += n.Count
            Next
            cnt += contents.Count
            Return cnt
        End Get
    End Property
    
    Public ReadOnly Property SubTreeContents As List(Of T)
        Get
            Dim results As New List(Of T)
            For Each n As Node(Of T) In nodes
                results.AddRange(n.SubTreeContents)
            Next
            results.AddRange(contents)
            Return results
        End Get
    End Property
    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return IIf(bound.IsEmpty OrElse nodes.Count = 0, True, False)
        End Get
    End Property
    Public Function Query(ByVal queryArea As Rectangle) As List(Of T)
        Dim results As New List(Of T)
        ' this quad contains items that are not entirely contained by
        ' it's four sub-quads. Iterate through the items in this quad 
        ' to see if they intersect.
        For Each item As T In contents
            If queryArea.IntersectsWith(item.Rect) Then results.Add(item)
        Next
        For Each node As Node(Of T) In nodes
            If node.IsEmpty Then Continue For

            ' Case 1: search area completely contained by sub-quad
            ' if a node completely contains the query area, go down that branch
            ' and skip the remaining nodes (break this loop)
            If node.bound.Contains(queryArea) Then
                results.AddRange(node.Query(queryArea))
                Exit For
            End If

            ' Case 2: Sub-quad completely contained by search area 
            ' if the query area completely contains a sub-quad,
            ' just add all the contents of that quad and it's children 
            ' to the result set. You need to continue the loop to test 
            ' the other quads
            If queryArea.Contains(node.bound) Then
                results.AddRange(node.SubTreeContents)
                Continue For
            End If

            ' Case 3: search area intersects with sub-quad
            ' traverse into this quad, continue the loop to search other
            ' quads
            If node.bound.IntersectsWith(queryArea) Then results.AddRange(node.Query(queryArea))
        Next

        Return results
    End Function
    Public Sub Insert(ByVal item As T)
        ' if the item is not contained in this quad, there's a problem
        If Not bound.Contains(item.Rect) Then
            'Trace.TraceWarning("feature is out of the bounds of this quadtree node");
            Return
        End If

        ' if the subnodes are null create them. may not be sucessfull: see below
        ' we may be at the smallest allowed size in which case the subnodes will not be created
        If nodes.Count = 0 Then CreateSubNodes()

        ' for each subnode:
        ' if the node contains the item, add the item to that node and return
        ' this recurses into the node that is just large enough to fit this item
        For Each node As Node(Of T) In nodes
            If node.bound.Contains(item.Rect) Then
                node.Insert(item)
                Return
            End If
        Next

        ' if we make it to here, either
        ' 1) none of the subnodes completely contained the item. or
        ' 2) we're at the smallest subnode size allowed 
        ' add the item to this node's contents.
        contents.Add(item)
    End Sub
    Public Sub ForEach(ByVal action As QuadTree(Of T).QTAction)
        action(Me)
        ' draw the child quads
        For Each node As Node(Of T) In nodes
            node.ForEach(action)
        Next
    End Sub
    Private Sub CreateSubNodes()
        ' the smallest subnode has an area 
        If (bound.Height * bound.Width) <= 10 Then Return
        Dim halfWidth As Single = (bound.Width / 2.0F)
        Dim halfHeight As Single = (bound.Height / 2.0F)
        nodes.Add(New Node(Of T)(New Rectangle(bound.Location, New Size(halfWidth, halfHeight))))
        nodes.Add(New Node(Of T)(New Rectangle(New Point(bound.Left, bound.Top + halfHeight), New Size(halfWidth, halfHeight))))
        nodes.Add(New Node(Of T)(New Rectangle(New Point(bound.Left + halfWidth, bound.Top), New Size(halfWidth, halfHeight))))
        nodes.Add(New Node(Of T)(New Rectangle(New Point(bound.Left + halfWidth, bound.Top + halfHeight), New Size(halfWidth, halfHeight))))
    End Sub
End Class
