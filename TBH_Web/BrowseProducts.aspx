<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="BrowseProducts.aspx.vb" Inherits="MB.TheBeerHouse.UI.BrowseProducts" title="The Beer House Products" %>
<%@ MasterType VirtualPath="~/Template.master" %>
<%@ Register Src="./Controls/ProductListing.ascx" TagName="ProductListing" TagPrefix="mb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Product Catalog</div>
   <p></p>
   <mb:ProductListing id="ProductListing1" runat="server" />
</asp:Content>

