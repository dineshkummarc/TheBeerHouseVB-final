Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class CategoryDetails
        ' ==========
        ' Private variables
        ' ==========
        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _title As String = ""
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageURL As String = ""

        ' ===========
        ' Properties
        ' ===========
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

        Public Property ImageURL() As String
            Get
                Return _imageURL
            End Get
            Set(ByVal value As String)
                _imageURL = value
            End Set
        End Property

        ' ============
        ' Constructors
        ' ============
        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
        ByVal title As String, ByVal importance As Integer, ByVal description As String, _
        ByVal imageURL As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Importance = importance
            Me.Description = description
            Me.ImageURL = imageURL
        End Sub
    End Class
End Namespace
