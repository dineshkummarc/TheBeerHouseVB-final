Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.UI
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class ProductListing
        'Inherits BaseWebPart

        Private _showDepartmentPicker As Boolean = True
        Public Property ShowDepartmentPicker() As Boolean
            Get
                Return _showDepartmentPicker
            End Get
            Set(ByVal value As Boolean)
                _showDepartmentPicker = value
                ddlDepartments.Visible = value
                lblDepartmentPicker.Visible = value
                lblSeparator.Visible = value
            End Set
        End Property

        Private _showPageSizePicker As Boolean = True
        Public Property ShowPageSizePicker() As Boolean
            Get
                Return _showPageSizePicker
            End Get
            Set(ByVal value As Boolean)
                _showPageSizePicker = value
                ddlProductsPerPage.Visible = value
                lblPageSizePicker.Visible = value
            End Set
        End Property

        Private _enablePaging As Boolean = True
        Public Property EnablePaging() As Boolean
            Get
                Return _enablePaging
            End Get
            Set(ByVal value As Boolean)
                _enablePaging = value
                gvwProducts.PagerSettings.Visible = value
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState As Object() = CType(savedState, Object())
            MyBase.LoadControlState(ctlState(0))
            Me.ShowDepartmentPicker = CBool(ctlState(1))
            Me.ShowPageSizePicker = CBool(ctlState(2))
            Me.EnablePaging = CBool(ctlState(3))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState As Object()
            ReDim ctlState(4)
            ctlState(0) = MyBase.SaveControlState()
            ctlState(1) = Me.ShowDepartmentPicker
            ctlState(2) = Me.ShowPageSizePicker
            ctlState(3) = Me.EnablePaging
            Return ctlState
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' hide the columns for editing and deleting a product if the current user
            ' is not an administrator or a store keeper
            Dim userCanEdit As Boolean = (Me.Page.User.Identity.IsAuthenticated AndAlso _
               (Me.Page.User.IsInRole("Administrators") OrElse Me.Page.User.IsInRole("StoreKeepers")))
            gvwProducts.Columns(5).Visible = userCanEdit
            gvwProducts.Columns(6).Visible = userCanEdit

            If Not Me.IsPostBack Then
                ' preselect the department whose ID is passed in the querystring
                If Not String.IsNullOrEmpty(Me.Request.QueryString("DepID")) Then
                    ddlDepartments.DataBind()
                    ddlDepartments.SelectedValue = Me.Request.QueryString("DepID")
                End If

                ' Set the page size as indicated in the config file. If an option for that size
                ' doesn't already exist, first create and then select it.
                Dim pageSize As Integer = Globals.Settings.Store.PageSize
                If IsNothing(ddlProductsPerPage.Items.FindByValue(pageSize.ToString())) Then
                    ddlProductsPerPage.Items.Add(New ListItem(pageSize.ToString(), pageSize.ToString()))
                End If
                ddlProductsPerPage.SelectedValue = pageSize.ToString()
                gvwProducts.PageSize = pageSize

                gvwProducts.DataBind()
            End If
        End Sub

        Protected Sub ddlProductsPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProductsPerPage.SelectedIndexChanged
            gvwProducts.PageSize = Integer.Parse(ddlProductsPerPage.SelectedValue)
            gvwProducts.PageIndex = 0
            gvwProducts.DataBind()
        End Sub

        Protected Sub ddlDepartments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartments.SelectedIndexChanged
            gvwProducts.PageIndex = 0
            gvwProducts.DataBind()
        End Sub

        Protected Sub gvwProducts_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwProducts.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(6).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this product?') == false) return false;"
            End If
        End Sub
    End Class
End Namespace
