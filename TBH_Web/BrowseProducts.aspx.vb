Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class BrowseProducts
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Master.EnablePersonalization = True
        End Sub
    End Class
End Namespace