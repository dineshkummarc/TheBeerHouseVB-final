Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageForums
        Inherits BasePage

        Protected Sub gvwForums_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwForums.SelectedIndexChanged
            dvwForums.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub gvwForums_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwForums.RowDeleted
            gvwForums.SelectedIndex = -1
            gvwForums.DataBind()
            dvwForums.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwForums_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwForums.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(3).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this forum?') == false) return false;"
            End If
        End Sub

        Protected Sub dvwForums_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwForums.ItemInserted
            gvwForums.SelectedIndex = -1
            gvwForums.DataBind()
        End Sub

        Protected Sub dvwForums_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwForums.ItemUpdated
            gvwForums.SelectedIndex = -1
            gvwForums.DataBind()
        End Sub

        Protected Sub dvwForums_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwForums.ItemCreated
            If dvwForums.CurrentMode = DetailsViewMode.Insert Then
                Dim txtImportance As TextBox = CType(dvwForums.FindControl("txtImportance"), TextBox)
                txtImportance.Text = "0"
            End If
        End Sub

        Protected Sub dvwForums_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwForums.ItemCommand
            If e.CommandName = "Cancel" Then
                gvwForums.SelectedIndex = -1
                gvwForums.DataBind()
            End If
        End Sub
    End Class
End Namespace
