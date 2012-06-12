Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageCategories
        Inherits BasePage

        Protected Sub gvwCategories_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwCategories.RowDeleted
            gvwCategories.SelectedIndex = -1
            gvwCategories.DataBind()
            dvwCategory.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwCategories_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwCategories.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(4).Controls(0), ImageButton)
                btn.OnClientClick = _
                    " if (confirm('Are you sure you want to delete this category?') == false) return false; "
            End If
        End Sub

        Protected Sub gvwCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwCategories.SelectedIndexChanged
            dvwCategory.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub dvwCategory_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwCategory.ItemInserted
            gvwCategories.SelectedIndex = -1
            gvwCategories.DataBind()
        End Sub

        Protected Sub dvwCategory_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwCategory.ItemUpdated
            gvwCategories.SelectedIndex = -1
            gvwCategories.DataBind()
        End Sub

        Protected Sub dvwCategory_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwCategory.ItemCreated
            If dvwCategory.CurrentMode = DetailsViewMode.Insert Then
                Dim txtImportance As TextBox = CType(dvwCategory.FindControl("txtImportance"), TextBox)
                txtImportance.Text = "0"
            End If
        End Sub

        Protected Sub dvwCategory_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwCategory.ItemCommand
            If e.CommandName = "Cancel" Then
                gvwCategories.SelectedIndex = -1
                gvwCategories.DataBind()
            End If
        End Sub
    End Class
End Namespace
