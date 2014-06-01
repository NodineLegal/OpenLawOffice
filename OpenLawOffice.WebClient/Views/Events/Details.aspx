<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Event Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Event Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                All Day Event
            </td>
            <td class="display-field">
                <%: Model.AllDay %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Start
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", Model.Start)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                End
            </td>
            <td class="display-field">
                <% if (Model.End.HasValue)
                   { %>
                <%: String.Format("{0:g}", Model.End.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Location
            </td>
            <td class="display-field">
                <%: Model.Location%>
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
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information about the event.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Event", "Create")%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matters", "Matters", "Events", new { id = Model.Id }, null)%> 
        (<%: Html.ActionLink("Attach", "SelectMatter", "EventMatter", new { id = RouteData.Values["Id"] }, null) %>)</li>
    <li>
        <%: Html.ActionLink("Tasks", "Tasks", "Events", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Tags", "Tags", "Events", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", "Events", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", "Events", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Events", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Events", EventId = Model.Id }, null)%>)</li>
</asp:Content>
