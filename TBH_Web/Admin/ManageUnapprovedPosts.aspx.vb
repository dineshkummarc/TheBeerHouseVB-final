Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageUnapprovedPosts
        Inherits BasePage

        Protected Sub gvwPosts_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwPosts.RowCommand
            If e.CommandName = "Approve" Then
                Dim postID As Integer = Convert.ToInt32( _
                   gvwPosts.DataKeys(Convert.ToInt32(e.CommandArgument))(0))
                MB.TheBeerHouse.BLL.Forums.Post.ApprovePost(postID)
                gvwPosts.EditIndex = -1
                gvwPosts.DataBind()
            End If
        End Sub

        Protected Sub gvwPosts_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwPosts.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btnApprove As ImageButton = CType(e.Row.Cells(3).Controls(0), ImageButton)
                btnApprove.OnClientClick = "if (confirm('Are you sure you want to approve this post?') == false) return false;"
                btnApprove.ToolTip = "Approve this post"
                Dim btnDelete As ImageButton = CType(e.Row.Cells(4).Controls(0), ImageButton)
                btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this post?') == false) return false;"
                btnDelete.ToolTip = "Delete this post"
            End If
        End Sub
    End Class
End Namespace