<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RssReader.ascx.vb" Inherits="MB.TheBeerHouse.UI.Controls.RssReader" %>
<div class="sectiontitle">
    <asp:Literal ID="lblTitle" runat="server"></asp:Literal>
    <asp:HyperLink ID="lnkRss" runat="server" ToolTip="Get the RSS for this content">
        <asp:Image ID="imgRss" runat="server" AlternateText="Get RSS feed" ImageUrl="~/Images/rss.gif" /></asp:HyperLink></div>
<asp:DataList ID="dlstRss" runat="server" EnableViewState="False">
    <ItemTemplate>
        <small><%# Eval("PubDate", "{0:d}") %></small>
        <br />
        <div class="sectionsubtitle">
            <asp:HyperLink ID="lnkTitle" runat="server" NavigateUrl='<%# Eval("Link") %>' Text='<%# Eval("Title") %>'></asp:HyperLink></div>
            <%# Eval("Description") %>
    </ItemTemplate>
</asp:DataList>
<p style="text-align:right">
<small>
    <asp:HyperLink ID="lnkMore" runat="server" /></small></p>
