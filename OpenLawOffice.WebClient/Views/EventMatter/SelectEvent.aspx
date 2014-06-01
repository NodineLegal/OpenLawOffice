<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Events.EventViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Select Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Select Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                Start
            </th>
            <th>
                End
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Events", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: String.Format("{0:g}", item.Start)%>
            </td>
            <td>
                <%: String.Format("{0:g}", item.End)%>
            </td>
            <td>
                <%: Html.ActionLink("Assign", "AssignEvent", new { id = RouteData.Values["Id"], EventId = item.Id }, new { @class = "btn-assignevent", title = "Assign Event" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Events are represent things that happen at some specified point in time.  Events are most usually 
        found useful for tracking appointments.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the event title will show the details of the event. Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/calendar-arrow.png" /> (assign event icon)
        to relate the event and the matter.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

