Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.BLL.Store
    Public Class OrderItem
        Inherits BaseStore

        ' ==========
        ' Private variables
        ' ==========

        Private _orderID As Integer = 0
        Private _productID As Integer = 0
        Private _title As String = ""
        Private _sku As String = ""
        Private _unitPrice As Decimal = 0.0
        Private _quantity As Integer = 0

        ' ==========
        ' Properties
        ' ==========

        Public Property OrderID() As Integer
            Get
                Return _orderID
            End Get
            Private Set(ByVal value As Integer)
                _orderID = value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _productID
            End Get
            Private Set(ByVal value As Integer)
                _productID = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Private Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property SKU() As String
            Get
                Return _sku
            End Get
            Private Set(ByVal value As String)
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

        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get
            Private Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal orderID As Integer, ByVal productID As Integer, ByVal title As String, ByVal sku As String, ByVal unitPrice As Decimal, ByVal quantity As Integer)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.OrderID = orderID
            Me.ProductID = productID
            Me.Title = title
            Me.SKU = sku
            Me.UnitPrice = unitPrice
            Me.Quantity = quantity
        End Sub
    End Class
End Namespace
