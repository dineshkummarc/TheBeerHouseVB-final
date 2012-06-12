<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="ShowNewsletter.aspx.vb" Inherits="MB.TheBeerHouse.UI.ShowNewsletter" title="The Beer House - Show Newsletter" ValidateRequest="false" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Newsletter: <asp:Literal runat="server" ID="lblSubject" /></div>
   <p></p>
   <small><b>Plain-text Body:</b></small>
   <div style="border: dashed 1px black; overflow: auto; width: 98%; height: 300px; padding: 5px;">
      <asp:Literal runat="server" ID="lblPlaintextBody" />
   </div>
   <p></p>
   <small><b>HTML Body:</b></small>
   <div style="border: dashed 1px black; overflow: auto; width: 98%; height: 300px; padding: 5px;">
      <asp:Literal runat="server" ID="lblHtmlBody" />
   </div>
</asp:Content>

