Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Polls
    Public Class Poll
        Inherits BasePoll

        ' ==========
        ' Private variables
        ' ==========
        Private _questionText As String = ""
        Private _isArchived As Boolean = False
        Private _isCurrent As Boolean = False
        Private _archivedDate As DateTime = DateTime.MinValue
        Private _votes As Integer = 0
        Private _options As List(Of [Option])

        ' ==========
        ' Properties
        ' ==========
        Public Property QuestionText() As String
            Get
                Return _questionText
            End Get
            Private Set(ByVal value As String)
                _questionText = value
            End Set
        End Property

        Public Property IsCurrent() As Boolean
            Get
                Return _isCurrent
            End Get
            Private Set(ByVal value As Boolean)
                _isCurrent = value
            End Set
        End Property

        Public Property IsArchived() As Boolean
            Get
                Return _isArchived
            End Get
            Private Set(ByVal value As Boolean)
                _isArchived = value
            End Set
        End Property

        Public Property ArchivedDate() As DateTime
            Get
                Return _archivedDate
            End Get
            Private Set(ByVal value As DateTime)
                _archivedDate = value
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

        Public ReadOnly Property Options() As List(Of [Option])
            Get
                If IsNothing(_options) Then
                    _options = [Option].GetOptions(Me.ID)
                End If
                Return _options
            End Get
        End Property

        ' ==========
        ' Methods
        ' ==========
        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
            ByVal questionText As String, ByVal isCurrent As Boolean, ByVal isArchived As Boolean, _
            ByVal archivedDate As DateTime, ByVal votes As Integer)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.QuestionText = questionText
            Me.IsCurrent = isCurrent
            Me.IsArchived = isArchived
            Me.ArchivedDate = archivedDate
            Me.Votes = votes
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = Poll.DeletePoll(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return Poll.UpdatePoll(Me.ID, Me.QuestionText, Me.IsCurrent)
        End Function

        Public Function Archive() As Boolean
            Dim success As Boolean = Poll.ArchivePoll(Me.ID)
            If success Then
                Me.IsCurrent = False
                Me.IsArchived = True
                Me.ArchivedDate = DateTime.Now
            End If
            Return success
        End Function

        Public Function Vote(ByVal optionID As Integer) As Boolean
            Dim success As Boolean = Poll.VoteOption(Me.ID, optionID)
            If success Then
                Me.Votes += 1
            End If
            Return success
        End Function

        ' ===========
        ' Shared Properties
        ' ===========
        Public Shared ReadOnly Property CurrentPollID() As Integer
            Get
                Dim pollID As Integer = -1
                Dim key As String = "Polls_Poll_Current"

                If BasePoll.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                    pollID = CInt(BizObject.Cache(key))
                Else
                    pollID = SiteProvider.Polls.GetCurrentPollID()
                    BasePoll.CacheData(key, pollID)
                End If

                Return pollID
            End Get
        End Property

        Public Shared ReadOnly Property CurrentPoll() As Poll
            Get
                Return GetPollByID(CurrentPollID)
            End Get
        End Property

        ' ===========
        ' Shared Methods
        ' ===========

        ' Returns a collection with all polls
        Public Shared Function GetPolls() As List(Of Poll)
            Return GetPolls(True, True)
        End Function

        Public Shared Function GetPolls(ByVal includeActive As Boolean, ByVal includeArchived As Boolean) As List(Of Poll)
            Dim polls As List(Of Poll)
            Dim key As String = "Polls_Polls_" & includeActive.ToString() & "_" & includeArchived.ToString()

            If BasePoll.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                polls = CType(BizObject.Cache(key), List(Of Poll))
            Else
                Dim recordset As List(Of PollDetails) = SiteProvider.Polls.GetPolls(includeActive, includeArchived)
                polls = GetPollListFromPollDetailsList(recordset)
                BasePoll.CacheData(key, polls)
            End If
            Return polls
        End Function

        ' Returns a Poll object with the specified ID
        Public Shared Function GetPollByID(ByVal pollID As Integer) As Poll
            Dim poll As Poll
            Dim key As String = "Polls_Poll_" + CStr(pollID)

            If BasePoll.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                poll = CType(BizObject.Cache(key), Poll)
            Else
                poll = GetPollFromPollDetails(SiteProvider.Polls.GetPollByID(pollID))
                BasePoll.CacheData(key, poll)
            End If
            Return poll
        End Function

        ' Updates an existing poll
        Public Shared Function UpdatePoll(ByVal id As Integer, ByVal questionText As String, _
                ByVal isCurrent As Boolean) _
                As Boolean

            Dim record As New PollDetails(id, DateTime.Now, "", questionText, _
                isCurrent, False, DateTime.Now, 0)
            Dim ret As Boolean = SiteProvider.Polls.UpdatePoll(record)
            BizObject.PurgeCacheItems("polls_polls_true")
            BizObject.PurgeCacheItems("polls_poll_" & CStr(id))
            If isCurrent Then
                BizObject.PurgeCacheItems("polls_poll_current")
            End If
            Return ret
        End Function

        ' Deletes an existing poll
        Public Shared Function DeletePoll(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Polls.DeletePoll(id)
            Dim ev As New RecordDeletedEvent("poll", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("polls_polls")
            BizObject.PurgeCacheItems("polls_poll_" & CStr(id))
            BizObject.PurgeCacheItems("polls_poll_current")
            Return ret
        End Function

        ' Archive an existing poll
        Public Shared Function ArchivePoll(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Polls.ArchivePoll(id)
            BizObject.PurgeCacheItems("polls_polls")
            BizObject.PurgeCacheItems("polls_poll_" & CStr(id))
            BizObject.PurgeCacheItems("polls_poll_current")
            Return ret
        End Function

        ' Votes for a poll option
        Public Shared Function VoteOption(ByVal pollID As Integer, ByVal optionID As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Polls.InsertVote(optionID)
            BizObject.PurgeCacheItems("polls_polls_true")
            BizObject.PurgeCacheItems("polls_poll_" & pollID.ToString())
            BizObject.PurgeCacheItems("polls_options_" & pollID.ToString())
            Return ret
        End Function

        ' Creates a new poll
        Public Shared Function InsertPoll(ByVal questionText As String, ByVal isCurrent As Boolean) As Integer
            Dim record As New PollDetails(0, DateTime.Now, BizObject.CurrentUserName, _
                questionText, isCurrent, False, DateTime.Now, 0)
            Dim ret As Integer = SiteProvider.Polls.InsertPoll(record)
            BizObject.PurgeCacheItems("polls_polls_true")
            If isCurrent Then
                BizObject.PurgeCacheItems("polls_poll_current")
            End If
            Return ret
        End Function

        ' Returns a Poll object filled with the data taken from the input PollDetails
        Public Shared Function GetPollFromPollDetails(ByVal record As PollDetails) As Poll
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Poll(record.ID, record.AddedDate, record.AddedBy, _
                   record.QuestionText, record.IsCurrent, _
                   record.IsArchived, record.ArchivedDate, record.Votes)
            End If
        End Function

        ' Returns a list of Poll objects filled with the data taken from the input list of PollDetails
        Public Shared Function GetPollListFromPollDetailsList( _
                ByVal recordset As List(Of PollDetails)) _
                As List(Of Poll)

            Dim polls As New List(Of Poll)
            For Each record As PollDetails In recordset
                polls.Add(GetPollFromPollDetails(record))
            Next
            Return polls
        End Function
    End Class
End Namespace
