Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class OrderHistory
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                dlstOrders.DataSource = Order.GetOrders(Me.User.Identity.Name)
                dlstOrders.DataBind()
            End If
        End Sub
    End Class
End Namespace