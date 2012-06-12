Imports FredCK.FCKeditorV2
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class EditOrder
        Inherits BasePage

        Protected Sub dvwOrder_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwOrder.DataBound
            Dim txtShippedDate As TextBox = CType(dvwOrder.FindControl("txtShippedDate"), TextBox)
            If Convert.ToDateTime(txtShippedDate.Text) = DateTime.MinValue Then _
               txtShippedDate.Text = ""
        End Sub
    End Class
End Namespace