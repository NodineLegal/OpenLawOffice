<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Not Found
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Not Found</h2>

    <p>
        We are sorry but you have requested a page that does not exist.
    </p>
    <% if (Model != null)
       { %>
    <p>
        Controller: <%: Model.ControllerName%><br />
        Action: <%: Model.ActionName%>
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
