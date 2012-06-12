<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="BrowseArticles.aspx.vb" Inherits="MB.TheBeerHouse.UI.BrowseArticles" title="The Beer House - Browse Articles" %>
<%@ MasterType VirtualPath="~/Template.master" %>
<%@ Register Src="Controls/ArticleListing.ascx" TagName="ArticleListing" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Article List</div>
   <p></p>
    <mb:ArticleListing ID="ArticleListing1" runat="server" PublishedOnly="true" />
</asp:Content>

