<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Events.EventViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Agenda for <%: ViewData["ContactDisplayName"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Agenda for <%: ViewData["ContactDisplayName"] %><a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                AllDay
            </th>
            <th>
                Start
            </th>
            <th>
                End
            </th>
            <th>
                Location
            </th>
            <th>
                Description
            </th>
            <th style="width: 20px;"></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Events", new { id = item.Id }, null)%>
            </td>
            <td>
                <% if (item.AllDay)
                   { %>Yes<% }
                   else
                   { %>No<% } %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.Start) %>
            </td>
            <td>
                <% if (item.End.HasValue)
                   { %>
                    <%: String.Format("{0:g}", item.End)%>
                <% } %>
            </td>
            <td>
                <%: item.Location %>
            </td>
            <td>
                <%: item.Description %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>            
            </td>
        </tr>
    
    <% } %>

    </table>    

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        The contact agenda shows events (only events) in the order which they will happen.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the event.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the event.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Event", "Create")%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Select Contact", "SelectContact", "Events", null, null)%></li>
</asp:Content>

