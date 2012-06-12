Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Polls
    Public Class [Option]
        Inherits BasePoll

        ' ==========
        ' Private variables
        ' ==========
        Private _pollID As Integer = 0
        Private _poll As Poll
        Private _optionText As String = ""
        Private _votes As Integer = 0
        Private _percentage As Double = 0.0

        ' ==========
        ' Properties
        ' ==========
        Public Property PollID() As Integer
            Get
                Return _pollID
            End Get
            Private Set(ByVal value As Integer)
                _pollID = value
            End Set
        End Property

        Public ReadOnly Property Poll() As Poll
            Get
                If IsNothing(_poll) Then
                    _poll = BLL.Polls.Poll.GetPollByID(Me.ID)
                End If
                Return _poll
            End Get
        End Property

        Public Property OptionText() As String
            Get
                Return _optionText
            End Get
            Set(ByVal value As String)
                _optionText = value
            End Set
        End Property

        Public Property Votes() As Integer
            Get
                Return _votes
            End Get
            Private Set(ByVal value As Integer)
                _votes = value
            End Set
        End Property

        Public Property Percentage() As Double
            Get
                Return _percentage
            End Get
            Private Set(ByVal value As Double)
                _percentage = value
            End Set
        End Property

        ' ==========
        ' Methods
        ' ==========
        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal pollID As Integer, ByVal optionText As String, ByVal votes As Integer, _
                ByVal percentage As Double)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.PollID = pollID
            Me.OptionText = optionText
            Me.Votes = votes
            Me.Percentage = percentage
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = [Option].DeleteOption(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return [Option].UpdateOption(Me.ID, Me.OptionText)
        End Function

        ' ==========
        ' Shared methods
        ' ==========

        ' Returns a collection with all options for the specified poll
        Public Shared Function GetOptions(ByVal pollID As Integer) As List(Of [Option])
            Dim options As List(Of [Option])
            Dim key As String = "Polls_Options_" & CStr(pollID)

            If BasePoll.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                options = CType(BizObject.Cache(key), List(Of [Option]))
            Else
                Dim recordset As List(Of PollOptionDetails) = SiteProvider.Polls.GetOptions(pollID)
                options = GetOptionListFromPollOptionDetailsList(recordset)
                BasePoll.CacheData(key, options)
            End If
            Return options
        End Function

        ' Returns a Option object with the specified ID
        Public Shared Function GetOptionByID(ByVal optionID As Integer) As [Option]
            Dim _option As [Option]
            Dim key As String = "Polls_Option_" & CStr(optionID)

            If BasePoll.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                _option = CType(BizObject.Cache(key), [Option])
            Else
                _option = GetOptionFromPollOptionDetails(SiteProvider.Polls.GetOptionByID(optionID))
                BasePoll.CacheData(key, _option)
            End If
            Return _option
        End Function

        ' Updates an existing option
        Public Shared Function UpdateOption(ByVal id As Integer, ByVal optionText As String) As Boolean
            Dim record As New PollOptionDetails(id, DateTime.Now, "", 0, optionText, 0, 0.0)
            Dim ret As Boolean = SiteProvider.Polls.UpdateOption(record)
            BizObject.PurgeCacheItems("polls_option")
            Return ret
        End Function

        ' Deletes an existing option
        Public Shared Function DeleteOption(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Polls.DeleteOption(id)
            Dim ev As New RecordDeletedEvent("poll option", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("polls_option")
            Return ret
        End Function

        ' Inserts a new option
        Public Shared Function InsertOption(ByVal pollID As Integer, ByVal optionText As String) As Integer
            Dim record As New PollOptionDetails(0, DateTime.Now, BizObject.CurrentUserName, _
                pollID, optionText, 0, 0.0)
            Dim ret As Integer = SiteProvider.Polls.InsertOption(record)
            BizObject.PurgeCacheItems("polls_poll_" & CStr(pollID))
            BizObject.PurgeCacheItems("polls_option")
            Return ret
        End Function

        ' Returns a Option object filled with the data taken from the input PollOptionDetails
        Public Shared Function GetOptionFromPollOptionDetails(ByVal record As PollOptionDetails) As [Option]
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New [Option](record.ID, record.AddedDate, record.AddedBy, record.PollID, _
                    record.OptionText, record.Votes, record.Percentage)
            End If
        End Function

        ' Returns a list of Option objects filled with the data taken from the input list of CommentDetails
        Public Shared Function GetOptionListFromPollOptionDetailsList(ByVal recordset As List(Of PollOptionDetails)) As List(Of [Option])
            Dim options As New List(Of [Option])
            For Each record As PollOptionDetails In recordset
                options.Add(GetOptionFromPollOptionDetails(record))
            Next
            Return options
        End Function
    End Class
End Namespace