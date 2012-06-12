Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class PollsProvider
        Inherits DataAccess

        Private Shared _instance As PollsProvider = Nothing

        ' Returns an instance of the provider type specified in the config file
        Public Shared ReadOnly Property Instance() As PollsProvider
            Get
                If IsNothing(_instance) Then
                    _instance = CType(Activator.CreateInstance( _
                        Type.GetType(Globals.Settings.Polls.ProviderType)), PollsProvider)
                End If
                Return _instance
            End Get
        End Property

        Public Sub New()
            Me.ConnectionString = Globals.Settings.Polls.ConnectionString
            Me.EnableCaching = Globals.Settings.Polls.EnableCaching
            Me.CacheDuration = Globals.Settings.Polls.CacheDuration
        End Sub

        ' methods that work with polls
        Public MustOverride Function GetPolls( _
            ByVal includeActive As Boolean, ByVal includeArchived As Boolean) As List(Of PollDetails)
        Public MustOverride Function GetPollByID(ByVal pollID As Integer) As PollDetails
        Public MustOverride Function GetCurrentPollID() As Integer
        Public MustOverride Function DeletePoll(ByVal pollID As Integer) As Boolean
        Public MustOverride Function ArchivePoll(ByVal pollID As Integer) As Boolean
        Public MustOverride Function UpdatePoll(ByVal poll As PollDetails) As Boolean
        Public MustOverride Function InsertPoll(ByVal poll As PollDetails) As Integer
        Public MustOverride Function InsertVote(ByVal optionID As Integer) As Boolean

        ' methods that work with poll options
        Public MustOverride Function GetOptions(ByVal pollID As Integer) As List(Of PollOptionDetails)
        Public MustOverride Function GetOptionByID(ByVal optionID As Integer) As PollOptionDetails
        Public MustOverride Function DeleteOption(ByVal optionID As Integer) As Boolean
        Public MustOverride Function UpdateOption(ByVal [option] As PollOptionDetails) As Boolean
        Public MustOverride Function InsertOption(ByVal [option] As PollOptionDetails) As Integer

        ' Returns a new PollDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetPollFromReader(ByVal reader As IDataReader) As PollDetails
            Dim archivedDate As DateTime = DateTime.MinValue
            If Not IsDBNull(reader("ArchivedDate")) Then archivedDate = CDate(reader("ArchivedDate"))
            Dim votes As Integer = 0
            If Not IsDBNull(reader("Votes")) Then votes = CInt(reader("Votes"))
            Return New PollDetails( _
                CInt(reader("PollID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString(), _
                reader("QuestionText").ToString(), _
                CBool(reader("IsCurrent")), _
                CBool(reader("IsArchived")), archivedDate, votes)
        End Function

        ' Returns a collection of PollDetails objects with the data read from the input DataReader
        Protected Overridable Function GetPollCollectionFromReader( _
                ByVal reader As IDataReader) _
                As List(Of PollDetails)

            Dim polls As New List(Of PollDetails)
            While reader.Read
                polls.Add(GetPollFromReader(reader))
            End While
            Return polls
        End Function

        ' Returns a new PollOptionDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetOptionFromReader(ByVal reader As IDataReader) As PollOptionDetails
            Dim [option] As New PollOptionDetails( _
                CInt(reader("OptionID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString(), _
                CInt(reader("PollID")), _
                reader("OptionText").ToString(), _
                CInt(reader("Votes")), _
                CDbl(reader("Percentage")))

            Return [option]
        End Function

        ' Returns a collection of PollOptionDetails objects with the data read from the input DataReader
        Protected Overridable Function GetOptionCollectionFromReader(ByVal reader As IDataReader) As List(Of PollOptionDetails)
            Dim options As New List(Of PollOptionDetails)
            While reader.Read
                options.Add(GetOptionFromReader(reader))
            End While
            Return options
        End Function
    End Class
End Namespace
