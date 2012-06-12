Imports Microsoft.VisualBasic
Imports System.data
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class NewslettersProvider
        Inherits DataAccess

        Private Shared _instance As NewslettersProvider

        ' Returns an instance of the provider type specified in the config file
        Public Shared ReadOnly Property Instance() As NewslettersProvider
            Get
                If IsNothing(_instance) Then
                    _instance = CType(Activator.CreateInstance( _
                        Type.GetType(Globals.Settings.Newsletters.ProviderType)), NewslettersProvider)
                End If
                Return _instance
            End Get
        End Property

        Public Sub New()
            Me.ConnectionString = Globals.Settings.Newsletters.ConnectionString
            Me.EnableCaching = Globals.Settings.Newsletters.EnableCaching
            Me.CacheDuration = Globals.Settings.Newsletters.CacheDuration
        End Sub

        ' Methods that work with newsletters
        Public MustOverride Function GetNewsLetters(ByVal toDate As DateTime) As List(Of NewsletterDetails)
        Public MustOverride Function GetNewsletterByID(ByVal newsletterID As Integer) As NewsletterDetails
        Public MustOverride Function DeleteNewsletter(ByVal newsletterID As Integer) As Boolean
        Public MustOverride Function UpdateNewsletter(ByVal newsletter As NewsletterDetails) As Boolean
        Public MustOverride Function InsertNewsletter(ByVal newsletter As NewsletterDetails) As Integer

        ' Returns a new NewsletterDetails instance filled with the
        ' DataReader's current record data
        Protected Overridable Function GetNewsletterFromReader(ByVal reader As IDataReader) As NewsletterDetails
            Return GetNewsletterFromReader(reader, True)
        End Function

        Protected Overridable Function GetNewsletterFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As NewsletterDetails
            Dim newsletter As New NewsletterDetails( _
                CInt(reader("NewsletterID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString, _
                reader("Subject").ToString, _
                Nothing, Nothing)

            If readBody Then
                newsletter.PlainTextBody = reader("PlainTextBody").ToString
                newsletter.HtmlBody = reader("HtmlBody").ToString
            End If

            Return newsletter
        End Function

        Protected Overridable Function GetNewsletterCollectionFromReader(ByVal reader As IDataReader) As List(Of NewsletterDetails)
            Return GetNewsletterCollectionFromReader(reader, True)
        End Function

        Protected Overridable Function GetNewsletterCollectionFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As List(Of NewsletterDetails)
            Dim newsletters As New List(Of NewsletterDetails)

            While reader.Read
                newsletters.Add(GetNewsletterFromReader(reader, readBody))
            End While

            Return newsletters
        End Function
    End Class
End Namespace
