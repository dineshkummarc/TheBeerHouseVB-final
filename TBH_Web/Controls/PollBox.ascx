<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PollBox.ascx.vb" Inherits="MB.TheBeerHouse.UI.Controls.PollBox" %>
<div class="pollbox">
    <asp:Panel ID="panHeader" runat="server" meta:resourcekey="panHeaderResource1">
        <div class="sectiontitle">
            <asp:Image ID="imgArrow" runat="server" ImageUrl="~/Images/ArrowR.gif" Style="float: left;
                margin-left: 3px; margin-right: 3px;" meta:resourcekey="imgArrowResource1" />
            <asp:Label ID="lblHeader" runat="server" meta:resourcekey="lblHeaderResource1"></asp:Label></div>
    </asp:Panel>
    <div class="pollcontent">
        <asp:Label ID="lblQuestion" runat="server" CssClass="pollquestion" meta:resourcekey="lblQuestionResource1"></asp:Label>
        <asp:Panel ID="panVote" runat="server" meta:resourcekey="panVoteResource1">
            <div class="polloptions">
                <asp:RadioButtonList ID="optlOptions" runat="server" DataTextField="OptionText" DataValueField="ID"
                    meta:resourcekey="optlOptionsResource1">
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="valRequireOption" runat="server" ControlToValidate="optlOptions"
                    Display="Dynamic" ErrorMessage="RequiredFieldValidator" SetFocusOnError="True"
                    ToolTip="You must select an option." ValidationGroup="PollVote" meta:resourcekey="valRequireOptionResource1">You must select an option.</asp:RequiredFieldValidator></div>
            <asp:Button ID="btnVote" runat="server" Text="Vote" ValidationGroup="PollVote" meta:resourcekey="btnVoteResource1" /></asp:Panel>
        <asp:Panel ID="panResults" runat="server" meta:resourcekey="panResultsResource1">
            <div class="polloptions">
                <asp:Repeater ID="rptOptions" runat="server">
                    <ItemTemplate>
                        <%# Eval("OptionText") %>
                        <small>(<%# Eval("Votes") %>
                            vote(s) -
                            <%# Eval("Percentage", "{0:N1}") %>
                            %)</small>
                        <br />
                        <div class="pollbar" style="width: <%# GetFixedPercentage(Eval("Percentage")) %>%">
                            &nbsp;</div>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <asp:Image runat="server" ID="imgSeparator" ImageUrl="~/Images/spacer.gif" Height="5px"
                            meta:resourcekey="imgSeparatorResource1" /><br />
                    </SeparatorTemplate>
                </asp:Repeater>
                <asp:Image runat="server" ID="imgSeparator" ImageUrl="~/Images/spacer.gif" Height="10px"
                    meta:resourcekey="imgSeparatorResource2" /><br />
                <b>
                    <asp:Localize runat="server" ID="locTotVotes" meta:resourcekey="locTotVotesResource1"
                        Text="Total votes:"></asp:Localize>
                    <asp:Label runat="server" ID="lblTotalVotes" meta:resourcekey="lblTotalVotesResource1" /></b>
            </div>
        </asp:Panel>
        <asp:Image runat="server" ID="Image1" ImageUrl="~/Images/spacer.gif" Height="10px"
            meta:resourcekey="Image1Resource1" /><br />
        <asp:HyperLink runat="server" ID="lnkArchive" NavigateUrl="~/ArchivedPolls.aspx"
            Text="Archived Polls" meta:resourcekey="lnkArchiveResource1" />
    </div>
</div>
