Namespace MB.TheBeerHouse.UI
    Partial Class ArchivedNewsletters
        Inherits BasePage

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim toDate As DateTime = DateTime.Now

            If Not Me.User.Identity.IsAuthenticated OrElse _
                (Not Me.User.IsInRole("Administrators") And Not Me.User.IsInRole("Editors")) Then
                toDate = toDate.Subtract( _
                    New TimeSpan(Globals.Settings.Newsletters.HideFromArchiveInterval, _
                    0, 0, 0))
            End If
            objNewsletters.SelectParameters("toDate").DefaultValue = toDate.ToString("f")
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' check whether this page can be accessed by anonymous users. If not, and if the
            ' current user is not authenticated, redirect to the login page
            If Not Me.User.Identity.IsAuthenticated AndAlso Not Globals.Settings.Newsletters.ArchiveIsPublic Then
                Me.RequestLogin()
            End If

            ' if the user is not an admin or editor, hide the grid's column with the delete button
            gvwNewsletters.Columns(1).Visible = (Me.User.Identity.IsAuthenticated And _
               (Me.User.IsInRole("Administrators") Or Me.User.IsInRole("Editors")))
        End Sub

        Protected Sub gvwNewsletters_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwNewsletters.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(1).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this newsletter?') == false) return false;"
            End If
        End Sub
    End Class
End Namespace