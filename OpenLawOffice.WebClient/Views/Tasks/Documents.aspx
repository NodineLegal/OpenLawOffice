<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Documents.DocumentViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Documents of Task
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Documents of Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Documents", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Documents", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Documents are files.  They can be word documents, PDFs or any other type of file.
        The documents on this page pertain to the task.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the document.  Click the 
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
            <%: Html.ActionLink("New Document", "Create", "Documents", new { TaskId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li>
            <%: Html.ActionLink("Task", "Details", "Tasks", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>