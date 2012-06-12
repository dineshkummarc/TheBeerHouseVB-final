Imports System.Collections.Generic
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Articles

Namespace MB.TheBeerHouse.UI
    Partial Class GetArticlesRss
        Inherits BasePage

        Private _rssTitle As String = "Recent Articles"

        Public Property RssTitle() As String
            Get
                Return _rssTitle
            End Get
            Set(ByVal value As String)
                _rssTitle = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim categoryID As Integer = 0
            If Not String.IsNullOrEmpty(Me.Request.QueryString("CatID")) Then
                categoryID = Integer.Parse(Me.Request.QueryString("CatID"))
                Dim category As Category = BLL.Articles.Category.GetCategoryByID(categoryID)
                _rssTitle = category.Title
            End If

            Dim articles As List(Of Article) = Article.GetArticles(True, categoryID, _
                0, Globals.Settings.Articles.RssItems)
            rptRss.DataSource = articles
            rptRss.DataBind()
        End Sub
    End Class
End Namespace
