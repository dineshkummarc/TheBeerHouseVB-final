Imports System.Collections.Generic
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageOrders
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                txtToDate.Text = DateTime.Now.ToShortDateString()
                txtFromDate.Text = DateTime.Now.Subtract( _
                   New TimeSpan(Globals.Settings.Store.DefaultOrderListInterval, 0, 0, 0)).ToShortDateString()
            End If

            lblOrderNotFound.Visible = False
            ' if the user is not an admin, hide the grid's column with the delete button
            gvwOrders.Columns(6).Visible = (Me.User.IsInRole("Administrators"))
        End Sub

        Protected Sub DoBinding()
            Dim listByCustomers As Boolean = False
            If Not String.IsNullOrEmpty(gvwOrders.Attributes("ListByCustomers")) Then _
                listByCustomers = Boolean.Parse(gvwOrders.Attributes("ListByCustomers"))

            Dim orders As List(Of Order)
            If listByCustomers Then
                orders = Order.GetOrders(txtCustomerName.Text)
            Else
                orders = Order.GetOrders(Convert.ToInt32(ddlOrderStatuses.SelectedValue), _
                   Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text))
            End If

            gvwOrders.DataSource = orders
            gvwOrders.DataBind()
        End Sub

        Protected Sub gvwOrders_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwOrders.RowDeleting
            Dim orderID As Integer = Convert.ToInt32(gvwOrders.DataKeys(e.RowIndex)(0))
            Order.DeleteOrder(orderID)
            DoBinding()
        End Sub

        Protected Sub gvwOrders_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwOrders.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(6).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this order?') == false) return false;"
            End If
        End Sub

        Protected Sub btnOrderLookup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrderLookup.Click
            ' if the order with the specified ID is not found, show the error label,
            ' otherwise redirect to EditOrder.aspx with the ID on the querystring
            Dim order As Order = BLL.Store.Order.GetOrderByID(Convert.ToInt32(txtOrderID.Text))
            If IsNothing(order) Then
                lblOrderNotFound.Visible = True
            Else
                Me.Response.Redirect("EditOrder.aspx?ID=" & txtOrderID.Text)
            End If
        End Sub

        Protected Sub btnListByCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnListByCustomer.Click
            gvwOrders.Attributes.Add("ListByCustomers", True.ToString())
            DoBinding()
        End Sub

        Protected Sub btnListByStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnListByStatus.Click
            gvwOrders.Attributes.Add("ListByCustomers", False.ToString())
            DoBinding()
        End Sub
    End Class
End Namespace
