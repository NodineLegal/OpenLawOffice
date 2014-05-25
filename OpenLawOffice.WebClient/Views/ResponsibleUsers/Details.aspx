<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.ResponsibleUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details of User Responsiblity for Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of User Responsiblity for Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Matter
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Matter.Title, "Details", "Matters", new { id = Model.Matter.Id }, null)  %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                User
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.User.Username, "Details", "Users", new { id = Model.User.Id }, null) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Responsiblity
            </td>
            <td class="display-field">
                <%: Model.Responsibility %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information of user responsibility for the matter.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Add Resp. User", "Create", new { id = Model.Matter.Id })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Responsible User List", "ResponsibleUsers", "Matters", new { id = Model.Matter.Id }, null)%></li>
</asp:Content>