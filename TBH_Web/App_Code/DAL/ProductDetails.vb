Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class ProductDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _departmentID As Integer = 0
        Private _departmentTitle As String = ""
        Private _title As String = ""
        Private _description As String = ""
        Private _sku As String = ""
        Private _unitPrice As Decimal = 0.0
        Private _discountPercentage As Integer = 0
        Private _unitsInStock As Integer = 0
        Private _smallImageUrl As String = ""
        Private _fullImageUrl As String = ""
        Private _votes As Integer = 0
        Private _totalRating As Integer = 0

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

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
            Me.SmallImageURL = smallImageUrl
            Me.FullImageUrl = fullImageUrl
            Me.Votes = votes
            Me.TotalRating = totalRating

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
            Set(ByVal value As String)
                _departmentTitle = value
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

        Public Property Description() As String
            Get
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
            Set(ByVal value As Decimal)
                _unitPrice = value
            End Set
        End Property

        Public Property DiscountPercentage() As Integer
            Get
                Return _discountPercentage
            End Get
            Set(ByVal value As Integer)
                _discountPercentage = value
            End Set
        End Property

        Public Property UnitsInStock() As Integer
            Get
                Return _unitsInStock
            End Get
            Set(ByVal value As Integer)
                _unitsInStock = value
            End Set
        End Property

        Public Property SmallImageURL() As String
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
            Set(ByVal value As Integer)
                _votes = value
            End Set
        End Property

        Public Property TotalRating() As Integer
            Get
                Return _totalRating
            End Get
            Set(ByVal value As Integer)
                _totalRating = value
            End Set
        End Property
    End Class
End Namespace
