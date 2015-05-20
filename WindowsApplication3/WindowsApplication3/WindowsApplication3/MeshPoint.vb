Public Class MeshPoint
    Public x As Integer
    Public y As Integer
    Public nearestPolygon As Integer = -1
    'Public visibleCheckpt As Integer = -1
    Public distanceFromPolygon As Double = 0
    'Public visibleNgbrs As List(Of Integer)
    'Public distanceOfVisibleNgbrs As List(Of Double)
    Public visibleNgbrs As SortedList(Of Double, Integer)
    Public Sub New(ByVal xx As Integer, ByVal yy As Integer)
        x = xx : y = yy
        visibleNgbrs = New SortedList(Of Double, Integer)
    End Sub
    Public Sub New(ByVal p As Point, ByVal np As Integer)
        x = p.X : y = p.Y : nearestPolygon = np
        visibleNgbrs = New SortedList(Of Double, Integer)
    End Sub
    Public Sub New(ByVal p As Point, ByVal np As Integer, ByVal d As Double)
        x = p.X : y = p.Y : nearestPolygon = np : distanceFromPolygon = d
        visibleNgbrs = New SortedList(Of Double, Integer)
    End Sub
    Public Function AddNgbr(ByVal i As Integer, ByVal d As Double) As Boolean
        If visibleNgbrs.ContainsValue(i) Then Return False
        While visibleNgbrs.ContainsKey(d)
            d += 0.000000001
        End While
        visibleNgbrs.Add(d, i)
        Return True
    End Function
    Public ReadOnly Property Location() As Point
        Get
            Return New Point(x, y)
        End Get
    End Property
End Class
