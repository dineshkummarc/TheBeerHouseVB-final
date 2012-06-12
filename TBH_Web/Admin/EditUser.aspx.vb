Imports System.Collections
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class EditUser
        Inherits BasePage

        Dim userName As String = ""

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' retrieve the username from the querystring
            userName = Me.Request.QueryString("UserName")

            lblRolesFeedbackOK.Visible = False
            lblProfileFeedbackOK.Visible = False

            If Not Me.IsPostBack Then
                UserProfile1.Username = userName

                ' show the user's details
                Dim user As MembershipUser = Membership.GetUser(userName)
                lblUserName.Text = user.UserName
                lnkEmail.Text = user.Email
                lnkEmail.NavigateUrl = "mailto:" & user.Email
                lblRegistered.Text = user.CreationDate.ToString("f")
                lblLastLogin.Text = user.LastLoginDate.ToString("f")
                lblLastActivity.Text = user.LastActivityDate.ToString("f")
                chkOnlineNow.Checked = user.IsOnline
                chkApproved.Checked = user.IsApproved
                chkLockedOut.Checked = user.IsLockedOut
                chkLockedOut.Enabled = user.IsLockedOut

                BindRoles()
            End If
        End Sub

        Private Sub BindRoles()
            ' fill the CheckBoxList with all the available roles, and the select
            ' those that the user belongs to
            chklRoles.DataSource = Roles.GetAllRoles
            chklRoles.DataBind()
            For Each role As String In Roles.GetRolesForUser(userName)
                chklRoles.Items.FindByText(role).Selected = True
            Next
        End Sub

        Protected Sub chkApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkApproved.CheckedChanged
            Dim user As MembershipUser = Membership.GetUser(userName)
            user.IsApproved = chkApproved.Checked
            Membership.UpdateUser(user)
        End Sub

        Protected Sub chkLockedOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLockedOut.CheckedChanged
            If Not chkLockedOut.Checked Then
                Dim user As MembershipUser = Membership.GetUser(userName)
                user.UnlockUser()
                chkLockedOut.Enabled = False
            End If
        End Sub

        Protected Sub btnUpdateRoles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateRoles.Click
            ' first remove the user from all roles...
            Dim currRoles() As String = Roles.GetRolesForUser(userName)
            If currRoles.Length > 0 Then
                Roles.RemoveUserFromRoles(userName, currRoles)
            End If

            ' and then add the user to the selected roles
            Dim newRoles As New List(Of String)
            For Each item As ListItem In chklRoles.Items
                If item.Selected Then
                    newRoles.Add(item.Text)
                End If
            Next
            Dim userNames() As String = {userName}
            Roles.AddUsersToRoles(userNames, newRoles.ToArray)

            lblRolesFeedbackOK.Visible = True
        End Sub

        Protected Sub btnCreateRole_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateRole.Click
            If Not Roles.RoleExists(txtNewRole.Text.Trim) Then
                Roles.CreateRole(txtNewRole.Text.Trim)
                BindRoles()
            End If
        End Sub

        Protected Sub btnUpdateProfile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateProfile.Click
            UserProfile1.SaveProfile()
            lblProfileFeedbackOK.Visible = True
        End Sub
    End Class
End Namespace
