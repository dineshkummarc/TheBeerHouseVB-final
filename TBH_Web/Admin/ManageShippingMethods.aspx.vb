Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageShippingMethods
        Inherits BasePage

        Protected Sub gvwShippingMethods_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwShippingMethods.SelectedIndexChanged
            dvwShippingMethod.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub gvwShippingMethods_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwShippingMethods.RowDeleted
            gvwShippingMethods.SelectedIndex = -1
            gvwShippingMethods.DataBind()
            dvwShippingMethod.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwShippingMethods_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwShippingMethods.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(3).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this shipping method?') == false) return false;"
            End If
        End Sub

        Protected Sub dvwShippingMethod_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwShippingMethod.ItemInserted
            gvwShippingMethods.SelectedIndex = -1
            gvwShippingMethods.DataBind()
        End Sub

        Protected Sub dvwShippingMethod_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwShippingMethod.ItemUpdated
            gvwShippingMethods.SelectedIndex = -1
            gvwShippingMethods.DataBind()
        End Sub

        Protected Sub dvwShippingMethod_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwShippingMethod.ItemCommand
            If e.CommandName = "Cancel" Then
                gvwShippingMethods.SelectedIndex = -1
                gvwShippingMethods.DataBind()
            End If
        End Sub
    End Class
End Namespace