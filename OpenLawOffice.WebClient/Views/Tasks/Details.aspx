<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Task Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Task Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Title
            </td>
            <td class="display-field">
                <%: Model.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Description
            </td>
            <td class="display-field">
                <%: Model.Description%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected Start
            </td>
            <td class="display-field">
                <% if (Model.ProjectedStart.HasValue)
                   { %>
                <%: String.Format("{0:g}", Model.ProjectedStart.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Due Date
            </td>
            <td class="display-field">
                <% if (Model.DueDate.HasValue)
                   { %>
                <%: String.Format("{0:g}", Model.DueDate.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected End
            </td>
            <td class="display-field">
                <% if (Model.ProjectedEnd.HasValue)
                   { %>
                <%: String.Format("{0:g}", Model.ProjectedEnd.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Actual End
            </td>
            <td class="display-field">
                <% if (Model.ActualEnd.HasValue)
                   { %>
                <%: String.Format("{0:g}", Model.ActualEnd.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Is a Grouping Task
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.IsGroupingTask, new { disabled=true }) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Parent
            </td>
            <td class="display-field">
                <% if (Model.Parent != null)
                   { %>
                <%: Model.Parent.Title%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Sequential Predecessor
            </td>
            <td class="display-field">
                <% if (Model.SequentialPredecessor != null)
                   { %>
                <%: Model.SequentialPredecessor.Title%>
                <% } %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information about the task.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Task", "Create", new { MatterId = ViewData["MatterId"] })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Tags", "Tags", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Events", "Events", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Events", new { controller = "Tasks", TaskId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Tasks", TaskId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Documents", "Documents", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Documents", new { controller = "Tasks", TaskId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Time", "Time", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "SelectContactToAssign", "TaskTime", new { TaskId = Model.Id }, null)%>)</li>
</asp:Content>