<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.OrderCompleted, MB.TheBeerHouse" title="The Beer House - Order Completed" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <div class="sectiontitle">Order Completed</div>
   <p></p>
   <b>Thank you for your order!</b>
   <p></p>
   The transaction is currently being verified and processed. 
   You can the status of your order at any time from the <a href="../OrderHistory.aspx">Order History</a> page.
</asp:Content>
