Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class ShippingMethodDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _title As String = ""
        Private _price As Decimal = 0.0

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal title As String, ByVal price As Decimal)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Price = price
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
    End Class
End Namespace
