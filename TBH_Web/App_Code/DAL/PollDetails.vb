Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class PollDetails

        ' ==========
        ' Private Variables
        ' ==========
        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _questionText As String = ""
        Private _isCurrent As Boolean = False
        Private _isArchived As Boolean = False
        Private _archivedDate As DateTime = DateTime.MinValue
        Private _votes As Integer = 0

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, _
                ByVal addedBy As String, ByVal questionText As String, _
                ByVal isCurrent As Boolean, ByVal isArchived As Boolean, _
                ByVal archivedDate As DateTime, ByVal votes As Integer)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.QuestionText = questionText
            Me.IsCurrent = isCurrent
            Me.IsArchived = isArchived
            Me.ArchivedDate = archivedDate
            Me.Votes = votes
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

        Public Property QuestionText() As String
            Get
                Return _questionText
            End Get
            Set(ByVal value As String)
                _questionText = value
            End Set
        End Property

        Public Property IsCurrent() As Boolean
            Get
                Return _isCurrent
            End Get
            Set(ByVal value As Boolean)
                _isCurrent = value
            End Set
        End Property

        Public Property IsArchived() As Boolean
            Get
                Return _isArchived
            End Get
            Set(ByVal value As Boolean)
                _isArchived = value
            End Set
        End Property

        Public Property ArchivedDate() As DateTime
            Get
                Return _archivedDate
            End Get
            Set(ByVal value As DateTime)
                _archivedDate = value
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
    End Class
End Namespace
