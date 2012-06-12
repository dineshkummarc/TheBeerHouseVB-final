Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Transactions
Imports System.Text
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Store
    Public Class Order
        Inherits BaseStore

        ' ==========
        ' Private variables
        ' ==========

        Private _statusID As Integer = 0
        Private _statusTitle As String = ""
        Private _status As OrderStatus
        Private _shippingMethod As String = ""
        Private _subTotal As Decimal = 0.0
        Private _shipping As Decimal = 0.0
        Private _shippingLastName As String = ""
        Private _shippingFirstName As String = ""
        Private _shippingStreet As String = ""
        Private _shippingPostalCode As String = ""
        Private _shippingCity As String = ""
        Private _shippingState As String = ""
        Private _shippingCountry As String = ""
        Private _customerEmail As String = ""
        Private _customerPhone As String = ""
        Private _customerFax As String = ""
        Private _transactionID As String = ""
        Private _shippedDate As DateTime = DateTime.MinValue
        Private _trackingID As String = ""
        Private _items As List(Of OrderItem)

        ' ==========
        ' Properties
        ' ==========

        Public Property StatusID() As Integer
            Get
                Return _statusID
            End Get
            Set(ByVal value As Integer)
                _statusID = value
            End Set
        End Property

        Public Property StatusTitle() As String
            Get
                Return _statusTitle
            End Get
            Private Set(ByVal value As String)
                _statusTitle = value
            End Set
        End Property

        Public ReadOnly Property Status() As OrderStatus
            Get
                If IsNothing(_status) Then _
                   _status = OrderStatus.GetOrderStatusByID(Me.StatusID)
                Return _status
            End Get
        End Property

        Public Property ShippingMethod() As String
            Get
                Return _shippingMethod
            End Get
            Private Set(ByVal value As String)
                _shippingMethod = value
            End Set
        End Property

        Public Property Subtotal() As Decimal
            Get
                Return _subTotal
            End Get
            Private Set(ByVal value As Decimal)
                _subTotal = value
            End Set
        End Property

        Public Property Shipping() As Decimal
            Get
                Return _shipping
            End Get
            Private Set(ByVal value As Decimal)
                _shipping = value
            End Set
        End Property

        Public Property ShippingLastName() As String
            Get
                Return _shippingLastName
            End Get
            Private Set(ByVal value As String)
                _shippingLastName = value
            End Set
        End Property

        Public Property ShippingFirstName() As String
            Get
                Return _shippingFirstName
            End Get
            Private Set(ByVal value As String)
                _shippingFirstName = value
            End Set
        End Property

        Public Property ShippingStreet() As String
            Get
                Return _shippingStreet
            End Get
            Private Set(ByVal value As String)
                _shippingStreet = value
            End Set
        End Property

        Public Property ShippingPostalCode() As String
            Get
                Return _shippingPostalCode
            End Get
            Private Set(ByVal value As String)
                _shippingPostalCode = value
            End Set
        End Property

        Public Property ShippingCity() As String
            Get
                Return _shippingCity
            End Get
            Private Set(ByVal value As String)
                _shippingCity = value
            End Set
        End Property

        Public Property ShippingState() As String
            Get
                Return _shippingState
            End Get
            Private Set(ByVal value As String)
                _shippingState = value
            End Set
        End Property

        Public Property ShippingCountry() As String
            Get
                Return _shippingCountry
            End Get
            Private Set(ByVal value As String)
                _shippingCountry = value
            End Set
        End Property

        Public Property CustomerEmail() As String
            Get
                Return _customerEmail
            End Get
            Private Set(ByVal value As String)
                _customerEmail = value
            End Set
        End Property

        Public Property CustomerPhone() As String
            Get
                Return _customerPhone
            End Get
            Private Set(ByVal value As String)
                _customerPhone = value
            End Set
        End Property

        Public Property CustomerFax() As String
            Get
                Return _customerFax
            End Get
            Private Set(ByVal value As String)
                _customerFax = value
            End Set
        End Property

        Public Property TransactionID() As String
            Get
                Return _transactionID
            End Get
            Set(ByVal value As String)
                _transactionID = value
            End Set
        End Property

        Public Property ShippedDate() As DateTime
            Get
                Return _shippedDate
            End Get
            Set(ByVal value As DateTime)
                _shippedDate = value
            End Set
        End Property

        Public Property TrackingID() As String
            Get
                Return _trackingID
            End Get
            Set(ByVal value As String)
                _trackingID = value
            End Set
        End Property

        Public Property Items() As List(Of OrderItem)
            Get
                Return _items
            End Get
            Private Set(ByVal value As List(Of OrderItem))
                _items = value
            End Set
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal statusID As Integer, ByVal statusTitle As String, ByVal shippingMethod As String, _
                ByVal subTotal As Decimal, ByVal shipping As Decimal, ByVal shippingFirstName As String, _
                ByVal shippingLastName As String, ByVal shippingStreet As String, _
                ByVal shippingPostalCode As String, ByVal shippingCity As String, _
                ByVal shippingState As String, ByVal shippingCountry As String, _
                ByVal customerEmail As String, ByVal customerPhone As String, ByVal customerFax As String, _
                ByVal shippedDate As DateTime, ByVal transactionID As String, ByVal trackingID As String, _
                ByVal items As List(Of OrderItem))

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.StatusID = statusID
            Me.StatusTitle = statusTitle
            Me.ShippingMethod = shippingMethod
            Me.Subtotal = subTotal
            Me.Shipping = shipping
            Me.ShippingFirstName = shippingFirstName
            Me.ShippingLastName = shippingLastName
            Me.ShippingStreet = shippingStreet
            Me.ShippingPostalCode = shippingPostalCode
            Me.ShippingCity = shippingCity
            Me.ShippingState = shippingState
            Me.ShippingCountry = shippingCountry
            Me.CustomerEmail = customerEmail
            Me.CustomerPhone = customerPhone
            Me.CustomerFax = customerFax
            Me.ShippedDate = shippedDate
            Me.TransactionID = transactionID
            Me.TrackingID = trackingID
            Me.Items = items
        End Sub

        Public Function Delete() As Boolean
            Dim success As Boolean = Order.DeleteOrder(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return Order.UpdateOrder(Me.ID, Me.StatusID, Me.ShippedDate, Me.TransactionID, Me.TrackingID)
        End Function

        Public Function GetPayPalPaymentUrl() As String
            Dim serverUrl As String
            If Globals.Settings.Store.SandboxMode Then
                serverUrl = "https://www.sandbox.paypal.com/us/cgi-bin/webscr"
            Else
                serverUrl = "https://www.paypal.com/us/cgi-bin/webscr"
            End If
            Dim amount As String = Me.Subtotal.ToString("N2").Replace(",", ".")
            Dim shipping As String = Me.Shipping.ToString("N2").Replace(",", ".")

            Dim baseUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace( _
               HttpContext.Current.Request.Url.PathAndQuery, "") & _
               HttpContext.Current.Request.ApplicationPath
            If Not baseUrl.EndsWith("/") Then baseUrl &= "/"

            Dim notifyUrl As String = HttpUtility.UrlEncode(baseUrl & "PayPal/Notify.aspx")
            Dim returnUrl As String = HttpUtility.UrlEncode(baseUrl & "PayPal/OrderCompleted.aspx?ID=" & _
                Me.ID.ToString())
            Dim cancelUrl As String = HttpUtility.UrlEncode(baseUrl & "PayPal/OrderCancelled.aspx")
            Dim business As String = HttpUtility.UrlEncode(Globals.Settings.Store.BusinessEmail)
            Dim itemName As String = HttpUtility.UrlEncode("Order #" & Me.ID.ToString())

            Dim url As New StringBuilder()
            url.AppendFormat( _
               "{0}?cmd=_xclick&upload=1&rm=2&no_shipping=1&no_note=1&currency_code={1}&business={2}&item_number={3}&custom={3}&item_name={4}&amount={5}&shipping={6}&notify_url={7}&return={8}&cancel_return={9}", _
               serverUrl, Globals.Settings.Store.CurrencyCode, business, Me.ID, itemName, _
               amount, Shipping, notifyUrl, returnUrl, cancelUrl)

            Return url.ToString()
        End Function

        ' ===========
        ' Shared methods
        ' ===========

        ' Returns a collection with all the order in a specified state, and within
        ' the specified range of date
        Public Shared Function GetOrders(ByVal statusID As Integer, ByVal fromDate As DateTime, ByVal toDate As DateTime) As List(Of Order)
            toDate = toDate.AddDays(1).Subtract(New TimeSpan(toDate.Hour, toDate.Minute, toDate.Second))
            Dim orders As List(Of Order)
            Dim key As String = "Store_Orders_" & statusID & "_" & fromDate.ToShortDateString() & "_" & toDate.ToShortDateString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                orders = CType(BizObject.Cache(key), List(Of Order))
            Else
                Dim recordset As List(Of OrderDetails) = SiteProvider.Store.GetOrders(statusID, fromDate, toDate)
                orders = GetOrderListFromOrderDetailsList(recordset)
                BaseStore.CacheData(key, orders)
            End If
            Return orders
        End Function

        ' Returns a collection with all the order for the specified customer
        Public Shared Function GetOrders(ByVal customerName As String) As List(Of Order)
            Dim orders As List(Of Order)
            Dim key As String = "Store_Orders_" & customerName

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                orders = CType(BizObject.Cache(key), List(Of Order))
            Else
                Dim recordset As List(Of OrderDetails) = SiteProvider.Store.GetOrders(customerName)
                orders = GetOrderListFromOrderDetailsList(recordset)
                BaseStore.CacheData(key, orders)
            End If
            Return orders
        End Function

        ' Returns an Order object with the specified ID
        Public Shared Function GetOrderByID(ByVal orderID As Integer) As Order
            Dim [order] As Order
            Dim key As String = "Store_Order_" & orderID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                [order] = CType(BizObject.Cache(key), Order)
            Else
                [order] = GetOrderFromOrderDetails(SiteProvider.Store.GetOrderByID(orderID))
                BaseStore.CacheData(key, [order])
            End If
            Return [order]
        End Function

        ' Updates an existing order
        Public Shared Function UpdateOrder(ByVal id As Integer, ByVal statusID As Integer, ByVal shippedDate As DateTime, ByVal transactionID As String, ByVal trackingID As String) As Boolean
            Using scope As New TransactionScope()
                transactionID = BizObject.ConvertNullToEmptyString(transactionID)
                trackingID = BizObject.ConvertNullToEmptyString(trackingID)

                ' retrieve the order's current status ID
                Dim [order] As Order = BLL.Store.Order.GetOrderByID(id)

                ' update the order
                Dim record As New OrderDetails(id, DateTime.Now, "", statusID, "", "", 0.0, 0.0, _
                   "", "", "", "", "", "", "", "", "", "", shippedDate, transactionID, trackingID)
                Dim ret As Boolean = SiteProvider.Store.UpdateOrder(record)

                ' if the new status ID is confirmed, than decrease the UnitsInStock for the purchased products
                If statusID = CInt(StatusCode.Confirmed) AndAlso _
                    [order].StatusID = CInt(StatusCode.WaitingForPayment) Then
                    For Each item As orderItem In order.Items
                        Product.DecrementProductUnitsInStock(item.ProductID, item.Quantity)
                    Next
                End If

                BizObject.PurgeCacheItems("store_order")
                scope.Complete()
                Return ret
            End Using
        End Function

        ' Deletes an existing order
        Public Shared Function DeleteOrder(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DeleteOrder(id)
            Dim ev As New RecordDeletedEvent("order", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("store_order")
            Return ret
        End Function

        ' Creates a new order
        Public Shared Function InsertOrder(ByVal shoppingCart As ShoppingCart, ByVal shippingMethod As String, ByVal shipping As Decimal, ByVal shippingFirstName As String, ByVal shippingLastName As String, ByVal shippingStreet As String, ByVal shippingPostalCode As String, ByVal shippingCity As String, ByVal shippingState As String, ByVal shippingCountry As String, ByVal customerEmail As String, ByVal customerPhone As String, ByVal customerFax As String, ByVal transactionID As String) As Integer
            Using scope As New TransactionScope()
                Dim userName As String = BizObject.CurrentUserName

                ' insert the master order
                Dim [order] As New OrderDetails(0, DateTime.Now, _
                   userName, 1, "", shippingMethod, shoppingCart.Total, shipping, _
                   shippingFirstName, shippingLastName, shippingStreet, shippingPostalCode, _
                   shippingCity, shippingState, shippingCountry, customerEmail, customerPhone, _
                   customerFax, DateTime.MinValue, transactionID, "")
                Dim orderID As Integer = SiteProvider.Store.InsertOrder([order])

                ' insert the child order items
                For Each item As ShoppingCartItem In shoppingCart.Items
                    Dim orderItem As New OrderItemDetails(0, DateTime.Now, userName, _
                       orderID, item.ID, item.Title, item.SKU, item.UnitPrice, item.Quantity)
                    SiteProvider.Store.InsertOrderItem(orderItem)
                Next

                BizObject.PurgeCacheItems("store_order")
                scope.Complete()

                Return orderID
            End Using
        End Function

        ' Returns a Order object filled with the data taken from the input OrderDetails
        Private Shared Function GetOrderFromOrderDetails(ByVal record As OrderDetails) As Order
            If IsNothing(record) Then
                Return Nothing
            Else
                ' create a list of OrderItems for the order            
                Dim orderItems As New List(Of OrderItem)
                Dim recordset As List(Of OrderItemDetails) = SiteProvider.Store.GetOrderItems(record.ID)
                For Each item As OrderItemDetails In recordset
                    orderItems.Add(New OrderItem(item.ID, item.AddedDate, item.AddedBy, _
                       item.OrderID, item.ProductID, item.Title, item.SKU, item.UnitPrice, item.Quantity))
                Next

                ' create new Order
                Return New Order(record.ID, record.AddedDate, record.AddedBy, record.StatusID, record.StatusTitle, _
                   record.ShippingMethod, record.SubTotal, record.Shipping, record.ShippingFirstName, _
                   record.ShippingLastName, record.ShippingStreet, record.ShippingPostalCode, record.ShippingCity, _
                   record.ShippingState, record.ShippingCountry, record.CustomerEmail, record.CustomerPhone, _
                   record.CustomerFax, record.ShippedDate, record.TransactionID, record.TrackingID, _
                   orderItems)
            End If
        End Function

        ' Returns a list of Order objects filled with the data taken from the input list of OrderDetails
        Private Shared Function GetOrderListFromOrderDetailsList(ByVal recordset As List(Of OrderDetails)) As List(Of Order)
            Dim orders As New List(Of Order)
            For Each record As OrderDetails In recordset
                orders.Add(GetOrderFromOrderDetails(record))
            Next

            Return orders
        End Function
    End Class
End Namespace
