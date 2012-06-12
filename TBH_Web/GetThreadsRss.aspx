<%@ Page Language="VB" AutoEventWireup="false" ContentType="text/xml" MaintainScrollPositionOnPostback="false" EnableTheming="false" CodeFile="GetThreadsRss.aspx.vb" Inherits="MB.TheBeerHouse.UI.GetThreadsRss" %>
<head id="Head1" runat="server" visible="false"></head>

<asp:Repeater id="rptRss" runat="server">
   <HeaderTemplate>
      <rss version="2.0">
         <channel>
            <title><![CDATA[The Beer House: <%# RssTitle %>]]></title>
            <link><%# FullBaseUrl %></link>
            <description>The Beer House: the site for beer fanatics</description>
            <copyright>Copyright 2005 by Marco Bellinaso</copyright>
   </HeaderTemplate>
   <ItemTemplate>
      <item>
         <title><![CDATA[<%# Eval("Title") %>]]></title>
         <author><![CDATA[<%# Eval("AddedBy") %>]]></author>
         <description></description>
         <link><![CDATA[<%# FullBaseUrl & "ShowThread.aspx?ID=" & Eval("ID") %>]]></link>
         <pubDate><%# string.Format("{0:R}", Eval("AddedDate")) %></pubDate>
      </item>
   </ItemTemplate>
   <FooterTemplate>
         </channel>
      </rss>  
   </FooterTemplate>
</asp:Repeater>
