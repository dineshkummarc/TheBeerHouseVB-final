<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.ArchivedNewsletters, MB.TheBeerHouse" title="The Beer House - Archived Newsletters" validaterequest="false" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Archived Newsletters</div>
   <p>Here is the list of archived newsletters sent in the past. Click on the newsletter's subject to read the whole content.
   If you want to receive future newsletters right into your e-mail box, 
   <a href="Register.aspx">register now</a> to the site, if you haven't done so yet.<asp:GridView
       ID="gvwNewsletters" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
       DataSourceID="objNewsletters" ShowHeader="False" Width="100%">
       <Columns>
           <asp:TemplateField>
               <ItemTemplate>
                   <img src="Images/ArrowR2.gif" style="vertical-align: middle; border-width: 0px" />
                   <asp:HyperLink ID="lnkNewsletter" runat="server" NavigateUrl='<%# "ShowNewsletter.aspx?ID=" & Eval("ID") %>'
                       Text='<%# Eval("Subject") %>'></asp:HyperLink><small>(sent on <%# Eval("AddedDate", "{0:d}") %>
                           )</small>
               </ItemTemplate>
           </asp:TemplateField>
           <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" DeleteText="Delete newsletter"
               ShowDeleteButton="True">
               <ItemStyle HorizontalAlign="Center" Width="20px" />
           </asp:CommandField>
       </Columns>
   </asp:GridView>
       <asp:ObjectDataSource
       ID="objNewsletters" runat="server" DeleteMethod="DeleteNewsletter" SelectMethod="GetNewsletters"
       TypeName="MB.TheBeerHouse.BLL.Newsletters.Newsletter">
       <DeleteParameters>
           <asp:Parameter Name="id" Type="Int32" />
       </DeleteParameters>
       <SelectParameters>
           <asp:Parameter Name="toDate" Type="DateTime" />
       </SelectParameters>
   </asp:ObjectDataSource>
   </p>
</asp:Content>

