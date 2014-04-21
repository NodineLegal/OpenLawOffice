<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Settings.UserTaskSettingsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Task Settings</h2>

    <h3>My Tasks Filters</h3>

    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Category
            </th>
            <th style="text-align: center;">
                Tag
            </th>
            <th style="width: 150px;"></th>
        </tr>

    <% foreach (var item in Model.MyTasksFilter) { %>
    
        <tr>
            <td>
                <%: item.Category %>
            </td>
            <td>
                <%: item.Tag %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "EditFilter", "UserTaskSettings", new { id = item.Id.Value }, null)%> |
                <%: Html.ActionLink("Details", "DetailsFilter", "UserTaskSettings", new { id = item.Id.Value }, null)%> |
                <%: Html.ActionLink("Delete", "DeleteFilter", "UserTaskSettings", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li><%: Html.ActionLink("New Filter", "CreateFilter")%></li>
</asp:Content>
