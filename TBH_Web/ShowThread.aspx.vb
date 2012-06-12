Imports System.Collections.Generic
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI
    Partial Class ShowThread
        Inherits BasePage

        Dim threadPostID As Integer = 0
        Dim profiles As New Hashtable()

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            gvwPosts.PageSize = Globals.Settings.Forums.PostsPageSize
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            threadPostID = Integer.Parse(Me.Request.QueryString("ID"))

            If Not Me.IsPostBack Then
                threadPostID = Integer.Parse(Me.Request.QueryString("ID"))
                Dim post As Post = BLL.Forums.Post.GetPostByID(threadPostID)
                post.IncrementViewCount()
                Me.Title = String.Format(Me.Title, post.Title)
                lblPageTitle.Text = String.Format(lblPageTitle.Text, post.ForumID, post.ForumTitle, post.Title)
                ShowCommandButtons(post.Closed, post.ForumID, threadPostID, post.AddedBy)
            End If
        End Sub

        Private Sub ShowCommandButtons(ByVal isClosed As Boolean, ByVal forumID As Integer, ByVal threadPostID As Integer, ByVal addedBy As String)
            If isClosed Then
                lnkNewReply1.Visible = False
                lnkNewReply2.Visible = False
                btnCloseThread1.Visible = False
                btnCloseThread2.Visible = False
                panClosed.Visible = True
            Else
                lnkNewReply1.NavigateUrl = String.Format(lnkNewReply1.NavigateUrl, forumID, threadPostID)
                lnkNewReply2.NavigateUrl = lnkNewReply1.NavigateUrl
                btnCloseThread1.Visible = (Me.User.Identity.IsAuthenticated And _
                   (Me.User.Identity.Name.ToLower().Equals(addedBy) Or _
                   (Me.User.IsInRole("Administrators") Or Me.User.IsInRole("Editors") Or Me.User.IsInRole("Moderators"))))
                btnCloseThread2.Visible = btnCloseThread1.Visible
            End If
        End Sub

        Protected Sub gvwPosts_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwPosts.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim post As Post = CType(e.Row.DataItem, BLL.Forums.Post)
                Dim threadID As Integer = post.ParentPostID
                If post.IsFirstPost Then threadID = post.ID

                ' the link for editing the post is visible to the post's author, and to
                ' administrators, editors and moderators
                Dim lnkEditPost As HyperLink = CType(e.Row.FindControl("lnkEditPost"), HyperLink)
                lnkEditPost.NavigateUrl = String.Format(lnkEditPost.NavigateUrl, post.ForumID, threadID, post.ID)
                lnkEditPost.Visible = (Me.User.Identity.IsAuthenticated And _
                   (Me.User.Identity.Name.ToLower().Equals(post.AddedBy.ToLower()) Or _
                   (Me.User.IsInRole("Administrators") Or Me.User.IsInRole("Editors") Or Me.User.IsInRole("Moderators"))))

                ' the link for deleting the thread/post is visible only to administrators, editors and moderators
                Dim btnDeletePost As ImageButton = CType(e.Row.FindControl("btnDeletePost"), ImageButton)
                If post.IsFirstPost Then
                    btnDeletePost.OnClientClick = String.Format(btnDeletePost.OnClientClick, "entire thread")
                    btnDeletePost.CommandName = "DeleteThread"
                Else
                    btnDeletePost.OnClientClick = String.Format(btnDeletePost.OnClientClick, "post")
                    btnDeletePost.CommandName = "DeletePost"
                End If
                btnDeletePost.CommandArgument = post.ID.ToString()
                btnDeletePost.Visible = (Me.User.IsInRole("Administrators") Or Me.User.IsInRole("Editors") Or Me.User.IsInRole("Moderators"))

                ' if the thread is not closed, show the link to quote the post
                Dim lnkQuotePost As HyperLink = CType(e.Row.FindControl("lnkQuotePost"), HyperLink)
                lnkQuotePost.NavigateUrl = String.Format(lnkQuotePost.NavigateUrl, _
                   post.ForumID, threadID, post.ID)
                If post.IsFirstPost Then
                    lnkQuotePost.Visible = Not post.Closed
                Else
                    lnkQuotePost.Visible = Not post.ParentPost.Closed
                End If
            End If
        End Sub

        Protected Sub gvwPosts_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwPosts.RowCommand
            If e.CommandName = "DeleteThread" Then
                Dim threadPostID As Integer = Convert.ToInt32(e.CommandArgument)
                Dim forumID As Integer = Post.GetPostByID(threadPostID).ID
                Post.DeletePost(threadPostID)
                Me.Response.Redirect("BrowseThreads.aspx?ForumID=" & forumID.ToString())
            ElseIf e.CommandName = "DeletePost" Then
                Dim postID As Integer = Convert.ToInt32(e.CommandArgument)
                Post.DeletePost(postID)
                gvwPosts.PageIndex = 0
                gvwPosts.DataBind()
            End If
        End Sub

        Protected Sub btnCloseThread_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseThread1.Click
            Post.CloseThread(threadPostID)
            ShowCommandButtons(True, 0, 0, "")
            gvwPosts.DataBind()
        End Sub

        ' Retrieves and returns the profile of the specified user. The profile is cached once
        ' retrieved for the first time, so that it is reused if the profile for the same user
        ' will be requested more times on the same request
        Protected Function GetUserProfile(ByVal userName As Object) As ProfileCommon
            Dim name As String = CStr(userName)
            If Not profiles.Contains(name) Then
                Dim profile As ProfileCommon = Me.Profile.GetProfile(name)
                profiles.Add(name, profile)
                Return profile
            Else
                Return CType(profiles(userName), ProfileCommon)
            End If
        End Function

        ' Returns the poster level description, according to the input post count
        Protected Function GetPosterDescription(ByVal posts As Integer) As String
            If posts >= Globals.Settings.Forums.GoldPosterPosts Then
                Return Globals.Settings.Forums.GoldPosterDescription
            ElseIf posts >= Globals.Settings.Forums.SilverPosterPosts Then
                Return Globals.Settings.Forums.SilverPosterDescription
            ElseIf posts >= Globals.Settings.Forums.BronzePosterPosts Then
                Return Globals.Settings.Forums.BronzePosterDescription
            Else
                Return ""
            End If
        End Function
    End Class
End Namespace
