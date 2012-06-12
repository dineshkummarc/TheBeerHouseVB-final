Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageDepartments
        Inherits BasePage

        Protected Sub gvwDepartments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwDepartments.SelectedIndexChanged
            dvwDepartment.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub gvwDepartments_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwDepartments.RowDeleted
            gvwDepartments.SelectedIndex = -1
            gvwDepartments.DataBind()
            dvwDepartment.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwDepartments_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwDepartments.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(4).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this department?') == false) return false;"
            End If
        End Sub

        Protected Sub dvwDepartment_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwDepartment.ItemInserted
            gvwDepartments.SelectedIndex = -1
            gvwDepartments.DataBind()
        End Sub

        Protected Sub dvwDepartment_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwDepartment.ItemUpdated
            gvwDepartments.SelectedIndex = -1
            gvwDepartments.DataBind()
        End Sub

        Protected Sub dvwDepartment_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwDepartment.ItemCreated
            If dvwDepartment.CurrentMode = DetailsViewMode.Insert Then
                Dim txtImportance As TextBox = CType(dvwDepartment.FindControl("txtImportance"), TextBox)
                txtImportance.Text = "0"
            End If
        End Sub

        Protected Sub dvwDepartment_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwDepartment.ItemCommand
            If e.CommandName = "Cancel" Then
                gvwDepartments.SelectedIndex = -1
                gvwDepartments.DataBind()
            End If
        End Sub
    End Class
End Namespace
