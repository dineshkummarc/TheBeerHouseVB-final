<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.ShowNewsletter, MB.TheBeerHouse" title="The Beer House - Show Newsletter" validaterequest="false" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
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

