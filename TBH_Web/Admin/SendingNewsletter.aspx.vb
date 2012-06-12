Imports FredCK.FCKeditorV2
Imports System.Threading
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Newsletters

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class SendingNewsletter
        Inherits BasePage
        Implements ICallbackEventHandler

        Dim callbackResult As String = ""

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim isSending As Boolean = False
            Newsletter.Lock.AcquireReaderLock(Timeout.Infinite)
            isSending = Newsletter.IsSending
            Newsletter.Lock.ReleaseReaderLock()

            ' If no newsletter is currently being sent, show a panel with a message saying so.
            ' Otherwise register the server-side callback procedure to update the progressbar
            If Not (Me.IsPostBack) AndAlso Not (isSending) Then
                panNoNewsletter.Visible = True
                panProgress.Visible = False
            Else
                Dim callbackRef As String = Me.ClientScript.GetCallbackEventReference( _
                   Me, "", "UpdateProgress", "null")

                lblScriptName.Text = callbackRef
                Me.ClientScript.RegisterStartupScript(Me.GetType(), "StartUpdateProgress", _
                   "<script type=""text/javascript"">CallUpdateProgress();</script>")
            End If
        End Sub


        Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
            Return callbackResult
        End Function

        Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
            Newsletter.Lock.AcquireReaderLock(Timeout.Infinite)
            callbackResult = Newsletter.PercentageCompleted.ToString("N0") & ";" & _
               Newsletter.SentMails.ToString() & ";" & Newsletter.TotalMails.ToString()
            Newsletter.Lock.ReleaseReaderLock()
        End Sub
    End Class
End Namespace
