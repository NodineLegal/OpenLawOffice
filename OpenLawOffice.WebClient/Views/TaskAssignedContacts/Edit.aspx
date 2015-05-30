<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskAssignedContactViewModel>" %>

<%--<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Contact Assignment to Task
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div class="one">Task: [<%: Html.ActionLink((string)ViewData["Task"], "Details", "Tasks", new { id = ViewData["TaskId"] }, null) %>]</div>
        <div id="current" class="two">Edit Contact Assignment to Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true) %>
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
                Assignment<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.EnumDropDownListFor(model => model.AssignmentType) %>
                <%: Html.ValidationMessageFor(model => model.AssignmentType)%>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Modify the information on this page to make changes to the assignmnet of a contact to a task.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span>.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create", "Contacts")%></li>
        <li>
            <%: Html.ActionLink("Details", "Details", "TaskAssignedContacts", new { id = RouteData.Values["Id"] }, null)%></li>
        <li>
            <%: Html.ActionLink("Delete", "Delete", "TaskAssignedContacts", new { id = RouteData.Values["Id"] }, null)%></li>
    </ul>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = Model.Task.Id }, null)%></li>
    <li><%: Html.ActionLink("Contacts of Task", "Contacts", "Tasks", new { id = Model.Task.Id }, null)%></li>
</asp:Content>