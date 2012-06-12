Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Articles
    Public Class Article
        Inherits BaseArticle

        ' ==========
        ' Private Variables
        ' ==========
        Private _categoryID As Integer = 0
        Private _categoryTitle As String = ""
        Private _category As Category = Nothing
        Private _title As String = ""
        Private _abstract As String = ""
        Private _body As String
        Private _country As String
        Private _state As String
        Private _city As String
        Private _releaseDate As DateTime = DateTime.Now
        Private _expireDate As DateTime = DateTime.MaxValue
        Private _approved As Boolean = True
        Private _listed As Boolean = True
        Private _commentsEnabled As Boolean = True
        Private _onlyForMembers As Boolean = True
        Private _viewCount As Integer = 0
        Private _votes As Integer = 0
        Private _totalRating As Integer = 0
        Private _comments As List(Of Comment) = Nothing

        ' ==========
        ' Properties
        ' ==========
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

        Public ReadOnly Property Category() As Category
            Get
                If IsNothing(_category) Then
                    _category = _
                        MB.TheBeerHouse.BLL.Articles.Category.GetCategoryByID(Me.CategoryID)
                End If
                Return _category
            End Get
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
                If IsNothing(_body) Then
                    _body = SiteProvider.Articles.GetArticleBody(Me.ID)
                End If
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

        Public ReadOnly Property Location() As String
            Get
                Dim _location As String = Me.City.Split(";")(0)

                If Me.State.Length > 0 Then
                    If _location.Length > 0 Then
                        _location += ", "
                    End If
                    _location += Me.State.Split(";")(0)
                End If
                If Me.Country.Length > 0 Then
                    If _location.Length > 0 Then
                        _location += ", "
                    End If
                    _location += Me.Country
                End If
                Return _location
            End Get
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

        Public ReadOnly Property AverageRating() As Double
            Get
                If Me.Votes >= 1 Then
                    Return CDbl(Me.TotalRating) / CDbl(Me.Votes)
                Else
                    Return 0.0
                End If
            End Get
        End Property

        Public ReadOnly Property Comments() As List(Of Comment)
            Get
                If IsNothing(_comments) Then
                    _comments = Comment.GetComments(Me.ID, 0, BizObject.MAXROWS)
                End If
                Return _comments
            End Get
        End Property

        Public ReadOnly Property Published() As Boolean
            Get
                Return (Me.Approved And Me.ReleaseDate <= DateTime.Now And Me.ExpireDate > DateTime.Now)
            End Get
        End Property

        ' ==========
        ' Constructors
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, _
                ByVal addedBy As String, ByVal categoryID As Integer, _
                ByVal categoryTitle As String, ByVal title As String, _
                ByVal artabstract As String, ByVal body As String, _
                ByVal country As String, ByVal state As String, ByVal city As String, _
                ByVal releaseDate As DateTime, ByVal expireDate As DateTime, _
                ByVal approved As Boolean, ByVal listed As Boolean, _
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

        ' ==========
        ' Methods
        ' ==========

        Public Function Delete() As Boolean
            Dim success As Boolean = Article.DeleteArticle(Me.ID)
            If success Then
                Me.ID = 0
            End If
            Return success
        End Function

        Public Function Update() As Boolean
            Return Article.UpdateArticle(Me.ID, Me.CategoryID, Me.Title, _
                Me.Abstract, Me.Body, Me.Country, Me.State, Me.City, _
                Me.ReleaseDate, Me.ExpireDate, Me.Approved, Me.Listed, _
                Me.CommentsEnabled, Me.OnlyForMembers)
        End Function

        Public Function Approve() As Boolean
            Dim success As Boolean = Article.ApproveArticle(Me.ID)
            If success Then
                Me.Approved = True
            End If
            Return success
        End Function

        Public Function IncrementViewCount() As Boolean
            Return Article.IncrementArticleViewCount(Me.ID)
        End Function

        Public Function Rate(ByVal rating As Integer) As Boolean
            Return Article.RateArticle(Me.ID, rating)
        End Function

        ' ============
        ' Shared methods
        ' ============

        ' Returns a collection with all articles
        Public Shared Function GetArticles() As List(Of Article)
            Return GetArticles(0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetArticles( _
                ByVal startRowIndex As Integer, ByVal maximumRows As Integer) _
                As List(Of Article)

            Dim articles As List(Of Article) = Nothing
            Dim key As String = _
                "Articles_Articles_" + startRowIndex.ToString() + "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articles = CType(BizObject.Cache(key), List(Of Article))
            Else
                Dim recordset As List(Of ArticleDetails) = _
                    SiteProvider.Articles.GetArticles( _
                    GetPageIndex(startRowIndex, maximumRows), maximumRows)
                articles = GetArticleListFromArticleDetailsList(recordset)
                BaseArticle.CacheData(key, articles)
            End If
            Return articles
        End Function

        ' Returns a collection with all articles for the specified category
        Public Shared Function GetArticles(ByVal categoryID As Integer) As List(Of Article)
            Return GetArticles(categoryID, 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetArticles(ByVal CategoryID As Integer, ByVal startRowIndex As Integer, ByVal maximumRows As Integer) As List(Of Article)
            If CategoryID <= 0 Then
                Return GetArticles(startRowIndex, maximumRows)
            End If

            Dim articles As List(Of Article) = Nothing
            Dim key As String = "Articles_Articles_" + CategoryID.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articles = CType(BizObject.Cache(key), List(Of Article))
            Else
                Dim recordset As List(Of ArticleDetails) = SiteProvider.Articles.GetArticles(CategoryID, _
                    GetPageIndex(startRowIndex, maximumRows), maximumRows)
                articles = GetArticleListFromArticleDetailsList(recordset)
                BaseArticle.CacheData(key, articles)
            End If

            Return articles
        End Function

        ' Returns the number of total articles
        Public Shared Function GetArticleCount() As Integer
            Dim articleCount As Integer = 0
            Dim key As String = "Articles_ArticleCount"

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articleCount = CInt(BizObject.Cache(key))
            Else
                articleCount = SiteProvider.Articles.GetArticleCount()
                BaseArticle.CacheData(key, articleCount)
            End If

            Return articleCount
        End Function

        ' Returns the number of total articles for the specified category
        Public Shared Function GetArticleCount(ByVal categoryID As Integer) As Integer
            If categoryID <= 0 Then
                Return GetArticleCount()
            End If

            Dim articleCount As Integer = 0
            Dim key As String = "Articles_ArticleCount_" + categoryID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articleCount = CInt(BizObject.Cache(key))
            Else
                articleCount = SiteProvider.Articles.GetArticleCount(categoryID)
                BaseArticle.CacheData(key, articleCount)
            End If

            Return articleCount
        End Function

        ' Returns a collection with all published articles
        Public Shared Function GetArticles(ByVal publishedOnly As Boolean) As List(Of Article)
            Return GetArticles(publishedOnly, 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetArticles( _
                ByVal publishedOnly As Boolean, ByVal startRowIndex As Integer, _
                ByVal maximumRows As Integer) _
                As List(Of Article)

            If Not publishedOnly Then
                Return GetArticles(startRowIndex, maximumRows)
            End If

            Dim articles As List(Of Article) = Nothing
            Dim key As String = "Articles_Articles_" + publishedOnly.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articles = CType(BizObject.Cache(key), List(Of Article))
            Else
                Dim recordset As List(Of ArticleDetails) = _
                    SiteProvider.Articles.GetPublishedArticles(DateTime.Now, _
                   GetPageIndex(startRowIndex, maximumRows), maximumRows)
                articles = GetArticleListFromArticleDetailsList(recordset)
                BaseArticle.CacheData(key, articles)
            End If
            Return articles
        End Function

        ' Returns a collection with all published articles for the specified category
        Public Shared Function GetArticles( _
            ByVal publishedOnly As Boolean, ByVal categoryID As Integer) _
            As List(Of Article)

            Return GetArticles(publishedOnly, categoryID, 0, BizObject.MAXROWS)
        End Function

        Public Shared Function GetArticles( _
                ByVal publishedOnly As Boolean, ByVal categoryID As Integer, _
                ByVal startRowIndex As Integer, ByVal maximumRows As Integer) _
                As List(Of Article)

            If Not publishedOnly Then
                Return GetArticles(categoryID, startRowIndex, maximumRows)
            End If

            If categoryID <= 0 Then
                Return GetArticles(publishedOnly, startRowIndex, maximumRows)
            End If

            Dim articles As List(Of Article) = Nothing
            Dim key As String = "Articles_Articles_" + publishedOnly.ToString() + _
                "_" + categoryID.ToString() + "_" + startRowIndex.ToString() + _
                "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articles = CType(BizObject.Cache(key), List(Of Article))
            Else
                Dim recordset As List(Of ArticleDetails) = _
                    SiteProvider.Articles.GetPublishedArticles( _
                    categoryID, DateTime.Now, GetPageIndex(startRowIndex, maximumRows), maximumRows)
                articles = GetArticleListFromArticleDetailsList(recordset)
                BaseArticle.CacheData(key, articles)
            End If
            Return articles
        End Function

        ' Returns the number of total published articles
        Public Shared Function GetArticleCount(ByVal publishedOnly As Boolean) As Integer
            If publishedOnly Then
                Return GetArticleCount()
            End If

            Dim articleCount As Integer = 0
            Dim key As String = "Articles_ArticleCount_" + publishedOnly.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articleCount = CInt(BizObject.Cache(key))
            Else
                articleCount = SiteProvider.Articles.GetPublishedArticleCount(DateTime.Now)
                BaseArticle.CacheData(key, articleCount)
            End If

            Return articleCount
        End Function

        ' Returns the number of total published articles for the specified category
        Public Shared Function GetArticleCount(ByVal publishedOnly As Boolean, ByVal categoryID As Integer) As Integer
            If Not publishedOnly Then
                Return GetArticleCount(categoryID)
            End If

            If categoryID <= 0 Then
                Return GetArticleCount(publishedOnly)
            End If

            Dim articleCount As Integer = 0
            Dim key As String = "Articles_ArticleCount_" + publishedOnly.ToString() + "_" + categoryID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                articleCount = CInt(BizObject.Cache(key))
            Else
                articleCount = SiteProvider.Articles.GetPublishedArticleCount(categoryID, DateTime.Now)
                BaseArticle.CacheData(key, articleCount)
            End If

            Return articleCount
        End Function

        ' Returns an Article object with the specified ID
        Public Shared Function GetArticleByID(ByVal articleID As Integer) As Article
            Dim m_article As Article = Nothing
            Dim key As String = "Articles_Article_" + articleID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                m_article = CType(BizObject.Cache(key), Article)
            Else
                m_article = GetArticleFromArticleDetails(SiteProvider.Articles.GetArticleByID(articleID))
                BaseArticle.CacheData(key, m_article)
            End If
            Return m_article
        End Function

        ' Updates an existing article
        Public Shared Function UpdateArticle( _
                ByVal id As Integer, ByVal categoryID As Integer, ByVal title As String, _
                ByVal Abstract As String, ByVal body As String, ByVal country As String, _
                ByVal state As String, ByVal city As String, ByVal releaseDate As DateTime, _
                ByVal expireDate As DateTime, ByVal approved As Boolean, _
                ByVal listed As Boolean, ByVal commentsEnabled As Boolean, _
                ByVal onlyForMembers As Boolean) _
                As Boolean

            title = BizObject.ConvertNullToEmptyString(title)
            Abstract = BizObject.ConvertNullToEmptyString(Abstract)
            body = BizObject.ConvertNullToEmptyString(body)
            country = BizObject.ConvertNullToEmptyString(country)
            state = BizObject.ConvertNullToEmptyString(state)
            city = BizObject.ConvertNullToEmptyString(city)

            If releaseDate = DateTime.MinValue Then
                releaseDate = DateTime.Now
            End If

            If expireDate = DateTime.MinValue Then
                expireDate = DateTime.MaxValue
            End If

            Dim record As ArticleDetails = New ArticleDetails(id, DateTime.Now, "", categoryID, _
               "", title, Abstract, body, country, state, city, releaseDate, expireDate, _
               approved, listed, commentsEnabled, onlyForMembers, 0, 0, 0)
            Dim ret As Boolean = SiteProvider.Articles.UpdateArticle(record)

            BizObject.PurgeCacheItems("articles_article_" + id.ToString())
            BizObject.PurgeCacheItems("articles_articles")

            Return ret
        End Function

        ' Creates a new article
        Public Shared Function InsertArticle( _
                ByVal categoryID As Integer, ByVal title As String, ByVal Abstract As String, _
                ByVal body As String, ByVal country As String, ByVal state As String, _
                ByVal city As String, ByVal releaseDate As DateTime, _
                ByVal expireDate As DateTime, ByVal approved As Boolean, _
                ByVal listed As Boolean, ByVal commentsEnabled As Boolean, _
                ByVal onlyForMembers As Boolean) _
                As Integer

            ' ensure that the "approved" option is false if the current user is not
            ' an administrator or a editor (it may be a contributor for example)
            Dim canApprove As Boolean = _
                (BizObject.CurrentUser.IsInRole("Administrators") Or _
                BizObject.CurrentUser.IsInRole("Editors"))
            If Not (canApprove) Then
                approved = False
            End If

            title = BizObject.ConvertNullToEmptyString(title)
            Abstract = BizObject.ConvertNullToEmptyString(Abstract)
            body = BizObject.ConvertNullToEmptyString(body)
            country = BizObject.ConvertNullToEmptyString(country)
            state = BizObject.ConvertNullToEmptyString(state)
            city = BizObject.ConvertNullToEmptyString(city)

            If releaseDate = DateTime.MinValue Then
                releaseDate = DateTime.Now
            End If

            If expireDate = DateTime.MinValue Then
                expireDate = DateTime.MaxValue
            End If

            Dim record As New ArticleDetails(0, DateTime.Now, BizObject.CurrentUserName, _
               categoryID, "", title, Abstract, body, country, state, city, releaseDate, expireDate, _
               approved, listed, commentsEnabled, onlyForMembers, 0, 0, 0)
            Dim ret As Integer = SiteProvider.Articles.InsertArticle(record)

            BizObject.PurgeCacheItems("articles_article")
            Return ret
        End Function

        ' Deletes an existing article
        Public Shared Function DeleteArticle(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Articles.DeleteArticle(id)
            Dim ev As New RecordDeletedEvent("article", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("articles_article")
            Return ret
        End Function

        ' Approves an existing article
        Public Shared Function ApproveArticle(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Articles.ApproveArticle(id)
            BizObject.PurgeCacheItems("articles_article_" + id.ToString())
            BizObject.PurgeCacheItems("articles_articles")
            Return ret
        End Function

        ' Increments an article's view count
        Public Shared Function IncrementArticleViewCount(ByVal id As Integer) As Boolean
            Return SiteProvider.Articles.IncrementArticleViewCount(id)
        End Function

        ' Increments an article's vote count
        Public Shared Function RateArticle(ByVal id As Integer, ByVal rating As Integer) As Boolean
            Return SiteProvider.Articles.RateArticle(id, rating)
        End Function

        ' Returns a Article object filled with the data taken from the input ArticleDetails
        Public Shared Function GetArticleFromArticleDetails( _
                ByVal record As ArticleDetails) _
                As Article

            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Article(record.ID, record.AddedDate, record.AddedBy, _
                   record.CategoryID, record.CategoryTitle, record.Title, _
                   record.Abstract, record.Body, record.Country, record.State, _
                   record.City, record.ReleaseDate, record.ExpireDate, _
                   record.Approved, record.Listed, record.CommentsEnabled, _
                   record.OnlyForMembers, record.ViewCount, record.Votes, _
                   record.TotalRating)
            End If
        End Function

        ' Returns a list of Article objects filled with the data taken from the input list of ArticleDetails
        Public Shared Function GetArticleListFromArticleDetailsList( _
                ByVal recordset As List(Of ArticleDetails)) _
                As List(Of Article)

            Dim articles As New List(Of Article)
            For Each record As ArticleDetails In recordset
                articles.Add(GetArticleFromArticleDetails(record))
            Next
            Return articles
        End Function
    End Class
End Namespace
