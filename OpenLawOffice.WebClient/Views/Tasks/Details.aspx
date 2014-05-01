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
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.ProjectedStart.Value, DateTimeKind.Utc).ToLocalTime())%>
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
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.DueDate.Value, DateTimeKind.Utc).ToLocalTime())%>
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
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.ProjectedEnd.Value, DateTimeKind.Utc).ToLocalTime())%>
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
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.ActualEnd.Value, DateTimeKind.Utc).ToLocalTime())%>
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
    <table class="detail_table">
        <tr>
            <td colspan="5" style="font-weight: bold;">
                Core Details
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Created By
            </td>
            <td class="display-field">
                <%: Model.CreatedBy.Username %>
            </td>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Created At
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcCreated.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Modified By
            </td>
            <td class="display-field">
                <%: Model.ModifiedBy.Username %>
            </td>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Modified At
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcModified.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Disabled By
            </td>
            <% if (Model.DisabledBy != null)
               { %>
            <td class="display-field">
                <%: Model.DisabledBy.Username%>
            </td>
            <% }
               else
               { %>
            <td />
            <% } %>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Disabled At
            </td>
            <% if (Model.UtcDisabled.HasValue)
               { %>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcDisabled.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
            <% }
               else
               { %>
            <td class="display-field">
            </td>
            <% } %>
        </tr>
    </table>

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
        <li>
            <%: Html.ActionLink("Matter ", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Tags", "Tags", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", "Tasks", new { id = Model.Id }, null)%></li>
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