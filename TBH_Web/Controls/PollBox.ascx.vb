Imports MB.TheBeerHouse
Imports MB.TheBeerHouse.UI
Imports MB.TheBeerHouse.BLL.Polls

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class PollBox
        Inherits BaseWebPart

        Private _pollID As Integer = -1

        Public Sub New()
            Me.Title = "Poll Box"
        End Sub

        <Personalizable(PersonalizationScope.Shared), _
            WebBrowsable(), _
            WebDisplayName("Show Archive Link"), _
            WebDescription("Specifies whether the link to the archive page is displayed")> _
        Public Property ShowArchiveLink() As Boolean
            Get
                Return lnkArchive.Visible
            End Get
            Set(ByVal value As Boolean)
                lnkArchive.Visible = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.Shared), _
            WebBrowsable(), _
            WebDisplayName("Show Header"), _
            WebDescription("Specifies whether the header is displayed")> _
        Public Property ShowHeader() As Boolean
            Get
                Return panHeader.Visible
            End Get
            Set(ByVal value As Boolean)
                panHeader.Visible = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.Shared), _
            WebBrowsable(), _
            WebDisplayName("Header Text"), _
            WebDescription("The text of the header")> _
        Public Property HeaderText() As String
            Get
                Return lblHeader.Text
            End Get
            Set(ByVal value As String)
                lblHeader.Text = value
            End Set
        End Property

        <Personalizable(PersonalizationScope.Shared), _
            WebBrowsable(), _
            WebDisplayName("Poll ID"), _
            WebDescription("The ID of the poll to show")> _
        Public Property PollID() As Integer
            Get
                Return _pollID
            End Get
            Set(ByVal value As Integer)
                _pollID = value
            End Set
        End Property

        Public Property ShowQuestion() As Boolean
            Get
                Return lblQuestion.Visible
            End Get
            Set(ByVal value As Boolean)
                lblQuestion.Visible = value
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState() As Object = CType(savedState, Object())
            MyBase.LoadControlState(ctlState(0))
            Me.PollID = CInt(ctlState(1))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState() As Object
            ReDim ctlState(2)
            ctlState(0) = MyBase.SaveControlState()
            ctlState(1) = Me.PollID
            Return ctlState
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then DoBinding()
        End Sub

        Public Overrides Sub DataBind()
            ' the call to the base DataBind makes a call to OnDataBinding,
            ' which parses and evaluates the control's binding expressions, i.e. the PollID prop
            MyBase.DataBind()
            ' with the PollID set, do the actual binding
            DoBinding()
        End Sub

        Protected Sub DoBinding()
            panResults.Visible = False
            panVote.Visible = False

            Dim pollID As Integer
            If Me.PollID = -1 Then
                pollID = Poll.CurrentPollID
            Else
                pollID = Me.PollID
            End If

            If pollID > -1 Then
                Dim poll As Poll = BLL.Polls.Poll.GetPollByID(pollID)

                lblQuestion.Text = poll.QuestionText
                lblTotalVotes.Text = poll.Votes.ToString
                valRequireOption.ValidationGroup &= poll.ID.ToString
                btnVote.ValidationGroup = valRequireOption.ValidationGroup
                optlOptions.DataSource = poll.Options
                optlOptions.DataBind()
                rptOptions.DataSource = poll.Options
                rptOptions.DataBind()
                If poll.IsArchived Or GetUserVote(pollID) > 0 Then
                    panResults.Visible = True
                Else
                    panVote.Visible = True
                End If
            End If
        End Sub

        Protected Sub btnVote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVote.Click
            Dim pollID As Integer

            If Me.PollID = -1 Then
                pollID = Poll.CurrentPollID
            Else
                pollID = Me.PollID
            End If

            ' check that the user has not already voted for this poll
            Dim userVote As Integer = GetUserVote(pollID)
            If userVote = 0 Then
                ' post the vote and tehn create a cookie to remember this user's vote
                userVote = Convert.ToInt32(optlOptions.SelectedValue)
                Poll.VoteOption(pollID, userVote)
                ' hide the panel with the radio buttons, and show the results
                DoBinding()
                panVote.Visible = False
                panResults.Visible = True

                Dim expireDate As DateTime = DateTime.Now.AddDays( _
                    Globals.Settings.Polls.VotingLockInterval)
                Dim key As String = "Vote_Poll" & pollID.ToString

                ' save the result to the cookie
                If Globals.Settings.Polls.VotingLockByCookie Then
                    Dim cookie As New HttpCookie(key, userVote.ToString)
                    cookie.Expires = expireDate
                    Me.Response.Cookies.Add(cookie)
                End If

                ' save the vote also to the cache
                If Globals.Settings.Polls.VotingLockByIP Then
                    Cache.Insert( _
                        Me.Request.UserHostAddress.ToString & "_" & key, _
                        userVote)
                End If
            End If
        End Sub

        Protected Function GetUserVote(ByVal pollID As Integer) As Integer
            Dim key As String = "Vote_Poll" & pollID.ToString
            Dim key2 As String = Me.Request.UserHostAddress.ToString & "_" & key

            ' check if the vote is in the cache
            If Globals.Settings.Polls.VotingLockByIP And Not IsNothing(Cache(key2)) Then
                Return CInt(Cache(key2))
            End If

            ' if the vote is not in cache, check if there's a client-side cookie
            If Globals.Settings.Polls.VotingLockByCookie Then
                Dim cookie As HttpCookie = Me.Request.Cookies(key)
                If Not IsNothing(cookie) Then
                    Return Integer.Parse(cookie.Value)
                End If
            End If

            Return 0
        End Function

        Protected Function GetFixedPercentage(ByVal val As Object) As Integer
            Dim percentage As Integer = Convert.ToInt32(val)
            If percentage = 100 Then
                percentage = 98
            End If
            Return percentage
        End Function
    End Class
End Namespace