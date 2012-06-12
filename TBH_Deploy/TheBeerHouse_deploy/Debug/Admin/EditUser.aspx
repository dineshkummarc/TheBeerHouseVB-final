<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Admin.EditUser, MB.TheBeerHouse" title="The Beer House - Edit User" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>

<%@ Register Src="../Controls/UserProfile.ascx" TagName="UserProfile" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="sectiontitle">
        General user information</div>
        <p></p>
    <table cellpadding="2">
        <tr>
            <td width="130" class="fieldname">UserName:
            </td>
            <td width="300">
                <asp:Literal ID="lblUserName" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td class="fieldname">
                E-Mail:</td>
            <td>
                <asp:HyperLink ID="lnkEmail" runat="server">[lnkEmail]</asp:HyperLink></td>
        </tr>
        <tr>
            <td class="fieldname">
                Registered:</td>
            <td>
                <asp:Literal ID="lblRegistered" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td class="fieldname">
                Last Login:</td>
            <td>
                <asp:Literal ID="lblLastLogin" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td class="fieldname">
                Last Activity</td>
            <td>
                <asp:Literal ID="lblLastActivity" runat="server"></asp:Literal></td>
        </tr>
        <tr>
            <td class="fieldname">
                Online Now:</td>
            <td>
                <asp:CheckBox ID="chkOnlineNow" runat="server" Enabled="False" /></td>
        </tr>
        <tr>
            <td class="fieldname">
                Approved:</td>
            <td>
                <asp:CheckBox ID="chkApproved" runat="server" AutoPostBack="True" /></td>
        </tr>
        <tr>
            <td class="fieldname">
                Locked Out:</td>
            <td>
                <asp:CheckBox ID="chkLockedOut" runat="server" AutoPostBack="True" /></td>
        </tr>
    </table>
    <p></p>
    <div class="sectiontitle">
        Edit user's roles</div>
        <p></p>
    <asp:CheckBoxList ID="chklRoles" runat="server" CellSpacing="4" RepeatColumns="5">
    </asp:CheckBoxList>
    <table cellpadding="2" width="450">
        <tr>
            <td align="right">
                <asp:Label ID="lblRolesFeedbackOK" runat="server" SkinID="FeedbackOK" Text="Roles updated successfully"
                    Visible="False"></asp:Label>
                <asp:Button ID="btnUpdateRoles" runat="server" Text="Update" /></td>
        </tr>
        <tr>
            <td align="right">
                <small>Create new role:</small><asp:TextBox ID="txtNewRole" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireNewRole" runat="server" ControlToValidate="txtNewRole"
                    ErrorMessage="Role name is required." SetFocusOnError="True" ValidationGroup="CreateRole"></asp:RequiredFieldValidator>
                <asp:Button ID="btnCreateRole" runat="server" Text="Create" ValidationGroup="CreateRole" /></td>
        </tr>
    </table>
    <p></p>
    <div class="sectiontitle">
        Edit user's profile
    </div><p></p><mb:UserProfile ID="UserProfile1" runat="server" />
    <table cellpadding="2" width="450">
    <tr><td align="right">
        <asp:Label ID="lblProfileFeedbackOK" runat="server" Text="Profile updated successfully" SkinID="FeedbackOK" Visible="False"></asp:Label><asp:Button ID="btnUpdateProfile"
            runat="server" Text="Update" ValidationGroup="EditProfile" /></td></tr></table>
</asp:Content>

