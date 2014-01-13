<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Security.AreaAclViewModel>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Area
            </th>
            <th style="text-align: center;">
                User
            </th>
            <th style="text-align: center;">Allowed</th>
            <th style="text-align: center;">Denied</th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Area.Name %>
            </td>
            <td>
                <%: item.User.Username %>
            </td>
            <td>
                <%: Html.DisplayFor(x => item.AllowPermissions, "PermissionViewModel") %>
            </td>
            <td>
                <%: Html.DisplayFor(x => item.DenyPermissions, "PermissionViewModel")%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id })%> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("Add Access", "Create") %></li>
    </ul>
</asp:Content>

