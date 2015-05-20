Public Class CheckPoint
    Public x As Integer
    Public y As Integer
    Public i As Integer
    Public cTimer As Short
    'Public poly As KPoly
    'Public co As Color
    Public Sub New(ByVal ind As Integer)
        i = ind : x = mesh(i).x : y = mesh(i).y
    End Sub
    'Public Sub DrawFill(ByVal g As Graphics)
    '
    'End Sub
End Class
