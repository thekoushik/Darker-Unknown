Public Class Anim
    Private alarm As Integer
    Private x As Integer, y As Integer, c As Short
    Public ReadOnly Property Active As Boolean
        Get
            If alarm > 0 Then Return True Else Return False
        End Get
    End Property
    Public Sub Start(ByVal t As Integer, ByVal xx As Integer, ByVal yy As Integer)
        alarm = t : x = xx : y = yy : c = 1
    End Sub
    Public Sub Update()
        If alarm > 0 Then alarm -= 1 : c += 2
    End Sub
    Public Sub Draw(ByVal g As Graphics)
        If alarm > 0 Then
            Dim c2 As Short = c + c
            g.FillEllipse(Brushes.Gold, x - c, y - c, c2, c2)
        End If
    End Sub
End Class
