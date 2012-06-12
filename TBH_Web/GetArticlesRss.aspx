<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GetArticlesRss.aspx.vb"
    Inherits="MB.TheBeerHouse.UI.GetArticlesRss" Title="Untitled Page" ContentType="text/xml"
    EnableTheming="false" MaintainScrollPositionOnPostback="false" %>

<head runat="server" visible="false">
</head>
<asp:repeater id="rptRss" runat="server">
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
         <description><![CDATA[<%# Eval("Abstract") %>]]></description>
         <link><![CDATA[<%# FullBaseUrl & "ShowArticle.aspx?ID=" & Eval("ID") %>]]></link>
         <pubDate><%# string.Format("{0:R}", Eval("ReleaseDate")) %></pubDate>
      </item>
   </ItemTemplate>
   <FooterTemplate>
         </channel>
      </rss>  
   </FooterTemplate>
</asp:repeater>
