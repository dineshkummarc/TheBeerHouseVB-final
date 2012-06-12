Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class OrderDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _statusID As Integer = 0
        Private _statusTitle As String = ""
        Private _shippingMethod As String = ""
        Private _subTotal As Decimal = 0.0
        Private _shipping As Decimal = 0.0
        Private _shippingFirstName As String = ""
        Private _shippingLastName As String = ""
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

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal statusID As Integer, ByVal statusTitle As String, ByVal shippingMethod As String, _
                ByVal subTotal As Decimal, ByVal shipping As Decimal, ByVal shippingFirstName As String, _
                ByVal shippingLastName As String, ByVal shippingStreet As String, _
                ByVal shippingPostalCode As String, ByVal shippingCity As String, _
                ByVal shippingState As String, ByVal shippingCountry As String, _
                ByVal customerEmail As String, ByVal customerPhone As String, ByVal customerFax As String, _
                ByVal shippedDate As DateTime, ByVal transactionID As String, ByVal trackingID As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.StatusID = statusID
            Me.StatusTitle = statusTitle
            Me.ShippingMethod = shippingMethod
            Me.SubTotal = subTotal
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
            Set(ByVal value As String)
                _statusTitle = value
            End Set
        End Property

        Public Property ShippingMethod() As String
            Get
                Return _shippingMethod
            End Get
            Set(ByVal value As String)
                _shippingMethod = value
            End Set
        End Property

        Public Property SubTotal() As Decimal
            Get
                Return _subTotal
            End Get
            Set(ByVal value As Decimal)
                _subTotal = value
            End Set
        End Property

        Public Property Shipping() As Decimal
            Get
                Return _shipping
            End Get
            Set(ByVal value As Decimal)
                _shipping = value
            End Set
        End Property

        Public Property ShippingFirstName() As String
            Get
                Return _shippingFirstName
            End Get
            Set(ByVal value As String)
                _shippingFirstName = value
            End Set
        End Property

        Public Property ShippingLastName() As String
            Get
                Return _shippingLastName
            End Get
            Set(ByVal value As String)
                _shippingLastName = value
            End Set
        End Property

        Public Property ShippingStreet() As String
            Get
                Return _shippingStreet
            End Get
            Set(ByVal value As String)
                _shippingStreet = value
            End Set
        End Property

        Public Property ShippingPostalCode() As String
            Get
                Return _shippingPostalCode
            End Get
            Set(ByVal value As String)
                _shippingPostalCode = value
            End Set
        End Property

        Public Property ShippingCity() As String
            Get
                Return _shippingCity
            End Get
            Set(ByVal value As String)
                _shippingCity = value
            End Set
        End Property

        Public Property ShippingState() As String
            Get
                Return _shippingState
            End Get
            Set(ByVal value As String)
                _shippingState = value
            End Set
        End Property

        Public Property ShippingCountry() As String
            Get
                Return _shippingCountry
            End Get
            Set(ByVal value As String)
                _shippingCountry = value
            End Set
        End Property

        Public Property CustomerEmail() As String
            Get
                Return _customerEmail
            End Get
            Set(ByVal value As String)
                _customerEmail = value
            End Set
        End Property

        Public Property CustomerPhone() As String
            Get
                Return _customerPhone
            End Get
            Set(ByVal value As String)
                _customerPhone = value
            End Set
        End Property

        Public Property CustomerFax() As String
            Get
                Return _customerFax
            End Get
            Set(ByVal value As String)
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
    End Class
End Namespace
