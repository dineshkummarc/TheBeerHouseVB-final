Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class MoveThread
        Inherits BasePage

        Private threadID As Integer = 0

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            threadID = Integer.Parse(Me.Request.QueryString("ThreadID"))

            If Not Me.IsPostBack Then
                Dim post As Post = BLL.Forums.Post.GetPostByID(threadID)
                lblThreadTitle.Text = post.Title
                lblForumTitle.Text = post.ForumTitle
                ddlForums.SelectedValue = post.ForumID.ToString()
            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim forumID As Integer = Integer.Parse(ddlForums.SelectedValue)
            Post.MoveThread(threadID, forumID)
            Me.Response.Redirect("~/BrowseThreads.aspx?ForumID=" & forumID.ToString())
        End Sub
    End Class
End Namespace
