Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManageUsers
        Inherits BasePage

        Private allUsers As MembershipUserCollection = Membership.GetAllUsers

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                lblTotUsers.Text = allUsers.Count.ToString
                lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline.ToString
                Dim alphabet As String() = _
                    "A;B;C;D;E;F;G;H;I;J;K;L;M;N;O;P;Q;R;S;T;U;V;W;X;Y;Z;All".Split(";")
                rptAlphabet.DataSource = alphabet
                rptAlphabet.DataBind()
            End If
        End Sub

        Private Sub BindUsers(ByVal reloadAllUsers As Boolean)
            If reloadAllUsers Then
                allUsers = Membership.GetAllUsers
            End If

            Dim users As MembershipUserCollection = Nothing

            Dim searchText As String = ""
            If Not String.IsNullOrEmpty(gvwUsers.Attributes("SearchText")) Then
                searchText = gvwUsers.Attributes("SearchText")
            End If

            Dim searchByEmail As Boolean = False
            If Not String.IsNullOrEmpty(gvwUsers.Attributes("SearchByEmail")) Then
                searchByEmail = Boolean.Parse(gvwUsers.Attributes("SearchByEmail"))
            End If

            If searchText.Length > 0 Then
                If searchByEmail Then
                    users = Membership.FindUsersByEmail(searchText)
                Else
                    users = Membership.FindUsersByName(searchText)
                End If
            Else
                users = allUsers
            End If

            gvwUsers.DataSource = users
            gvwUsers.DataBind()
        End Sub

        Protected Sub rptAlphabet_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptAlphabet.ItemCommand
            gvwUsers.Attributes.Add("SearchByEmail", Boolean.FalseString)

            If e.CommandArgument.ToString.Length = 1 Then
                gvwUsers.Attributes.Add("SearchText", e.CommandArgument.ToString + "%")
                BindUsers(False)
            Else
                gvwUsers.Attributes.Add("SearchText", "")
                BindUsers(False)
            End If
        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Dim searchByEmail As Boolean = (ddlSearchTypes.SelectedValue = "E-mail")
            gvwUsers.Attributes.Add("SearchText", "%" + txtSearchText.Text + "%")
            gvwUsers.Attributes.Add("SearchByEmail", searchByEmail.ToString)
            BindUsers(False)
        End Sub

        Protected Sub gvwUsers_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwUsers.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(6).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this user account?') == false) return false;"
            End If
        End Sub

        Protected Sub gvwUsers_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwUsers.RowDeleting
            Dim userName As String = gvwUsers.DataKeys(e.RowIndex).Value.ToString
            ProfileManager.DeleteProfile(userName)
            Membership.DeleteUser(userName)
            BindUsers(True)
            lblTotUsers.Text = allUsers.Count.ToString
        End Sub
    End Class
End Namespace
