Namespace MB.TheBeerHouse.UI
    Partial Class [Error]
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            lbl404.Visible = (Not IsNothing(Me.Request.QueryString("code")) And Me.Request.QueryString("code") = "404")
            lbl408.Visible = (Not IsNothing(Me.Request.QueryString("code")) And Me.Request.QueryString("code") = "408")
            lbl505.Visible = (Not IsNothing(Me.Request.QueryString("code")) And Me.Request.QueryString("code") = "505")
            lblError.Visible = (String.IsNullOrEmpty(Me.Request.QueryString("code")))
        End Sub
    End Class
End Namespace