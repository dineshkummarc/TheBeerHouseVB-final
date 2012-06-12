Namespace MB.TheBeerHouse.UI
    Partial Class EditProfile
        Inherits BasePage

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
            UserProfile1.SaveProfile()
            lblFeedbackOK.Visible = True
        End Sub
    End Class
End Namespace
