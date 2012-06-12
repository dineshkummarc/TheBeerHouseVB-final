<%@ control language="VB" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Controls.FileUploader, MB.TheBeerHouse" %>
Upload a file:
<asp:FileUpload ID="filUpload" runat="server" />&nbsp;
<asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="false" /><br />
<asp:Label ID="lblFeedbackOK" SkinID="FeedbackOK" runat="server"></asp:Label>
<asp:Label ID="lblFeedbackKO" SkinID="FeedbackKO" runat="server"></asp:Label>
