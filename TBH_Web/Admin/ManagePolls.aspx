<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="ManagePolls.aspx.vb" Inherits="MB.TheBeerHouse.UI.Admin.ManagePolls" title="The Beer House - Manage Polls" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Manage Polls</div>
   <p></p>
   <ul style="list-style-type: square">
      <li><asp:HyperLink runat="server" ID="lnkArchive" NavigateUrl="~/ArchivedPolls.aspx">Manage Archived Polls</asp:HyperLink></li></ul>
   <p></p>
   <asp:GridView ID="gvwPolls" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
        DataSourceID="objPolls" Width="100%">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="QuestionText" HeaderText="Poll">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Votes" HeaderText="Votes" ReadOnly="True" SortExpression="Votes">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Is Current">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Image ID="imgIsCurrent" runat="server" ImageUrl="~/Images/OK.gif" Visible='<%# Eval("IsCurrent") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif" SelectText="Edit poll"
                ShowSelectButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
            <asp:ButtonField ButtonType="Image" CommandName="Archive" ImageUrl="~/Images/Folder.gif"
                Text="Button">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:ButtonField>
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" DeleteText="Delete poll"
                ShowDeleteButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
        </Columns>
        <EmptyDataTemplate><b>No Polls to show</b></EmptyDataTemplate>
    </asp:GridView>
   <asp:ObjectDataSource ID="objPolls" runat="server" SelectMethod="GetPolls"
      TypeName="MB.TheBeerHouse.BLL.Polls.Poll" DeleteMethod="DeletePoll">
      <DeleteParameters>
         <asp:Parameter Name="id" Type="Int32" />
      </DeleteParameters>
      <SelectParameters>
         <asp:Parameter DefaultValue="true" Name="includeActive" Type="Boolean" />
         <asp:Parameter DefaultValue="false" Name="includeArchived" Type="Boolean" />
      </SelectParameters>
   </asp:ObjectDataSource>
   <p></p><table width="100%">
        <tr>
            <td style="width: 50%" valign="top">
                <asp:DetailsView ID="dvwPoll" runat="server" AutoGenerateEditButton="True" AutoGenerateInsertButton="True"
                    AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="objCurrPoll" DefaultMode="Insert"
                    HeaderText="Poll Details" Height="50px" Width="100%">
                    <Fields>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                            SortExpression="ID" />
                        <asp:BoundField DataField="AddedDate" HeaderText="AddedDate" InsertVisible="False"
                            ReadOnly="True" SortExpression="AddedDate" />
                        <asp:BoundField DataField="AddedBy" HeaderText="AddedBy" InsertVisible="False" ReadOnly="True"
                            SortExpression="AddedBy" />
                        <asp:BoundField DataField="Votes" HeaderText="Votes" InsertVisible="False" ReadOnly="True"
                            SortExpression="Votes" />
                        <asp:TemplateField HeaderText="Question" SortExpression="QuestionText">
                            <EditItemTemplate>
                                &nbsp;<asp:TextBox ID="txtQuestion" runat="server" MaxLength="256" Text='<%# Bind("QuestionText") %>'
                                    Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valRequireQuestion" runat="server" ControlToValidate="txtQuestion"
                                    Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
                                    ValidationGroup="Poll">The Question field is required.</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("QuestionText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsCurrent" HeaderText="Is Current" ReadOnly="True"
                            SortExpression="IsCurrent" />
                    </Fields>
                    <FieldHeaderStyle Width="100px" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="objCurrPoll" runat="server" InsertMethod="InsertPoll" SelectMethod="GetPollByID"
                    TypeName="MB.TheBeerHouse.BLL.Polls.Poll" UpdateMethod="UpdatePoll">
                    <UpdateParameters>
                        <asp:Parameter Name="questionText" Type="String" />
                        <asp:Parameter Name="isCurrent" Type="Boolean" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="gvwPolls" Name="pollID" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="questionText" Type="String" />
                        <asp:Parameter Name="isCurrent" Type="Boolean" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </td>
            <td style="width: 50%" valign="top">
                <asp:Panel ID="panOptions" runat="server" Visible="False" Width="100%">
                    <asp:GridView ID="gvwOptions" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                        DataSourceID="objOptions" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="OptionText" HeaderText="Option" SortExpression="OptionText">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Votes" HeaderText="Votes" ReadOnly="True" SortExpression="Votes">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Percentage" DataFormatString="{0:N1}" HeaderText="%" HtmlEncode="False"
                                ReadOnly="True" SortExpression="Percentage">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif" SelectText="Edit option"
                                ShowSelectButton="True">
                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            </asp:CommandField>
                            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" DeleteText="Delete option"
                                ShowDeleteButton="True">
                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            </asp:CommandField>
                        </Columns>
                        <EmptyDataTemplate><b>No options to show for the selected poll</b></EmptyDataTemplate>
                    </asp:GridView>
                            <asp:ObjectDataSource ID="objOptions" runat="server" DeleteMethod="DeleteOption"
               SelectMethod="GetOptions" TypeName="MB.TheBeerHouse.BLL.Polls.Option">
               <DeleteParameters>
                  <asp:Parameter Name="id" Type="Int32" />
               </DeleteParameters>
               <SelectParameters>
                  <asp:ControlParameter ControlID="gvwPolls" Name="pollID" PropertyName="SelectedValue"
                     Type="Int32" />
               </SelectParameters>
            </asp:ObjectDataSource>
            <p></p>
                    <asp:DetailsView ID="dvwOption" runat="server" AutoGenerateEditButton="True" AutoGenerateInsertButton="True"
                        AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="objCurrOption" DefaultMode="Insert"
                        HeaderText="Option Details" Width="100%">
                        <Fields>
                            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                SortExpression="ID" />
                            <asp:BoundField DataField="AddedDate" HeaderText="AddedDate" InsertVisible="False"
                                ReadOnly="True" SortExpression="AddedDate" />
                            <asp:BoundField DataField="AddedBy" HeaderText="AddedBy" InsertVisible="False" ReadOnly="True"
                                SortExpression="AddedBy" />
                            <asp:BoundField DataField="Votes" HeaderText="Votes" InsertVisible="False" ReadOnly="True"
                                SortExpression="Votes" />
                            <asp:BoundField DataField="Percentage" DataFormatString="{0:N1}" HeaderText="Percentage"
                                HtmlEncode="False" InsertVisible="False" ReadOnly="True" SortExpression="Percentage" />
                            <asp:TemplateField HeaderText="Option">
                                <EditItemTemplate>
                                    &nbsp;<asp:TextBox ID="txtOption" runat="server" MaxLength="256" Text='<%# Bind("OptionText") %>'
                                        Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequireOption" runat="server" ControlToValidate="txtOption"
                                        Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
                                        ValidationGroup="Option">The Option field is required.</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOption" runat="server" Text='<%# Eval("OptionText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                        <FieldHeaderStyle Width="100px" />
                    </asp:DetailsView>
                    &nbsp;
            <asp:ObjectDataSource ID="objCurrOption" runat="server" InsertMethod="InsertOption" SelectMethod="GetOptionByID" TypeName="MB.TheBeerHouse.BLL.Polls.Option" UpdateMethod="UpdateOption">
               <SelectParameters>
                  <asp:ControlParameter ControlID="gvwOptions" Name="optionID" PropertyName="SelectedValue"
                     Type="Int32" />
               </SelectParameters>
               <UpdateParameters>
                  <asp:Parameter Name="id" Type="Int32" />
                  <asp:Parameter Name="optionText" Type="String" />
               </UpdateParameters>
               <InsertParameters>
                  <asp:ControlParameter ControlID="gvwPolls" Name="pollID" PropertyName="SelectedValue"
                     Type="Int32" />
                  <asp:Parameter Name="optionText" Type="String" />
               </InsertParameters>
            </asp:ObjectDataSource>

                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Content>

