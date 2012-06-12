Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class ForumDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _title As String = ""
        Private _moderated As Boolean = False
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageUrl As String = ""

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedby As String, ByVal title As String, ByVal moderated As Boolean, ByVal importance As Integer, ByVal description As String, ByVal imageURL As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedby
            Me.Title = title
            Me.Moderated = moderated
            Me.Importance = importance
            Me.Description = description
            Me.ImageURL = imageURL
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

        Public Property Moderated() As Boolean
            Get
                Return _moderated
            End Get
            Set(ByVal value As Boolean)
                _moderated = value
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
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property
    End Class
End Namespace
