Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Store
    Public Class ShippingMethod
        Inherits BaseStore

        ' ==========
        ' Private Variables
        ' ==========

        Private _title As String = ""
        Private _price As Decimal = 0.0

        ' ==========
        ' Properties
        ' ==========

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Price() As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
            End Set
        End Property

        Public ReadOnly Property TitleAndPrice() As String
            Get
                Return String.Format("{0} ({1:N2} {2})", Me.Title, Me.Price, _
                    Globals.Settings.Store.CurrencyCode)
            End Get
        End Property

        ' ===========
        ' Constructor
        ' ===========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, ByVal price As Decimal)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Price = price
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = ShippingMethod.DeleteShippingMethod(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return ShippingMethod.UpdateShippingMethod(Me.ID, Me.Title, Me.Price)
        End Function

        ' ===========
        ' Shared methods
        ' ===========

        ' Returns a collection with all the shipping methods
        Public Shared Function GetShippingMethods() As List(Of ShippingMethod)
            Dim shippingMethods As List(Of ShippingMethod)
            Dim key As String = "Store_ShippingMethods"

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                shippingMethods = CType(BizObject.Cache(key), List(Of ShippingMethod))
            Else
                Dim recordset As List(Of ShippingMethodDetails) = SiteProvider.Store.GetShippingMethods()
                shippingMethods = GetShippingMethodListFromShippingMethodDetailsList(recordset)
                BaseStore.CacheData(key, shippingMethods)
            End If
            Return shippingMethods
        End Function

        ' Returns a ShippingMethod object with the specified ID
        Public Shared Function GetShippingMethodByID(ByVal shippingMethodID As Integer) As ShippingMethod
            Dim shippingMethod As ShippingMethod
            Dim key As String = "Store_ShippingMethod_" & shippingMethodID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                shippingMethod = CType(BizObject.Cache(key), ShippingMethod)
            Else
                shippingMethod = GetShippingMethodFromShippingMethodDetails(SiteProvider.Store.GetShippingMethodByID(shippingMethodID))
                BaseStore.CacheData(key, shippingMethod)
            End If
            Return shippingMethod
        End Function

        ' Updates an existing shipping method
        Public Shared Function UpdateShippingMethod(ByVal id As Integer, ByVal title As String, ByVal price As Decimal) As Boolean
            Dim record As New ShippingMethodDetails(id, DateTime.Now, "", title, price)
            Dim ret As Boolean = SiteProvider.Store.UpdateShippingMethod(record)
            BizObject.PurgeCacheItems("store_shippingmethod")
            Return ret
        End Function

        ' Deletes an existing shipping method
        Public Shared Function DeleteShippingMethod(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DeleteShippingMethod(id)
            Dim ev As New RecordDeletedEvent("shipping method", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("store_shippingmethod")
            Return ret
        End Function

        ' Creates a new shipping method
        Public Shared Function InsertShippingMethod(ByVal title As String, ByVal price As Decimal) As Integer
            Dim record As New ShippingMethodDetails(0, DateTime.Now, _
               BizObject.CurrentUserName, title, price)
            Dim ret As Integer = SiteProvider.Store.InsertShippingMethod(record)
            BizObject.PurgeCacheItems("store_shippingmethod")
            Return ret
        End Function

        ' Returns a ShippingMethod object filled with the data taken from the input ShippingMethodDetails
        Private Shared Function GetShippingMethodFromShippingMethodDetails(ByVal record As ShippingMethodDetails) As ShippingMethod
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New ShippingMethod(record.ID, record.AddedDate, record.AddedBy, record.Title, record.Price)
            End If
        End Function

        ' Returns a list of ShippingMethod objects filled with the data taken from the input list of ShippingMethodDetails
        Private Shared Function GetShippingMethodListFromShippingMethodDetailsList(ByVal recordset As List(Of ShippingMethodDetails)) As List(Of ShippingMethod)
            Dim shippingMethods As New List(Of ShippingMethod)
            For Each record As ShippingMethodDetails In recordset
                shippingMethods.Add(GetShippingMethodFromShippingMethodDetails(record))
            Next

            Return shippingMethods
        End Function
    End Class
End Namespace
