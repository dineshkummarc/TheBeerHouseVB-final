Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageComments
        Inherits BasePage

        Protected Sub ddlCommentsPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCommentsPerPage.SelectedIndexChanged
            gvwComments.PageSize = Integer.Parse(ddlCommentsPerPage.SelectedValue)
            CancelCurrentEdit()
        End Sub

        Protected Sub gvwComments_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwComments.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(2).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this comment?') == false) return false;"
            End If
        End Sub

        Protected Sub gvwComments_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwComments.PageIndexChanged
            CancelCurrentEdit()
        End Sub

        Protected Sub gvwComments_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwComments.RowDeleted
            CancelCurrentEdit()
        End Sub

        Protected Sub gvwComments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwComments.SelectedIndexChanged
            dvwComment.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub dvwComment_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwComment.ItemCommand
            If e.CommandName = "Cancel" Then
                CancelCurrentEdit()
            End If
        End Sub

        Protected Sub dvwComment_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwComment.ItemUpdated
            CancelCurrentEdit()
        End Sub

        Private Sub CancelCurrentEdit()
            dvwComment.ChangeMode(DetailsViewMode.ReadOnly)
            gvwComments.SelectedIndex = -1
            gvwComments.DataBind()
        End Sub
    End Class
End Namespace
