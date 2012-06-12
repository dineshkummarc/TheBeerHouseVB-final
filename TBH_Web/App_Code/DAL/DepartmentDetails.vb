Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class DepartmentDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _title As String = ""
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageUrl As String = ""

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Importance = importance
            Me.Description = description
            Me.ImageUrl = imageUrl
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

        Public Property Importance() As Integer
            Get
                Return _importance
            End Get
            Set(ByVal value As Integer)
                _importance = value
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

        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property
    End Class
End Namespace
