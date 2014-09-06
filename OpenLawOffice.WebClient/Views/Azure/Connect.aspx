<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Connect to Office365
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Connect to Office365</h2>
    
    To connect OpenLawOffice to your Office 365 account, you need to setup Microsoft Azure to allow
    access.  To do this, follow these directions:
    
    <ol>
        <li>Log on to your Azure account</li>
        <li>In the left navigation, choose Active Directory</li>
        <li>Choose your directory</li>
        <li>In the top navigation, choose Applications</li>
        <li>On the Active Directory tab, choose Applications</li>
        <li>Add a new application in your Office 365 domain (created at Office 365 sign up) by choosing the "ADD" icon at the bottom of the portal screen.  This will bring up a dialog box to tell Azure about your application</li>
        <li>Choose Add an application my organization is developing</li>
        <li>For the name of the application, enter OpenLawOffice.  The the type, leave Web application and/or Web API. Then choose the arrow to move to step 2.</li>
        <li>For the Sign-On URL, enter the URL for your application (if developing on localhost then your entry may look similar to http://localhost:62914/). </li>
        <li>Enter an App ID URL (something like http://localhost:62914/olo) </li>
        <li>Choose the checkmark to finish adding the application.</li>
    </ol>
    
    You now need to give OpenLawOffice some information so that it may communicate with Office 365.  All
    of this information is found within the OpenLawOffice app you just created within Azure.  From your 
    Active Directory Application listing select OpenLawOffice.  Click the Configure tab at the top.  Scroll
    down to "keys".  Select a duration to create a new key (it will create on saving).  Scroll down to "Reply Url" 
    and enter your website followed by the path "Azure".  For example, if you are developing on localhost you may 
    use something similar to "http://localhost:62914/Azure".  Then scroll down
    to "permissions to other applications", setup Office 365 Exchange Online for everything.  Save.  Make
    sure to copy the key value as you will never see it again.
    
    <br /><br />
    To complete this, you will need to open OpenLawOffice's Web.config file for editing.
    <br /><br />
    
    In openLawOffice.system you will find "office365AuthEndpoint", "office365TokenEndpoint", 
    "office365ClientId", "office365ClientKey".  Populate these with the respective values from Azure.
    
    <br /><br />
    Auth Endpoint: At the bottom of the page, click View Endpoints to find the OAUTH 2.0 AUTHORIZATION ENDPOINT information.  

    <br /><br />
    Token Endpoint: At the bottom of the page, click View Endpoints to find the OAUTH 2.0 TOKEN ENDPOINT information.<br />
    Special Note: if the endpoint ends in '?api-version=1.0' then do not include that in your web.config entry.  So, your web.config entry should end in '/token'.
    
    <br /><br />
    ClientId: Your Client ID is displayed under the logo in the center of the page.

    <br /><br />
    ClientKey: The key you created above that you should have copied down.
    
    <br /><br />
    Depending on your web server you may need to restart your website for the Web.config to be reloaded.
    
    <br /><br />
    <% if (ViewData["NotSetup"] != null)
       { %>
        Once you have supplied values and the Web.config is properly loaded, you 
        will see a "Connect" link here.
    <% }
       else
       { %>

       It appears your Office 365 connection has values.  We have no way to verify if these are right or not
       but, we can give it a shot!  Click connect below to try.

       <br /><br />

       <a href="<%: ViewData["Url"] %>">Connect</a>

    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
