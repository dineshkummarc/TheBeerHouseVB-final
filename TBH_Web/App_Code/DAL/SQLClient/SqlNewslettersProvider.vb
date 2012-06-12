Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Caching

Namespace MB.TheBeerHouse.DAL.SqlClient
    Public Class SqlNewslettersProvider
        Inherits NewslettersProvider

        ' Returns a collection with all the newsletters sent before the specified date
        Public Overrides Function GetNewsLetters(ByVal toDate As Date) As System.Collections.Generic.List(Of NewsletterDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Newsletters_GetNewsletters", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate
                cn.Open()
                Return GetNewsletterCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns an existing newsletter with the specified ID
        Public Overrides Function GetNewsletterByID(ByVal newsletterID As Integer) As NewsletterDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Newsletters_GetNewsletterByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@NewsletterID", SqlDbType.Int).Value = newsletterID
                cn.Open()
                Dim reader As IDataReader
                reader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetNewsletterFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes a newsletter
        Public Overrides Function DeleteNewsletter(ByVal newsletterID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Newsletters_DeleteNewsletter", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@NewsletterID", SqlDbType.Int).Value = newsletterID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates a newsletter
        Public Overrides Function UpdateNewsletter(ByVal newsletter As NewsletterDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Newsletters_UpdateNewsletter", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@NewsletterID", SqlDbType.Int).Value = newsletter.ID
                cmd.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = newsletter.Subject
                cmd.Parameters.Add("@PlainTextBody", SqlDbType.NText).Value = newsletter.PlainTextBody
                cmd.Parameters.Add("@HtmlBody", SqlDbType.NText).Value = newsletter.HtmlBody
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Creates a new newsletter
        Public Overrides Function InsertNewsletter(ByVal newsletter As NewsletterDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Newsletters_InsertNewsletter", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = newsletter.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = newsletter.AddedBy
                cmd.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = newsletter.Subject
                cmd.Parameters.Add("@PlainTextBody", SqlDbType.NText).Value = newsletter.PlainTextBody
                cmd.Parameters.Add("@HtmlBody", SqlDbType.NText).Value = newsletter.HtmlBody
                cmd.Parameters.Add("@NewsletterID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@NewsletterID").Value)
            End Using
        End Function
    End Class
End Namespace
