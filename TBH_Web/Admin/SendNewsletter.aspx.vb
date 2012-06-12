Imports FredCK.FCKeditorV2
Imports System.Threading
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Newsletters

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class SendNewsletter
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim isSending As Boolean = False
            Newsletter.Lock.AcquireReaderLock(Timeout.Infinite)
            isSending = Newsletter.IsSending
            Newsletter.Lock.ReleaseReaderLock()

            If Not Me.IsPostBack AndAlso isSending Then
                panWait.Visible = True
                panSend.Visible = False
            End If
            txtHtmlBody.BasePath = Me.BaseUrl + "FCKeditor/"
        End Sub

        Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
            Dim isSending As Boolean = False
            Newsletter.Lock.AcquireReaderLock(Timeout.Infinite)
            isSending = Newsletter.IsSending
            Newsletter.Lock.ReleaseReaderLock()

            ' if another newsletter is currently being sent, show the panel with the wait message,
            ' but don't hide the input controls so that the user doesn't loose the newsletter's text
            If isSending Then
                panWait.Visible = True
            Else
                ' if no newsletter is currently being sent, send this new one and
                ' redirect to the page showing the progress
                Dim id As Integer = Newsletter.SendNewsletter(txtSubject.Text, txtPlainTextBody.Text, txtHtmlBody.Value)
                Me.Response.Redirect("SendingNewsletters.aspx")
            End If
        End Sub
    End Class
End Namespace