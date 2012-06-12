Imports System.Web.UI.WebControls.WebParts

Namespace MB.TheBeerHouse.UI
    Partial Class _Default
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Master.EnablePersonalization = True
        End Sub
    End Class
End Namespace
