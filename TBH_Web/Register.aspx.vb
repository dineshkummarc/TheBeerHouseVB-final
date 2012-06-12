Namespace MB.TheBeerHouse.UI
    Partial Class Register
        Inherits BasePage

        Protected Email As String = ""

        Protected Sub CreateUserWizard1_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles CreateUserWizard1.FinishButtonClick
            UserProfile1.SaveProfile()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack AndAlso Not String.IsNullOrEmpty(Me.Request.QueryString("Email")) Then
                Email = Me.Request.QueryString("Email")
                CreateUserWizard1.DataBind()
            End If
        End Sub
    End Class
End Namespace
