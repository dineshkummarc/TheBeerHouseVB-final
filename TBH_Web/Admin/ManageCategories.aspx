<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="ManageCategories.aspx.vb" Inherits="MB.TheBeerHouse.UI.Admin.ManageCategories" title="The Beer House - Manage Categories" %>

<%@ Register Src="../Controls/FileUploader.ascx" TagName="FileUploader" TagPrefix="mb" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Manage Article Categories</div>
   <p></p>
    <asp:GridView ID="gvwCategories" runat="server" AutoGenerateColumns="False" DataSourceID="objAllCategories" Width="100%" ShowHeader="False" DataKeyNames="ID">
        <Columns>
            <asp:ImageField DataImageUrlField="ImageUrl">
            <ItemStyle Width="100px" />
            </asp:ImageField>
            <asp:TemplateField>
            <ItemTemplate>
            <div class="setionsubtitle">
            <asp:Literal runat="server" ID="lblCatTitle" Text='<%# Eval("Title") %>' />
            </div><br />
                <asp:Literal ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Literal>
            </ItemTemplate></asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="ManageArticles.aspx?ID={0}"
                Text="&lt;img border='0' src='../Images/ArrowR.gif' alt='View articles' /&gt;" >
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:HyperLinkField>
            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif" SelectText="Update category"
                ShowSelectButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" DeleteText="Delete category"
                ShowDeleteButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="objAllCategories" runat="server" DeleteMethod="DeleteCategory" SelectMethod="GetCategories" TypeName="MB.TheBeerHouse.BLL.Articles.Category"></asp:ObjectDataSource>
    <asp:DetailsView ID="dvwCategory" runat="server" Height="50px" Width="50px" AutoGenerateEditButton="True" AutoGenerateInsertButton="True" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="objCurrCategory" DefaultMode="Insert" HeaderText="Category Details">
        <Fields>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                SortExpression="ID" />
            <asp:BoundField DataField="AddedDate" HeaderText="AddedDate" InsertVisible="False"
                ReadOnly="True" SortExpression="AddedDate" />
            <asp:BoundField DataField="AddedBy" HeaderText="AddedBy" InsertVisible="False" ReadOnly="True"
                SortExpression="AddedBy" />
            <asp:TemplateField HeaderText="Title" SortExpression="Title">
                <EditItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind("Title") %>' MaxLength="256" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireTitle" runat="server" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
                        ToolTip="The Title field is required.">The Title field is required.</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importance" SortExpression="Importance">
                <EditItemTemplate>
                    <asp:TextBox ID="txtImportance" runat="server" Text='<%# Bind("Importance") %>' MaxLength="256" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireImportance" runat="server" ControlToValidate="txtImportance"
                        Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
                        ToolTip="The Importance field is required.">The Importance field is required.</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="valImportanceType" runat="server" ControlToValidate="txtImportance"
                        Display="Dynamic" ErrorMessage="CompareValidator" Operator="DataTypeCheck" ToolTip="The Importance must be an integer."
                        Type="Integer">The Importance must be an integer.</asp:CompareValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblImportance" runat="server" Text='<%# Eval("Importance") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="Image" SortExpression="ImageUrl">
                <EditItemTemplate>
                    <asp:TextBox ID="txtImageUrl" runat="server" Text='<%# Bind("ImageUrl") %>' MaxLength="256" Width="100%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    &nbsp;<asp:Image ID="Image1" runat="server" AlternateText='<%# Eval("Title") %>'
                        ImageUrl='<%# Eval("ImageUrl") %>' Visible='<%# Not string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "ImageUrl").ToString()) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                <EditItemTemplate>
                    <asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("Description") %>' MaxLength="4000" Rows="5" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' Width="100%"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Fields>
        <FieldHeaderStyle Width="100px" />
    </asp:DetailsView>
    <asp:ObjectDataSource ID="objCurrCategory" runat="server" InsertMethod="InsertCategory"
        SelectMethod="GetCategoryByID" TypeName="MB.TheBeerHouse.BLL.Articles.Category"
        UpdateMethod="UpdateCategory">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvwCategories" Name="categoryID" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <mb:FileUploader ID="FileUploader1" runat="server" />
</asp:Content>

