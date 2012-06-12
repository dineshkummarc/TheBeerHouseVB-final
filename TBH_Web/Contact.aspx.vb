Imports System.Net.Mail

Namespace MB.TheBeerHouse.UI
    Partial Class Contact
        Inherits BasePage

        Protected Sub txtSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubmit.Click
            Try
                ' send the mail
                Dim msg As New MailMessage
                msg.IsBodyHtml = False
                msg.From = New MailAddress(txtEmail.Text, txtName.Text)
                msg.To.Add(New MailAddress(Globals.Settings.ContactForm.MailTo))
                If Not String.IsNullOrEmpty(Globals.Settings.ContactForm.MailCC) Then
                    msg.CC.Add(New MailAddress(Globals.Settings.ContactForm.MailCC))
                End If
                msg.Subject = String.Format( _
                    Globals.Settings.ContactForm.MailSubject, txtSubject.Text)
                msg.Body = txtBody.Text
                Dim client As New SmtpClient()
                client.Send(msg)
                ' show a confirmation message, and reset the fields
                lblFeedbackOK.Visible = True
                lblFeedbackKO.Visible = False
                txtName.Text = ""
                txtEmail.Text = ""
                txtSubject.Text = ""
                txtBody.Text = ""
                Exit Try

            Catch ex As Exception
                lblFeedbackOK.Visible = False
                lblFeedbackKO.Visible = True
            End Try
        End Sub
    End Class
End Namespace