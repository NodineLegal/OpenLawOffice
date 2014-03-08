<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Matters.MatterTagViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tags
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tags</h2>

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

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.TagCategory.Name %>
            </td>
            <td>
                <%: item.Tag %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "MatterTags", new { id = item.Id.Value }, null)%> |
                <%: Html.ActionLink("Details", "Details", "MatterTags", new { id = item.Id.Value }, null)%> |
                <%: Html.ActionLink("Delete", "Delete", "MatterTags", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("Add Tag", "Create", "MatterTags", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Matter", "Details", "Matters", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>

