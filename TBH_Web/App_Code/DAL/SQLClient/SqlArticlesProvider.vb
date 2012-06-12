Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL.SqlClient
    Public Class SqlArticlesProvider
        Inherits ArticlesProvider

        ' Returns a collection with all the categories
        Public Overrides Function GetCategories() As List(Of CategoryDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCategories", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetCategoryCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns an existing category with the specified ID
        Public Overrides Function GetCategoryByID(ByVal categoryID As Integer) As CategoryDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCategoryByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetCategoryFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes a category
        Public Overrides Function DeleteCategory(ByVal categoryID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_DeleteCategory", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates a category
        Public Overrides Function UpdateCategory(ByVal category As CategoryDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_UpdateCategory", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = category.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = category.Title
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = category.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = category.ImageURL
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = category.Description
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' creates a new category
        Public Overrides Function InsertCategory(ByVal category As CategoryDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_InsertCategory", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = category.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = category.AddedBy
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = category.Title
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = category.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = category.ImageURL
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = category.Description
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@CategoryID").Value)
            End Using
        End Function

        ' retreives all articles
        Public Overloads Overrides Function GetArticles( _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of ArticleDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticles", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cn.Open()
                Return GetArticleCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of articles
        Public Overloads Overrides Function GetArticleCount() As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticleCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all articles for the specified category
        Public Overloads Overrides Function GetArticles(ByVal categoryID As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer) As System.Collections.Generic.List(Of ArticleDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticlesByCategory", cn)
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetArticleCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of articles for the specified category
        Public Overloads Overrides Function GetArticleCount(ByVal categoryID As Integer) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticleCountByCategory", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all published articles
        Public Overloads Overrides Function GetPublishedArticles( _
                ByVal currentDate As Date, ByVal pageIndex As Integer, _
                ByVal pageSize As Integer) _
                As List(Of ArticleDetails)

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetPublishedArticles", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = currentDate
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cn.Open()
                Return GetArticleCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of published articles
        Public Overloads Overrides Function GetPublishedArticleCount( _
                ByVal currentDate As Date) _
                As Integer

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetPublishedArticleCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = currentDate
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all published articles for the specified category
        Public Overloads Overrides Function GetPublishedArticles( _
                ByVal categoryID As Integer, ByVal currentDate As Date, _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of ArticleDetails)

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetPublishedArticlesByCategory", cn)
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = currentDate
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetArticleCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of published articles for the specified category
        Public Overloads Overrides Function GetPublishedArticleCount( _
                ByVal categoryID As Integer, ByVal currentDate As Date) _
                As Integer

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetPublishedArticleCountByCategory", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryID
                cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = currentDate
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves the article with the specified ID
        Public Overrides Function GetArticleByID(ByVal articleID As Integer) As ArticleDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticleByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetArticleFromReader(reader, True)
                Else
                    Return Nothing
                End If
           End Using
        End Function

        ' Retrieves the body for the article with the specified ID
        Public Overrides Function GetArticleBody(ByVal articleID As Integer) As String
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetArticleBody", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Return CStr(ExecuteScalar(cmd))
            End Using
        End Function

        ' Deletes an article
        Public Overrides Function DeleteArticle(ByVal articleID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_DeleteArticle", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new article
        Public Overrides Function InsertArticle(ByVal article As ArticleDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_InsertArticle", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = article.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = article.AddedBy
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = article.CategoryID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = article.Title
                cmd.Parameters.Add("@Abstract", SqlDbType.NVarChar).Value = article.Abstract
                cmd.Parameters.Add("@Body", SqlDbType.NText).Value = article.Body
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = article.Country
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = article.State
                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = article.City
                cmd.Parameters.Add("@ReleaseDate", SqlDbType.DateTime).Value = article.ReleaseDate
                cmd.Parameters.Add("@ExpireDate", SqlDbType.DateTime).Value = article.ExpireDate
                cmd.Parameters.Add("@Approved", SqlDbType.Bit).Value = article.Approved
                cmd.Parameters.Add("@Listed", SqlDbType.Bit).Value = article.Listed
                cmd.Parameters.Add("@CommentsEnabled", SqlDbType.Bit).Value = article.CommentsEnabled
                cmd.Parameters.Add("@OnlyForMembers", SqlDbType.Bit).Value = article.OnlyForMembers
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@ArticleID").Value)
            End Using
        End Function

        ' Updates an article
        Public Overrides Function UpdateArticle(ByVal article As ArticleDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_UpdateArticle", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = article.ID
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = article.CategoryID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = article.Title
                cmd.Parameters.Add("@Abstract", SqlDbType.NVarChar).Value = article.Abstract
                cmd.Parameters.Add("@Body", SqlDbType.NText).Value = article.Body
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = article.Country
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = article.State
                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = article.City
                cmd.Parameters.Add("@ReleaseDate", SqlDbType.DateTime).Value = article.ReleaseDate
                cmd.Parameters.Add("@ExpireDate", SqlDbType.DateTime).Value = article.ExpireDate
                cmd.Parameters.Add("@Approved", SqlDbType.Bit).Value = article.Approved
                cmd.Parameters.Add("@Listed", SqlDbType.Bit).Value = article.Listed
                cmd.Parameters.Add("@CommentsEnabled", SqlDbType.Bit).Value = article.CommentsEnabled
                cmd.Parameters.Add("@OnlyForMembers", SqlDbType.Bit).Value = article.OnlyForMembers
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Approves an article
        Public Overrides Function ApproveArticle(ByVal articleID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_ApproveArticle", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Increments the ViewCount of the specified article
        Public Overrides Function IncrementArticleViewCount(ByVal articleID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_IncrementViewCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a vote for the specified article
        Public Overrides Function RateArticle(ByVal articleID As Integer, ByVal rating As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_InsertVote", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cmd.Parameters.Add("@Rating", SqlDbType.Int).Value = rating
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Retrieves all comments
        Public Overloads Overrides Function GetComments( _
                ByVal pageIndex As Integer, ByVal pageSize As Integer) _
                As List(Of CommentDetails)

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetComments", cn)
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetCommentCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns the total number of comments
        Public Overloads Overrides Function GetCommentCount() As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCommentCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all comments for the specified article
        Public Overloads Overrides Function GetComments( _
                ByVal articleID As Integer, ByVal pageIndex As Integer, _
                ByVal pageSize As Integer) _
                As List(Of CommentDetails)

            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCommentsByArticle", cn)
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetCommentCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns the total number of comments for the specified article
        Public Overloads Overrides Function GetCommentCount(ByVal articleID As Integer) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCommentCountByArticle", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = articleID
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves the comment with the specified ID
        Public Overrides Function GetCommentByID(ByVal commentID As Integer) As CommentDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_GetCommentByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CommentID", SqlDbType.Int).Value = commentID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetCommentFromReader(reader)
                Else
                    Return Nothing
                End If
           End Using
        End Function

        ' Deletes a comment
        Public Overrides Function DeleteComment(ByVal commentID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_DeleteComment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CommentID", SqlDbType.Int).Value = commentID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new comment
        Public Overrides Function InsertComment(ByVal comment As CommentDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_InsertComment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = comment.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = comment.AddedBy
                cmd.Parameters.Add("@AddedByEmail", SqlDbType.NVarChar).Value = comment.AddedByEmail
                cmd.Parameters.Add("@AddedByIP", SqlDbType.NVarChar).Value = comment.AddedByIP
                cmd.Parameters.Add("@ArticleID", SqlDbType.Int).Value = comment.ArticleID
                cmd.Parameters.Add("@Body", SqlDbType.NVarChar).Value = comment.Body
                cmd.Parameters.Add("@CommentID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@CommentID").Value)
            End Using
        End Function

        ' Updates a comment
        Public Overrides Function UpdateComment(ByVal comment As CommentDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Articles_UpdateComment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@CommentID", SqlDbType.Int).Value = comment.ID
                cmd.Parameters.Add("@Body", SqlDbType.NVarChar).Value = comment.Body
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function
    End Class
End Namespace
