Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.data
Imports System.Data.SqlClient
Imports System.Web.Caching

Namespace MB.TheBeerHouse.DAL.SqlClient
    Public Class SqlForumsProvider
        Inherits ForumsProvider

        ' Returns a collection with all the forums
        Public Overrides Function GetForums() As List(Of ForumDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetForums", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetForumCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns an existing forum with the specified ID
        Public Overrides Function GetForumByID(ByVal forumID As Integer) As ForumDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetForumByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = forumID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetForumFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes a forum
        Public Overrides Function DeleteForum(ByVal forumID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_DeleteForum", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = forumID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates a forum
        Public Overrides Function UpdateForum(ByVal forum As ForumDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_UpdateForum", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = forum.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = forum.Title
                cmd.Parameters.Add("@Moderated", SqlDbType.Bit).Value = forum.Moderated
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = forum.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = forum.ImageURL
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = forum.Description
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Creates a new forum
        Public Overrides Function InsertForum(ByVal forum As ForumDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_InsertForum", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = forum.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = forum.AddedBy
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = forum.Title
                cmd.Parameters.Add("@Moderated", SqlDbType.Bit).Value = forum.Moderated
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = forum.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = forum.ImageURL
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = forum.Description
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@ForumID").Value)
            End Using
        End Function

        ' Retrieves all unapproveds posts
        Public Overrides Function GetUnapprovedPosts() As System.Collections.Generic.List(Of PostDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetUnapprovedPosts", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetPostCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Retrieves forum's approved threads by page
        Public Overloads Overrides Function GetThreads(ByVal forumID As Integer, ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As System.Collections.Generic.List(Of PostDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                sortExpression = EnsureValidSortExpression(sortExpression)
                Dim lowerBound As Integer = pageIndex * pageSize + 1
                Dim upperBound As Integer = (pageIndex + 1) * pageSize
                Dim sql As String = String.Format("SELECT * FROM " & _
                    "(SELECT tbh_Posts.PostID, tbh_Posts.AddedDate, tbh_Posts.AddedBy, tbh_Posts.AddedByIP, " & _
                        "tbh_Posts.ForumID, tbh_Posts.ParentPostID, tbh_Posts.Title, tbh_Posts.Approved, " & _
                        "tbh_Posts.Closed, tbh_Posts.ViewCount, tbh_Posts.ReplyCount, tbh_Posts.LastPostDate, " & _
                        "tbh_Posts.LastPostBy, tbh_Forums.Title AS ForumTitle, " & _
                        "ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum " & _
                        "FROM tbh_Posts INNER JOIN " & _
                        "tbh_Forums ON tbh_Posts.ForumID = tbh_Forums.ForumID " & _
                        "WHERE tbh_Posts.ForumID = {1} AND ParentPostID = 0 AND Approved = 1) ForumThreads " & _
                        "WHERE ForumThreads.RowNum BETWEEN {2} AND {3} " & _
                        "ORDER BY RowNum ASC", _
                    sortExpression, forumID, lowerBound, upperBound)
                Dim cmd As New SqlCommand(sql, cn)
                cn.Open()
                Return GetPostCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of approved threads for the specified forum
        Public Overloads Overrides Function GetThreadCount(ByVal forumID As Integer) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetThreadCountByForum", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = forumID
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves approved threads (from any forum) by page
        Public Overloads Overrides Function GetThreads(ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As System.Collections.Generic.List(Of PostDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                sortExpression = EnsureValidSortExpression(sortExpression)
                Dim lowerBound As Integer = pageIndex * pageSize + 1
                Dim upperBound As Integer = (pageIndex + 1) * pageSize
                Dim sql As String = String.Format("SELECT * FROM(" & _
                    "SELECT tbh_Posts.PostID, tbh_Posts.AddedDate, tbh_Posts.AddedBy, tbh_Posts.AddedByIP, " & _
                    "tbh_Posts.ForumID, tbh_Posts.ParentPostID, tbh_Posts.Title, tbh_Posts.Approved, " & _
                    "tbh_Posts.Closed, tbh_Posts.ViewCount, tbh_Posts.ReplyCount, tbh_Posts.LastPostDate, " & _
                    "tbh_Posts.LastPostBy, tbh_Forums.Title AS ForumTitle, " & _
                        "ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum " & _
                        "FROM tbh_Posts INNER JOIN tbh_Forums ON tbh_Posts.ForumID = tbh_Forums.ForumID " & _
                        "WHERE(ParentPostID = 0 And Approved = 1)) ForumThreads " & _
                    "WHERE ForumThreads.RowNum BETWEEN {1} AND {2} " & _
                    "ORDER BY RowNum ASC", _
                    sortExpression, lowerBound, upperBound)
                Dim cmd As New SqlCommand(sql, cn)
                cn.Open()
                Return GetPostCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of approved threads for any forum
        Public Overloads Overrides Function GetThreadCount() As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetThreadCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves the post with the specified ID
        Public Overrides Function GetPostByID(ByVal postID As Integer) As PostDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetPostByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = postID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetPostFromReader(reader, True)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Retrieves the body for the post with the specified ID
        Public Overrides Function GetPostBody(ByVal postID As Integer) As String
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetPostBody", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = postID
                cn.Open()
                Return CStr(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all posts of a given thread
        Public Overrides Function GetThreadByID(ByVal threadPostID As Integer) As System.Collections.Generic.List(Of PostDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetThreadByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ThreadPostID", SqlDbType.Int).Value = threadPostID
                cn.Open()
                Return GetPostCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns the total number of approved posts for the specified thread
        Public Overrides Function GetPostCountByThread(ByVal threadPostID As Integer) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_GetPostCountByThread", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ThreadPostID", SqlDbType.Int).Value = threadPostID
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Deletes a post (if the post represent the first message of a thread, 
        ' the child posts are deleted as well)
        Public Overrides Function DeletePost(ByVal postID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_DeletePost", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = postID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret >= 1)
            End Using
        End Function

        ' Inserts a new post
        Public Overrides Function InsertPost(ByVal post As PostDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_InsertPost", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = post.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = post.AddedBy
                cmd.Parameters.Add("@AddedByIP", SqlDbType.NChar).Value = post.AddedByIP
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = post.ForumID
                cmd.Parameters.Add("@ParentPostID", SqlDbType.Int).Value = post.ParentPostID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = post.Title
                cmd.Parameters.Add("@Body", SqlDbType.NText).Value = post.Body
                cmd.Parameters.Add("@Approved", SqlDbType.Bit).Value = post.Approved
                cmd.Parameters.Add("@Closed", SqlDbType.Bit).Value = post.Closed
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@PostID").Value)
            End Using
        End Function

        ' Updates a post
        Public Overrides Function UpdatePost(ByVal post As PostDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_UpdatePost", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = post.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = post.Title
                cmd.Parameters.Add("@Body", SqlDbType.NText).Value = post.Body
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Approves a post
        Public Overrides Function ApprovePost(ByVal postID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_ApprovePost", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = postID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret >= 1)
            End Using
        End Function

        ' Increments the ViewCount of the specified post
        Public Overrides Function IncrementPostViewCount(ByVal postID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_IncrementViewCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@PostID", SqlDbType.Int).Value = postID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Closes a thread
        Public Overrides Function CloseThread(ByVal threadPostID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_CloseThread", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ThreadPostID", SqlDbType.Int).Value = threadPostID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Moves a thread (the parent post and all its child posts) to a different forum
        Public Overrides Function MoveThread(ByVal threadPostID As Integer, ByVal forumID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Forums_MoveThread", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ThreadPostID", SqlDbType.Int).Value = threadPostID
                cmd.Parameters.Add("@ForumID", SqlDbType.Int).Value = forumID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret >= 1)
            End Using
        End Function
    End Class
End Namespace