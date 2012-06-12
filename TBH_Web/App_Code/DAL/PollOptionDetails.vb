Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class PollOptionDetails

        ' ==========
        ' Private Variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _pollID As Integer = 0
        Private _optionText As String = ""
        Private _votes As Integer = 0
        Private _percentage As Double = 0.0

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, _
                ByVal addedBy As String, ByVal pollID As Integer, _
                ByVal optionText As String, ByVal votes As Integer, ByVal percentage As Double)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.PollID = pollID
            Me.OptionText = optionText
            Me.Votes = votes
            Me.Percentage = percentage
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

        Public Property PollID() As Integer
            Get
                Return _pollID
            End Get
            Set(ByVal value As Integer)
                _pollID = value
            End Set
        End Property

        Public Property OptionText() As String
            Get
                Return _optionText
            End Get
            Set(ByVal value As String)
                _optionText = value
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

        Public Property Percentage() As Double
            Get
                Return _percentage
            End Get
            Set(ByVal value As Double)
                _percentage = value
            End Set
        End Property
    End Class
End Namespace