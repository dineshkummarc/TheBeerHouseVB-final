Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageOrderStatuses
        Inherits BasePage

        Protected Sub gvwOrderStatuses_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwOrderStatuses.SelectedIndexChanged
            dvwOrderStatus.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub gvwOrderStatuses_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwOrderStatuses.RowDeleted
            gvwOrderStatuses.SelectedIndex = -1
            gvwOrderStatuses.DataBind()
            dvwOrderStatus.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwOrderStatuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwOrderStatuses.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(2).Controls(0), ImageButton)
                ' if this row if for a order status with ID from 1 to 3, don't allow to delete it
                ' because it's a built-in status that can only be renamed. Otherwise add the confirmation dialog to it
                Dim orderStatusID As Integer = Convert.ToInt32(gvwOrderStatuses.DataKeys(e.Row.RowIndex)(0))
                If orderStatusID > 3 Then
                    btn.OnClientClick = "if (confirm('Are you sure you want to delete this order status?') == false) return false;"
                Else
                    btn.Visible = False
                End If
            End If
        End Sub

        Protected Sub dvwOrderStatus_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwOrderStatus.ItemInserted
            gvwOrderStatuses.SelectedIndex = -1
            gvwOrderStatuses.DataBind()
        End Sub

        Protected Sub dvwOrderStatus_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwOrderStatus.ItemUpdated
            gvwOrderStatuses.SelectedIndex = -1
            gvwOrderStatuses.DataBind()
        End Sub

        Protected Sub dvwOrderStatus_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwOrderStatus.ItemCommand
            If e.CommandName = "Cancel" Then
                gvwOrderStatuses.SelectedIndex = -1
                gvwOrderStatuses.DataBind()
            End If
        End Sub
    End Class
End Namespace
