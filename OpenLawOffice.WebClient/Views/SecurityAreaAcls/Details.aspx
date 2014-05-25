<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaAclViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details</h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Area
            </td>
            <td class="display-field">
                <%: Html.LabelFor(model => model.Area.Name)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                User
            </td>
            <td class="display-field">
                <%: Html.LabelFor(model => model.User) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Allowed
            </td>
            <td class="display-field">
                <%: Html.DisplayFor(x => Model.AllowPermissions, "PermissionViewModel") %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Denied
            </td>
            <td class="display-field">
                <%: Html.DisplayFor(x => Model.DenyPermissions, "PermissionViewModel")%>
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
</asp:Content>