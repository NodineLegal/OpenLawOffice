<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskAssignedContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Unassign Contact from Task
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div class="one">Task: [<%: Html.ActionLink((string)ViewData["Task"], "Details", "Tasks", new { id = ViewData["TaskId"] }, null) %>]</div>
        <div id="current" class="two">Unassign Contact from Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <h3>
        Are you sure you want to unlink the contact from the task?</h3>
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
    <p>
        <input type="submit" value="Delete" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a contact to be removed from a task by "unassigning" that contact.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        To unassign the contact from the task, click the unassign button.
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
            <%: Html.ActionLink("Details", "Details", "TaskAssignedContacts", new { id = RouteData.Values["Id"] }, null)%></li>
    </ul>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = Model.Task.Id }, null)%></li>
    <li><%: Html.ActionLink("Contacts of Task", "Contacts", "Tasks", new { id = Model.Task.Id }, null)%></li>
</asp:Content>