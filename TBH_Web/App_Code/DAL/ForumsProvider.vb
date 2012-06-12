Imports Microsoft.VisualBasic
Imports System.data
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class ForumsProvider
        Inherits DataAccess

        Private Shared _instance As ForumsProvider

        ' Returns an instance of the provider type specified in the config file
        Public Shared ReadOnly Property Instance() As ForumsProvider
            Get
                If IsNothing(_instance) Then
                    _instance = CType(Activator.CreateInstance( _
                       Type.GetType(Globals.Settings.Forums.Providertype)), ForumsProvider)
                End If
                Return _instance
            End Get
        End Property

        ' ==========
        ' Constructor
        ' ==========
        Public Sub New()
            Me.ConnectionString = Globals.Settings.Forums.ConnectionString
            Me.EnableCaching = Globals.Settings.Forums.EnableCaching
            Me.CacheDuration = Globals.Settings.Forums.CacheDuration
        End Sub

        ' ==========
        ' Override methods
        ' ==========

        ' methods that work with forums
        Public MustOverride Function GetForums() As List(Of ForumDetails)
        Public MustOverride Function GetForumByID(ByVal forumID As Integer) As ForumDetails
        Public MustOverride Function DeleteForum(ByVal forumID As Integer) As Boolean
        Public MustOverride Function UpdateForum(ByVal forum As ForumDetails) As Boolean
        Public MustOverride Function InsertForum(ByVal forum As ForumDetails) As Integer

        ' methods that work with posts
        Public MustOverride Function GetThreads(ByVal forumID As Integer, ByVal sortExpression As String, _
            ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of PostDetails)
        Public MustOverride Function GetThreadCount(ByVal forumID As Integer) As Integer
        Public MustOverride Function GetThreads(ByVal sortExpression As String, _
            ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of PostDetails)
        Public MustOverride Function GetThreadCount() As Integer
        Public MustOverride Function GetThreadByID(ByVal threadPostID As Integer) As List(Of PostDetails)
        Public MustOverride Function GetPostCountByThread(ByVal threadPostID As Integer) As Integer
        Public MustOverride Function GetUnapprovedPosts() As List(Of PostDetails)
        Public MustOverride Function GetPostByID(ByVal postID As Integer) As PostDetails
        Public MustOverride Function DeletePost(ByVal postID As Integer) As Boolean
        Public MustOverride Function UpdatePost(ByVal post As PostDetails) As Boolean
        Public MustOverride Function InsertPost(ByVal post As PostDetails) As Integer
        Public MustOverride Function ApprovePost(ByVal postID As Integer) As Boolean
        Public MustOverride Function CloseThread(ByVal threadPostID As Integer) As Boolean
        Public MustOverride Function MoveThread(ByVal threadPostID As Integer, ByVal forumID As Integer) _
            As Boolean
        Public MustOverride Function IncrementPostViewCount(ByVal postID As Integer) As Boolean
        Public MustOverride Function GetPostBody(ByVal postID As Integer) As String

        ' Returns a valid sort expression
        Public Overridable Function EnsureValidSortExpression(ByVal sortExpression As String) As String
            If String.IsNullOrEmpty(sortExpression) Then
                Return "tbh_Posts.LastPostDate DESC"
            End If

            Dim sortExpr As String = sortExpression.ToLower()
            If Not sortExpr.Equals("lastpostdate") AndAlso Not sortExpr.Equals("lastpostdate asc") AndAlso _
                Not sortExpr.Equals("lastpostdate desc") AndAlso Not sortExpr.Equals("viewcount") AndAlso _
                Not sortExpr.Equals("viewcount asc") AndAlso Not sortExpr.Equals("viewcount desc") AndAlso _
                Not sortExpr.Equals("replycount") AndAlso Not sortExpr.Equals("replycount asc") AndAlso _
                Not sortExpr.Equals("replycount desc") AndAlso Not sortExpr.Equals("addeddate") AndAlso _
                Not sortExpr.Equals("addeddate asc") AndAlso Not sortExpr.Equals("addeddate desc") AndAlso _
                Not sortExpr.Equals("addedby") AndAlso Not sortExpr.Equals("addedby asc") AndAlso _
                Not sortExpr.Equals("addedby desc") AndAlso Not sortExpr.Equals("title") AndAlso _
                Not sortExpr.Equals("title asc") AndAlso Not sortExpr.Equals("title desc") AndAlso _
                Not sortExpr.Equals("lastpostby") AndAlso Not sortExpr.Equals("lastpostby asc") AndAlso _
                Not sortExpr.Equals("lastpostby desc") Then

                sortExpr = "lastpostdate desc"
            End If
            If Not sortExpr.StartsWith("tbh_posts") Then
                sortExpr = "tbh_posts." & sortExpr
            End If
            If Not sortExpr.StartsWith("tbh_products.lastpostdate") Then
                sortExpr += ", LastPostDate DESC"
            End If
            Return sortExpr
        End Function

        ' Returns a new ForumDetails instance filled with the DataReader's current record data
        Public Overridable Function GetForumFromReader(ByVal reader As idatareader) As ForumDetails
            Return New ForumDetails( _
               CInt(reader("ForumID")), _
               CDate(reader("AddedDate")), _
               reader("AddedBy").ToString(), _
               reader("Title").ToString(), _
               CBool(reader("Moderated")), _
               CInt(reader("Importance")), _
               reader("Description").ToString(), _
               reader("ImageUrl").ToString())
        End Function

        ' Returns a collection of ForumDetails objects with the data read from the input DataReader
        Public Overridable Function GetForumCollectionFromReader(ByVal reader As IDataReader) As List(Of ForumDetails)
            Dim forums As New List(Of ForumDetails)
            While reader.Read()
                forums.Add(GetForumFromReader(reader))
            End While
            Return forums
        End Function

        ' Returns a new PostDetails instance filled with the DataReader's current record data
        Public Overridable Function GetPostFromReader(ByVal reader As IDataReader) As PostDetails
            Return GetPostFromReader(reader, True)
        End Function

        Public Overridable Function GetPostFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As PostDetails
            Dim post As New PostDetails( _
                CInt(reader("PostID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString(), _
                reader("AddedByIP").ToString(), _
                CInt(reader("ForumID")), _
                reader("ForumTitle").ToString(), _
                CInt(reader("ParentPostID")), _
                reader("Title").ToString(), _
                Nothing, _
                CBool(reader("Approved")), _
                CBool(reader("Closed")), _
                CInt(reader("ViewCount")), _
                CInt(reader("ReplyCount")), _
                CDate(reader("LastPostDate")), _
                reader("LastPostBy").ToString())

            If readBody Then post.Body = reader("Body").ToString()

            Return post
        End Function

        ' Returns a collection of PostDetails objects with the data read from the input DataReader
        Public Overridable Function GetPostCollectionFromReader(ByVal reader As IDataReader) As List(Of PostDetails)
            Return GetPostCollectionFromReader(reader, True)
        End Function

        Public Overridable Function GetPostCollectionFromReader(ByVal reader As IDataReader, ByVal readBody As Boolean) As List(Of PostDetails)
            Dim posts As New List(Of PostDetails)
            While reader.Read()
                posts.Add(GetPostFromReader(reader, readBody))
            End While
            Return posts
        End Function
    End Class
End Namespace
