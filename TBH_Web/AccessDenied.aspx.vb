Namespace MB.TheBeerHouse.UI
    Partial Class AccessDenied
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            lblInsufficientPermissions.Visible = Me.User.Identity.IsAuthenticated
            lblLoginRequired.Visible = Not (Me.User.Identity.IsAuthenticated) AndAlso _
                String.IsNullOrEmpty(Me.Request.QueryString("loginfailure"))
            lblInvalidCredentials.Visible = Not (Me.Request.QueryString("loginfailure") = Nothing) AndAlso _
                Me.Request.QueryString("loginfailure") = "1"
        End Sub
    End Class
End Namespace
