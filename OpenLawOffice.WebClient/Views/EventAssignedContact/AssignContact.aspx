<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventAssignedContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Assign Contact to Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Assign Contact to Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Event
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Event.Id)%>
                <%: Html.ActionLink(Model.Event.Title, "Details", "Events", new { id = Model.Event.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Contact.Id) %>
                <%: Html.ActionLink(Model.Contact.DisplayName, "Details", "Contacts", new { id = Model.Contact.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Role<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Role) %>
                <%: Html.ValidationMessageFor(model => model.Role)%>
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
        Fill in the information on this page to assign a contact to an event.  Required fields are indicated with an
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
    </ul>
    <li><%: Html.ActionLink("Event", "Details", "Events", new { id = Request["EventId"] }, null)%></li>
    <li><%: Html.ActionLink("Contacts of Event", "Contacts", "Events", new { id = Request["EventId"] }, null)%></li>
</asp:Content>
