<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Events.EventAssignedContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Contacts Assigned to Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                Role
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Contact.DisplayName, "Details", "Contacts", new { id = item.Contact.Id }, null)%>
            </td>
            <td>
                <%: Html.ActionLink(item.Role, "Details", "EventContact", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "EventContact", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Unassign", "Delete", "EventContact", new { id = item.Id }, new { @class = "btn-remove", title = "Unassign" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Contacts are the people (individuals or organizations) that do things or have things done to or for them. 
        The contacts on this page are assigned to the event with the role stated.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the contact will show the details of the contact.
        Clicking the role will show the details of the contact assignment.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the contact.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/cross.png" /> (remove icon) to 
        unassign the contact from the event.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create", "Contacts", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li>
            <%: Html.ActionLink("Assign Contact", "SelectContactToAssign", "EventContact", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
    <li><%: Html.ActionLink("Event", "Details", "Events", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>

