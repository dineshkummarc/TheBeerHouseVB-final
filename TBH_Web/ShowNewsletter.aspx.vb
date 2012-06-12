Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Newsletters

Namespace MB.TheBeerHouse.UI
    Partial Class ShowNewsletter
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' check whether this page can be accessed by anonymous users. If not, and if the
            ' current user is not authenticated, redirect to the login page
            If Not Me.User.Identity.IsAuthenticated AndAlso Not Globals.Settings.Newsletters.ArchiveIsPublic Then
                Me.RequestLogin()
            End If

            ' load the newsletter with the ID passed on the querystring
            Dim newsletter As Newsletter = BLL.Newsletters.Newsletter.GetNewslettersByID( _
               Integer.Parse(Me.Request.QueryString("ID")))

            ' check that the newsletter can be viewed, according to the number of days
            ' that must pass before it is published in the archive
            Dim days As Integer = CType(DateTime.Now - newsletter.AddedDate, TimeSpan).Days
            If Globals.Settings.Newsletters.HideFromArchiveInterval > days AndAlso _
               (Not Me.User.Identity.IsAuthenticated OrElse _
               (Not Me.User.IsInRole("Administrators") And Not Me.User.IsInRole("Editors"))) Then

                Me.RequestLogin()
            End If

            ' show the newsletter's data
            Me.Title += newsletter.Subject
            lblSubject.Text = newsletter.Subject
            lblPlaintextBody.Text = HttpUtility.HtmlEncode(newsletter.PlainTextBody).Replace( _
               "  ", "&nbsp; ").Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br/>")
            lblHtmlBody.Text = newsletter.HtmlBody

        End Sub
    End Class
End Namespace