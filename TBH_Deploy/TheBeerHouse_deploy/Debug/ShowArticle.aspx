<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.ShowArticle, MB.TheBeerHouse" title="The Beer House - Article" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>

<%@ Register Src="Controls/RatingDisplay.ascx" TagName="RatingDisplay" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="articlebox">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblTitle" runat="server" CssClass="articletitle"></asp:Label>
                    <asp:Label runat="server" ID="lblNotApproved" Text="Not approved" SkinID="NotApproved" />
                </td>
                <td style="text-align: right">
                    <asp:Panel ID="panEditArticle" runat="server" Height="50px" Width="125px">
                        <asp:ImageButton ID="btnApprove" runat="server" AlternateText="Approve article" CausesValidation="False"
                            ImageUrl="~/Images/checkmark.gif" OnClientClick="if (confirm('Are you sure you want to approve this article?') == false) return false;" />&nbsp;
                        <asp:HyperLink ID="lnkEditArticle" runat="server" ImageUrl="~/Images/Edit.gif" NavigateUrl="~/Admin/AddEditArticle.aspx?ID={0}"
                            ToolTip="Edit article">HyperLink</asp:HyperLink>&nbsp;
                        <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete article" CausesValidation="False"
                            ImageUrl="~/Images/Delete.gif" OnClientClick="if (confirm('Are you sure you want to delete this article?') == false) return false;" /></asp:Panel>
                </td>
            </tr>
        </table>
        <b>Rating: </b>
        <asp:Literal ID="lblRating" runat="server" Text="{0} user(s) have rated this article "></asp:Literal>
        <mb:RatingDisplay ID="ratDisplay" runat="server" />
        <br />
        <b>Posted by: </b>
        <asp:Literal ID="lblAddedBy" runat="server"></asp:Literal>, on
        <asp:Literal ID="lblReleaseDate" runat="server"></asp:Literal>, in category "<asp:Literal
            runat="server" ID="lblCategory" />"
        <br />
        <b>Views: </b>
        <asp:Literal runat="server" ID="lblViews" Text="this article has been read {0} times" />
        <asp:Literal runat="server" ID="lblLocation" Text="<br /><b>Location: </b>{0}" />
        <br />
        <div class="articleabstract">
            <b>Abstract: </b>
            <asp:Literal runat="server" ID="lblAbstract" />
        </div>
    </div>
    <p>
    </p>
    <asp:Literal runat="server" ID="lblBody" />
    <p>
    </p>
    <hr style="width: 100%; height: 1px;" />
    <div class="sectiontitle">
        How would you rate this article?</div>
    <asp:DropDownList ID="ddlRatings" runat="server">
        <asp:ListItem Value="1">1 beer</asp:ListItem>
        <asp:ListItem Value="2">2 beers</asp:ListItem>
        <asp:ListItem Value="3">3 beers</asp:ListItem>
        <asp:ListItem Value="4">4 beers</asp:ListItem>
        <asp:ListItem Selected="True" Value="5">5 beers</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnRate" runat="server" CausesValidation="False" Text="Rate" />
    <asp:Literal ID="lblUserRating" runat="server" Text="You rated this article {0} beer(s). Thank you for your feedback."
        Visible="False"></asp:Literal>
        <p></p>
    <asp:Panel ID="panComments" runat="server">
       <div class="sectiontitle">User Feedback</div>
        <asp:DataList ID="dlstComments" runat="server" DataKeyField="ID" DataSourceID="objComments">
      <ItemTemplate>
         <div class="comment">
         <table style="width: 100%;">
         <tr><td>
         <b>Comment posted by
            <asp:HyperLink ID="lnkAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'
               NavigateUrl='<%# "mailto:" + Eval("AddedByEmail") %>' />
            on <asp:Literal ID="lblAddedDate" runat="server" Text='<%# Eval("AddedDate", "{0:f}") %>' />            
         </b>
         </td>
         <td style="text-align: right;">
            <asp:Panel runat="server" ID="panAdmin" Visible='<%# UserCanEdit %>'>
            <asp:ImageButton runat="server" ID="btnSelect" CommandName="Select"
               CausesValidation="false" AlternateText="Edit comment" ImageUrl="~/Images/Edit.gif"  />
            &nbsp;&nbsp;
            <asp:ImageButton runat="server" ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'
               CausesValidation="false" AlternateText="Delete comment" ImageUrl="~/Images/Delete.gif"
               OnClientClick="if (confirm('Are you sure you want to delete this comment?') == false) return false;" />
            </asp:Panel>
         </td></tr>
         </table>
         <asp:Literal ID="lblBody" runat="server" Text='<%# Eval("EncodedBody") %>' />         
         </div>
      </ItemTemplate>
        </asp:DataList>
        <asp:ObjectDataSource ID="objComments" runat="server" SelectMethod="GetComments"
            TypeName="MB.TheBeerHouse.BLL.Articles.Comment">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="articleID" QueryStringField="ID"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
           <p></p>
   <div class="sectionsubtitle">Post your comment</div>
        <asp:DetailsView ID="dvwComment" runat="server" AutoGenerateEditButton="True" AutoGenerateInsertButton="True" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="objCurrComment" DefaultMode="Insert">
            <Fields>
                <asp:BoundField DataField="ID" HeaderText="ID:" InsertVisible="False" ReadOnly="True" />
                <asp:BoundField DataField="AddedDate" HeaderText="AddedDate:" InsertVisible="False"
                    ReadOnly="True" />
                <asp:BoundField DataField="AddedByIP" HeaderText="AddedByIP:" InsertVisible="False"
                    ReadOnly="True" />
                <asp:TemplateField HeaderText="Name:">
                    <ItemTemplate>
                        <asp:Label ID="lblAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="txtAddedBy" runat="server" MaxLength="256" Text='<%# Bind("AddedBy") %>'
                            Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireAddedBy" runat="server" ControlToValidate="txtAddedBy"
                            Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">Your name is required.</asp:RequiredFieldValidator>
                    </InsertItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="E-mail:">
                    <InsertItemTemplate>
                        <asp:TextBox ID="txtAddedByEmail" runat="server" MaxLength="256" Text='<%# Bind("AddedByEmail") %>'
                            Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireAddedByEmail" runat="server" ControlToValidate="txtAddedByEmail"
                            Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">Your e-mail is required.</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valEmailPattern" runat="server" ControlToValidate="txtAddedByEmail"
                            Display="Dynamic" ErrorMessage="RegularExpressionValidator" SetFocusOnError="True"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">The e-mail address is not well formed.</asp:RegularExpressionValidator>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkAddedByEmail" runat="server" NavigateUrl='<%# "mailto:" & Eval("AddedByEmail") %>'
                            Text='<%# Eval("AddedByEmail") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comment:">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtBody" runat="server" Rows="5" Text='<%# Bind("Body") %>' TextMode="MultiLine"
                            Width="100%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireBody" runat="server" ControlToValidate="txtBody"
                            Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True">The comment text is required.</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblBody" runat="server" Text='<%# Eval("Body") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
            <FieldHeaderStyle Width="80px" />
        </asp:DetailsView>
           <asp:ObjectDataSource ID="objCurrComment" runat="server" InsertMethod="InsertComment"
      SelectMethod="GetCommentByID" TypeName="MB.TheBeerHouse.BLL.Articles.Comment"
      UpdateMethod="UpdateComment">
      <UpdateParameters>
         <asp:Parameter Name="id" Type="Int32" />
         <asp:Parameter Name="body" Type="String" />
      </UpdateParameters>
      <SelectParameters>
         <asp:ControlParameter ControlID="dlstComments" Name="commentID" PropertyName="SelectedValue"
            Type="Int32" />
      </SelectParameters>
      <InsertParameters>
         <asp:Parameter Name="addedBy" Type="String" />
         <asp:Parameter Name="addedByEmail" Type="String" />
         <asp:QueryStringParameter Name="articleID" QueryStringField="ID" Type="Int32" />
         <asp:Parameter Name="body" Type="String" />
      </InsertParameters>
   </asp:ObjectDataSource>

    </asp:Panel>
</asp:Content>
