Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class OrderCompleted
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim order As Order = BLL.Store.Order.GetOrderByID(Convert.ToInt32(Me.Request.QueryString("ID")))
            If order.StatusID = CInt(StatusCode.WaitingForPayment) Then
                order.StatusID = CInt(StatusCode.Confirmed)
                order.Update()
            End If
        End Sub
    End Class
End Namespace