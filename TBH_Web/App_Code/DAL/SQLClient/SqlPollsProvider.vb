Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Caching

Namespace MB.TheBeerHouse.DAL.SqlClient
    Public Class SqlPollsProvider
        Inherits PollsProvider

        ' Returns the ID of the current poll, or -1 if there is no current poll
        Public Overrides Function GetCurrentPollID() As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_GetCurrentPollID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@PollID").Value)
            End Using
        End Function

        ' Returns a collection with all the polls
        Public Overrides Function GetPolls( _
                ByVal includeActive As Boolean, ByVal includeArchived As Boolean) _
                As System.Collections.Generic.List(Of PollDetails)

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_GetPolls", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@IncludeActive", SqlDbType.Bit).Value = includeActive
                cmd.Parameters.Add("@IncludeArchived", SqlDbType.Bit).Value = includeArchived
                cn.Open()
                Return GetPollCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns the poll with the specified ID
        Public Overrides Function GetPollByID(ByVal pollID As Integer) As PollDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_GetPollByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If (reader.Read()) Then
                    Return GetPollFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes the existing poll with the specified ID
        Public Overrides Function DeletePoll(ByVal pollID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_DeletePoll", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Archives the existing poll with the specified ID
        Public Overrides Function ArchivePoll(ByVal pollID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_ArchivePoll", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using

        End Function

        ' Updates the specified poll
        Public Overrides Function UpdatePoll(ByVal poll As PollDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_UpdatePoll", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = poll.ID
                cmd.Parameters.Add("@QuestionText", SqlDbType.NVarChar).Value = poll.QuestionText
                cmd.Parameters.Add("@IsCurrent", SqlDbType.Bit).Value = poll.IsCurrent
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new poll and returns its ID
        Public Overrides Function InsertPoll(ByVal poll As PollDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_InsertPoll", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = poll.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = poll.AddedBy
                cmd.Parameters.Add("@QuestionText", SqlDbType.NVarChar).Value = poll.QuestionText
                cmd.Parameters.Add("@IsCurrent", SqlDbType.Bit).Value = poll.IsCurrent
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@PollID").Value)
            End Using
        End Function

        ' Returns the poll option with the specified ID
        Public Overrides Function GetOptionByID(ByVal optionID As Integer) As PollOptionDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_GetOptionByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OptionID", SqlDbType.Int).Value = optionID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetOptionFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Get a collection with all options for the specified poll
        Public Overrides Function GetOptions(ByVal pollID As Integer) As System.Collections.Generic.List(Of PollOptionDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_GetOptions", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID
                cn.Open()
                Return GetOptionCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Deletes a poll option with the specified ID
        Public Overrides Function DeleteOption(ByVal optionID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_DeleteOption", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OptionID", SqlDbType.Int).Value = optionID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates the specified poll option
        Public Overrides Function UpdateOption(ByVal [option] As PollOptionDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_UpdateOption", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OptionID", SqlDbType.Int).Value = [option].ID
                cmd.Parameters.Add("@OptionText", SqlDbType.NVarChar).Value = [option].OptionText
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new poll option and returns its ID
        Public Overrides Function InsertOption(ByVal [option] As PollOptionDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_InsertOption", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = [option].AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = [option].AddedBy
                cmd.Parameters.Add("@PollID", SqlDbType.Int).Value = [option].PollID
                cmd.Parameters.Add("@OptionText", SqlDbType.NVarChar).Value = [option].OptionText
                cmd.Parameters.Add("@OptionID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@OptionID").Value)
            End Using
        End Function

        ' Votes for the specified poll option
        Public Overrides Function InsertVote(ByVal optionID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Polls_InsertVote", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OptionID", SqlDbType.Int).Value = optionID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function
    End Class
End Namespace
