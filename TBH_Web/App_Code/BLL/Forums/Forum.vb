Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Forums
    Public Class Forum
        Inherits BaseForum

        ' ==========
        ' Private variables
        ' ==========

        Private _title As String = ""
        Private _moderated As Boolean = False
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageUrl As String = ""
        Private _allThreads As List(Of Post)

        ' ==========
        ' Properties
        ' ==========

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Moderated() As Boolean
            Get
                Return _moderated
            End Get
            Set(ByVal value As Boolean)
                _moderated = value
            End Set
        End Property

        Public Property Importance() As Integer
            Get
                Return _importance
            End Get
            Private Set(ByVal value As Integer)
                _importance = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property

        Public ReadOnly Property AllThread() As List(Of Post)
            Get
                If IsNothing(_allThreads) Then
                    _allThreads = Post.GetThreads(Me.ID, "", 0, BizObject.MAXROWS)
                End If
                Return _allThreads
            End Get
        End Property

        ' ===========
        ' Constructor
        ' ===========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal title As String, ByVal moderated As Boolean, ByVal importance As Integer, _
                ByVal description As String, ByVal imageUrl As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Moderated = moderated
            Me.Importance = importance
            Me.Description = description
            Me.ImageUrl = imageUrl
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = Forum.DeleteForum(Me.ID)
            If success Then
                Me.ID = 0
            End If
            Return success
        End Function

        Public Function Update() As Boolean
            Return Forum.UpdateForum(Me.ID, Me.Title, Me.Moderated, Me.Importance, _
                Me.Description, Me.ImageUrl)
        End Function

        ' ==========
        ' Shared methods
        ' ==========

        ' Returns a collection with all the forums
        Public Shared Function GetForums() As List(Of Forum)
            Dim forums As List(Of Forum)
            Dim key As String = "Forums_Forums"

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                forums = CType(BizObject.Cache(key), List(Of Forum))
            Else
                Dim recordset As List(Of ForumDetails) = SiteProvider.Forums.GetForums()
                forums = GetForumListFromForumDetailsList(recordset)
                BaseForum.CacheData(key, forums)
            End If
            Return forums
        End Function

        ' Returns a Forum object with the specified ID
        Public Shared Function GetForumByID(ByVal forumID As Integer) As Forum
            Dim forum As Forum
            Dim key As String = "Forums_Forum_" & forumID.ToString()

            If BaseForum.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                forum = CType(BizObject.Cache(key), Forum)
            Else
                forum = GetForumFromForumDetails(SiteProvider.Forums.GetForumByID(forumID))
                BaseForum.CacheData(key, forum)
            End If
            Return forum
        End Function

        ' Updates an existing forum
        Public Shared Function UpdateForum(ByVal id As Integer, ByVal title As String, _
                ByVal moderated As Boolean, ByVal importance As Integer, ByVal description As String, _
                ByVal imageUrl As String) _
                As Boolean

            Dim record As New ForumDetails(id, DateTime.Now, "", title, moderated, importance, description, imageUrl)
            Dim ret As Boolean = SiteProvider.Forums.UpdateForum(record)
            BizObject.PurgeCacheItems("forums_forum")
            Return ret
        End Function

        ' Deletes an existing forum
        Public Shared Function DeleteForum(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Forums.DeleteForum(id)
            Dim ev As New RecordDeletedEvent("forum", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("forums_forum")
            Return ret
        End Function

        ' Creates a new forum
        Public Shared Function InsertForum(ByVal title As String, ByVal moderated As Boolean, _
                ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String) _
                As Integer

            Dim record As New ForumDetails(0, DateTime.Now, _
               BizObject.CurrentUserName, title, moderated, importance, description, imageUrl)
            Dim ret As Integer = SiteProvider.Forums.InsertForum(record)
            BizObject.PurgeCacheItems("forums_forum")
            Return ret
        End Function

        ' Returns a Forum object filled with the data taken from the input ForumDetails
        Public Shared Function GetForumFromForumDetails(ByVal record As ForumDetails) As Forum
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Forum(record.ID, record.AddedDate, record.AddedBy, _
                   record.Title, record.Moderated, record.Importance, record.Description, record.ImageURL)
            End If
        End Function

        ' Returns a list of Forum objects filled with the data taken from the input list of ForumDetails
        Public Shared Function GetForumListFromForumDetailsList(ByVal recordset As List(Of ForumDetails)) As List(Of Forum)
            Dim forums As New List(Of Forum)
            For Each record As ForumDetails In recordset
                forums.Add(GetForumFromForumDetails(record))
            Next
            Return forums
        End Function
    End Class
End Namespace
