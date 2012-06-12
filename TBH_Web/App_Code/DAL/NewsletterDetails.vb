Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public Class NewsletterDetails

        ' ==========
        ' Private Variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _subject As String = ""
        Private _plainTextBody As String = ""
        Private _htmlBody As String = ""

        ' ===========
        ' Constructors
        ' ===========

        Public Sub New()

        End Sub

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal subject As String, ByVal plainTextBody As String, ByVal htmlBody As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Subject = subject
            Me.PlainTextBody = plainTextBody
            Me.HtmlBody = htmlBody
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

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                _subject = value
            End Set
        End Property

        Public Property PlainTextBody() As String
            Get
                Return _plainTextBody
            End Get
            Set(ByVal value As String)
                _plainTextBody = value
            End Set
        End Property

        Public Property HtmlBody() As String
            Get
                Return _htmlBody
            End Get
            Set(ByVal value As String)
                _htmlBody = value
            End Set
        End Property
    End Class
End Namespace
