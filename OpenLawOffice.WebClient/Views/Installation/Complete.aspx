<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Complete
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Complete</h2>
    Installation has completed. After login, please make sure to update account profile for the Administrator user.
    To login to the system, click the link below. Your initial account information is as follows:
    <br />
    <br />
    Username: Administrator<br />
    Password: password<br />
    <br />
    <%: Html.ActionLink("Login", "Login", "Account") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>