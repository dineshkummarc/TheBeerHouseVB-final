Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class PostDetails

        ' ==========
        ' Private variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _addedByIP As String = ""
        Private _forumID As Integer = 0
        Private _forumTitle As String = ""
        Private _parentPostID As Integer = 0
        Private _title As String = ""
        Private _body As String = ""
        Private _approved As Boolean = True
        Private _closed As Boolean = True
        Private _viewCount As Integer = 0
        Private _replyCount As Integer = 0
        Private _lastPostBy As String = ""
        Private _lastPostDate As DateTime = DateTime.MinValue

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal addedByIP As String, ByVal forumID As Integer, ByVal forumTitle As String, _
                ByVal parentPostID As Integer, ByVal title As String, ByVal body As String, _
                ByVal approved As Boolean, ByVal closed As Boolean, ByVal viewCount As Integer, _
                ByVal replyCount As Integer, ByVal lastPostDate As DateTime, ByVal lastPostBy As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.AddedByIP = addedByIP
            Me.ForumID = forumID
            Me.ForumTitle = forumTitle
            Me.ParentPostID = parentPostID
            Me.Title = title
            Me.Body = body
            Me.Approved = approved
            Me.Closed = closed
            Me.ViewCount = viewCount
            Me.ReplyCount = replyCount
            Me.LastPostDate = lastPostDate
            Me.LastPostBy = lastPostBy

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

        Public Property AddedByIP() As String
            Get
                Return _addedByIP
            End Get
            Set(ByVal value As String)
                _addedByIP = value
            End Set
        End Property

        Public Property ForumID() As Integer
            Get
                Return _forumID
            End Get
            Set(ByVal value As Integer)
                _forumID = value
            End Set
        End Property

        Public Property ForumTitle() As String
            Get
                Return _forumTitle
            End Get
            Set(ByVal value As String)
                _forumTitle = value
            End Set
        End Property

        Public Property ParentPostID() As Integer
            Get
                Return _parentPostID
            End Get
            Set(ByVal value As Integer)
                _parentPostID = value
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

        Public Property Body() As String
            Get
                Return _body
            End Get
            Set(ByVal value As String)
                _body = value
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

        Public Property Closed() As Boolean
            Get
                Return _closed
            End Get
            Set(ByVal value As Boolean)
                _closed = value
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

        Public Property ReplyCount() As Integer
            Get
                Return _replyCount
            End Get
            Set(ByVal value As Integer)
                _replyCount = value
            End Set
        End Property

        Public Property LastPostDate() As DateTime
            Get
                Return _lastPostDate
            End Get
            Set(ByVal value As DateTime)
                _lastPostDate = value
            End Set
        End Property

        Public Property LastPostBy() As String
            Get
                Return _lastPostBy
            End Get
            Set(ByVal value As String)
                _lastPostBy = value
            End Set
        End Property
    End Class
End Namespace

