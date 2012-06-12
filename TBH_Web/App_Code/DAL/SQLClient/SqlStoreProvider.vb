Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Caching

Namespace MB.TheBeerHouse.DAL.SqlClient
    Public Class SqlStoreProvider
        Inherits StoreProvider

        ' Returns a collection with all the departments
        Public Overrides Function GetDepartments() As System.Collections.Generic.List(Of DepartmentDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetDepartments", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetDepartmentCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns an existing department with the specified ID
        Public Overrides Function GetDepartmentByID(ByVal departmentID As Integer) As DepartmentDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetDepartmentByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetDepartmentFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes a department
        Public Overrides Function DeleteDepartment(ByVal departmentID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DeleteDepartment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates a department
        Public Overrides Function UpdateDepartment(ByVal department As DepartmentDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_UpdateDepartment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = department.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = department.Title
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = department.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = department.ImageUrl
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = department.Description
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Creates a new department
        Public Overrides Function InsertDepartment(ByVal department As DepartmentDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertDepartment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = department.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = department.AddedBy
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = department.Title
                cmd.Parameters.Add("@Importance", SqlDbType.Int).Value = department.Importance
                cmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = department.ImageUrl
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = department.Description
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@DepartmentID").Value)
            End Using
        End Function

        ' Returns a collection with all the order statuses
        Public Overrides Function GetOrderStatuses() As System.Collections.Generic.List(Of OrderStatusDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrderStatuses", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetOrderStatusCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns an existing order status with the specified ID
        Public Overrides Function GetOrderStatusByID(ByVal orderStatusID As Integer) As OrderStatusDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrderStatusByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = orderStatusID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetOrderStatusFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes an order status
        Public Overrides Function DeleteOrderStatus(ByVal orderStatusID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DeleteOrderStatus", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = orderStatusID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates an order status
        Public Overrides Function UpdateOrderStatus(ByVal orderStatus As OrderStatusDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_UpdateOrderStatus", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderStatusID", SqlDbType.Int).Value = orderStatus.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = orderStatus.Title
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Creates a new order status
        Public Overrides Function InsertOrderStatus(ByVal orderStatus As OrderStatusDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertOrderStatus", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = orderStatus.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = orderStatus.AddedBy
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = orderStatus.Title
                cmd.Parameters.Add("@OrderStatusID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@OrderStatusID").Value)
            End Using
        End Function

        ' Returns a collection with all the shipping methods
        Public Overrides Function GetShippingMethods() As System.Collections.Generic.List(Of ShippingMethodDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetShippingMethods", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return GetShippingMethodCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Returns an existing shipping method with the specified ID
        Public Overrides Function GetShippingMethodByID(ByVal shippingMethodID As Integer) As ShippingMethodDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetShippingMethodByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ShippingMethodID", SqlDbType.Int).Value = shippingMethodID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetShippingMethodFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes a shipping method
        Public Overrides Function DeleteShippingMethod(ByVal shippingMethodID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DeleteShippingMethod", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ShippingMethodID", SqlDbType.Int).Value = shippingMethodID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Updates a shipping method
        Public Overrides Function UpdateShippingMethod(ByVal shippingMethod As ShippingMethodDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_UpdateShippingMethod", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ShippingMethodID", SqlDbType.Int).Value = shippingMethod.ID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = shippingMethod.Title
                cmd.Parameters.Add("@Price", SqlDbType.Money).Value = shippingMethod.Price
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Creates a new shipping method
        Public Overrides Function InsertShippingMethod(ByVal shippingMethod As ShippingMethodDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertShippingMethod", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = shippingMethod.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = shippingMethod.AddedBy
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = shippingMethod.Title
                cmd.Parameters.Add("@Price", SqlDbType.Money).Value = shippingMethod.Price
                cmd.Parameters.Add("@ShippingMethodID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@ShippingMethodID").Value)
            End Using
        End Function

        ' Retrieves all products
        Public Overloads Overrides Function GetProducts(ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As System.Collections.Generic.List(Of ProductDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                sortExpression = EnsureValidProductsSortExpression(sortExpression)
                Dim lowerBound As Integer = pageIndex * pageSize + 1
                Dim upperBound As Integer = (pageIndex + 1) * pageSize
                Dim sql As String = String.Format("SELECT * FROM " & _
                    "(SELECT tbh_Products.ProductID, tbh_Products.AddedDate, tbh_Products.AddedBy, tbh_Products.DepartmentID, tbh_Products.Title, " & _
                    "tbh_Products.Description, tbh_Products.SKU, tbh_Products.UnitPrice, tbh_Products.DiscountPercentage, " & _
                    "tbh_Products.UnitsInStock, tbh_Products.SmallImageUrl, tbh_Products.FullImageUrl, tbh_Products.Votes, " & _
                    "tbh_Products.TotalRating, tbh_Departments.Title AS DepartmentTitle, " & _
                    "ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum " & _
                    "FROM tbh_Products INNER JOIN " & _
                    "tbh_Departments ON tbh_Products.DepartmentID = tbh_Departments.DepartmentID " & _
                    ") AllProducts " & _
                    "WHERE AllProducts.RowNum BETWEEN {1} AND {2} " & _
                    "ORDER BY RowNum ASC", sortExpression, lowerBound, upperBound)
                Dim cmd As New SqlCommand(sql, cn)
                cn.Open()
                Return GetProductCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of products
        Public Overloads Overrides Function GetProductCount() As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetProductCount", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves all products for the specified department
        Public Overloads Overrides Function GetProducts(ByVal departmentID As Integer, ByVal sortExpression As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As System.Collections.Generic.List(Of ProductDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                sortExpression = EnsureValidProductsSortExpression(sortExpression)
                Dim lowerBound As Integer = pageIndex * pageSize + 1
                Dim upperBound As Integer = (pageIndex + 1) * pageSize
                Dim sql As String = String.Format("SELECT * FROM " & _
                    "(SELECT tbh_Products.ProductID, tbh_Products.AddedDate, tbh_Products.AddedBy, tbh_Products.DepartmentID, tbh_Products.Title, " & _
                    "tbh_Products.Description, tbh_Products.SKU, tbh_Products.UnitPrice, tbh_Products.DiscountPercentage, " & _
                    "tbh_Products.UnitsInStock, tbh_Products.SmallImageUrl, tbh_Products.FullImageUrl, tbh_Products.Votes, " & _
                    "tbh_Products.TotalRating, tbh_Departments.Title AS DepartmentTitle, " & _
                    "ROW_NUMBER() OVER (ORDER BY {0}) AS RowNum " & _
                    "FROM tbh_Products INNER JOIN " & _
                    "tbh_Departments ON tbh_Products.DepartmentID = tbh_Departments.DepartmentID " & _
                    "WHERE tbh_Products.DepartmentID = {1} " & _
                    ") DepartmentProducts " & _
                    "WHERE DepartmentProducts.RowNum BETWEEN {2} AND {3} " & _
                    "ORDER BY RowNum ASC", sortExpression, departmentID, lowerBound, upperBound)
                Dim cmd As New SqlCommand(sql, cn)
                cn.Open()
                Return GetProductCollectionFromReader(ExecuteReader(cmd), False)
            End Using
        End Function

        ' Returns the total number of products for the specified department
        Public Overloads Overrides Function GetProductCount(ByVal departmentID As Integer) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetProductCountByDepartment", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID
                cn.Open()
                Return CInt(ExecuteScalar(cmd))
            End Using
        End Function

        ' Retrieves the product with the specified ID
        Public Overrides Function GetProductByID(ByVal productID As Integer) As ProductDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetProductByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetProductFromReader(reader, True)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Retrieves the description for the product with the specified ID
        Public Overrides Function GetProductDescription(ByVal productID As Integer) As String
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetProductDescription", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID
                cn.Open()
                Return CStr(ExecuteScalar(cmd))
            End Using
        End Function

        ' Deletes a product
        Public Overrides Function DeleteProduct(ByVal productID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DeleteProduct", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new product
        Public Overrides Function InsertProduct(ByVal product As ProductDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertProduct", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = product.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = product.AddedBy
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = product.DepartmentID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = product.Title
                cmd.Parameters.Add("@Description", SqlDbType.NText).Value = product.Description
                cmd.Parameters.Add("@SKU", SqlDbType.NVarChar).Value = product.SKU
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = product.UnitPrice
                cmd.Parameters.Add("@DiscountPercentage", SqlDbType.Int).Value = product.DiscountPercentage
                cmd.Parameters.Add("@UnitsInStock", SqlDbType.Int).Value = product.UnitsInStock
                cmd.Parameters.Add("@SmallImageUrl", SqlDbType.NVarChar).Value = product.SmallImageURL
                cmd.Parameters.Add("@FullImageUrl", SqlDbType.NVarChar).Value = product.FullImageUrl
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@ProductID").Value)
            End Using
        End Function

        ' Updates a product
        Public Overrides Function UpdateProduct(ByVal product As ProductDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_UpdateProduct", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = product.ID
                cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = product.DepartmentID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = product.Title
                cmd.Parameters.Add("@Description", SqlDbType.NText).Value = product.Description
                cmd.Parameters.Add("@SKU", SqlDbType.NVarChar).Value = product.SKU
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = product.UnitPrice
                cmd.Parameters.Add("@DiscountPercentage", SqlDbType.Int).Value = product.DiscountPercentage
                cmd.Parameters.Add("@UnitsInStock", SqlDbType.Int).Value = product.UnitsInStock
                cmd.Parameters.Add("@SmallImageUrl", SqlDbType.NVarChar).Value = product.SmallImageURL
                cmd.Parameters.Add("@FullImageUrl", SqlDbType.NVarChar).Value = product.FullImageUrl
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a vote for the specified product
        Public Overrides Function RateProduct(ByVal productID As Integer, ByVal rating As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertVote", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID
                cmd.Parameters.Add("@Rating", SqlDbType.Int).Value = rating
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Decrements the UnitsInStock field of the specified quantity for the specified product
        Public Overrides Function DecrementProductUnitsInStock(ByVal productID As Integer, ByVal quantity As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DecrementUnitsInStock", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Retrieves the list of orders for the specified customer
        Public Overloads Overrides Function GetOrders(ByVal addedBy As String) As System.Collections.Generic.List(Of OrderDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrdersByCustomer", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = addedBy
                cn.Open()
                Return GetOrderCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Retrieves the list of orders in the specified state, and within the specified date range
        Public Overloads Overrides Function GetOrders(ByVal statusID As Integer, ByVal fromDate As Date, ByVal toDate As Date) As System.Collections.Generic.List(Of OrderDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrdersByStatus", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusID
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate
                cn.Open()
                Return GetOrderCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Retrieves the order with the specified ID
        Public Overrides Function GetOrderByID(ByVal orderID As Integer) As OrderDetails
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrderByID", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderID
                cn.Open()
                Dim reader As IDataReader = ExecuteReader(cmd, CommandBehavior.SingleRow)
                If reader.Read() Then
                    Return GetOrderFromReader(reader)
                Else
                    Return Nothing
                End If
            End Using
        End Function

        ' Deletes an order
        Public Overrides Function DeleteOrder(ByVal orderID As Integer) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_DeleteOrder", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Inserts a new order
        Public Overrides Function InsertOrder(ByVal order As OrderDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertOrder", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = order.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = order.AddedBy
                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = order.StatusID
                cmd.Parameters.Add("@ShippingMethod", SqlDbType.NVarChar).Value = order.ShippingMethod
                cmd.Parameters.Add("@SubTotal", SqlDbType.Money).Value = order.SubTotal
                cmd.Parameters.Add("@Shipping", SqlDbType.Money).Value = order.Shipping
                cmd.Parameters.Add("@ShippingFirstName", SqlDbType.NVarChar).Value = order.ShippingFirstName
                cmd.Parameters.Add("@ShippingLastName", SqlDbType.NVarChar).Value = order.ShippingLastName
                cmd.Parameters.Add("@ShippingStreet", SqlDbType.NVarChar).Value = order.ShippingStreet
                cmd.Parameters.Add("@ShippingPostalCode", SqlDbType.NVarChar).Value = order.ShippingPostalCode
                cmd.Parameters.Add("@ShippingCity", SqlDbType.NVarChar).Value = order.ShippingCity
                cmd.Parameters.Add("@ShippingState", SqlDbType.NVarChar).Value = order.ShippingState
                cmd.Parameters.Add("@ShippingCountry", SqlDbType.NVarChar).Value = order.ShippingCountry
                cmd.Parameters.Add("@CustomerEmail", SqlDbType.NVarChar).Value = order.CustomerEmail
                cmd.Parameters.Add("@CustomerPhone", SqlDbType.NVarChar).Value = order.CustomerPhone
                cmd.Parameters.Add("@CustomerFax", SqlDbType.NVarChar).Value = order.CustomerFax
                cmd.Parameters.Add("@TransactionID", SqlDbType.NVarChar).Value = order.TransactionID
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@OrderID").Value)
            End Using
        End Function

        ' Updates an existing order
        Public Overrides Function UpdateOrder(ByVal order As OrderDetails) As Boolean
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim shippedDate As Object = order.ShippedDate
                If order.ShippedDate = DateTime.MinValue Then
                    shippedDate = DBNull.Value
                End If

                Dim cmd As New SqlCommand("tbh_Store_UpdateOrder", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = order.ID
                cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = order.StatusID
                cmd.Parameters.Add("@ShippedDate", SqlDbType.DateTime).Value = shippedDate
                cmd.Parameters.Add("@TransactionID", SqlDbType.NVarChar).Value = order.TransactionID
                cmd.Parameters.Add("@TrackingID", SqlDbType.NVarChar).Value = order.TrackingID
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return (ret = 1)
            End Using
        End Function

        ' Get a collection with all order items for the specified order
        Public Overrides Function GetOrderItems(ByVal orderID As Integer) As System.Collections.Generic.List(Of OrderItemDetails)
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_GetOrderItems", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderID
                cn.Open()
                Return GetOrderItemCollectionFromReader(ExecuteReader(cmd))
            End Using
        End Function

        ' Inserts a new order item
        Public Overrides Function InsertOrderItem(ByVal orderItem As OrderItemDetails) As Integer
            Using cn As New SqlConnection(Me.ConnectionString)
                Dim cmd As New SqlCommand("tbh_Store_InsertOrderItem", cn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = orderItem.AddedDate
                cmd.Parameters.Add("@AddedBy", SqlDbType.NVarChar).Value = orderItem.AddedBy
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderItem.OrderID
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderItem.ProductID
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = orderItem.Title
                cmd.Parameters.Add("@SKU", SqlDbType.NVarChar).Value = orderItem.SKU
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = orderItem.UnitPrice
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderItem.Quantity
                cmd.Parameters.Add("@OrderItemID", SqlDbType.Int).Direction = ParameterDirection.Output
                cn.Open()
                Dim ret As Integer = ExecuteNonQuery(cmd)
                Return CInt(cmd.Parameters("@OrderItemID").Value)
            End Using
        End Function
    End Class
End Namespace
