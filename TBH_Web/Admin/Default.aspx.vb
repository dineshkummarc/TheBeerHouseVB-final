Imports System.Security
Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class _Default
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            panAdmin.Visible = (Me.User.IsInRole("Administrators"))
            panEditor.Visible = (Me.User.IsInRole("Administrators") OrElse _
                Me.User.IsInRole("Editors"))
            panStoreKeeper.Visible = (Me.User.IsInRole("Administrators") OrElse _
                Me.User.IsInRole("StoreKeepers"))
            panModerator.Visible = (Me.User.IsInRole("Administrators") OrElse _
                Me.User.IsInRole("Editors") OrElse Me.User.IsInRole("Modearators"))
            panContributor.Visible = (Me.User.IsInRole("Administrators") OrElse _
                Me.User.IsInRole("Editors") OrElse Me.User.IsInRole("Contributors"))
        End Sub
    End Class
End Namespace
