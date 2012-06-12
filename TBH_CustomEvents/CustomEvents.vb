Imports System.Web
Imports System.Web.Management

Namespace MB.TheBeerHouse
    Public MustInherit Class WebCustomEvent
        Inherits WebBaseEvent

        Public Sub New(ByVal message As String, ByVal eventSource As Object, ByVal eventCode As Integer)
            MyBase.New(message, eventSource, eventCode)
        End Sub
    End Class

    Public Class RecordDeletedEvent
        Inherits WebCustomEvent

        Shadows Const eventCode As Integer = WebEventCodes.WebExtendedBase + 10
        Shadows Const message As String = _
            "The {0} with ID = {1} was deleted by user {2}."

        Public Sub New(ByVal entity As String, ByVal id As Integer, _
                ByVal eventSource As Object)
            MyBase.New(String.Format(message, entity, id, _
                HttpContext.Current.User.Identity.Name), eventSource, eventCode)
        End Sub
    End Class
End Namespace
