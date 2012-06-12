Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class StoreProvider
        Inherits DataAccess

        Private Shared _instance As StoreProvider

        ' Returns an instance of the provider type specified in the config file
        Public Shared ReadOnly Property Instance() As StoreProvider
            Get
                If IsNothing(_instance) Then
                    _instance = CType(Activator.CreateInstance( _
                       Type.GetType(Globals.Settings.Store.ProviderType)), StoreProvider)
                End If

                Return _instance
            End Get
        End Property

        Public Sub New()
            Me.ConnectionString = Globals.Settings.Store.ConnectionString
            Me.EnableCaching = Globals.Settings.Store.EnableCaching
            Me.CacheDuration = Globals.Settings.Store.CacheDuration
        End Sub

        ' methods that work with departments
        Public MustOverride Function GetDepartments() As List(Of DepartmentDetails)
        Public MustOverride Function GetDepartmentByID(ByVal departmentID As Integer) As DepartmentDetails
        Public MustOverride Function DeleteDepartment(ByVal departmentID As Integer) As Boolean
        Public MustOverride Function UpdateDepartment(ByVal department As DepartmentDetails) As Boolean
        Public MustOverride Function InsertDepartment(ByVal department As DepartmentDetails) As Integer

        ' methods that work with order statuses
        Public MustOverride Function GetOrderStatuses() As List(Of OrderStatusDetails)
        Public MustOverride Function GetOrderStatusByID(ByVal orderStatusID As Integer) As OrderStatusDetails
        Public MustOverride Function DeleteOrderStatus(ByVal orderStatusID As Integer) As Boolean
        Public MustOverride Function UpdateOrderStatus(ByVal orderStatus As OrderStatusDetails) As Boolean
        Public MustOverride Function InsertOrderStatus(ByVal orderStatus As OrderStatusDetails) As Integer

        ' methods that work with shipping methods
        Public MustOverride Function GetShippingMethods() As List(Of ShippingMethodDetails)
        Public MustOverride Function GetShippingMethodByID(ByVal shippingMethodID As Integer) As ShippingMethodDetails
        Public MustOverride Function DeleteShippingMethod(ByVal shippingMethodID As Integer) As Boolean
        Public MustOverride Function UpdateShippingMethod(ByVal shippingMethod As ShippingMethodDetails) As Boolean
        Public MustOverride Function InsertShippingMethod(ByVal shippingMethod As ShippingMethodDetails) As Integer

        ' methods that work with products
        Public MustOverride Function GetProducts(ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of ProductDetails)
        Public MustOverride Function GetProducts(ByVal departmentID As Integer, ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As List(Of ProductDetails)
        Public MustOverride Function GetProductCount() As Integer
        Public MustOverride Function GetProductCount(ByVal departmentID As Integer) As Integer
        Public MustOverride Function GetProductByID(ByVal productID As Integer) As ProductDetails
        Public MustOverride Function DeleteProduct(ByVal productID As Integer) As Boolean
        Public MustOverride Function UpdateProduct(ByVal product As ProductDetails) As Boolean
        Public MustOverride Function InsertProduct(ByVal product As ProductDetails) As Integer
        Public MustOverride Function RateProduct(ByVal productID As Integer, ByVal rating As Integer) As Boolean
        Public MustOverride Function DecrementProductUnitsInStock(ByVal productID As Integer, ByVal quantity As Integer) As Boolean
        Public MustOverride Function GetProductDescription(ByVal productID As Integer) As String

        ' methods that work with orders
        Public MustOverride Function GetOrders(ByVal statusID As Integer, ByVal fromDate As DateTime, ByVal toDate As DateTime) As List(Of OrderDetails)
        Public MustOverride Function GetOrders(ByVal addedBy As String) As List(Of OrderDetails)
        Public MustOverride Function GetOrderByID(ByVal orderID As Integer) As OrderDetails
        Public MustOverride Function DeleteOrder(ByVal orderID As Integer) As Boolean
        Public MustOverride Function InsertOrder(ByVal order As OrderDetails) As Integer
        Public MustOverride Function UpdateOrder(ByVal order As OrderDetails) As Boolean

        ' methods that work with order items
        Public MustOverride Function GetOrderItems(ByVal orderID As Integer) As List(Of OrderItemDetails)
        Public MustOverride Function InsertOrderItem(ByVal orderItem As OrderItemDetails) As Integer

        ' Returns a valid sort expression for the products
        Protected Overridable Function EnsureValidProductsSortExpression(ByVal sortExpression As String) As String
            If String.IsNullOrEmpty(sortExpression) Then Return "tbh_Products.Title ASC"

            Dim sortExpr As String = sortExpression.ToLower()
            If Not sortExpr.Equals("unitprice") AndAlso _
                Not sortExpr.Equals("unitprice asc") AndAlso _
                Not sortExpr.Equals("unitprice desc") AndAlso _
                Not sortExpr.Equals("discountpercentage") AndAlso _
                Not sortExpr.Equals("discountpercentage asc") AndAlso _
                Not sortExpr.Equals("discountpercentage desc") AndAlso _
                Not sortExpr.Equals("addeddate") AndAlso _
                Not sortExpr.Equals("addeddate asc") AndAlso _
                Not sortExpr.Equals("addeddate desc") AndAlso _
                Not sortExpr.Equals("addedby") AndAlso _
                Not sortExpr.Equals("addedby asc") AndAlso _
                Not sortExpr.Equals("addedby desc") AndAlso _
                Not sortExpr.Equals("unitsinstock") AndAlso _
                Not sortExpr.Equals("unitsinstock asc") AndAlso _
                Not sortExpr.Equals("unitsinstock desc") AndAlso _
                Not sortExpr.Equals("title") AndAlso _
                Not sortExpr.Equals("title asc") AndAlso _
                Not sortExpr.Equals("title desc") Then

                sortExpr = "title asc"
            End If
            If Not sortExpr.StartsWith("tbh_products") Then
                sortExpr = "tbh_products." & sortExpr
            End If
            If Not sortExpr.StartsWith("tbh_products.title") Then
                sortExpr &= ", tbh_products.title asc"
            End If
            Return sortExpr
        End Function

        ' Returns a new DepartmentDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetDepartmentFromReader(ByVal reader As IDataReader) As DepartmentDetails
            Return New DepartmentDetails(CInt(reader("DepartmentID")), _
               CDate(reader("AddedDate")), _
               reader("AddedBy").ToString, _
               reader("Title").ToString(), _
               CInt(reader("Importance")), _
               reader("Description").ToString(), _
               reader("ImageUrl").ToString())
        End Function

        ' Returns a collection of DepartmentDetails objects with the data read from the input DataReader
        Protected Overridable Function GetDepartmentCollectionFromReader(ByVal reader As IDataReader) As List(Of DepartmentDetails)
            Dim departments As New List(Of DepartmentDetails)
            While reader.Read()
                departments.Add(GetDepartmentFromReader(reader))
            End While
            Return departments
        End Function

        ' Returns a new OrderStatusDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetOrderStatusFromReader(ByVal reader As IDataReader) As OrderStatusDetails
            Return New OrderStatusDetails(CInt(reader("OrderStatusID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString(), _
                reader("Title").ToString())
        End Function

        ' Returns a collection of OrderStatusDetails objects with the data read from the input DataReader
        Protected Overridable Function GetOrderStatusCollectionFromReader(ByVal reader As IDataReader) As List(Of OrderStatusDetails)
            Dim orderStatuses As New List(Of OrderStatusDetails)
            While reader.Read()
                orderStatuses.Add(GetOrderStatusFromReader(reader))
            End While

            Return orderStatuses
        End Function

        ' Returns a new ShippingMethodDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetShippingMethodFromReader(ByVal reader As IDataReader) As ShippingMethodDetails
            Return New ShippingMethodDetails(CInt(reader("ShippingMethodID")), _
                CDate(reader("AddedDate")), _
                reader("AddedBy").ToString(), _
                reader("Title").ToString(), _
                CDec(reader("Price")))
        End Function

        ' Returns a collection of ShippingMethodDetails objects with the data read from the input DataReader
        Protected Overridable Function GetShippingMethodCollectionFromReader(ByVal reader As IDataReader) As List(Of ShippingMethodDetails)
            Dim shippingMethods As New List(Of ShippingMethodDetails)
            While reader.Read()
                shippingMethods.Add(GetShippingMethodFromReader(reader))
            End While
            Return shippingMethods
        End Function

        ' Returns a new ProductDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetProductFromReader(ByVal reader As IDataReader) As ProductDetails
            Return GetProductFromReader(reader, True)
        End Function

        Protected Overridable Function GetProductFromReader(ByVal reader As IDataReader, ByVal readDescription As Boolean) As ProductDetails
            Dim product As New ProductDetails(CInt(reader("ProductID")), _
               CDate(reader("AddedDate")), _
               reader("AddedBy").ToString(), _
               CInt(reader("DepartmentID")), _
               reader("DepartmentTitle").ToString(), _
               reader("Title").ToString(), _
               Nothing, _
               reader("SKU").ToString(), _
               CDec(reader("UnitPrice")), _
               CInt(reader("DiscountPercentage")), _
               CInt(reader("UnitsInStock")), _
               reader("SmallImageUrl").ToString(), _
               reader("FullImageUrl").ToString(), _
               CInt(reader("Votes")), _
               CInt(reader("TotalRating")))

            If readDescription Then _
                product.Description = reader("Description").ToString()

            Return product
        End Function

        'Returns a collection of ProductDetails objects with the data read from the input DataReader
        Protected Overridable Function GetProductCollectionFromReader(ByVal reader As IDataReader) As List(Of ProductDetails)
            Return GetProductCollectionFromReader(reader, True)
        End Function

        Protected Overridable Function GetProductCollectionFromReader(ByVal reader As IDataReader, ByVal readDescription As Boolean) As List(Of ProductDetails)
            Dim products As New List(Of ProductDetails)
            While reader.Read()
                products.Add(GetProductFromReader(reader, readDescription))
            End While
            Return products
        End Function

        ' Returns a new OrderItemDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetOrderItemFromReader(ByVal reader As IDataReader) As OrderItemDetails
            Return New OrderItemDetails(CInt(reader("OrderItemID")), _
               CDate(reader("AddedDate")), _
               reader("AddedBy").ToString(), _
               CInt(reader("OrderID")), _
               CInt(reader("ProductID")), _
               reader("Title").ToString(), _
               reader("SKU").ToString(), _
               CDec(reader("UnitPrice")), _
               CInt(reader("Quantity")))
        End Function

        ' Returns a collection of OrderItemDetails objects with the data read from the input DataReader
        Protected Overridable Function GetOrderItemCollectionFromReader(ByVal reader As IDataReader) As List(Of OrderItemDetails)
            Dim orderItems As New List(Of OrderItemDetails)
            While reader.Read()
                orderItems.Add(GetOrderItemFromReader(reader))
            End While
            Return orderItems
        End Function

        ' Returns a new OrderDetails instance filled with the DataReader's current record data
        Protected Overridable Function GetOrderFromReader(ByVal reader As IDataReader) As OrderDetails
            Dim shippedDate As DateTime
            If IsDBNull(reader("ShippedDate")) Then
                shippedDate = DateTime.MinValue
            Else
                shippedDate = reader("ShippedDate")
            End If

            Return New OrderDetails(CInt(reader("OrderID")), _
               CDate(reader("AddedDate")), _
               reader("AddedBy").ToString(), _
               CInt(reader("StatusID")), _
               reader("StatusTitle").ToString(), _
               reader("ShippingMethod").ToString(), _
               CDec(reader("SubTotal")), _
               CDec(reader("Shipping")), _
               reader("ShippingFirstName").ToString(), _
               reader("ShippingLastName").ToString(), _
               reader("ShippingStreet").ToString(), _
               reader("ShippingPostalCode").ToString(), _
               reader("ShippingCity").ToString(), _
               reader("ShippingState").ToString(), _
               reader("ShippingCountry").ToString(), _
               reader("CustomerEmail").ToString(), _
               reader("CustomerPhone").ToString(), _
               reader("CustomerFax").ToString(), _
               shippedDate, _
               reader("TransactionID").ToString(), _
               reader("TrackingID").ToString())
        End Function

        ' Returns a collection of OrderDetails objects with the data read from the input DataReader
        Protected Overridable Function GetOrderCollectionFromReader(ByVal reader As IDataReader) As List(Of OrderDetails)
            Dim orders As New List(Of OrderDetails)
            While reader.Read()
                orders.Add(GetOrderFromReader(reader))
            End While
            Return orders
        End Function
    End Class
End Namespace

