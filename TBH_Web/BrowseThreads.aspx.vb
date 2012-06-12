Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI
    Partial Class BrowseThreads
        Inherits BasePage

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            gvwThreads.PageSize = Globals.Settings.Forums.ThreadsPageSize
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Master.EnablePersonalization = True

            If Not Me.IsPostBack Then
                Dim forumID As String = Me.Request.QueryString("ForumID")
                lnkNewThread1.NavigateUrl = _
                    String.Format(lnkNewThread1.NavigateUrl, forumID)
                lnkNewThread2.NavigateUrl = lnkNewThread1.NavigateUrl

                Dim forum As Forum = BLL.Forums.Forum.GetForumByID( _
                    Integer.Parse(forumID))
                Me.Title = String.Format(Me.Title, forum.Title)
                ddlForums.SelectedValue = forumID

                ' if the user is not an admin, editor or moderator, hide the grid's column with 
                ' the commands to delete, close or move a thread
                Dim canEdit As Boolean = (Me.User.Identity.IsAuthenticated And _
                   (Me.User.IsInRole("Administrators") Or _
                   Me.User.IsInRole("Editors") Or Me.User.IsInRole("Moderators")))
                gvwThreads.Columns(5).Visible = canEdit
                gvwThreads.Columns(6).Visible = canEdit
                gvwThreads.Columns(7).Visible = canEdit
            End If
        End Sub

        Protected Sub gvwThreads_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwThreads.RowCommand
            If e.CommandName = "Close" Then
                Dim threadPostID As Integer = Convert.ToInt32( _
                   gvwThreads.DataKeys(Convert.ToInt32(e.CommandArgument))(0))

                MB.TheBeerHouse.BLL.Forums.Post.CloseThread(threadPostID)
            End If
        End Sub

        Protected Sub gvwThreads_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwThreads.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btnClose As ImageButton = CType(e.Row.Cells(6).Controls(0), ImageButton)
                btnClose.OnClientClick = "if (confirm('Are you sure you want to close this thread?') == false) return false;"
                btnClose.ToolTip = "Close this thread"
                Dim btnDelete As ImageButton = CType(e.Row.Cells(7).Controls(0), ImageButton)
                btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this thread?') == false) return false;"
            End If
        End Sub
    End Class
End Namespace
