<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventMatterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Assign Matter for Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Assign Matter for Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Matter
            </td>
            <td class="display-field">
                <%: Model.Matter.Title %>
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
        If you would like to link the event and matter on this page, click the 'Link' button.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click 'Link' to relate the matter and event.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter", "Create", "Matters")%></li>
    </ul>
    <li><%: Html.ActionLink("Event", "Details", "Events", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>
