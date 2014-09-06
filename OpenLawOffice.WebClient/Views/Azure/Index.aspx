<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Office365 Connector
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Office365 Connector</h2>

    <% if (ViewData["Error"] != null)
       { %>

       An error occurred while attempting to connect OpenLawOffice to Office365, please see below:
       <br /><br />

       Error: <%: Request["error"]%><br />
       Description: <%: Request["error_description"]%>

    <% }
       else
       { %>

       You are connected!  Please feel free to use Office365 connected features.

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
