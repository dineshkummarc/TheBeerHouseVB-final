Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class ArticleDetails

        ' ============
        ' Private variables
        ' ============
        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _categoryID As Integer = 0
        Private _categoryTitle As String = ""
        Private _title As String = ""
        Private _abstract As String = ""
        Private _body As String = ""
        Private _country As String = ""
        Private _state As String = ""
        Private _city As String = ""
        Private _releaseDate As DateTime = DateTime.Now
        Private _expireDate As DateTime = DateTime.MaxValue
        Private _approved As Boolean = True
        Private _listed As Boolean = True
        Private _commentsEnabled As Boolean = True
        Private _onlyForMembers As Boolean = True
        Private _viewCount As Integer = 0
        Private _votes As Integer = 0
        Private _totalRating As Integer = 0

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

        Public Property CategoryID() As Integer
            Get
                Return _categoryID
            End Get
            Set(ByVal value As Integer)
                _categoryID = value
            End Set
        End Property

        Public Property CategoryTitle() As String
            Get
                Return _categoryTitle
            End Get
            Set(ByVal value As String)
                _categoryTitle = value
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

        Public Property Abstract() As String
            Get
                Return _abstract
            End Get
            Set(ByVal value As String)
                _abstract = value
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

        Public Property Country() As String
            Get
                Return _country
            End Get
            Set(ByVal value As String)
                _country = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return _state
            End Get
            Set(ByVal value As String)
                _state = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return _city
            End Get
            Set(ByVal value As String)
                _city = value
            End Set
        End Property

        Public Property ReleaseDate() As DateTime
            Get
                Return _releaseDate
            End Get
            Set(ByVal value As DateTime)
                _releaseDate = value
            End Set
        End Property

        Public Property ExpireDate() As DateTime
            Get
                Return _expireDate
            End Get
            Set(ByVal value As DateTime)
                _expireDate = value
            End Set
        End Property

        Public Property Approved() As Boolean
            Get
                Return _approved
            End Get
            Set(ByVal value As Boolean)
                _approved = value
            End Set
        End Property

        Public Property Listed() As Boolean
            Get
                Return _listed
            End Get
            Set(ByVal value As Boolean)
                _listed = value
            End Set
        End Property

        Public Property CommentsEnabled() As Boolean
            Get
                Return _commentsEnabled
            End Get
            Set(ByVal value As Boolean)
                _commentsEnabled = value
            End Set
        End Property

        Public Property OnlyForMembers() As Boolean
            Get
                Return _onlyForMembers
            End Get
            Set(ByVal value As Boolean)
                _onlyForMembers = value
            End Set
        End Property

        Public Property ViewCount() As Integer
            Get
                Return _viewCount
            End Get
            Set(ByVal value As Integer)
                _viewCount = value
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


        ' ==========
        ' Constructors
        ' ==========
        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
        ByVal categoryID As Integer, ByVal categoryTitle As String, ByVal title As String, _
        ByVal artabstract As String, ByVal body As String, ByVal country As String, _
        ByVal state As String, ByVal city As String, ByVal releaseDate As DateTime, _
        ByVal expireDate As DateTime, ByVal approved As Boolean, ByVal listed As Boolean, _
        ByVal commentsEnabled As Boolean, ByVal onlyForMembers As Boolean, _
        ByVal viewCount As Integer, ByVal votes As Integer, ByVal totalRating As Integer)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.CategoryID = categoryID
            Me.CategoryTitle = categoryTitle
            Me.Title = title
            Me.Abstract = artabstract
            Me.Body = body
            Me.Country = country
            Me.State = state
            Me.City = city
            Me.ReleaseDate = releaseDate
            Me.ExpireDate = expireDate
            Me.Approved = approved
            Me.Listed = listed
            Me.CommentsEnabled = commentsEnabled
            Me.OnlyForMembers = onlyForMembers
            Me.ViewCount = viewCount
            Me.Votes = votes
            Me.TotalRating = totalRating
        End Sub

    End Class
End Namespace
