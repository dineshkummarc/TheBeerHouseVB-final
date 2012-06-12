Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class ArticlesProvider
        Inherits DataAccess

        Private Shared _instance As ArticlesProvider = Nothing

        Public Shared ReadOnly Property Instance() As ArticlesProvider
            Get
                If IsNothing(_instance) Then
                    _instance = CType(Activator.CreateInstance( _
                        Type.GetType(Globals.Settings.Articles.ProviderType)), _
                        ArticlesProvider)
                End If

                Return _instance
            End Get
        End Property

        Public Sub New()
            Me.ConnectionString = Globals.Settings.Articles.ConnectionString
            Me.EnableCaching = Globals.Settings.Articles.EnableCaching
            Me.CacheDuration = Globals.Settings.Articles.CacheDuration
        End Sub

        ' methods that work with categories
        Public MustOverride Function GetCategories() As List(Of CategoryDetails)
        Public MustOverride Function GetCategoryByID( _
                ByVal categoryID As Integer) As CategoryDetails
        Public MustOverride Function DeleteCategory( _
                ByVal categoryID As Integer) As Boolean
        Public MustOverride Function UpdateCategory( _
                ByVal category As CategoryDetails) As Boolean
        Public MustOverride Function InsertCategory( _
                ByVal category As CategoryDetails) As Integer

        ' methods that work with articles
        Public MustOverride Function GetArticles( _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of ArticleDetails)
        Public MustOverride Function GetArticles( _
                ByVal categoryID As Integer, ByVal pageIndex As Integer, _
                ByVal pageSize As Integer) _
                As List(Of ArticleDetails)
        Public MustOverride Function GetArticleCount() As Integer
        Public MustOverride Function GetArticleCount( _
                ByVal categoryID As Integer) As Integer
        Public MustOverride Function GetPublishedArticles( _
                ByVal currentDate As DateTime, ByVal pageIndex As Integer, _
                ByVal pageSize As Integer) _
                As List(Of ArticleDetails)
        Public MustOverride Function GetPublishedArticles( _
                ByVal categoryID As Integer, ByVal currentDate As DateTime, _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of ArticleDetails)
        Public MustOverride Function GetPublishedArticleCount( _
                ByVal currentDate As DateTime) As Integer
        Public MustOverride Function GetPublishedArticleCount( _
                ByVal categoryID As Integer, ByVal currentDate As DateTime) _
                As Integer
        Public MustOverride Function GetArticleByID( _
                ByVal articleID As Integer) As ArticleDetails
        Public MustOverride Function DeleteArticle( _
                ByVal articleID As Integer) As Boolean
        Public MustOverride Function UpdateArticle( _
                ByVal article As ArticleDetails) As Boolean
        Public MustOverride Function InsertArticle( _
                ByVal article As ArticleDetails) As Integer
        Public MustOverride Function ApproveArticle( _
                ByVal articleID As Integer) As Boolean
        Public MustOverride Function IncrementArticleViewCount( _
                ByVal articleID As Integer) As Boolean
        Public MustOverride Function RateArticle( _
                ByVal articleID As Integer, ByVal rating As Integer) _
                As Boolean
        Public MustOverride Function GetArticleBody( _
                ByVal articleID As Integer) As String

        ' methods that work with comments
        Public MustOverride Function GetComments( _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of CommentDetails)
        Public MustOverride Function GetComments( _
                ByVal articleID As Integer, ByVal pageIndex As Integer, _
                ByVal pageSize As Integer) _
                As List(Of CommentDetails)
        Public MustOverride Function GetCommentCount() As Integer
        Public MustOverride Function GetCommentCount( _
                ByVal articleID As Integer) As Integer
        Public MustOverride Function GetCommentByID( _
                ByVal commentID As Integer) As CommentDetails
        Public MustOverride Function DeleteComment( _
                ByVal commentID As Integer) As Boolean
        Public MustOverride Function UpdateComment( _
                ByVal comment As CommentDetails) As Boolean
        Public MustOverride Function InsertComment( _
                ByVal comment As CommentDetails) As Integer

        ' Returns a new CategoryDetails instance filled with the 
        ' DataReader's current record data
        Protected Overridable Function GetCategoryFromReader( _
                ByVal reader As IDataReader) _
                As CategoryDetails

            Return New CategoryDetails( _
                CInt(reader("CategoryID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString, _
                reader("Title").ToString, _
                CInt(reader("Importance")), _
                reader("Description").ToString, _
                reader("ImageUrl").ToString)
        End Function

        ' Returns a collection of CategoryDetails objects with 
        ' the data read from the input DataReader
        Protected Overridable Function GetCategoryCollectionFromReader( _
                ByVal reader As IDataReader) _
                As List(Of CategoryDetails)

            Dim categories As New List(Of CategoryDetails)
            While (reader.Read)
                categories.Add(GetCategoryFromReader(reader))
            End While
            Return categories
        End Function

        ' Returns a new ArticleDetails instance filled with the
        ' DataReader's current record data
        Protected Overridable Function GetArticlesFromReader( _
                ByVal reader As IDataReader) _
                As ArticleDetails

            Return GetArticleFromReader(reader, True)
        End Function

        Protected Overridable Function GetArticleFromReader( _
                ByVal reader As IDataReader, ByVal readBody As Boolean) _
                As ArticleDetails

            Dim article As New ArticleDetails( _
                CInt(reader("articleID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString, _
                CInt(reader("CategoryID")), _
                reader("CategoryTitle").ToString, _
                reader("Title").ToString, _
                reader("Abstract").ToString, _
                Nothing, _
                reader("Country").ToString, _
                reader("State").ToString, _
                reader("City").ToString, _
                CDate(reader("ReleaseDate")), _
                CDate(reader("ExpireDate")), _
                CBool(reader("Approved")), _
                CBool(reader("Listed")), _
                CBool(reader("CommentsEnabled")), _
                CBool(reader("OnlyForMembers")), _
                CInt(reader("ViewCount")), _
                CInt(reader("Votes")), _
                CInt(reader("TotalRating")))
            If readBody Then
                article.Body = reader("Body").ToString
            End If

            Return article
        End Function

        ' Returns a collection of ArticleDetails objects with the 
        ' data read from the input DataReader
        Protected Overridable Function GetArticleCollectionFromReader( _
                ByVal reader As IDataReader) _
                As List(Of ArticleDetails)

            Return GetArticleCollectionFromReader(reader, True)
        End Function

        Protected Overridable Function GetArticleCollectionFromReader( _
                ByVal reader As IDataReader, ByVal readBody As Boolean) _
                As List(Of ArticleDetails)
            Dim articles As New List(Of ArticleDetails)
            While (reader.Read())
                articles.Add(GetArticleFromReader(reader, readBody))
            End While
            Return articles
        End Function

        ' Returns a new CommentDetails instance filled with the _
        ' DataReader's current record data
        Protected Overridable Function GetCommentFromReader( _
                ByVal reader As IDataReader) _
                As CommentDetails

            Dim comment As New CommentDetails( _
                CInt(reader("CommentID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString, _
                reader("AddedByEmail").ToString, _
                reader("AddedByIP").ToString, _
                CInt(reader("ArticleID")), _
                reader("ArticleTitle").ToString, _
                reader("Body").ToString)
            Return comment
        End Function

        ' Returns a collection of CommentDetails objects with the data read from the input DataReader
        Protected Overridable Function GetCommentCollectionFromReader( _
                ByVal reader As IDataReader) _
                As List(Of CommentDetails)

            Dim comments As New List(Of CommentDetails)
            While (reader.Read)
                comments.Add(GetCommentFromReader(reader))
            End While
            Return comments
        End Function
    End Class
End Namespace
