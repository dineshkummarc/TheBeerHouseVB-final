Imports System.Security.Principal
Imports System.Collections
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.BLL
    Public MustInherit Class BizObject
        Protected Const MAXROWS As Integer = Integer.MaxValue

        Protected Shared ReadOnly Property Cache() As Cache
            Get
                Return HttpContext.Current.Cache
            End Get
        End Property

        Protected Shared ReadOnly Property CurrentUser() As IPrincipal
            Get
                Return HttpContext.Current.User
            End Get
        End Property

        Protected Shared ReadOnly Property CurrentUserName() As String
            Get
                Dim userName As String = ""
                If HttpContext.Current.User.Identity.IsAuthenticated Then
                    userName = HttpContext.Current.User.Identity.Name
                End If
                Return userName
            End Get
        End Property

        Protected Shared ReadOnly Property CurrentUserIP() As String
            Get
                Return HttpContext.Current.Request.UserHostAddress
            End Get
        End Property

        Protected Shared Function GetPageIndex( _
                ByVal startRowIndex As Integer, ByVal maximumRows As Integer) _
                As Integer
            If maximumRows <= 0 Then
                Return 0
            Else
                Return CInt(Math.Floor(CDbl(startRowIndex) / CDbl(maximumRows)))
            End If
        End Function

        Protected Shared Function EncodeText(ByVal content As String) As String
            content = HttpUtility.HtmlEncode(content)
            content = content.Replace("  ", " &nbsp;&nbsp;").Replace("\n", "<br>")
            Return content
        End Function

        Protected Shared Function ConvertNullToEmptyString(ByVal input As String) As String
            If input = Nothing Then
                Return ""
            Else
                Return input
            End If
        End Function

        Protected Shared Sub PurgeCacheItems(ByVal prefix As String)
            prefix = prefix.ToLower
            Dim itemsToRemove As New List(Of String)

            Dim enumerator As IDictionaryEnumerator = BizObject.Cache.GetEnumerator()
            While enumerator.MoveNext
                If enumerator.Key.ToString.ToLower.StartsWith(prefix) Then
                    itemsToRemove.Add(enumerator.Key.ToString)
                End If
            End While

            For Each itemToRemove As String In itemsToRemove
                BizObject.Cache.Remove(itemToRemove)
            Next
        End Sub
    End Class
End Namespace
