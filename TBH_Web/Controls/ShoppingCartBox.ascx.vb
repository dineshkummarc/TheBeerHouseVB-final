Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.UI
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class ShoppingCartBox
        Inherits BaseWebPart

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                If Me.Profile.ShoppingCart.Items.Count > 0 Then
                    repOrderItems.DataSource = Me.Profile.ShoppingCart.Items
                    repOrderItems.DataBind()

                    lblSubtotal.Text = CType(Me.Page, BasePage).FormatPrice(Me.Profile.ShoppingCart.Total)
                    lblSubtotal.Visible = True
                    lblSubtotalHeader.Visible = True
                    panLinkShoppingCart.Visible = True
                    lblCartIsEmpty.Visible = False
                Else
                    lblSubtotal.Visible = False
                    lblSubtotalHeader.Visible = False
                    panLinkShoppingCart.Visible = False
                    lblCartIsEmpty.Visible = True
                End If
            End If
        End Sub
    End Class
End Namespace
