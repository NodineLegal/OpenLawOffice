<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Notes.NoteTaskViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matter Notes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Matter Notes<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>

    <%
    foreach (var item in Model)
    { %>
    <p>
        <div>Date/Time: <%: item.Note.Timestamp %></div>
        <div>Task: <% if (item.Task != null) { %>
                    <%: Html.ActionLink(item.Task.Title, "Details", "Tasks", new { id = item.Task.Id }, null)%>
                    <% } %></div>
        <div>Title: <%: Html.ActionLink(item.Note.Title, "Details", "Notes", new { id = item.Note.Id }, null)%></div>
        <div><%= item.Note.Body %></div>
    </p><hr />
    <% } %>
        
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Notes allow users to make comments or record information on matters.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the note.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the note.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Note", "Create", "Notes", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { Id  = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>