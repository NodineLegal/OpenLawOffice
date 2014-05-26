<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventResponsibleUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details of User Responsiblity for Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of User Responsiblity for Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Task
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Event.Title, "Details", "Events", new { id = Model.Event.Id }, null)%>
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
        Displays detailed information of user responsibility for the event.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Add Resp. User", "Create", new { id = Model.Event.Id })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Responsible User List", "ResponsibleUsers", "Events", new { id = Model.Event.Id }, null)%></li>
</asp:Content>
