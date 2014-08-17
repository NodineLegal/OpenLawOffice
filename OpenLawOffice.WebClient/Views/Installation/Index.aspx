<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Installation.InstallationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Installation
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Installation</h2>
    Welcome to OpenLawOffice!
    <br />
    <br />
    I'm the installation helper. I will help you install OpenLawOffice. During this
    process I will create the necessary database information.
    <br />
    <br />
    I can give you two options, I can install only the database and mandatory data or
    I can do that and setup some basic data for use in the system. If you are going
    to be using me in production, I highly suggest doing only the "Install Only" and
    not "Install with Data".
    <br />
    <br />
    Before starting this, I need you to create a database within an accessable postgres
    database server and then setup my connection string in Web.config.
    <br />
    <br />

    After that, give me a username and password so that I can create a new administrator account
    for you.
    <br />
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true)%>
    Username: <%: Html.TextBoxFor(model => model.Username)%><br />
    Password: <%: Html.PasswordFor(model => model.Password)%><br />
    <input type="submit" value="Install Now" />
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>