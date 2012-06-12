<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.BrowseProducts, MB.TheBeerHouse" title="The Beer House Products" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<%@ MasterType VirtualPath="~/Template.master" %>
<%@ Register Src="./Controls/ProductListing.ascx" TagName="ProductListing" TagPrefix="mb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Product Catalog</div>
   <p></p>
   <mb:ProductListing id="ProductListing1" runat="server" />
</asp:Content>

