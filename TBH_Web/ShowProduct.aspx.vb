Imports System.Security
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class ShowProduct
        Inherits BasePage

        Private _userCanEdit As Boolean = False
        Protected Property UserCanEdit() As Boolean
            Get
                Return _userCanEdit
            End Get
            Set(ByVal value As Boolean)
                _userCanEdit = value
            End Set
        End Property

        Private _productID As Integer = 0

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            UserCanEdit = (Me.User.Identity.IsAuthenticated AndAlso _
               (Me.User.IsInRole("Administrators") OrElse Me.User.IsInRole("StoreKeepers")))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If String.IsNullOrEmpty(Me.Request.QueryString("ID")) Then
                Throw New ApplicationException("Missing parameter on the querystring.")
            Else
                _productID = Integer.Parse(Me.Request.QueryString("ID"))
            End If

            If Not Me.IsPostBack Then
                ' try to load the product with the specified ID, and raise an exception if it doesn't exist
                Dim product As Product = BLL.Store.Product.GetProductByID(_productID)
                If IsNothing(product) Then _
                   Throw New ApplicationException("No product was found for the specified ID.")

                ' display all article's data on the page
                Me.Title = String.Format(Me.Title, product.Title)
                lblTitle.Text = product.Title
                lblRating.Text = String.Format(lblRating.Text, product.Votes)
                ratDisplay.Value = product.AverageRating
                ratDisplay.Visible = (product.Votes > 0)
                availDisplay.Value = product.UnitsInStock
                lblDescription.Text = product.Description
                panEditProduct.Visible = Me.UserCanEdit
                lnkEditProduct.NavigateUrl = String.Format(lnkEditProduct.NavigateUrl, _productID)
                lblPrice.Text = Me.FormatPrice(product.FinalUnitPrice)
                lblDiscountedPrice.Text = String.Format(lblDiscountedPrice.Text, _
                   Me.FormatPrice(product.UnitPrice), product.DiscountPercentage)
                lblDiscountedPrice.Visible = (product.DiscountPercentage > 0)
                If product.SmallImageUrl.Length > 0 Then _
                    imgProduct.ImageUrl = product.SmallImageUrl
                If product.FullImageUrl.Length > 0 Then
                    lnkFullImage.NavigateUrl = product.FullImageUrl
                    lnkFullImage.Visible = True
                Else
                    lnkFullImage.Visible = False
                End If

                ' hide the rating box controls if the current user has already voted for this product
                Dim userRating As Integer = GetUserRating()
                If userRating > 0 Then _
                    ShowUserRating(userRating)
            End If
        End Sub

        Protected Sub btnRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRate.Click
            ' check whether the user has already rated this article
            Dim userRating As Integer = GetUserRating()
            If userRating > 0 Then
                ShowUserRating(userRating)
            Else
                ' rate the product, then create a cookie to remember this user's rating
                userRating = ddlRatings.SelectedIndex + 1
                Product.RateProduct(_productID, userRating)
                ShowUserRating(userRating)

                Dim cookie As New HttpCookie( _
                   "Rating_Product" & _productID.ToString(), userRating.ToString())
                cookie.Expires = DateTime.Now.AddDays(Globals.Settings.Store.RatingLockInterval)
                Me.Response.Cookies.Add(cookie)
            End If
        End Sub

        Protected Sub ShowUserRating(ByVal rating As Integer)
            lblUserRating.Text = String.Format(lblUserRating.Text, rating)
            ddlRatings.Visible = False
            btnRate.Visible = False
            lblUserRating.Visible = True
        End Sub

        Protected Function GetUserRating() As Integer
            Dim rating As Integer = 0
            Dim cookie As HttpCookie = Me.Request.Cookies("Rating_Product" & _productID.ToString())
            If Not IsNothing(cookie) Then _
               rating = Integer.Parse(cookie.Value)
            Return rating
        End Function

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
            Product.DeleteProduct(_productID)
            Me.Response.Redirect("BrowseProducts.aspx", False)
        End Sub

        Protected Sub btnAddToCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCart.Click
            Dim product As Product = BLL.Store.Product.GetProductByID(_productID)
            Me.Profile.ShoppingCart.InsertItem(product.ID, product.Title, product.SKU, product.FinalUnitPrice)
            Me.Response.Redirect("ShoppingCart.aspx", False)
        End Sub
    End Class
End Namespace