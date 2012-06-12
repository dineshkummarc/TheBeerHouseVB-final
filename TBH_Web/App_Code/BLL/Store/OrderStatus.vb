Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Store
    Public Class OrderStatus
        Inherits BaseStore

        Private _title As String = ""

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal title As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = OrderStatus.DeleteOrderStatus(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return OrderStatus.UpdateOrderStatus(Me.ID, Me.Title)
        End Function

        ' ==========
        ' Shared methods
        ' ==========

        ' Returns a collection with all the order statuses
        Public Shared Function GetOrderStatuses() As List(Of OrderStatus)
            Dim orderStatuses As List(Of OrderStatus)
            Dim key As String = "Store_OrderStatuses"

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                orderStatuses = CType(BizObject.Cache(key), List(Of OrderStatus))
            Else
                Dim recordset As List(Of OrderStatusDetails) = SiteProvider.Store.GetOrderStatuses()
                orderStatuses = GetOrderStatusListFromOrderStatusDetailsList(recordset)
                BaseStore.CacheData(key, orderStatuses)
            End If
            Return orderStatuses
        End Function

        ' Returns a OrderStatus object with the specified ID
        Public Shared Function GetOrderStatusByID(ByVal orderStatusID As Integer) As OrderStatus
            Dim orderStatus As OrderStatus
            Dim key As String = "Store_OrderStatus_" & orderStatusID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                orderStatus = CType(BizObject.Cache(key), OrderStatus)
            Else
                orderStatus = GetOrderStatusFromOrderStatusDetails(SiteProvider.Store.GetOrderStatusByID(orderStatusID))
                BaseStore.CacheData(key, orderStatus)
            End If
            Return orderStatus
        End Function

        ' Updates an existing order status
        Public Shared Function UpdateOrderStatus(ByVal id As Integer, ByVal title As String) As Boolean
            Dim record As New OrderStatusDetails(id, DateTime.Now, "", title)
            Dim ret As Boolean = SiteProvider.Store.UpdateOrderStatus(record)
            BizObject.PurgeCacheItems("store_orderstatus")
            Return ret
        End Function

        ' Deletes an existing order status
        Public Shared Function DeleteOrderStatus(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DeleteOrderStatus(id)
            Dim ev As New RecordDeletedEvent("order status", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("store_orderstatus")
            Return ret
        End Function

        ' Creates a new order status
        Public Shared Function InsertOrderStatus(ByVal title As String) As Integer
            Dim record As New OrderStatusDetails(0, DateTime.Now, _
               BizObject.CurrentUserName, title)
            Dim ret As Integer = SiteProvider.Store.InsertOrderStatus(record)
            BizObject.PurgeCacheItems("store_orderstatus")
            Return ret
        End Function

        ' Returns a OrderStatus object filled with the data taken from the input OrderStatusDetails
        Private Shared Function GetOrderStatusFromOrderStatusDetails(ByVal record As OrderStatusDetails) As OrderStatus
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New OrderStatus(record.ID, record.AddedDate, record.AddedBy, record.Title)
            End If
        End Function

        ' Returns a list of OrderStatus objects filled with the data taken from the input list of OrderStatusDetails
        Private Shared Function GetOrderStatusListFromOrderStatusDetailsList(ByVal recordset As List(Of OrderStatusDetails)) As List(Of OrderStatus)
            Dim orderStatuses As New List(Of OrderStatus)
            For Each record As OrderStatusDetails In recordset
                orderStatuses.Add(GetOrderStatusFromOrderStatusDetails(record))
            Next
            Return orderStatuses
        End Function
    End Class
End Namespace