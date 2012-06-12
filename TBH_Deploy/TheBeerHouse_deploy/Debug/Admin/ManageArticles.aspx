<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Admin.ManageArticles, MB.TheBeerHouse" title="The Beer House - Manage Articles" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<%@ Register Src="../Controls/ArticleListing.ascx" TagName="ArticleListing" TagPrefix="mb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Manage Articles</div>
   <p></p>
   <ul style="list-style-type: square">
      <li><asp:HyperLink runat="server" ID="lnkManageCategories" NavigateUrl="ManageCategories.aspx">Manage Categories</asp:HyperLink></li>
      <li><asp:HyperLink runat="server" ID="lnkManageComments" NavigateUrl="ManageComments.aspx">Manage Comments</asp:HyperLink></li>
      <li><asp:HyperLink runat="server" ID="lnkAddNewArticle" NavigateUrl="AddEditArticle.aspx">Add New Article</asp:HyperLink></li>
   </ul>
   <p></p>
   <mb:ArticleListing id="ArticleListing1" runat="server" PublishedOnly="False" />

</asp:Content>

