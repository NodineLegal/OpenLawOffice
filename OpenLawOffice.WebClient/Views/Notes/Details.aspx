<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Notes.NoteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Note Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <% if (ViewData["Task"] != null)
           { %>
        <div class="one">Task: [<%: Html.ActionLink((string)ViewData["Task"], "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%>]</div>
        <div id="current" class="two">Note Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
        <% }
           else
           { %>           
        <div id="current" class="one">Note Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
        <% } %>
    </div>
    
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
                Body
            </td>
            <td class="display-field">
                <%: Model.Body%>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information of the note.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Note", "Create", new { MatterId = ViewData["MatterId"], TaskId = ViewData["TaskId"] })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
    </ul>
        <% if (ViewData["MatterId"] != null)
           { %>
    
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes of Matter", "Notes", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <% }
           else if (ViewData["TaskId"] != null)
           { %>
    <li>
        <%: Html.ActionLink("Task", "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes of Task", "Notes", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <% } %>
</asp:Content>