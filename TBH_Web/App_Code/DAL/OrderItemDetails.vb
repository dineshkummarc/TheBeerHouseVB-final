Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class OrderItemDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _orderID As Integer = 0
        Private _productID As Integer = 0
        Private _title As String = ""
        Private _sku As String = ""
        Private _unitPrice As Decimal = 0.0
        Private _quantity As Integer = 0

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal orderID As Integer, ByVal productID As Integer, ByVal title As String, _
                ByVal sku As String, ByVal unitPrice As Decimal, ByVal quantity As Integer)

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

        ' ==========
        ' Properties
        ' ==========

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property AddedDate() As DateTime
            Get
                Return _addedDate
            End Get
            Set(ByVal value As DateTime)
                _addedDate = value
            End Set
        End Property

        Public Property AddedBy() As String
            Get
                Return _addedBy
            End Get
            Set(ByVal value As String)
                _addedBy = value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _orderID
            End Get
            Set(ByVal value As Integer)
                _orderID = value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _productID
            End Get
            Set(ByVal value As Integer)
                _productID = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
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
            Set(ByVal value As Decimal)
                _unitPrice = value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property
    End Class
End Namespace
