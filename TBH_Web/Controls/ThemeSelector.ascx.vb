Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class ThemeSelector
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Globals.ThemesSelectorID.Length = 0 Then
                Globals.ThemesSelectorID = ddlThemes.UniqueID
            End If

            ddlThemes.DataSource = Helpers.GetThemes()
            ddlThemes.DataBind()

            ddlThemes.SelectedValue = Me.Page.Theme
        End Sub
    End Class
End Namespace
