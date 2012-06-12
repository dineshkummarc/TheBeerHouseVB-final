Imports System.IO
Imports System.Security

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class FileUploader
        Inherits System.Web.UI.UserControl

        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            If Not IsNothing(filUpload.PostedFile) AndAlso filUpload.PostedFile.ContentLength > 0 Then
                Try
                    ' if not already present, create a directory
                    ' names /uploads/{CurrentUserName}
                    Dim dirUrl As String = CType(Me.Page, MB.TheBeerHouse.UI.BasePage).BaseUrl + _
                        "Uploads/" + Me.Page.User.Identity.Name
                    Dim dirPath As String = Server.MapPath(dirUrl)
                    If Not Directory.Exists(dirPath) Then
                        Directory.CreateDirectory(dirPath)
                    End If
                    ' save the file under the users's personal folder
                    Dim fileUrl As String = dirUrl + "/" + _
                        Path.GetFileName(filUpload.PostedFile.FileName)
                    filUpload.PostedFile.SaveAs(Server.MapPath(fileUrl))

                    lblFeedbackOK.Visible = True
                    lblFeedbackOK.Text = "File successfully uploaded: " + fileUrl
                Catch ex As Exception
                    lblFeedbackKO.Visible = True
                    lblFeedbackKO.Text = ex.Message
                End Try
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' this control can only work for authenticated users
            If Not Me.Page.User.Identity.IsAuthenticated Then
                Throw New SecurityException("Anonymous users cannot upload files.")
                lblFeedbackKO.Visible = False
                lblFeedbackOK.Visible = False
            End If
        End Sub
    End Class
End Namespace
