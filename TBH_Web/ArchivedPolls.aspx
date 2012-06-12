<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="ArchivedPolls.aspx.vb" Inherits="MB.TheBeerHouse.UI.ArchivedPolls" title="The Beer House - Archived Polls" %>

<%@ Register Src="Controls/PollBox.ascx" TagName="PollBox" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <script type="text/javascript">
      function toggleDivState(divName)
      {
         var ctl = window.document.getElementById(divName);
         if (ctl.style.display == "none")
            ctl.style.display = "";
         else
            ctl.style.display = "none";
      }
   </script>
   <div class="sectiontitle">Archived Polls</div>
   <p>Here is the complete list of archived polls run in the past. Click on the poll's question to see its results.</p>
    <asp:GridView ID="gvwPolls" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="objPolls" ShowHeader="False" Width="100%">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <img src="Images/ArrowR2.gif" />
                    <a href="javascript:toggleDivState('poll<%# Eval("ID") %>');">
                    <%# Eval("QuestionText") %></a>
                    <small>(archived on <%# Eval("ArchivedDate", "{0:d}") %>)</small>
                    
                    <div style="Display: none;" id="poll<%# Eval("ID") %>">
               <mb:PollBox id="PollBox1" runat="server" PollID='<%# Eval("ID") %>'
                  ShowHeader="False" ShowQuestion="False" ShowArchiveLink="False" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" DeleteText="Delete poll"
                ShowDeleteButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
        </Columns>
        <EmptyDataTemplate>
            <b>No polls to show</b>
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:ObjectDataSource ID="objPolls" runat="server" DeleteMethod="DeletePoll"
           SelectMethod="GetPolls" TypeName="MB.TheBeerHouse.BLL.Polls.Poll">
           <DeleteParameters>
               <asp:Parameter Name="id" Type="Int32" />
           </DeleteParameters>
           <SelectParameters>
               <asp:Parameter DefaultValue="false" Name="includeActive" Type="Boolean" />
               <asp:Parameter DefaultValue="true" Name="includeArchived" Type="Boolean" />
           </SelectParameters>
       </asp:ObjectDataSource>
   

</asp:Content>

