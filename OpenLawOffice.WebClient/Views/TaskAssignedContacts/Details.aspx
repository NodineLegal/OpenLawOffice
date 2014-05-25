<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskAssignedContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details of Contact Assignment to Task
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of Contact Assignment to Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Task
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Task.Title, "Details", "Tasks", new { id = Model.Task.Id }, null) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                User
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Contact.DisplayName, "Details", "Contacts", new { id = Model.Contact.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Assignment
            </td>
            <td class="display-field">
                <%: Model.AssignmentType %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the assignment of the contact to the task.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create", "Contacts")%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", "TaskAssignedContacts", new { id = RouteData.Values["Id"] }, null)%></li>
        <li>
            <%: Html.ActionLink("Delete", "Delete", "TaskAssignedContacts", new { id = RouteData.Values["Id"] }, null)%></li>
    </ul>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = Model.Task.Id }, null)%></li>
    <li><%: Html.ActionLink("Contacts of Task", "Contacts", "Tasks", new { id = Model.Task.Id }, null)%></li>
</asp:Content>