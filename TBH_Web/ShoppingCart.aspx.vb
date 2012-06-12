Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class ShoppingCart
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                ddlShippingMethods.DataBind()
                UpdateTotals()
            Else
                Dim isAuthenticated As Boolean = Me.User.Identity.IsAuthenticated
                If isAuthenticated Then
                    mvwShipping.ActiveViewIndex = 1
                Else
                    mvwShipping.ActiveViewIndex = 0
                    wizSubmitOrder.StartNextButtonText = ""
                End If
            End If
        End Sub

        Protected Sub UpdateTotals()
            ' update the quantities
            For Each row As GridViewRow In gvwOrderItems.Rows
                Dim id As Integer = Convert.ToInt32(gvwOrderItems.DataKeys(row.RowIndex)(0))
                Dim quantity As Integer = Convert.ToInt32(CType(row.FindControl("txtQuantity"), TextBox).Text)
                Me.Profile.ShoppingCart.UpdateItemQuantity(id, quantity)
            Next

            ' display the subtotal and the total amounts
            lblSubtotal.Text = Me.FormatPrice(Me.Profile.ShoppingCart.Total)
            lblTotal.Text = Me.FormatPrice(Me.Profile.ShoppingCart.Total + _
               Convert.ToDecimal(ddlShippingMethods.SelectedValue))

            ' if the shopping cart is empty, hide the link to proceed
            If Me.Profile.ShoppingCart.Items.Count = 0 Then
                wizSubmitOrder.StartNextButtonText = ""
                panTotals.Visible = False
            End If

            gvwOrderItems.DataBind()
        End Sub

        Protected Sub gvwOrderItems_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwOrderItems.RowDeleted
            UpdateTotals()
        End Sub

        Protected Sub btnUpdateTotals_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateTotals.Click
            UpdateTotals()
        End Sub

        Protected Sub gvwOrderItems_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwOrderItems.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(3).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to remove this product from the shopping cart?') == false) return false;"
            End If
        End Sub

        Protected Sub wizSubmitOrder_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles wizSubmitOrder.FinishButtonClick
            ' check that the user is still logged in (the cookie may have expired)
            ' and if not redirect to the login page
            If Me.User.Identity.IsAuthenticated Then
                Dim shippingMethod As String = ddlShippingMethods.SelectedItem.Text
                shippingMethod = shippingMethod.Substring(0, shippingMethod.LastIndexOf("("))

                ' saves the order into the DB, and clear the shopping cart in the profile
                Dim orderID As Integer = BLL.Store.Order.InsertOrder(Me.Profile.ShoppingCart, shippingMethod, Convert.ToDecimal(ddlShippingMethods.SelectedValue), _
                   txtFirstName.Text, txtLastName.Text, txtStreet.Text, txtPostalCode.Text, txtCity.Text, _
                   txtState.Text, ddlCountries.SelectedValue, txtEmail.Text, txtPhone.Text, txtFax.Text, "")

                Me.Profile.ShoppingCart.Clear()

                ' redirect to PayPal for the credit-card payment
                Dim order As Order = BLL.Store.Order.GetOrderByID(orderID)
                Me.Response.Redirect(order.GetPayPalPaymentUrl(), False)
            Else
                Me.RequestLogin()
            End If
        End Sub

        Protected Sub wizSubmitOrder_ActiveStepChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wizSubmitOrder.ActiveStepChanged
            If wizSubmitOrder.ActiveStepIndex = 1 Then
                UpdateTotals()

                If Me.User.Identity.IsAuthenticated Then
                    If ddlCountries.Items.Count = 1 Then
                        ddlCountries.DataSource = Helpers.GetCountries()
                        ddlCountries.DataBind()
                    End If

                    If txtFirstName.Text.Trim().Length = 0 Then _
                       txtFirstName.Text = Me.Profile.FirstName
                    If txtLastName.Text.Trim().Length = 0 Then _
                       txtLastName.Text = Me.Profile.LastName
                    If txtEmail.Text.Trim().Length = 0 Then _
                       txtEmail.Text = Membership.GetUser().Email
                    If txtStreet.Text.Trim().Length = 0 Then _
                       txtStreet.Text = Me.Profile.Address.Street
                    If txtPostalCode.Text.Trim().Length = 0 Then _
                       txtPostalCode.Text = Me.Profile.Address.PostalCode
                    If txtCity.Text.Trim().Length = 0 Then _
                       txtCity.Text = Me.Profile.Address.City
                    If txtState.Text.Trim().Length = 0 Then _
                       txtState.Text = Me.Profile.Address.State

                    ' The original C# code doesn't check the Profile to ensure that ddlCountries doesn't
                    ' get a null value.
                    If ddlCountries.SelectedIndex = 0 AndAlso Not isnothing(Me.Profile.Address.Country) Then _
                       ddlCountries.SelectedValue = Me.Profile.Address.Country

                    If txtPhone.Text.Trim().Length = 0 Then _
                       txtPhone.Text = Me.Profile.Contacts.Phone
                    If txtFax.Text.Trim().Length = 0 Then _
                       txtFax.Text = Me.Profile.Contacts.Fax
                End If
            ElseIf wizSubmitOrder.ActiveStepIndex = 2 Then
                lblReviewFirstName.Text = txtFirstName.Text
                lblReviewLastName.Text = txtLastName.Text
                lblReviewStreet.Text = txtStreet.Text
                lblReviewCity.Text = txtCity.Text
                lblReviewState.Text = txtState.Text
                lblReviewPostalCode.Text = txtPostalCode.Text
                lblReviewCountry.Text = ddlCountries.SelectedValue

                lblReviewSubtotal.Text = Me.FormatPrice(Me.Profile.ShoppingCart.Total)
                lblReviewShippingMethod.Text = ddlShippingMethods.SelectedItem.Text
                lblReviewTotal.Text = Me.FormatPrice(Me.Profile.ShoppingCart.Total + _
                   Convert.ToDecimal(ddlShippingMethods.SelectedValue))
            End If
        End Sub
    End Class
End Namespace