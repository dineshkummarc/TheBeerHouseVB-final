Imports System.Security
Imports System.Web.Security
Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.BLL.Articles

Namespace MB.TheBeerHouse.UI
    Partial Class ShowArticle
        Inherits BasePage

        ' ==========
        ' Private variables
        ' ==========
        Private _userCanEdit As Boolean = False
        Private _articleID As Integer = 0

        ' ==========
        ' Properties
        ' ==========
        Public Property UserCanEdit() As Boolean
            Get
                Return _userCanEdit
            End Get
            Set(ByVal value As Boolean)
                _userCanEdit = value
            End Set
        End Property

        ' ==========
        ' Page events
        ' ==========

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            UserCanEdit = (Me.User.Identity.IsAuthenticated AndAlso _
               (Me.User.IsInRole("Administrators") OrElse Me.User.IsInRole("Editors")))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If String.IsNullOrEmpty(Me.Request.QueryString("ID")) Then
                Throw New ApplicationException("Missing parameter on the querystring.")
            Else
                _articleID = Integer.Parse(Me.Request.QueryString("ID"))
            End If

            If Not Me.IsPostBack Then
                ' try to load the article with the specified ID, and raise an exception
                ' if it doesn't exist
                Dim article As Article = BLL.Articles.Article.GetArticleByID(_articleID)
                If IsNothing(article) Then
                    Throw New ApplicationException("No article found for the specified ID.")
                End If

                ' Check if the article is published (appproved + released + not yet expired).
                ' If not, continue only if the current user is an administrator or an Editor
                If Not article.Published Then
                    If Not Me.UserCanEdit Then
                        Throw New SecurityException( _
                            "What are you trying to do??? You're not allowed to view this article!")
                    End If
                End If

                ' if the article has the OnlyForMemebers = true, and the current user
                ' is anonymous, redirect to the login page
                If article.OnlyForMembers AndAlso Not Me.User.Identity.IsAuthenticated Then
                    Me.RequestLogin()
                End If

                article.IncrementViewCount()

                ' if we get here, display all article's data on the page
                Me.Title = String.Format(Me.Title, article.Title)
                lblTitle.Text = article.Title
                lblNotApproved.Visible = Not article.Approved
                lblAddedBy.Text = article.AddedBy
                lblReleaseDate.Text = article.ReleaseDate.ToShortDateString()
                lblCategory.Text = article.CategoryTitle
                lblLocation.Visible = (article.Location.Length > 0)
                If lblLocation.Visible Then
                    lblLocation.Text = String.Format(lblLocation.Text, article.Location)
                End If
                lblRating.Text = String.Format(lblRating.Text, article.Votes)
                ratDisplay.Value = article.AverageRating
                ratDisplay.Visible = (article.Votes > 0)
                lblViews.Text = String.Format(lblViews.Text, article.ViewCount)
                lblAbstract.Text = article.Abstract
                lblBody.Text = article.Body
                panComments.Visible = article.CommentsEnabled
                panEditArticle.Visible = Me.UserCanEdit
                btnApprove.Visible = Not article.Approved
                lnkEditArticle.NavigateUrl = String.Format(lnkEditArticle.NavigateUrl, _articleID)

                ' hide the rating box controls if the current user has already voted for this article
                Dim userRating As Integer = GetUserRating()
                If userRating > 0 Then
                    ShowUserRating(userRating)
                End If
            End If
        End Sub

        ' ==========
        ' Control Event Methods
        ' ==========

        Protected Sub btnRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRate.Click
            ' check whether the user has already rated this article
            Dim userRating As Integer = GetUserRating
            If userRating > 0 Then
                ShowUserRating(userRating)
            Else
                ' rate the article, then create a cookie to remember this user's rating
                userRating = ddlRatings.SelectedIndex + 1
                Article.RateArticle(_articleID, userRating)
                ShowUserRating(userRating)

                Dim cookie As New HttpCookie("Rating_Article" & _articleID.ToString, userRating.ToString)
                cookie.Expires = DateTime.Now.AddDays( _
                    Globals.Settings.Articles.RatingLockInterval)
                Me.Response.Cookies.Add(cookie)
            End If
        End Sub

        Protected Sub dlstComments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlstComments.SelectedIndexChanged
            dvwComment.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub dvwComment_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwComment.ItemCommand
            If e.CommandName = "Cancel" Then
                dlstComments.SelectedIndex = -1
                dlstComments.DataBind()
            End If
        End Sub

        Protected Sub dvwComment_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwComment.ItemInserted
            dlstComments.SelectedIndex = -1
            dlstComments.DataBind()
        End Sub

        Protected Sub dvwComment_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwComment.ItemUpdated
            dlstComments.SelectedIndex = -1
            dlstComments.DataBind()
        End Sub

        Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApprove.Click
            Article.ApproveArticle(_articleID)
            btnApprove.Visible = False
        End Sub

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
            Article.DeleteArticle(_articleID)
            Me.Response.Redirect("BrowseArticles.aspx", False)
        End Sub

        Protected Sub dlstComments_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlstComments.ItemCommand
            If e.CommandName = "Delete" Then
                Dim commentID As Integer = Integer.Parse(e.CommandArgument.ToString())
                Comment.DeleteComment(commentID)
                dvwComment.ChangeMode(DetailsViewMode.Insert)
                dlstComments.SelectedIndex = -1
                dlstComments.DataBind()
            End If
        End Sub

        Protected Sub dvwComment_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwComment.ItemCreated
            ' when in Insert Mode, pre-fill the username and e-mail fields with the
            ' current user's information, if she is authenticated
            If dvwComment.CurrentMode = DetailsViewMode.Insert AndAlso _
               Me.User.Identity.IsAuthenticated Then

                Dim user As MembershipUser = Membership.GetUser()
                CType(dvwComment.FindControl("txtAddedBy"), TextBox).Text = user.UserName
                CType(dvwComment.FindControl("txtAddedByEmail"), TextBox).Text = user.Email
            End If
        End Sub

        ' ==========
        ' Protected and Private Methods
        ' ==========
        Protected Function GetUserRating() As Integer
            Dim rating As Integer = 0
            Dim cookie As HttpCookie = Me.Request.Cookies("Rating_Article" & _articleID.ToString)
            If Not IsNothing(cookie) Then
                rating = Integer.Parse(cookie.Value)
            End If
            Return rating
        End Function

        Protected Sub ShowUserRating(ByVal rating As Integer)
            lblUserRating.Text = String.Format(lblUserRating.Text, rating)
            ddlRatings.Visible = False
            btnRate.Visible = False
            lblUserRating.Visible = True
        End Sub
    End Class
End Namespace
