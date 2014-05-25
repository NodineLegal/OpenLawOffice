<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Settings.TagFilterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tag Filter Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Tag Filter Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Category
            </td>
            <td class="display-field">
                <%: Model.Category %>
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
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the tag filter.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Filter", "CreateFilter")%></li>
        <li>
            <%: Html.ActionLink("Edit", "EditFilter", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete ", "DeleteFilter", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Task Settings", "Index") %></li>
</asp:Content>