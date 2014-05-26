<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventAssignedContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details of Contact Assignment to Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of Contact Assignment to Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Event
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Event.Title, "Details", "Events", new { id = Model.Event.Id.Value }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Contact.DisplayName, "Details", "Contacts", new { id = Model.Contact.Id.Value }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Role
            </td>
            <td class="display-field">
                <%: Model.Role %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the assignment of the contact to the event.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete", "Delete", new { id = Model.Id })%></li>
    </ul>    
    <li><%: Html.ActionLink("Contacts of Event", "Contacts", "Events", new { id = Model.Event.Id.Value }, null)%></li>
</asp:Content>
