<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventMatterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Unlink Event and Matter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Unlink Event and Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
    { %>
    <%: Html.ValidationSummary(true) %>
    <%: Html.HiddenFor(x => x.Id) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Event
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Event.Title, "Details", "Events", new { id = Model.Event.Id }, null) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Matter
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Matter.Title, "Details", "Matters", new { id = Model.Matter.Id }, null)%>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Unlink" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        If you would like to unlink the event and matter on this page, click the 'Unlink' button.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click 'Unlink' to remove the link between the matter and event.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter", "Create", "Matters")%></li>
        <li>
            <%: Html.ActionLink("New Event", "Create", "Events")%></li>
    </ul>
    <li><%: Html.ActionLink("Matter", "Details", "Matters", new { id = Model.Matter.Id }, null)%></li>
    <li><%: Html.ActionLink("Event", "Details", "Events", new { id = Model.Event.Id }, null)%></li>
</asp:Content>
