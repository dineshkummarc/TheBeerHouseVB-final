<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.BrowseArticles, MB.TheBeerHouse" title="The Beer House - Browse Articles" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<%@ MasterType VirtualPath="~/Template.master" %>
<%@ Register Src="Controls/ArticleListing.ascx" TagName="ArticleListing" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Article List</div>
   <p></p>
    <mb:ArticleListing ID="ArticleListing1" runat="server" PublishedOnly="true" />
</asp:Content>

