<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Home.DashboardViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    OpenLawOffice
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>
        My Todo List</h4>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Matter
            </th>
            <th style="text-align: center;">
                Task
            </th>
            <th style="text-align: center;">
                Due Date
            </th>
        </tr>
        <% foreach (var item in Model.MyTodoList)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Item1.Title, "Details", "Matters", new { id = item.Item1.Id.Value }, null)%>
            </td>
            <td>
                <%: Html.ActionLink(item.Item2.Title, "Details", "Tasks", new { id = item.Item2.Id.Value }, null)%>
            </td>
            <td>
                <% if (item.Item2.DueDate.HasValue)
                   { %>
                <%: String.Format("{0:g}", item.Item2.DueDate.Value)%>
                <% } %>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>