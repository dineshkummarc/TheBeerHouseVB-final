Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Store
    Public Class Product
        Inherits BaseStore

        ' ==========
        ' Private variables
        ' ==========

        Private _departmentID As Integer = 0
        Private _departmentTitle As String = ""
        Private _department As Department
        Private _title As String = ""
        Private _description As String
        Private _sku As String = ""
        Private _unitPrice As Decimal = 0.0
        Private _discountPercentage As Integer = 0
        Private _unitsInStock As Integer = 0
        Private _smallImageUrl As String = ""
        Private _fullImageUrl As String = ""
        Private _votes As Integer = 0
        Private _totalRating As Integer = 0

        ' ==========
        ' Properties
        ' ==========

        Public Property DepartmentID() As Integer
            Get
                Return _departmentID
            End Get
            Set(ByVal value As Integer)
                _departmentID = value
            End Set
        End Property
        Public Property DepartmentTitle() As String
            Get
                Return _departmentTitle
            End Get
            Private Set(ByVal value As String)
                _departmentTitle = value
            End Set
        End Property

        Public ReadOnly Property Department() As Department
            Get
                If IsNothing(_department) Then
                    _department = BLL.Store.Department.GetDepartmentByID(Me.DepartmentID)
                End If

                Return _department
            End Get
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                If IsNothing(_description) Then
                    _description = SiteProvider.Store.GetProductDescription(Me.ID)
                End If
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property SKU() As String
            Get
                Return _sku
            End Get
            Set(ByVal value As String)
                _sku = value
            End Set
        End Property

        Public Property UnitPrice() As Decimal
            Get
                Return _unitPrice
            End Get
            Private Set(ByVal value As Decimal)
                _unitPrice = value
            End Set
        End Property

        Public Property DiscountPercentage() As Integer
            Get
                Return _discountPercentage
            End Get
            Private Set(ByVal value As Integer)
                _discountPercentage = value
            End Set
        End Property

        Public ReadOnly Property FinalUnitPrice() As Decimal
            Get
                If Me.DiscountPercentage > 0 Then
                    Return Me.UnitPrice - (Me.UnitPrice * Me.DiscountPercentage / 100)
                Else
                    Return Me.UnitPrice
                End If
            End Get
        End Property

        Public Property UnitsInStock() As Integer
            Get
                Return _unitsInStock
            End Get
            Private Set(ByVal value As Integer)
                _unitsInStock = value
            End Set
        End Property

        Public Property SmallImageUrl() As String
            Get
                Return _smallImageUrl
            End Get
            Set(ByVal value As String)
                _smallImageUrl = value
            End Set
        End Property

        Public Property FullImageUrl() As String
            Get
                Return _fullImageUrl
            End Get
            Set(ByVal value As String)
                _fullImageUrl = value
            End Set
        End Property

        Public Property Votes() As Integer
            Get
                Return _votes
            End Get
            Private Set(ByVal value As Integer)
                _votes = value
            End Set
        End Property

        Public Property TotalRating() As Integer
            Get
                Return _totalRating
            End Get
            Private Set(ByVal value As Integer)
                _totalRating = value
            End Set
        End Property

        Public ReadOnly Property AverageRating() As Double
            Get
                If Me.Votes >= 1 Then
                    Return (CDbl(Me.TotalRating) / CDbl(Me.Votes))
                Else
                    Return 0.0
                End If
            End Get
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal departmentID As Integer, ByVal departmentTitle As String, ByVal title As String, _
                ByVal description As String, ByVal sku As String, ByVal unitPrice As Decimal, _
                ByVal discountPercentage As Integer, ByVal unitsInStock As Integer, _
                ByVal smallImageUrl As String, ByVal fullImageUrl As String, ByVal votes As Integer, _
                ByVal totalRating As Integer)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.DepartmentID = departmentID
            Me.DepartmentTitle = departmentTitle
            Me.Title = title
            Me.Description = description
            Me.SKU = sku
            Me.UnitPrice = unitPrice
            Me.DiscountPercentage = discountPercentage
            Me.UnitsInStock = unitsInStock
            Me.SmallImageUrl = smallImageUrl
            Me.FullImageUrl = fullImageUrl
            Me.Votes = votes
            Me.TotalRating = totalRating
        End Sub

        ' ==========
        ' Methods
        ' ==========

        Public Function Delete() As Boolean
            Dim success As Boolean = Product.DeleteProduct(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return Product.UpdateProduct(Me.ID, Me.DepartmentID, Me.Title, _
               Me.Description, Me.SKU, Me.UnitPrice, Me.DiscountPercentage, Me.UnitsInStock, _
               Me.SmallImageUrl, Me.FullImageUrl)
        End Function

        Public Function Rate(ByVal rating As Integer) As Boolean
            Return Product.RateProduct(Me.ID, rating)
        End Function

        Public Function DecrementUnitsInStock(ByVal quantity As Integer) As Boolean
            Dim success As Boolean = Product.DecrementProductUnitsInStock(Me.ID, quantity)
            If success Then Me.UnitsInStock -= quantity
            Return success
        End Function

        ' ===========
        ' Shared Methods
        ' ===========

        ' Returns a collection with all products
        Public Shared Function GetProducts() As List(Of Product)
            Return GetProducts("", 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetProducts(ByVal sortExpression As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Product)
            If IsNothing(sortExpression) Then sortExpression = ""

            Dim products As List(Of Product)
            Dim key As String = "Store_Products_" & sortExpression & "_" & startRowIndex.ToString() & "_" & maximumRows.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                products = CType(BizObject.Cache(key), List(Of Product))
            Else
                Dim recordset As List(Of ProductDetails) = SiteProvider.Store.GetProducts( _
                   sortExpression, GetPageIndex(startRowIndex, maximumRows), maximumRows)
                products = GetProductListFromProductDetailsList(recordset)
                BaseStore.CacheData(key, products)
            End If
            Return products
        End Function

        ' Returns a collection with all products for the specified store department
        Public Shared Function GetProducts(ByVal departmentID As Integer) As List(Of Product)
            Return GetProducts(departmentID, "", 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetProducts(ByVal departmentID As Integer, ByVal sortExpression As String, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Product)
            If departmentID <= 0 Then
                Return GetProducts(sortExpression, startRowIndex, maximumRows)
            End If

            Dim products As List(Of Product)
            Dim key As String = "Store_Products_" & departmentID.ToString() & "_" & sortExpression & "_" & _
               startRowIndex.ToString() & "_" & maximumRows.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                products = CType(BizObject.Cache(key), List(Of Product))
            Else
                Dim recordset As List(Of ProductDetails) = SiteProvider.Store.GetProducts(departmentID, _
                   sortExpression, GetPageIndex(startRowIndex, maximumRows), maximumRows)
                products = GetProductListFromProductDetailsList(recordset)
                BaseStore.CacheData(key, products)
            End If

            Return products
        End Function

        ' Returns the number of total products
        Public Shared Function GetProductCount() As Integer
            Dim productCount As Integer = 0
            Dim key As String = "Store_ProductCount"

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                productCount = CInt(BizObject.Cache(key))
            Else
                productCount = SiteProvider.Store.GetProductCount()
                BaseStore.CacheData(key, productCount)
            End If
            Return productCount
        End Function

        ' Returns the number of total products for the specified department
        Public Shared Function GetProductCount(ByVal departmentID As Integer) As Integer
            If departmentID <= 0 Then Return GetProductCount()

            Dim productCount As Integer = 0
            Dim key As String = "Store_ProductCount_" & departmentID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                productCount = CInt(BizObject.Cache(key))
            Else
                productCount = SiteProvider.Store.GetProductCount(departmentID)
                BaseStore.CacheData(key, productCount)
            End If

            Return productCount
        End Function

        ' Returns an Product object with the specified ID
        Public Shared Function GetProductByID(ByVal productID As Integer) As Product
            Dim product As Product
            Dim key As String = "Store_Product_" & productID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                product = CType(BizObject.Cache(key), Product)
            Else
                product = GetProductFromProductDetails(SiteProvider.Store.GetProductByID(productID))
                BaseStore.CacheData(key, product)
            End If
            Return product
        End Function

        ' Updates an existing product
        Public Shared Function UpdateProduct(ByVal id As Integer, ByVal departmentID As Integer, _
                ByVal title As String, ByVal description As String, ByVal sku As String, _
                ByVal unitPrice As Decimal, ByVal discountPercentage As Integer, _
                ByVal unitsInStock As Integer, ByVal smallImageUrl As String, _
                ByVal fullImageUrl As String) _
                As Boolean

            title = BizObject.ConvertNullToEmptyString(title)
            description = BizObject.ConvertNullToEmptyString(description)
            sku = BizObject.ConvertNullToEmptyString(sku)
            smallImageUrl = BizObject.ConvertNullToEmptyString(smallImageUrl)
            fullImageUrl = BizObject.ConvertNullToEmptyString(fullImageUrl)

            Dim record As New ProductDetails(id, DateTime.Now, "", departmentID, _
               "", title, description, sku, unitPrice, discountPercentage, unitsInStock, _
               smallImageUrl, fullImageUrl, 0, 0)
            Dim ret As Boolean = SiteProvider.Store.UpdateProduct(record)

            BizObject.PurgeCacheItems("store_product_" & id.ToString())
            BizObject.PurgeCacheItems("store_products")
            Return ret
        End Function

        ' Creates a new product
        Public Shared Function InsertProduct(ByVal departmentID As Integer, ByVal title As String, _
                ByVal description As String, ByVal sku As String, ByVal unitPrice As Decimal, _
                ByVal discountPercentage As Integer, ByVal unitsInStock As Integer, _
                ByVal smallImageUrl As String, ByVal fullImageUrl As String) _
                As Integer

            title = BizObject.ConvertNullToEmptyString(title)
            description = BizObject.ConvertNullToEmptyString(description)
            sku = BizObject.ConvertNullToEmptyString(sku)
            smallImageUrl = BizObject.ConvertNullToEmptyString(smallImageUrl)
            fullImageUrl = BizObject.ConvertNullToEmptyString(fullImageUrl)

            Dim record As New ProductDetails(0, DateTime.Now, BizObject.CurrentUserName, _
               departmentID, "", title, description, sku, unitPrice, discountPercentage, _
               unitsInStock, smallImageUrl, fullImageUrl, 0, 0)
            Dim ret As Integer = SiteProvider.Store.InsertProduct(record)

            BizObject.PurgeCacheItems("store_product")
            Return ret
        End Function

        ' Deletes an existing product
        Public Shared Function DeleteProduct(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DeleteProduct(id)
            Dim ev As New RecordDeletedEvent("product", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("store_product")
            Return ret
        End Function

        ' Increments a product's view count
        Public Shared Function RateProduct(ByVal id As Integer, ByVal rating As Integer) As Boolean
            Return SiteProvider.Store.RateProduct(id, rating)
        End Function

        ' Decrements a product's UnitsInStock field
        Public Shared Function DecrementProductUnitsInStock(ByVal id As Integer, ByVal quantity As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DecrementProductUnitsInStock(id, quantity)
            BizObject.PurgeCacheItems("store_product_" & id.ToString())
            Return ret
        End Function

        ' Returns a Product object filled with the data taken from the input ProductDetails
        Private Shared Function GetProductFromProductDetails(ByVal record As ProductDetails) As Product
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Product(record.ID, record.AddedDate, record.AddedBy, _
                   record.DepartmentID, record.DepartmentTitle, record.Title, record.Description, _
                   record.SKU, record.UnitPrice, record.DiscountPercentage, record.UnitsInStock, _
                   record.SmallImageURL, record.FullImageUrl, record.Votes, record.TotalRating)
            End If
        End Function

        ' Returns a list of Product objects filled with the data taken from the input list of ProductDetails
        Private Shared Function GetProductListFromProductDetailsList(ByVal recordset As List(Of ProductDetails)) As List(Of Product)
            Dim products As New List(Of Product)
            For Each record As ProductDetails In recordset
                products.Add(GetProductFromProductDetails(record))
            Next

            Return products
        End Function
    End Class
End Namespace
