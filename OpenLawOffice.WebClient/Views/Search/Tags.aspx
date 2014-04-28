<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Search.TagSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Search Tags
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Search Tags<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <div style="width: 100%; text-align: center;">
        <div style="width: 100%; text-align: center;">
            <%: Html.TextBoxFor(model => model.Query, new { style = "width: 250px;" })%><input
                type="submit" value="Search" style="width: 15%;" />
        </div>
        Matters:
        <%: Html.CheckBoxFor(model => model.SearchMatters) %>
        Tasks:
        <%: Html.CheckBoxFor(model => model.SearchTasks) %>
    </div>
    <% } %>
    <% if (Model.MatterTags != null && Model.MatterTags.Count > 0)
       {%>
    <br />
    <h3>
        Matters</h3>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Matter
            </th>
            <th style="text-align: center;">
                Category
            </th>
            <th style="text-align: center;">
                Tag
            </th>
        </tr>
        <% foreach (var item in Model.MatterTags)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Matter.Title, "Details", "Matters", new { id = item.Matter.Id.Value }, null)%>
            </td>
            <td>
                <%: item.TagCategory.Name %>
            </td>
            <td>
                <%: Html.ActionLink(item.Tag, "Details", "MatterTags", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
        <% } %>
    </table>
    <% } %>
    <% if (Model.TaskTags != null && Model.TaskTags.Count > 0)
       {%>
    <br />
    <h3>
        Tasks</h3>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Task
            </th>
            <th style="text-align: center;">
                Category
            </th>
            <th style="text-align: center;">
                Tag
            </th>
        </tr>
        <% foreach (var item in Model.TaskTags)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Task.Title, "Details", "Tasks", new { id = item.Task.Id.Value }, null)%>
            </td>
            <td>
                <%: item.TagCategory.Name %>
            </td>
            <td>
                <%: Html.ActionLink(item.Tag, "Details", "TaskTags", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
        <% } %>
    </table>
    <% } %>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>        
        Click the matter name to view its details or the tag to view its details.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Selecting Matters or Tasks will include results of those tags.  Enter a part or a whole tag and click search.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>