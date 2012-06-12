Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL
Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.BLL.Articles
    Public Class Comment
        Inherits BaseArticle

        ' ============
        ' Private variables
        ' ============

        Private _addedByEmail As String = ""
        Private _addedByIP As String = ""
        Private _articleID As Integer = 0
        Private _articleTitle As String = ""
        Private _article As Article = Nothing
        Private _body As String = ""

        ' ==========
        ' Properties
        ' ==========
        Public Property AddedByEmail() As String
            Get
                Return _addedByEmail
            End Get
            Set(ByVal value As String)
                _addedByEmail = value
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

        Public Property ArticleID() As Integer
            Get
                Return _articleID
            End Get
            Private Set(ByVal value As Integer)
                _articleID = value
            End Set
        End Property

        Public Property ArticleTitle() As String
            Get
                Return _articleTitle
            End Get
            Set(ByVal value As String)
                _articleTitle = value
            End Set
        End Property

        Public ReadOnly Property Article() As Article
            Get
                If IsNothing(_article) Then
                    _article = MB.TheBeerHouse.BLL.Articles.Article.GetArticleByID(Me.ArticleID)
                End If
                Return _article
            End Get
        End Property

        Public Property Body() As String
            Get
                Return _body
            End Get
            Set(ByVal value As String)
                _body = value
            End Set
        End Property

        Public ReadOnly Property EncodedBody() As String
            Get
                Return UI.Helpers.ConvertToHtml(Me.Body)
            End Get
        End Property

        ' ==========
        ' Constructors
        ' ==========
        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, _
                ByVal addedBy As String, ByVal addedByEmail As String, _
                ByVal addedByIP As String, ByVal articleID As Integer, _
                ByVal articleTitle As String, ByVal body As String)

            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.AddedByEmail = addedByEmail
            Me.AddedByIP = addedByIP
            Me.ArticleID = articleID
            Me.ArticleTitle = articleTitle
            Me.Body = body
        End Sub

        ' ==========
        ' Methods
        ' ==========
        Public Function Delete() As Boolean
            Dim success As Boolean = Comment.DeleteComment(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return Comment.UpdateComment(Me.ID, Me.Body)
        End Function

        ' ***********
        ' Shared Methods
        ' ***********

        ' Returns a collection with all comments
        Public Shared Function GetComments() As List(Of Comment)
            Dim comments As List(Of Comment) = GetComments(0, BizObject.MAXROWS)
            comments.Sort(New CommentComparer("AddedDate ASC"))
            Return comments
        End Function

        Public Shared Function GetComments( _
                ByVal startRowIndex As Integer, ByVal maximumRows As Integer) _
                As List(Of Comment)

            Dim comments As List(Of Comment) = Nothing
            Dim key As String = _
                "Articles_Comments_" + startRowIndex.ToString() + "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                comments = CType(BizObject.Cache(key), List(Of Comment))
            Else
                Dim recordset As List(Of CommentDetails) = SiteProvider.Articles.GetComments( _
                    GetPageIndex(startRowIndex, maximumRows), maximumRows)
                comments = GetCommentListFromCommentDetailsList(recordset)
                BaseArticle.CacheData(key, comments)
            End If

            Return comments
        End Function

        ' Returns a collection with all comments for the specified article
        Public Shared Function GetComments(ByVal articleID As Integer) As List(Of Comment)
            Dim comments As List(Of Comment) = GetComments(articleID, 0, BizObject.MAXROWS)
            comments.Sort(New CommentComparer("AddedDate ASC"))
            Return comments
        End Function

        Public Shared Function GetComments( _
                ByVal articleID As Integer, ByVal startRowIndex As Integer, _
                ByVal maximumRows As Integer) _
                As List(Of Comment)

            Dim comments As List(Of Comment) = Nothing
            Dim key As String = "Articles_Comments_" + articleID.ToString() + "_" + startRowIndex.ToString() + "_" + maximumRows.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                comments = CType(BizObject.Cache(key), List(Of Comment))
            Else
                Dim recordset As List(Of CommentDetails) = _
                    SiteProvider.Articles.GetComments(articleID, _
                    GetPageIndex(startRowIndex, maximumRows), maximumRows)
                comments = GetCommentListFromCommentDetailsList(recordset)
                BaseArticle.CacheData(key, comments)
            End If

            Return comments
        End Function

        ' Returns the number of total comments
        Public Shared Function GetCommentCount() As Integer
            Dim commentCount As Integer = 0
            Dim key As String = "Articles_CommentCount"

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                commentCount = CInt(BizObject.Cache(key))
            Else
                commentCount = SiteProvider.Articles.GetCommentCount()
                BaseArticle.CacheData(key, commentCount)
            End If

            Return commentCount
        End Function

        ' Returns the number of total comments for the specified article
        Public Shared Function GetCommentCount(ByVal articleID As Integer) As Integer
            Dim commentCount As Integer = 0
            Dim key As String = "Articles_CommentCount_" + articleID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                commentCount = CInt(BizObject.Cache(key))
            Else
                commentCount = SiteProvider.Articles.GetCommentCount(articleID)
                BaseArticle.CacheData(key, commentCount)
            End If

            Return commentCount
        End Function

        ' Returns a Comment object with the specified ID
        Public Shared Function GetCommentByID(ByVal commentID As Integer) As Comment
            Dim comment As Comment = Nothing
            Dim key As String = "Articles_Comment_" + commentID.ToString()

            If BaseArticle.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                comment = CType(BizObject.Cache(key), Comment)
            Else
                comment = GetCommentFromCommentDetails( _
                    SiteProvider.Articles.GetCommentByID(commentID))
                BaseArticle.CacheData(key, comment)
            End If

            Return comment
        End Function

        ' Updates an existing comment
        Public Shared Function UpdateComment(ByVal id As Integer, ByVal body As String) As Boolean
            Dim record As New CommentDetails(id, DateTime.Now, "", "", "", 0, "", body)
            Dim ret As Boolean = SiteProvider.Articles.UpdateComment(record)
            BizObject.PurgeCacheItems("articles_comment")
            Return ret
        End Function

        ' Deletes an existing comment
        Public Shared Function DeleteComment(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Articles.DeleteComment(id)
            Dim ev As New RecordDeletedEvent("comment", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("articles_comment")
            Return ret
        End Function

        ' Creates a new comment
        Public Shared Function InsertComment( _
                ByVal addedBy As String, ByVal addedByEmail As String, _
                ByVal articleID As Integer, ByVal body As String) _
                As Integer

            Dim record As New CommentDetails(0, DateTime.Now, addedBy, addedByEmail, _
                BizObject.CurrentUserIP, articleID, "", body)
            Dim ret As Integer = SiteProvider.Articles.InsertComment(record)
            BizObject.PurgeCacheItems("articles_comment")
            Return ret
        End Function

        ' Returns a Comment object filled with the data taken from the input CommentDetails
        Public Shared Function GetCommentFromCommentDetails(ByVal record As CommentDetails) As Comment
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Comment(record.ID, record.AddedDate, record.AddedBy, _
                   record.AddedByEmail, record.AddedByIP, _
                   record.ArticleID, record.ArticleTitle, record.Body)
            End If
        End Function

        ' Returns a list of Comment objects filled with the data taken from the input list of CommentDetails
        Public Shared Function GetCommentListFromCommentDetailsList( _
                ByVal recordset As List(Of CommentDetails)) _
                As List(Of Comment)

            Dim comments As New List(Of Comment)
            For Each record As CommentDetails In recordset
                comments.Add(GetCommentFromCommentDetails(record))
            Next

            Return comments
        End Function

        ' Comparer class, to be used with List(Of Comment).Sort
        Public Class CommentComparer
            Implements IComparer(Of Comment)

            Private _sortBy As String
            Private _reverse As Boolean

            Public Sub New(ByVal sortBy As String)
                If Not String.IsNullOrEmpty(sortBy) Then
                    sortBy = sortBy.ToLower()
                    _reverse = sortBy.EndsWith(" desc")
                    _sortBy = sortBy.Replace(" desc", "").Replace(" asc", "")
                End If
            End Sub

            Shadows Function Equals(ByVal x As Comment, ByVal y As Comment) As Boolean
                Return (x.ID = y.ID)
            End Function

            Public Function Compare(ByVal x As Comment, ByVal y As Comment) As Integer _
                    Implements System.Collections.Generic.IComparer(Of Comment).Compare

                Dim ret As Integer = 0
                Select Case (_sortBy)
                    Case "addeddate"
                        ret = DateTime.Compare(x.AddedDate, y.AddedDate)

                    Case "addedby"
                        ret = String.Compare(x.AddedBy, y.AddedBy, StringComparison.InvariantCultureIgnoreCase)
                End Select

                If _reverse Then
                    Return ret * -1
                Else
                    Return ret
                End If
            End Function
        End Class
    End Class
End Namespace
