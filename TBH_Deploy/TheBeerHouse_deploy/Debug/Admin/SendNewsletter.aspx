<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Admin.SendNewsletter, MB.TheBeerHouse" title="The Beer House - Send Newsletter" validaterequest="false" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="sectiontitle">
        <asp:Literal runat="server" ID="lblSendNewsletter" Text="Create & Send Newsletter" />
    </div>
    <p>
    </p>
    <asp:Panel runat="server" ID="panSend">
        Fill the fields below with the newsletter's subject, the body in plain-text and
        HTML format. Only the plain-text body is compulsory. If you don't specify the HTML
        version, the plain-text body will be used for HTML subscriptions as well.
        <p>
        </p>
        <small><b>
            <asp:Literal runat="server" ID="lblTitle" Text="Subject:" /></b></small><br />
        <asp:TextBox ID="txtSubject" runat="server" MaxLength="256" Width="99%"></asp:TextBox>
        <asp:RequiredFieldValidator ID="valRequireSubject" runat="server" ControlToValidate="txtSubject"
            Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
            ValidationGroup="Newsletter">The Subject field is required.</asp:RequiredFieldValidator>
        <p>
        </p>
        <small><b>
            <asp:Literal runat="server" ID="lblPlainTextBody" Text="Plain-text Body:" /></b></small><br />
        <asp:TextBox ID="txtPlainTextBody" runat="server" Rows="14" TextMode="MultiLine"
            Width="99%"></asp:TextBox>
        <asp:RequiredFieldValidator ID="valRequirePlainTextBody" runat="server" ControlToValidate="txtPlainTextBody"
            Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
            ValidationGroup="Newsletter">The plain-text body is required.</asp:RequiredFieldValidator>
        <p>
        </p>
        <small><b>
            <asp:Literal runat="server" ID="lblHtmlBody" Text="HTML Body:" /></b></small><br />
        <FCKeditorV2:FCKeditor ID="txtHtmlBody" runat="server" Height="400px" ToolbarSet="TheBeerHouse"
            Width="99%">
        </FCKeditorV2:FCKeditor>
        <p>
        </p>
        <asp:Button ID="btnSend" runat="server" Text="Send" OnClientClick="if (confirm('Are you sure you want to send the newsletter?') == false) return false;"
            ValidationGroup="Newsletter" />
    </asp:Panel>
    <asp:Panel ID="panWait" runat="server" Visible="false">
        <asp:Label runat="server" ID="lblWait" SkinID="FeedbackKO">
      <p>Another newsletter is currently being sent. Please wait until it completes
      before compiling and sending a new one.</p>
      <p>You can check the current newsletter's completion status from <a href="SendingNewsletter.aspx">this page</a>.</p>
        </asp:Label>
    </asp:Panel>
</asp:Content>
