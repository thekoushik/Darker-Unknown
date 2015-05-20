Public Class QuadTree(Of T As IHasRect)
    Public root As Node(Of T)
    Private rect As Rectangle
    Public Delegate Sub QTAction(ByVal obj As Node(Of T))
    Public Sub New(ByVal r As Rectangle)
        rect = r
        root = New Node(Of T)(r)
    End Sub
    Public ReadOnly Property Count
        Get
            Return root.Count
        End Get
    End Property
    Public Sub Insert(ByVal item As T)
        root.Insert(item)
    End Sub
    Public Function Query(ByVal r As Rectangle) As List(Of T)
        Return root.Query(r)
    End Function
    Public Sub ForEach(ByVal action As QTAction)
        root.ForEach(action)
    End Sub
End Class
