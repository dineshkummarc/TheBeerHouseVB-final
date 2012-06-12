<%@ page language="VB" masterpagefile="~/Template.master" autoeventwireup="false" inherits="MB.TheBeerHouse.UI.Admin.SendingNewsletter, MB.TheBeerHouse" title="The Beer House - Sending Newsletter" validaterequest="false" theme="TemplateMonster" maintainScrollPositionOnPostBack="true" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="panProgress" runat="server">
        <div class="sectiontitle">
            <asp:Literal runat="server" ID="lblSendNewsletter" Text="Sending Newsletter..." />
        </div>
        <p>
        </p>
        <div class="progressbarcontainer">
            <div class="progressbar" id="progressbar">
            </div>
        </div>
        <br />
        <br />
        <div id="progressdescription">
        </div>
        <br />
        <br />
        <div style="text-align: center; display: none;" id="panelcomplete">
            <img src="../Images/100ok.gif" width="70px" alt="" /></div>
    </asp:Panel>
    <asp:Panel ID="panNoNewsletter" runat="server" Visible="false">
        <b>No newsletter is currently being sent.</b>
    </asp:Panel>

    <script type="text/javascript">
    function CallUpdateProgress()
    {
        <asp:Literal runat="server" ID="lblScriptName" />;
    }
    
    function UpdateProgress(result, context)
    {
        //  result is a semicolon-separated list of value, so split it
        var params = result.split(";");
        var percentage = params[0];
        var sentMails = params[1];
        var totalMails = params[2];
        
        if (totalMails < 0)
            totalMails = '???';
        
        // update progressbar's width and description text
        var progBar = window.document.getElementById('progressbar');
        progBar.style.width = persectang + '%';
        var descr = window.document.getElementById('progressdescription');
        descr.innerHTML = '<b>' + percentage + '% completed</b> - ' +
            sentMails + ' out of ' + totalMails + ' emails have been sent.';
            
        // if the current percentage is less than 100%,
        // recall the server callback method in 2 secods
        if (percentage == '100')
            window.document.getElementById('panelcomplete').style.display = '';
        else
            setTimeout('CallUpdateProgress()', 2000);
    }
    </script>

</asp:Content>
