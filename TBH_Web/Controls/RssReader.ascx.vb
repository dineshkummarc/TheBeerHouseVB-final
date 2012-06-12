Imports System.Xml
Imports System.Data
Imports MB.TheBeerHouse.UI

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class RssReader
        Inherits BaseWebPart

        Public Sub New()
            Me.Title = "RSS Reader"
        End Sub

        <Personalizable(PersonalizationScope.User), _
        WebBrowsable(), _
        WebDisplayName("Rss Url"), _
        WebDescription("The Url of the RSS feed")> _
        Public Property RssUrl() As String
            Get
                Return lnkRss.NavigateUrl
            End Get
            Set(ByVal value As String)
                Dim url As String = value
                If value.StartsWith("/") OrElse value.StartsWith("~/") Then
                    url = CType(Me.Page, BasePage).FullBaseUrl & value
                    url = url.Replace("~/", "")
                End If
                lnkRss.NavigateUrl = url
            End Set
        End Property


        <Personalizable(PersonalizationScope.User), _
        WebBrowsable(), _
        WebDisplayName("Header Text"), _
        WebDescription("The header's text")> _
        Public Property HeaderText() As String
            Get
                Return lblTitle.Text
            End Get
            Set(ByVal value As String)
                lblTitle.Text = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.User), _
        WebBrowsable(), _
        WebDisplayName("Number of columns"), _
        WebDescription("The grid's number of columns")> _
        Public Property RepeatColumns() As Integer
            Get
                Return dlstRss.RepeatColumns
            End Get
            Set(ByVal value As Integer)
                dlstRss.RepeatColumns = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.User), _
        WebBrowsable(), _
        WebDisplayName("More Url"), _
        WebDescription("The Url of the link pointing to more content")> _
        Public Property MoreUrl() As String
            Get
                Return lnkMore.NavigateUrl
            End Get
            Set(ByVal value As String)
                lnkMore.NavigateUrl = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.User), _
        WebBrowsable(), _
        WebDisplayName("More Text"), _
        WebDescription("The text of the link pointing to more content")> _
        Public Property MoreText() As String
            Get
                Return lnkMore.Text
            End Get
            Set(ByVal value As String)
                lnkMore.Text = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            DoBinding()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            DoBinding()
        End Sub

        Protected Sub DoBinding()
            Try
                If Me.RssUrl.Length = 0 Then
                    Throw New ApplicationException("The RssUrl cannot be null.")
                End If

                ' create a DataTable and fill it with the RSS data,
                ' then bind ir to the Repeater control
                Dim feed As New XmlDataDocument
                feed.Load(Me.RssUrl)
                Dim posts As XmlNodeList = feed.GetElementsByTagName("item")

                Dim table As New DataTable("Feed")
                table.Columns.Add("Title", GetType(String))
                table.Columns.Add("Description", GetType(String))
                table.Columns.Add("Link", GetType(String))
                table.Columns.Add("PubDate", GetType(DateTime))

                For Each post As XmlNode In posts
                    Dim row As DataRow = table.NewRow
                    row("Title") = post("title").InnerText
                    row("Description") = post("description").InnerText.Trim
                    row("Link") = post("link").InnerText
                    row("PubDate") = DateTime.Parse(post("pubDate").InnerText)
                    table.Rows.Add(row)
                Next

                dlstRss.DataSource = table
                dlstRss.DataBind()

            Catch ex As Exception
                Me.Visible = False
            End Try
        End Sub
    End Class
End Namespace
