<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventTagViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Delete Event Tag
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Delete Event Tag<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <h3>
        Are you sure you want to delete this?</h3>
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
                Event
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Event.Title, "Details", "Events", new { id = Model.Event.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Category
            </td>
            <td class="display-field">
                <%: Model.TagCategory.Name %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Tag
            </td>
            <td class="display-field">
                <%: Model.Tag %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <p>
        <input type="submit" value="Delete" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a tag to be removed.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        To remove the tag, click the Delete button.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Event Tag", "Create", new { id = Model.Event.Id })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Details ", "Details", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Event Tags", "Tags", "Events", new { id = Model.Event.Id }, null)%></li>
</asp:Content>
