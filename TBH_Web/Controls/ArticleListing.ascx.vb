Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.UI
Imports MB.TheBeerHouse.BLL.Articles

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class ArticleListing
        Inherits BaseWebPart

        ' ==========
        ' Private variables
        ' ==========
        Private _enableHighlighter As Boolean = True
        Private _publishedOnly As Boolean = True
        Private _showCategoryPicker As Boolean = True
        Private _showPageSizePicker As Boolean = True
        Private _enablePaging As Boolean = True
        Private _userCanEdit As Boolean = False
        Private _userCountry As String = ""
        Private _userState As String = ""
        Private _userCity As String = ""

        ' ==========
        ' Properties
        ' ==========
        Public Property EnableHighlighter() As Boolean
            Get
                Return _enableHighlighter
            End Get
            Set(ByVal value As Boolean)
                _enableHighlighter = value
            End Set
        End Property

        Public Property PublishedOnly() As Boolean
            Get
                Return _publishedOnly
            End Get
            Set(ByVal value As Boolean)
                _publishedOnly = value
                objArticles.SelectParameters("publishedOnly").DefaultValue = _
                    value.ToString
            End Set
        End Property

        Public Property ShowCategoryPicker() As Boolean
            Get
                Return _showCategoryPicker
            End Get
            Set(ByVal value As Boolean)
                _showCategoryPicker = value
                ddlCategories.Visible = value
                lblCategoryPicker.Visible = value
                lblSeparator.Visible = value
            End Set
        End Property

        Public Property ShowPageSizePicker() As Boolean
            Get
                Return _showPageSizePicker
            End Get
            Set(ByVal value As Boolean)
                _showPageSizePicker = value
                ddlArticlesPerPage.Visible = value
                lblPageSizePicker.Visible = value
            End Set
        End Property

        Public Property EnablePaging() As Boolean
            Get
                Return _enablePaging
            End Get
            Set(ByVal value As Boolean)
                _enablePaging = value
                gvwArticles.PagerSettings.Visible = value
            End Set
        End Property

        Protected Property UserCanEdit() As Boolean
            Get
                Return _userCanEdit
            End Get
            Set(ByVal value As Boolean)
                _userCanEdit = value
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)

            Me.UserCanEdit = (Me.Page.User.Identity.IsAuthenticated.ToString AndAlso _
                (Me.Page.User.IsInRole("Administrators") Or _
                Me.Page.User.IsInRole("Editors")))
            Try
                If Me.Page.User.Identity.IsAuthenticated Then
                    _userCountry = Me.Profile.Address.Country.ToLower
                    _userState = Me.Profile.Address.State.ToLower
                    _userCity = Me.Profile.Address.City.ToLower
                End If
            Catch ex As Exception

            End Try
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState As Object() = CType(savedState, Object())
            MyBase.LoadControlState(ctlState(0))
            Me.EnableHighlighter = CBool(ctlState(1))
            Me.PublishedOnly = CBool(ctlState(2))
            Me.ShowCategoryPicker = CBool(ctlState(3))
            Me.ShowPageSizePicker = CBool(ctlState(4))
            Me.EnablePaging = CBool(ctlState(5))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState() As Object
            ReDim ctlState(6)

            ctlState(0) = MyBase.SaveControlState
            ctlState(1) = Me.EnableHighlighter
            ctlState(2) = Me.PublishedOnly
            ctlState(3) = Me.ShowCategoryPicker
            ctlState(4) = Me.ShowPageSizePicker
            ctlState(5) = Me.EnablePaging

            Return ctlState
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                ' preselect the category whose ID is passed in the querystring
                If Not String.IsNullOrEmpty(Me.Request.QueryString("CatID")) Then
                    ddlCategories.DataBind()
                    ddlCategories.SelectedValue = Me.Request.QueryString("CatID")
                End If

                ' Set the page size as indicated in the config file. If an option for that
                ' size doesn't already exist, first create and then select it.
                Dim pageSize As Integer = Globals.Settings.Articles.PageSize
                If IsNothing(ddlArticlesPerPage.Items.FindByValue(pageSize.ToString)) Then
                    ddlArticlesPerPage.Items.Add(New ListItem(pageSize.ToString, pageSize.ToString))
                End If
                ddlArticlesPerPage.SelectedValue = pageSize.ToString
                gvwArticles.PageSize = pageSize

                gvwArticles.DataBind()
            End If
        End Sub

        Protected Sub ddlCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategories.SelectedIndexChanged
            gvwArticles.PageIndex = 0
            gvwArticles.DataBind()
        End Sub

        Protected Sub ddlArticlesPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArticlesPerPage.SelectedIndexChanged
            gvwArticles.PageSize = Integer.Parse(ddlArticlesPerPage.SelectedValue)
            gvwArticles.PageIndex = 0
            gvwArticles.DataBind()
        End Sub

        Protected Sub gvwArticles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwArticles.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow AndAlso _
                Me.Page.User.Identity.IsAuthenticated AndAlso _
                Me.EnableHighlighter Then

                ' highlight the article row according to whether the current user's
                ' city, state or country is found in the article's city, state or country
                Dim article As Article = CType(e.Row.DataItem, Article)
                If article.Country.ToLower = _userCountry Then
                    e.Row.CssClass = "highlightcountry"

                    If Array.IndexOf(Of String)( _
                        article.State.ToLower.Split(";"), _userState) > -1 Then

                        e.Row.CssClass = "highlightstate"

                        If Array.IndexOf(Of String)( _
                            article.City.ToLower.Split(";"), _userCity) > -1 Then

                            e.Row.CssClass = "highlightcity"
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub gvwArticles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwArticles.RowCommand
            If e.CommandName = "Approve" Then
                Dim articleID As Integer = Integer.Parse(e.CommandArgument.ToString)
                Article.ApproveArticle(articleID)
                gvwArticles.DataBind()
            End If
        End Sub
    End Class
End Namespace
