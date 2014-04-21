<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Home.DashboardViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>My Todo List</h4>
    
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Task
            </th>
            <th style="text-align: center;">
                Due Date
            </th>
        </tr>

    <% foreach (var item in Model.MyTodoList) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Tasks", new { id = item.Id.Value }, null) %>
            </td>
            <td>
            <% if (item.DueDate.HasValue)
               { %>
                <%: String.Format("{0:g}", DateTime.SpecifyKind(item.DueDate.Value, DateTimeKind.Utc).ToLocalTime()) %>
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>
