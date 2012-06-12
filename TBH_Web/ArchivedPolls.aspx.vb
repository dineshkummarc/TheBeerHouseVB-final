Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI
    Partial Class ArchivedPolls
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.User.Identity.IsAuthenticated AndAlso Not Globals.Settings.Polls.ArchiveIsPublic Then
                Me.RequestLogin()
            End If

            gvwPolls.Columns(1).Visible = Me.User.Identity.IsAuthenticated And _
                (Me.User.IsInRole("Administrators") Or Me.User.IsInRole("Editors"))
        End Sub

        Protected Sub gvwPolls_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwPolls.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(1).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this poll?') == false) return false;"
            End If
        End Sub
    End Class
End Namespace