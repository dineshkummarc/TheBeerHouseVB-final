Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class CommentDetails
        ' ==========
        ' Private variables
        ' ==========
        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _addedByEmail As String = ""
        Private _addedByIP As String = ""
        Private _body As String = ""
        Private _articleID As Integer = -1
        Private _articleTitle As String = ""

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

        Public Property AddedByEmail() As String
            Get
                Return _addedByEmail
            End Get
            Set(ByVal value As String)
                _addedByEmail = value
            End Set
        End Property

        Public Property AddedByIP() As String
            Get
                Return _addedByIP
            End Get
            Set(ByVal value As String)
                _addedByIP = value
            End Set
        End Property

        Public Property Body() As String
            Get
                Return _body
            End Get
            Set(ByVal value As String)
                _body = value
            End Set
        End Property

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Set(ByVal value As Integer)
                _articleID = value
            End Set
        End Property

        Public Property ArticleTitle() As String
            Get
                Return _articleTitle
            End Get
            Set(ByVal value As String)
                _articleTitle = value
            End Set
        End Property

        ' ==========
        ' Constructors
        ' ==========
        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
        ByVal addedByEmail As String, ByVal addedByIP As String, ByVal articleID As Integer, _
        ByVal articleTitle As String, ByVal body As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.AddedByEmail = addedByEmail
            Me.AddedByIP = addedByIP
            Me.ArticleID = articleID
            Me.ArticleTitle = articleTitle
            Me.Body = body
        End Sub
    End Class
End Namespace
