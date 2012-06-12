Imports System.Web
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.UI
    Public Class BasePage
        Inherits System.Web.UI.Page

        Protected Overrides Sub InitializeCulture()
            Dim culture As String = CType(HttpContext.Current.Profile, ProfileCommon).Preferences.Culture
            Me.Culture = culture
            Me.UICulture = culture
        End Sub

        Protected Overrides Sub OnPreInit(ByVal e As System.EventArgs)
            Dim id As String = Globals.ThemesSelectorID

            If id.Length > 0 Then
                Dim profTheme As ProfileCommon = HttpContext.Current.Profile
                ' if this is a postback caused by the theme selector's dropdownlist,
                ' retrieve the selected theme and use it for the current page request
                If Me.Request.Form("__EVENTTARGET") = id AndAlso _
                        Not String.IsNullOrEmpty(Me.Request.Form(id)) Then
                    Me.Theme = Me.Request.Form(id)
                    profTheme.Preferences.Theme = Me.Theme
                Else
                    ' if not a postback, or a postback caused by controls other then the theme selector,
                    ' set the page's theme with the value found in the user's profile, if present
                    If Not String.IsNullOrEmpty(profTheme.Preferences.Theme) Then
                        Me.Theme = profTheme.Preferences.Theme
                    End If
                End If
            End If

            MyBase.OnPreInit(e)
        End Sub

        Public ReadOnly Property BaseUrl() As String
            Get
                Dim url As String = Me.Request.ApplicationPath
                If url.EndsWith("/") Then
                    Return url
                Else
                    Return url + "/"
                End If
            End Get
        End Property

        Public ReadOnly Property FullBaseUrl() As String
            Get
                Return Me.Request.Url.AbsoluteUri.Replace( _
                    Me.Request.Url.PathAndQuery, "") + Me.BaseUrl
            End Get
        End Property

        Public Sub RequestLogin()
            Me.Response.Redirect(FormsAuthentication.LoginUrl & _
                "?ReturnUrl=" + Me.Request.Url.PathAndQuery)
        End Sub

        Public Function FormatPrice(ByVal price As Object) As String
            Return Convert.ToDecimal(price).ToString("N2") & " " & Globals.Settings.Store.CurrencyCode
        End Function
    End Class
End Namespace
