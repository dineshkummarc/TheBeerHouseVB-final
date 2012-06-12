Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class PersonalizationManager
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then _
                UpdateUI()
        End Sub

        Protected Sub UpdateUI()
            btnBrowseView.Enabled = WebPartManager1.SupportedDisplayModes.Contains(WebPartManager.BrowseDisplayMode)
            btnDesignView.Enabled = WebPartManager1.SupportedDisplayModes.Contains(WebPartManager.DesignDisplayMode)
            btnEditView.Enabled = WebPartManager1.SupportedDisplayModes.Contains(WebPartManager.EditDisplayMode)
            btnCatalogView.Visible = WebPartManager1.SupportedDisplayModes.Contains(WebPartManager.CatalogDisplayMode)
            panPersonalizationModeToggle.Visible = WebPartManager1.Personalization.CanEnterSharedScope
            btnPersonalizationModeToggle.Text = String.Format(btnPersonalizationModeToggle.Text, _
               WebPartManager1.Personalization.Scope.ToString())
        End Sub

        Protected Sub btnBrowseView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowseView.Click
            WebPartManager1.DisplayMode = WebPartManager.BrowseDisplayMode
            UpdateUI()
        End Sub

        Protected Sub btnDesignView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesignView.Click
            WebPartManager1.DisplayMode = WebPartManager.DesignDisplayMode
            UpdateUI()
        End Sub

        Protected Sub btnEditView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditView.Click
            WebPartManager1.DisplayMode = WebPartManager.EditDisplayMode
            UpdateUI()
        End Sub

        Protected Sub btnCatalogView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCatalogView.Click
            WebPartManager1.DisplayMode = WebPartManager.CatalogDisplayMode
            UpdateUI()
        End Sub

        Protected Sub btnPersonalizationModeToggle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizationModeToggle.Click
            WebPartManager1.Personalization.ToggleScope()
            UpdateUI()
        End Sub
    End Class
End Namespace
