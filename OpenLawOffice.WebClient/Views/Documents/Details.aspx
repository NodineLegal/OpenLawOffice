<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.DocumentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Document Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <% if (ViewData["Task"] != null)
           { %>
        <div class="one">Task: [<%: Html.ActionLink((string)ViewData["Task"], "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%>]</div>
        <div id="current" class="two">Document Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
        <% }
           else
           { %>           
        <div id="current" class="one">Document Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
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
                Date
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy}", Model.Date) %>
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
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <h3>
        Versions</h3>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Version
            </th>
            <th style="width: 150px;">
            </th>
        </tr>
        <% foreach (var item in Model.Versions)
           { %>
        <tr>
            <td>
                <%: item.VersionNumber %>
            </td>
            <td>
                <%: Html.ActionLink("Details", "Details", "Versions", new { id = item.Id }, null)%>
                |
                <%: Html.ActionLink("Download", "Download", "Versions", new { id = item.Id }, null) %>
            </td>
        </tr>
        <% } %>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the document.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Version", "Create", "Versions", new { DocumentId = Model.Id }, null)%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
    </ul>
<% if (ViewData["MatterId"] != null)
    { %>
    <li><%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <li><%: Html.ActionLink("Matter Documents", "Documents", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
<%  }
    if (ViewData["TaskId"] != null)
    { %>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <li><%: Html.ActionLink("Task Documents", "Documents", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
<%  } %>
</asp:Content>