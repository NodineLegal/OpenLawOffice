<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Documents.SelectableDocumentViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Documents of Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Documents of Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>

    <div style="width: 70px; padding-bottom: 5px;">
        <a id="SelectAll" href="javascript:void(0);">Select All</a><br />
        <a id="DeselectAll" href="javascript:void(0);">Deselect All</a>
<script language="javascript">

    $(function () {    
        var cbs = [];

        <% 
        foreach (var item in Model)
        { %>
            <%= "cbs.push('" + item.Id + "');" %>
          <%
        }
        %>

        $("#SelectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', true);
            }
        });

        $("#DeselectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', false);
            }
        });
    });
</script>
    </div>

    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="listing_table">
        <tr>
            <th style="text-align: center; width: 20px;" />
            <th style="text-align: center;">Date</th>
            <th style="text-align: center;">Title</th>
            <th style="text-align: center;">Task</th>
            <th style="text-align: center;">Ext</th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% 
           foreach (var item in Model)
           { %>
        <tr>
            <td>
                <input type="checkbox" id="CB_<%: item.Id.Value %>" name="CB_<%: item.Id.Value %>" />
            </td>
            <td>
                <%: string.Format("{0:MMM d, yyyy}", item.Date) %>
            </td>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Documents", new { id = item.Id }, null)%>
            </td>
            <td>
                <% if (item.Task != null)
                   { %>
                    <%: Html.ActionLink(item.Task.Title, "Details", "Tasks", new { id = item.Task.Id.Value }, null) %>
                <% } %>
            </td>
            <td>
                <%: item.Extension %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Edit", "Edit", "Documents", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Download", "Download", "Documents", new { id = item.Id }, new { @class = "btn-download", title = "Download" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <input type="submit" value="Download all Selected" style="margin-top: 5px;" />
    
    <% } %>

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
    </ul>
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>