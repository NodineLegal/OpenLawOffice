<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventTaskViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Assign Event for Task
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Assign Event for Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
    { %>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Event
            </td>
            <td class="display-field">
                <%: Model.Event.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Task
            </td>
            <td class="display-field">
                <%: Model.Task.Title%>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Link" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        If you would like to link the event and task on this page, click the 'Link' button.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click 'Link' to relate the matter and event.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Event", "Create", "Events")%></li>
    </ul>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>
