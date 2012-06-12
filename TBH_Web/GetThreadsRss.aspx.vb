Imports System.Collections.Generic
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI
    Partial Class GetThreadsRss
        Inherits BasePage

        Private _rssTitle As String = "Latest Threads"
        Public Property RssTitle() As String
            Get
                Return _rssTitle
            End Get
            Set(ByVal value As String)
                _rssTitle = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim forumID As Integer = 0
            ' if a ForumID param is passed on the querystring, load the Forum with that ID,
            ' and use its title for the RSS's title
            If Not String.IsNullOrEmpty(Me.Request.QueryString("ForumID")) Then
                forumID = Integer.Parse(Me.Request.QueryString("ForumID"))
                Dim forum As Forum = BLL.Forums.Forum.GetForumByID(forumID)
                _rssTitle = forum.Title
            End If

            Dim sortExpr As String = ""
            If Not String.IsNullOrEmpty(Me.Request.QueryString("SortExpr")) Then
                sortExpr = Me.Request.QueryString("SortExpr")
            End If

            Dim posts As List(Of Post) = Post.GetThreads(forumID, sortExpr, 0, Globals.Settings.Forums.RssItems)
            rptRss.DataSource = posts
            rptRss.DataBind()
        End Sub
    End Class
End Namespace