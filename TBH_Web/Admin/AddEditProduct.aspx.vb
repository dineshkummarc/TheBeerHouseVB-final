Imports System.Security
Imports System.Web.Security
Imports MB.TheBeerHouse
Imports FredCK.FCKeditorV2

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class AddEditProduct
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                ' if a ID param is present on the querystring, switch to Edit mode for that product,
                ' but only after checking that the current user is an Administrator or an Editor
                If Not String.IsNullOrEmpty(Me.Request.QueryString("ID")) Then
                    If Me.User.Identity.IsAuthenticated AndAlso _
                       (Me.User.IsInRole("Administrators") OrElse Me.User.IsInRole("Editors")) Then
                        dvwProduct.ChangeMode(DetailsViewMode.Edit)
                        UpdateTitle()
                    Else
                        Throw New SecurityException("You are not allowed to edit existent products!")
                    End If
                End If
            End If
        End Sub

        Protected Sub dvwProduct_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwProduct.ItemCreated
            Dim ctl As Control = dvwProduct.FindControl("txtDescription")
            If Not IsNothing(ctl) Then
                Dim txtDescription As FCKeditor = CType(ctl, FCKeditor)
                txtDescription.BasePath = Me.BaseUrl & "FCKeditor/"
            End If
        End Sub

        Protected Sub dvwProduct_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwProduct.DataBound
            ' In InserMode, set the UnitsInStock to 1000000 and DiscountPercentage to 0
            If dvwProduct.CurrentMode = DetailsViewMode.Insert Then
                CType(dvwProduct.FindControl("txtUnitsInStock"), TextBox).Text = "1000000"
                CType(dvwProduct.FindControl("txtDiscountPercentage"), TextBox).Text = "0"
            End If
        End Sub

        Protected Sub dvwProduct_ModeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwProduct.ModeChanged
            UpdateTitle()
        End Sub

        Private Sub UpdateTitle()
            lblNewProduct.Visible = (dvwProduct.CurrentMode = DetailsViewMode.Insert)
            lblEditProduct.Visible = Not lblNewProduct.Visible
        End Sub
    End Class
End Namespace
