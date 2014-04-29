<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTagViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Task Tags
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Task Tags<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Category
            </th>
            <th style="text-align: center;">
                Tag
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: item.TagCategory.Name %>
            </td>
            <td>
                <%: Html.ActionLink(item.Tag, "Details", "TaskTags", new { id = item.Id.Value }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "TaskTags", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Remove", "Delete", "TaskTags", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Tags allow users to describe a task and are searchable.  Tags allow for categorization so as to provide 
        additional meaning.  For instance a tag of "Active" makes more sense when it has a category of "Status".<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the tag.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the tag.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/cross.png" /> 
        (remove icon) to remove the tag from the list.  
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Add Tag", "Create", "TaskTags", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
        <li>
            <%: Html.ActionLink("Task", "Details", "Tasks", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>