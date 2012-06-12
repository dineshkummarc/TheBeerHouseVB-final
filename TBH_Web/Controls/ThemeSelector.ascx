<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ThemeSelector.ascx.vb" Inherits="MB.TheBeerHouse.UI.Controls.ThemeSelector" %>
<b><asp:Localize runat="server" ID="locTheme" meta:resourcekey="locThemeResource1" Text="Theme: "></asp:Localize></b>
<asp:DropDownList runat="server" ID="ddlThemes" AutoPostBack="True" meta:resourcekey="ddlThemesResource1" />