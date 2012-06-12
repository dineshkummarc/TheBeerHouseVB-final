Imports Microsoft.VisualBasic
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts

Namespace MB.TheBeerHouse.UI
    Public Class BaseWebPart
        Inherits UserControl
        Implements IWebPart

        Private _catalogIconImageUrl As String = ""
        Private _description As String = ""
        Protected _subTitle As String = ""
        Protected _title As String = ""
        Private _titleIconImageUrl As String = ""
        Private _titleUrl As String = ""

        Property CatalogIconImageUrl() As String _
            Implements IWebPart.CatalogIconImageUrl
            Get
                Return _catalogIconImageUrl
            End Get
            Set(ByVal value As String)
                _catalogIconImageUrl = value
            End Set
        End Property

        Public Property Description() As String _
            Implements IWebPart.Description
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public ReadOnly Property SubTitle() As String _
            Implements IWebPart.Subtitle
            Get
                Return _subTitle
            End Get
        End Property

        Public Property Title() As String _
            Implements IWebPart.Title
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property TitleIconImageUrl() As String _
            Implements IWebPart.TitleIconImageUrl
            Get
                Return _titleIconImageUrl
            End Get
            Set(ByVal value As String)
                _titleIconImageUrl = value
            End Set
        End Property

        Public Property TitleUrl() As String _
            Implements IWebPart.TitleUrl
            Get
                Return _titleUrl
            End Get
            Set(ByVal value As String)
                _titleUrl = value
            End Set
        End Property
    End Class
End Namespace
