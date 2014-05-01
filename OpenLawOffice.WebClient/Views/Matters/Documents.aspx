﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Documents.DocumentViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Documents of Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Documents of Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">Title</th>
            <th style="text-align: center;">Task</th>
            <th style="width: 50px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Documents", new { id = item.Id }, null)%>
            </td>
            <td>
                <% if (item.Task != null)
                   { %>
                    <%: Html.ActionLink(item.Task.Title, "Details", "Tasks", new { id = item.Task.Id.Value }, null) %>
                <% } %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Edit", "Edit", "Documents", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Download", "Download", "Documents", new { id = item.Id }, new { @class = "btn-download", title = "Download" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Documents are files.  They can be word documents, PDFs or any other type of file.
        The documents on this page pertain to the matter.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        If a document was created from within a task, that task will appear within that column.
        Clicking the title will show the details of the document.  Clicking the task will show the 
        details of the task.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the document.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/download-cloud.png" /> (download icon) to 
        download the current version of the document.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Document", "Create", "Documents", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li>
            <%: Html.ActionLink("Matter", "Details", "Matters", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>