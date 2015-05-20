Public Class AI
    Public Enum NPCOBJ
        Dummy = 0
        ReachLocation = 1
        ReachNPC = 2
        ReachDOC = 3
        AvoidNPC = 4
        StayHidden = 5
    End Enum
    Public Enum NPState
        Stay = 0
        SearchNormal = 1 'don't know
        SearchAlert = 2 'player must be here
        Chase = 3
        Investigate = 4 'player gone invisible
        PlanSearch = 5 'think where can he go and where should i go and do that
        SpecialPathfindForDebug = 6

        UnknownExplorePhase1 = 7
        UnknownExplorePhase1Move = 8
        UnknownExploreMovePath = 9
        UnknownExplorePhase2 = 10
        UnknownExplorePhase2Move = 11

        Avoid = 12
        Flee = 13

        VisiblePathMove = 14
    End Enum
    Public Const ChaseSpeed As Short = 5
    Public Speed As Short = 4
    Public State As NPState
    Public clever As Short
    Public LocEstimate As Boolean = True
    Public DoingLocEst As Boolean = False
    Public memory As Integer = -1
    Public lastLocations As List(Of Point)
    Public Sub New(ByVal c As Integer)
        State = NPState.Stay
        clever = c
        lastLocations = New List(Of Point)
    End Sub
End Class
