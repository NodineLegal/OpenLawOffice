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
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id.Value }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id.Value })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id.Value })%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

