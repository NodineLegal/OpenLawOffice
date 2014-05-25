<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details</h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Name
            </td>
            <td class="display-field">
                <%: Model.Name %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Parent
            </td>
            <td class="display-field">
                <% if (Model.Parent != null)
                   { %>
                <%: Model.Parent.Name %>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Description
            </td>
            <td class="display-field">
                <%: Model.Description %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Permissions", "Acls", "Matters")%></li>
</asp:Content>