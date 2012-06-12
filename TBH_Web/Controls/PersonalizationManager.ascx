<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PersonalizationManager.ascx.vb" Inherits="MB.TheBeerHouse.UI.Controls.PersonalizationManager" %>
<div style="text-align: right;">
   <asp:WebPartManager ID="WebPartManager1" runat="server" />
   <asp:LinkButton ID="btnBrowseView" runat="server" meta:resourcekey="btnBrowseViewResource1">Browse View</asp:LinkButton>&nbsp;|&nbsp;
   <asp:LinkButton ID="btnDesignView" runat="server" meta:resourcekey="btnDesignViewResource1">Design View</asp:LinkButton>&nbsp;|&nbsp;
   <asp:LinkButton ID="btnEditView" runat="server" meta:resourcekey="btnEditViewResource1">Edit View</asp:LinkButton>&nbsp;|&nbsp;
   <asp:LinkButton ID="btnCatalogView" runat="server" meta:resourcekey="btnCatalogViewResource1">Catalog View</asp:LinkButton>
   <asp:Label runat="server" ID="panPersonalizationModeToggle">
      &nbsp;|&nbsp;
      <asp:LinkButton ID="btnPersonalizationModeToggle" runat="server" meta:resourcekey="btnPersonalizationModeToggleResource1">Switch Scope (current = {0})</asp:LinkButton>
   </asp:Label>
</div>