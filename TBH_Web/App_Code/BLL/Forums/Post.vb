Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Forums
    Public Class Post
        Inherits BaseForum

        ' =============
        ' Private variables
        ' =============

        Private _addedByIP As String = ""
        Private _forumID As Integer = 0
        Private _forumTitle As String = ""
        Private _forum As Forum
        Private _parentPostID As Integer = 0
        Private _parentPost As Post
        Private _title As String = ""
        Private _body As String
        Private _approved As Boolean = True
        Private _closed As Boolean = False
        Private _viewCount As Integer = 0
        Private _replyCount As Integer = 0
        Private _lastPostDate As DateTime = DateTime.Now
        Private _lastPostBy As String = ""

        ' ==========
        ' Properties
        ' ==========

        Public Property AddedByIP() As String
            Get
                Return _addedByIP
            End Get
            Private Set(ByVal value As String)
                _addedByIP = value
            End Set
        End Property

        Public Property ForumID() As Integer
            Get
                Return _forumID
            End Get
            Private Set(ByVal value As Integer)
                _forumID = value
            End Set
        End Property

        Public Property ForumTitle() As String
            Get
                Return _forumTitle
            End Get
            Private Set(ByVal value As String)
                _forumTitle = value
            End Set
        End Property

        Public Property Forum() As Forum
            Get
                If IsNothing(_forum) Then
                    _forum = Forums.Forum.GetForumByID(Me.ForumID)
                End If
                Return _forum
            End Get
            Private Set(ByVal value As Forum)
                _forum = value
            End Set
        End Property

        Public Property ParentPostID() As Integer
            Get
                Return _parentPostID
            End Get
            Private Set(ByVal value As Integer)
                _parentPostID = value
            End Set
        End Property

        Public Property ParentPost() As Post
            Get
                If IsNothing(_parentPost) Then
                    _parentPost = Post.GetPostByID(Me.ParentPostID)
                End If
                Return _parentPost
            End Get
            Private Set(ByVal value As Post)
                _parentPost = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Body() As String
            Get
                If IsNothing(_body) Then
                    _body = SiteProvider.Forums.GetPostBody(Me.ID)
                End If
                Return _body
            End Get
            Set(ByVal value As String)
                _body = value
            End Set
        End Property

        Public Property Approved() As Boolean
            Get
                Return _approved
            End Get
            Private Set(ByVal value As Boolean)
                _approved = value
            End Set
        End Property

        Public Property Closed() As Boolean
            Get
                Return _closed
            End Get
            Private Set(ByVal value As Boolean)
                _closed = value
            End Set
        End Property

        Public Property ViewCount() As Integer
            Get
                Return _viewCount
            End Get
            Private Set(ByVal value As Integer)
                _viewCount = value
            End Set
        End Property

        Public Property ReplyCount() As Integer
            Get
                Return _replyCount
            End Get
            Set(ByVal value As Integer)
                _replyCount = value
            End Set
        End Property

        Public Property LastPostDate() As DateTime
            Get
                Return _lastPostDate
            End Get
            Private Set(ByVal value As DateTime)
                _lastPostDate = value
            End Set
        End Property

        Public Property LastPostBy() As String
            Get
                Return _lastPostBy
            End Get
            Private Set(ByVal value As String)
                _lastPostBy = value
            End Set
        End Property

        Public ReadOnly Property IsFirstPost() As Boolean
            Get
                Return (Me.ParentPostID = 0)
            End Get
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal addedByIP As String, ByVal forumID As Integer, ByVal forumTitle As String, _
                ByVal parentPostID As Integer, ByVal title As String, ByVal body As String, _
                ByVal approved As Boolean, ByVal closed As Boolean, ByVal viewCount As Integer, _
                ByVal replyCount As Integer, ByVal lastPostBy As String, ByVal lastPostDate As DateTime)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.AddedByIP = addedByIP
            Me.ForumID = forumID
            Me.ForumTitle = forumTitle
            Me.ParentPostID = parentPostID
            Me.Title = title
            Me.Body = body
            Me.Approved = approved
            Me.Closed = closed
            Me.ViewCount = viewCount
            Me.ReplyCount = replyCount
            Me.LastPostBy = lastPostBy
            Me.LastPostDate = lastPostDate
        End Sub

        ' ==========
        ' Methods
        ' ==========

        Public Function Delete() As Boolean
            Dim success As Boolean = Post.DeletePost(Me.ID)
            If success Then
                Me.ID = 0
            End If
            Return success
        End Function

        Public Function Update() As Boolean
            Return Post.UpdatePost(Me.ID, Me.Title, Me.Body)
        End Function

        Public Function Approve() As Boolean
            Dim ret As Boolean = Post.ApprovePost(Me.ID)
            If ret Then
                Me.Approved = True
            End If
            Return ret
        End Function

        Public Function Move() As Boolean
            If Not Me.IsFirstPost Then
                Return False
            End If

            Dim ret As Boolean = Post.MoveThread(Me.ID, ForumID)
            If ret Then
                Me.ForumID = ForumID
                Dim forum As Forum = BLL.Forums.Forum.GetForumByID(ForumID)
                Me.ForumTitle = forum.Title
                Me.Forum = forum
            End If
            Return ret
        End Function

        Public Function Close() As Boolean
            If Not Me.IsFirstPost Then
                Return False
            End If

            Dim ret As Boolean = Post.CloseThread(Me.ID)
            If ret Then
                Me.Closed = True
            End If
            Return ret
        End Function

        Public Function IncrementViewCount() As Boolean
            Return Post.IncrementPostViewCount(Me.ID)
        End Function

        ' ===========
        ' Shared methods
        ' ===========

        ' Returns a collection with all threads
        Public Shared Function GetThreads() As List(Of Post)
            Return GetThreads("", 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetThreads(ByVal sortExpression As String, ByVal startRowIndex As Integer, _
                ByVal maximumRows As Integer) _
                As List(Of Post)

            If IsNothing(sortExpression) Then sortExpression = ""

            Dim posts As List(Of Post)
            Dim key As String = "Forums_Threads_" & sortExpression & "_" & startRowIndex.ToString() & _
                "_" & maximumRows.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                posts = CType(BizObject.Cache(key), List(Of Post))
            Else
                Dim recordset As List(Of PostDetails) = SiteProvider.Forums.GetThreads( _
                    sortExpression, GetPageIndex(startRowIndex, maximumRows), maximumRows)
                posts = GetPostListFromPostDetailsList(recordset)
                BaseForum.CacheData(key, posts)
            End If
            Return posts
        End Function

        ' Returns a collection with all threads for the specified forum
        Public Shared Function GetThreads(ByVal forumID As Integer) As List(Of Post)
            Return GetThreads(forumID, "", 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetThreads(ByVal forumID As Integer, ByVal sortExpression As String, _
                ByVal startRowIndex As Integer, ByVal maximumRows As Integer) _
                As List(Of Post)

            If forumID <= 0 Then
                Return GetThreads(sortExpression, startRowIndex, maximumRows)
            End If

            Dim posts As List(Of Post)
            Dim key As String = "Forums_Threads_" & forumID.ToString() & "_" & sortExpression & _
                "_" & startRowIndex.ToString() & "_" & maximumRows.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                posts = CType(BizObject.Cache(key), List(Of Post))
            Else
                Dim recordset As List(Of PostDetails) = SiteProvider.Forums.GetThreads( _
                    forumID, sortExpression, GetPageIndex(startRowIndex, maximumRows), maximumRows)
                posts = GetPostListFromPostDetailsList(recordset)
                BaseForum.CacheData(key, posts)
            End If
            Return posts
        End Function

        ' Returns the number of total threads
        Public Shared Function GetThreadCount() As Integer
            Dim postCount As Integer = 0
            Dim key As String = "Forums_ThreadCount"

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                postCount = CInt(BizObject.Cache(key))
            Else
                postCount = SiteProvider.Forums.GetThreadCount()
                BaseForum.CacheData(key, postCount)
            End If
            Return postCount
        End Function

        ' Returns the number of total posts for the specified forum
        Public Shared Function GetThreadCount(ByVal forumID As Integer) As Integer
            If forumID <= 0 Then Return GetThreadCount()

            Dim postCount As Integer = 0
            Dim key As String = "Forums_ThreadCount_" & forumID.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                postCount = CInt(BizObject.Cache(key))
            Else
                postCount = SiteProvider.Forums.GetThreadCount(forumID)
                BaseForum.CacheData(key, postCount)
            End If
            Return postCount
        End Function

        ' Returns a collection with all unapproved posts
        Public Shared Function GetUnapprovedPosts() As List(Of Post)
            Dim posts As List(Of Post)
            Dim key As String = "Forums_UnapprovedPosts"

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                posts = CType(BizObject.Cache(key), List(Of Post))
            Else
                Dim recordset As List(Of PostDetails) = SiteProvider.Forums.GetUnapprovedPosts()
                posts = GetPostListFromPostDetailsList(recordset)
                BaseForum.CacheData(key, posts)
            End If
            Return posts
        End Function

        ' Returns the collection of Post object for the specified thread
        Public Shared Function GetThreadByID(ByVal threadPostID As Integer) As List(Of Post)
            Return GetThreadByID(threadPostID, 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetThreadByID(ByVal threadPostID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Post)
            Dim posts As List(Of Post)
            Dim key As String = "Forums_Thread_" & threadPostID.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                posts = CType(BizObject.Cache(key), List(Of Post))
            Else
                Dim recordset As List(Of PostDetails) = SiteProvider.Forums.GetThreadByID(threadPostID)
                posts = GetPostListFromPostDetailsList(recordset)
                BaseForum.CacheData(key, posts)
            End If

            Dim count As Integer = maximumRows
            If posts.Count < startRowIndex + maximumRows Then count = posts.Count - startRowIndex
            Dim array As Post()
            ReDim array(count - 1)
            posts.CopyTo(startRowIndex, array, 0, count) ' was count
            posts.Clear()
            posts.AddRange(array)
            Return posts
        End Function

        ' Returns the number of total posts for the specified thread
        Public Shared Function GetPostCountByThread(ByVal threadPostID As Integer) As Integer
            Return SiteProvider.Forums.GetPostCountByThread(threadPostID)
        End Function

        ' Returns a Post object with the specified ID
        Public Shared Function GetPostByID(ByVal postID As Integer) As Post
            Dim _post As Post
            Dim key As String = "Forums_Post_" & postID.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                _post = CType(BizObject.Cache(key), Post)
            Else
                _post = GetPostFromPostDetails(SiteProvider.Forums.GetPostByID(postID))
                BaseForum.CacheData(key, _post)
            End If
            Return _post
        End Function

        ' Updates an existing post
        Public Shared Function UpdatePost(ByVal id As Integer, ByVal title As String, ByVal body As String) _
                As Boolean

            title = BizObject.ConvertNullToEmptyString(title)
            body = BizObject.ConvertNullToEmptyString(body)

            Dim record As New PostDetails(id, DateTime.Now, "", "", 0, "", 0, title, body, True, _
                False, 0, 0, DateTime.Now, "")
            Dim ret As Boolean = SiteProvider.Forums.UpdatePost(record)

            BizObject.PurgeCacheItems("forums_unapprovedposts")
            BizObject.PurgeCacheItems("forums_threads")
            BizObject.PurgeCacheItems("forums_threadcount")
            BizObject.PurgeCacheItems("forums_thread_" & id.ToString())
            BizObject.PurgeCacheItems("forums_post_" & id.ToString())
            Return ret
        End Function

        ' Creates a new post
        Public Shared Function InsertPost(ByVal forumID As Integer, ByVal parentPostID As Integer, _
                ByVal title As String, ByVal body As String, ByVal closed As Boolean) _
                As Integer

            title = BizObject.ConvertNullToEmptyString(title)
            body = BizObject.ConvertNullToEmptyString(body)

            ' if the target forum is moderated, the current user must be an 
            ' admin, editor or moderator to insert the post in approved status
            Dim approved As Boolean = True
            Dim forum As Forum = BLL.Forums.Forum.GetForumByID(forumID)
            If (forum.Moderated) Then
                If Not BizObject.CurrentUser.IsInRole("Administrators") AndAlso _
                   Not BizObject.CurrentUser.IsInRole("Editors") AndAlso _
                   Not BizObject.CurrentUser.IsInRole("Moderators") Then
                    approved = False
                End If
            End If

            Dim record As New PostDetails(0, DateTime.Now, BizObject.CurrentUserName, _
                BizObject.CurrentUserIP, forumID, "", parentPostID, title, body, approved, closed, _
                0, 0, DateTime.Now, BizObject.CurrentUserName)
            Dim ret As Integer = SiteProvider.Forums.InsertPost(record)

            If approved Then
                BizObject.PurgeCacheItems("forums_threads")
                BizObject.PurgeCacheItems("forums_thread_" & parentPostID.ToString())
                BizObject.PurgeCacheItems("forums_threadcount")
            Else
                BizObject.PurgeCacheItems("forums_unapprovedposts")
            End If
            Return ret
        End Function

        ' Deletes an existing post and all the child posts
        Public Shared Function DeletePost(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Forums.DeletePost(id)
            Dim ev As New RecordDeletedEvent("post", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("forums_unapprovedposts")
            BizObject.PurgeCacheItems("forums_threads")
            BizObject.PurgeCacheItems("forums_threadcount")
            BizObject.PurgeCacheItems("forums_thread_" & id.ToString())
            BizObject.PurgeCacheItems("forums_post_" & id.ToString())
            Return ret
        End Function

        ' Approves an existing post
        Public Shared Function ApprovePost(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Forums.ApprovePost(id)
            BizObject.PurgeCacheItems("forums_unapprovedposts")
            BizObject.PurgeCacheItems("forums_threads")
            BizObject.PurgeCacheItems("forums_threadcount")
            BizObject.PurgeCacheItems("forums_thread_" & id.ToString())
            BizObject.PurgeCacheItems("forums_post_" & id.ToString())
            Return ret
        End Function

        ' Increments an post's view count
        Public Shared Function IncrementPostViewCount(ByVal id As Integer) As Boolean
            Return SiteProvider.Forums.IncrementPostViewCount(id)
        End Function

        ' Moves a thread to a different forum
        Public Shared Function MoveThread(ByVal threadPostID As Integer, ByVal forumID As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Forums.MoveThread(threadPostID, forumID)
            BizObject.PurgeCacheItems("forums_unapprovedposts")
            BizObject.PurgeCacheItems("forums_threads")
            BizObject.PurgeCacheItems("forums_threadcount")
            BizObject.PurgeCacheItems("forums_thread_" & threadPostID.ToString())
            BizObject.PurgeCacheItems("forums_post_" & threadPostID.ToString())
            Return ret
        End Function

        ' Closes a thread
        Public Shared Function CloseThread(ByVal threadPostID As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Forums.CloseThread(threadPostID)
            BizObject.PurgeCacheItems("forums_thread_" & threadPostID.ToString())
            BizObject.PurgeCacheItems("forums_post_" & threadPostID.ToString())
            Return ret
        End Function

        ' Returns a Post object filled with the data taken from the input PostDetails
        Public Shared Function GetPostFromPostDetails(ByVal record As PostDetails) As Post
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Post(record.ID, record.AddedDate, record.AddedBy, record.AddedByIP, _
                   record.ForumID, record.ForumTitle, record.ParentPostID, record.Title, _
                   record.Body, record.Approved, record.Closed, record.ViewCount, _
                   record.ReplyCount, record.LastPostBy, record.LastPostDate)
            End If
        End Function

        ' Returns a list of Post objects filled with the data taken from the input list of PostDetails
        Public Shared Function GetPostListFromPostDetailsList(ByVal recordset As List(Of PostDetails)) As List(Of Post)
            Dim posts As New List(Of Post)
            For Each record As PostDetails In recordset
                posts.Add(GetPostFromPostDetails(record))
            Next
            Return posts
        End Function
    End Class
End Namespace
