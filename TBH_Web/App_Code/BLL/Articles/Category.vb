Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Articles
    Public Class Category
        Inherits BaseArticle

        ' ==========
        ' Private Variables
        ' ==========
        Private _title As String = ""
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageUrl As String = ""
        Private _allArticles As List(Of Article) = Nothing
        Private _publishedArticles As List(Of Article) = Nothing

        ' ==========
        ' Properties
        ' ==========
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Importance() As Integer
            Get
                Return _importance
            End Get
            Set(ByVal value As Integer)
                _importance = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property

        Public ReadOnly Property AllArticles() As List(Of Article)
            Get
                If IsNothing(_allArticles) Then
                    _allArticles = Article.GetArticles(Me.ID, 0, BizObject.MAXROWS)
                End If
                Return _allArticles
            End Get
        End Property

        Public ReadOnly Property PublishedArticles() As List(Of Article)
            Get
                If IsNothing(_publishedArticles) Then
                    _publishedArticles = Article.GetArticles(True, Me.ID, 0, BizObject.MAXROWS)
                End If
                Return _publishedArticles
            End Get
        End Property

        ' ===========
        ' Constructor
        ' ===========
        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Importance = importance
            Me.Description = description
            Me.ImageUrl = imageUrl
        End Sub

        ' ==========
        ' Methods
        ' ==========
        Public Function Delete() As Boolean
            Dim success As Boolean = Category.DeleteCategory(Me.ID)
            If success Then
                Me.ID = 0
            End If
            Return success
        End Function

        Public Function Update() As Boolean
            Return Category.UpdateCategory(Me.ID, Me.Title, Me.Importance, Me.Description, Me.ImageUrl)
        End Function

        ' ==========
        ' Shared methods
        ' ==========

        ' Returns a collection with all the categories
        Public Shared Function GetCategories() As List(Of Category)
            Dim categories As List(Of Category) = Nothing
            Dim key As String = "Articles_Categories"

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                categories = CType(BizObject.Cache(key), List(Of Category))
            Else
                Dim recordset As List(Of CategoryDetails) = SiteProvider.Articles.GetCategories
                categories = GetCategoryListFromCategoryDetailsList(recordset)
                BaseArticle.CacheData(key, categories)
            End If

            Return categories
        End Function

        ' Returns a Category object with the specified ID
        Public Shared Function GetCategoryByID(ByVal categoryID As Integer) As Category
            Dim m_category As Category = Nothing
            Dim key As String = "Articles_Category_" + categoryID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                m_category = CType(BizObject.Cache(key), Category)
            Else
                m_category = GetCategoryFromCategoryDetails( _
                    SiteProvider.Articles.GetCategoryByID(categoryID))
                BaseArticle.CacheData(key, m_category)
            End If

            Return m_category
        End Function

        ' Updates an existing category
        Public Shared Function UpdateCategory( _
                ByVal id As Integer, ByVal title As String, ByVal importance As Integer, _
                ByVal description As String, ByVal imageUrl As String) _
                As Boolean

            Dim record As New CategoryDetails( _
                id, DateTime.Now, "", title, importance, description, imageUrl)
            Dim ret As Boolean = SiteProvider.Articles.UpdateCategory(record)
            BizObject.PurgeCacheItems("articles_categor")
            Return ret
        End Function

        ' Deletes an existing category
        Public Shared Function DeleteCategory(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Articles.DeleteCategory(id)
            Dim ev As New RecordDeletedEvent("category", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("articles_categor")
            Return ret
        End Function

        ' Creates a new category
        Public Shared Function InsertCategory( _
                ByVal title As String, ByVal importance As Integer, _
                ByVal description As String, ByVal imageUrl As String) _
                As Integer

            Dim record As New CategoryDetails( _
                0, DateTime.Now, BizObject.CurrentUserName, title, importance, description, imageUrl)
            Dim ret As Integer = SiteProvider.Articles.InsertCategory(record)
            BizObject.PurgeCacheItems("articles_categor")
            Return ret
        End Function

        ' Returns a Category object filled with the data taken from the input CategoryDetails
        Public Shared Function GetCategoryFromCategoryDetails(ByVal record As CategoryDetails) As Category
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Category(record.ID, record.AddedDate, record.AddedBy, _
                   record.Title, record.Importance, record.Description, record.ImageURL)
            End If
        End Function

        ' Returns a list of Category objects filled with the data taken from the input list of CategoryDetails
        Public Shared Function GetCategoryListFromCategoryDetailsList( _
                ByVal recordset As List(Of CategoryDetails)) _
                As List(Of Category)

            Dim categories As New List(Of Category)
            For Each record As CategoryDetails In recordset
                categories.Add(GetCategoryFromCategoryDetails(record))
            Next
            Return categories
        End Function
    End Class
End Namespace
