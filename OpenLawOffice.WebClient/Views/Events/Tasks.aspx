<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Tasks.TaskViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tasks for Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tasks for Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                Due Date
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Matters", new { id = item.Id }, null)%>
            </td>
            <td>
                <% if (item.DueDate.HasValue)
                   { %>
                <%: String.Format("{0:g}", item.DueDate.Value)%>
                <% } %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Tasks", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Unassign", "Delete", "EventMatter", new { id = item.Id }, new { @class = "btn-remove", title = "Unassign" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page shows a list of all matters to which this event is related.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the matter.
        Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the task.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/cross.png" /> (remove icon) to 
        unlink the event and the task.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Event", "Create") %></li>
        <li>
            <%: Html.ActionLink("New Task", "Create", "Tasks", null, null) %></li>
        <li>
            <%: Html.ActionLink("Attach Task", "SelectTask", "EventTask", new { EventId = RouteData.Values["Id"] }, null) %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Event", "Details", new { id = RouteData.Values["Id"] })%></li>
</asp:Content>
