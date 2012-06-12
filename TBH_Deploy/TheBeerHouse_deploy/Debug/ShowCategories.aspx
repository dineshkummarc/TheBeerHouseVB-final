<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.ShowCategories, MB.TheBeerHouse" title="The Beer House - Article Categories" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<%@ MasterType VirtualPath="~/Template.master" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:DataList ID="dlstCategories" runat="server" DataKeyField="ID" DataSourceID="objAllCategories"
        EnableTheming="False" RepeatColumns="2" Width="100%">
        <ItemTemplate>
            <table cellpadding="6" style="width: 100%">
                <tr>
                    <td style="width: 1px">
                        <asp:HyperLink ID="lnkCatImage" runat="server" NavigateUrl='<%# "BrowseArticles.aspx?CatID=" & Eval("ID") %>'>
                            <asp:Image ID="imgCategory" runat="server" BorderWidth="0px" AlternateText='<%# Eval("Title") %>' ImageUrl='<%# Eval("ImageUrl") %>' /></asp:HyperLink></td>
                    <td>
                        <div class="sectionsubtitle">
                            <asp:HyperLink ID="lnkCatRss" runat="server" NavigateUrl='<%# "GetArticlesRss.aspx?CatID=" & Eval("ID") %>'><img src="Images/rss.gif" alt="Get the Rss for this category" style="border-width: 0px;" /></asp:HyperLink>
                            <asp:HyperLink ID="lnkCatTitle" runat="server" NavigateUrl='<%# "BrowseArticles.aspx?CatID=" & Eval("ID") %>'
                                Text='<%# Eval("Title") %>'></asp:HyperLink></div><br />
                        <asp:Literal ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Literal>
                    </td>
                </tr>
            </table>
            <br />
        </ItemTemplate>
    </asp:DataList>
    <asp:ObjectDataSource ID="objAllCategories" runat="server" SelectMethod="GetCategories"
        TypeName="MB.TheBeerHouse.BLL.Articles.Category"></asp:ObjectDataSource>
</asp:Content>

