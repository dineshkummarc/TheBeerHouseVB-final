<%@ Page Language="VB" MasterPageFile="~/Template.master" AutoEventWireup="false" CodeFile="OrderCompleted.aspx.vb" Inherits="MB.TheBeerHouse.UI.OrderCompleted" title="The Beer House - Order Completed" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Order Completed</div>
   <p></p>
   <b>Thank you for your order!</b>
   <p></p>
   The transaction is currently being verified and processed. 
   You can the status of your order at any time from the <a href="../OrderHistory.aspx">Order History</a> page.
</asp:Content>
