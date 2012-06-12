<%@ control language="VB" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Controls.ArticleListing, MB.TheBeerHouse" %>
<%@ Register Src="../Controls/RatingDisplay.ascx" TagName="RatingDisplay" TagPrefix="mb" %>
<asp:Literal ID="lblCategoryPicker" runat="server" Text="Filter by category:"></asp:Literal>
<asp:DropDownList ID="ddlCategories" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
    DataSourceID="objAllCategories" DataTextField="Title" DataValueField="ID">
    <asp:ListItem Value="0">All categories</asp:ListItem>
</asp:DropDownList>
<asp:ObjectDataSource ID="objAllCategories" runat="server" SelectMethod="GetCategories"
    TypeName="MB.TheBeerHouse.BLL.Articles.Category"></asp:ObjectDataSource>
<asp:Literal runat="server" ID="lblSeparator">&nbsp;&nbsp;&nbsp;</asp:Literal>
<asp:Literal runat="server" ID="lblPageSizePicker"><small><b>Articles per page:</b></small></asp:Literal>
<asp:DropDownList ID="ddlArticlesPerPage" runat="server" AutoPostBack="True">
    <asp:ListItem Value="5">5</asp:ListItem>
    <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
    <asp:ListItem Value="25">25</asp:ListItem>
    <asp:ListItem Value="50">50</asp:ListItem>
    <asp:ListItem Value="100">100</asp:ListItem>
</asp:DropDownList>
<p>
</p>
<asp:GridView ID="gvwArticles" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    DataKeyNames="ID" DataSourceID="objArticles" EmptyDataText="<b>There is no article to show for the selected category</b>"
    ShowHeader="False" SkinID="Articles">
    <Columns>
        <asp:TemplateField HeaderText="Article List (including those not yet published)">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <div class="articlebox">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td>
                                <div class="articletitle">
                                    <asp:HyperLink runat="server" ID="lnkTitle" CssClass="articletitle" Text='<%# Eval("Title") %>'
                                        NavigateUrl='<%# String.Format("~/ShowArticle.aspx?ID={0}", Eval("ID")) %>' />
                                    <asp:Image ID="imgKey" runat="server" AlternateText="Requires login" ImageUrl="~/Images/key.gif"
                                        Visible='<%# Eval("OnlyForMembers") And Not Page.User.Identity.IsAuthenticated %>' />
                                    <asp:Label ID="lblNotApproved" runat="server" SkinID="NotApproved" Text="Not Approved"
                                        Visible='<%# Not CBool(Eval("Approved")) %>'></asp:Label></div>
                            </td>
                            <td style="text-align: right">
                                <asp:Panel ID="panEditArticle" runat="server" Height="50px" Visible="<%# UserCanEdit %>"
                                    Width="125px">
                                    <asp:ImageButton ID="btnApprove" runat="server" AlternateText="Approve article" CommandArgument='<%# Eval("ID") %>'
                                        CommandName="Approve" ImageUrl="~/Images/checkmark.gif" OnClientClick="if (confirm('Are you sure you want to approve this article?') == false) return false;"
                                        Visible='<%# Not CBool(Eval("Approved")) %>' />&nbsp;&nbsp;
                                    <asp:HyperLink ID="lnkEdit" runat="server" ImageUrl="~/Images/Edit.gif" NavigateUrl='<%# String.Format("~/Admin/AddEditArticle.aspx?ID={0}", Eval("ID")) %>'
                                        ToolTip="Edit article">HyperLink</asp:HyperLink>
                                    &nbsp;&nbsp;<asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete article"
                                        CommandName="Delete" ImageUrl="~/Images/Delete.gif" OnClientClick=" if (confirm('Are you sure you want to delete this article?') == false) return false;" /></asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <b>Rating:</b>
                    <asp:Literal ID="lblRating" runat="server" Text='<%# String.Format("{0} user(s) have rated this article ", Eval("Votes")) %>'></asp:Literal>
                    <mb:RatingDisplay ID="RatingDisplay1" runat="server" Value='<%# Eval("AverageRating") %>' />
                    <br />
                    <b>Posted by: </b>
                    <asp:Literal ID="lblAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'></asp:Literal>,
                    on
                    <asp:Literal ID="lblAddedDate" runat="server" Text='<%# Eval("ReleaseDate") %>'></asp:Literal>,
                    in category
                    <asp:Literal ID="lblCategory" runat="server" Text='<%# Eval("CategoryTitle") %>'></asp:Literal><br />
                    <b>Views: </b>
                    <asp:Literal ID="lblViews" runat="server" Text='<%# String.Format("this article has been read {0} times", Eval("ViewCount")) %>'></asp:Literal><asp:Literal
                        ID="lblLocation" runat="server" Text='<%# "<br /><b>Location: <b>" + Eval("Location") %>'
                        Visible='<%# Eval("Location").ToString().Length > 0 %>'></asp:Literal><br />
                    <div class="articleabstract">
                        <b>Abstract: </b>
                        <asp:Literal ID="lblAbstract" runat="server" Text='<%# Eval("Abstract") %>'></asp:Literal></div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <b>No articles to show</b></EmptyDataTemplate>
</asp:GridView>
<asp:ObjectDataSource ID="objArticles" runat="server" DeleteMethod="DeleteArticle"
    EnablePaging="True" SelectCountMethod="GetArticleCount" SelectMethod="GetArticles"
    TypeName="MB.TheBeerHouse.BLL.Articles.Article">
    <DeleteParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter DefaultValue="true" Name="publishedOnly" Type="Boolean" />
        <asp:ControlParameter ControlID="ddlCategories" Name="categoryID" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
