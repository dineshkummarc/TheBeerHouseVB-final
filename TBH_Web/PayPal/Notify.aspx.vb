Imports System.Globalization
Imports System.Net
Imports System.IO
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Store

Namespace MB.TheBeerHouse.UI
    Partial Class Notify
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsVerifiedNotification() Then
                Dim orderID As Integer = Convert.ToInt32(Me.Request.Params("custom"))
                Dim status As String = Me.Request.Params("payment_status")
                Dim amount As Decimal = Convert.ToDecimal(Me.Request.Params("mc_gross"), _
                   CultureInfo.CreateSpecificCulture("en-US"))

                ' get the Order object corresponding to the input orderID, 
                ' and check that its total matches the input total
                Dim order As Order = BLL.Store.Order.GetOrderByID(orderID)
                Dim origAmount As Decimal = (order.Subtotal + order.Shipping)
                If amount >= origAmount Then
                    order.StatusID = CInt(StatusCode.Verified)
                    order.Update()
                End If
            End If
        End Sub

        Protected Function IsVerifiedNotification() As Boolean
            Dim response As String = ""
            Dim post As String = Request.Form.ToString() & "&cmd=_notify-validate"
            Dim serverUrl As String
            If Globals.Settings.Store.SandboxMode Then
                serverUrl = "https://www.sandbox.paypal.com/us/cgi-bin/webscr"
            Else
                serverUrl = "https://www.paypal.com/us/cgi-bin/webscr"
            End If

            Dim req As HttpWebRequest = CType(WebRequest.Create(serverUrl), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/x-www-form-urlencoded"
            req.ContentLength = post.Length

            Dim writer As New StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII)
            writer.Write(post)
            writer.Close()

            Dim reader As New StreamReader(req.GetResponse().GetResponseStream())
            response = reader.ReadToEnd()
            reader.Close()

            Return (response = "VERIFIED")
        End Function
    End Class
End Namespace