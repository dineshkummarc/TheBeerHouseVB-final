Imports MB.TheBeerHouse.BLL.Forums

Namespace MB.TheBeerHouse.UI
    Partial Class AddEditPost
        Inherits BasePage

        Private forumID As Integer = 0
        Private threadID As Integer = 0
        Private postID As Integer = 0
        Private quotePostID As Integer = 0
        Private isNewThread As Boolean = False
        Private isNewReply As Boolean = False
        Private isEditingPost As Boolean = False

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' retrieve the querystring parameters
            forumID = Integer.Parse(Me.Request.QueryString("ForumID"))
            If Not String.IsNullOrEmpty(Me.Request.QueryString("ThreadID")) Then
                threadID = Integer.Parse(Me.Request.QueryString("ThreadID"))
                If Not String.IsNullOrEmpty(Me.Request.QueryString("QuotePostID")) Then
                    quotePostID = Integer.Parse(Me.Request.QueryString("QuotePostID"))
                End If
            End If
            If Not String.IsNullOrEmpty(Me.Request.QueryString("PostID")) Then
                postID = Integer.Parse(Me.Request.QueryString("PostID"))
            End If

            isNewThread = ((postID = 0) And (threadID = 0))
            isEditingPost = Not (postID = 0)
            isNewReply = (Not isNewThread And Not isEditingPost)

            ' show/hide controls and load data according to the parameters above
            If Not Me.IsPostBack Then
                Dim isModerator As Boolean = (Me.User.IsInRole("Administrators") Or _
                    Me.User.IsInRole("Editors") Or Me.User.IsInRole("Moderators"))

                lnkThreadList.NavigateUrl = String.Format(lnkThreadList.NavigateUrl, forumID)
                lnkThreadPage.NavigateUrl = String.Format(lnkThreadPage.NavigateUrl, threadID)
                txtBody.BasePath = Me.BaseUrl & "FCKeditor/"
                chkClosed.Visible = isNewThread

                If isEditingPost Then
                    ' load the post to edit, and check that the current user has the
                    ' permission to do so
                    Dim post As Post = BLL.Forums.Post.GetPostByID(postID)
                    If Not isModerator AndAlso _
                        Not (Me.User.Identity.IsAuthenticated And _
                        Me.User.Identity.Name.Equals(post.AddedBy.ToLower)) Then
                        Me.RequestLogin()
                    End If

                    lblEditPost.Visible = True
                    btnSubmit.Text = "Update"
                    txtTitle.Text = post.Title
                    txtBody.Value = post.Body
                    panTitle.Visible = isModerator
                ElseIf isNewReply Then
                    ' chech whether the thread the user is adding a reply to is still open
                    Dim post As Post = BLL.Forums.Post.GetPostByID(threadID)
                    If post.Closed Then
                        Throw New ApplicationException( _
                            "The thread you tried to reply to has been closed.")
                    End If

                    lblNewReply.Visible = True
                    txtTitle.Text = "Re: " & post.Title
                    lblNewReply.Text = String.Format(lblNewReply.Text, post.Title)
                    ' if the ID of a post to be quoted is passed on the querystring, load
                    ' that post and prefill the new reply's body with that post's body
                    If quotePostID > 0 Then
                        Dim quotePost As Post = BLL.Forums.Post.GetPostByID(quotePostID)
                        txtBody.Value = String.Format( _
                            "<blockquote><hr noshade="""" size=""1"" />" & _
                            "<b>Originally posted by {0}</b><br /><br />{1}" & _
                            "<hr noshade="""" size=""1"" /></blockquote>", _
                            quotePost.AddedBy, quotePost.Body)
                    End If
                ElseIf isNewThread Then
                    lblNewThread.Visible = True
                    lnkThreadList.Visible = True
                    lnkThreadPage.Visible = False
                End If
            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If isEditingPost Then
                ' when editing a post, a line containing the current Date/Time and the
                ' name of the user making the edit is added to the post's body so that
                ' the operation gets logged
                Dim body As String = txtBody.Value
                body &= String.Format("<p>-- {0}: post edited by {1}.</p>", _
                    DateTime.Now.ToString, Me.User.Identity.Name)
                ' edit an existing post
                Post.UpdatePost(postID, txtTitle.Text, body)
                panInput.Visible = False
                panFeedback.Visible = True
            Else
                ' insert the new post
                Post.InsertPost(forumID, threadID, _
                    txtTitle.Text, txtBody.Value, chkClosed.Checked)
                panInput.Visible = False
                ' increment the user's post counter
                Me.Profile.Forum.Posts += 1
                ' show the confirmation message saying that approval is
                ' required, according to the target forum's moderated property
                Dim forum As Forum = BLL.Forums.Forum.GetForumByID(forumID)
                If forum.Moderated Then
                    If Not Me.User.IsInRole("Administrators") AndAlso _
                        Not Me.User.IsInRole("Editors") AndAlso _
                        Not Me.User.IsInRole("Moderators") Then
                        panApprovalRequired.Visible = True
                    Else
                        panFeedback.Visible = True
                    End If
                Else
                    panFeedback.Visible = True
                End If
            End If
        End Sub
    End Class
End Namespace
