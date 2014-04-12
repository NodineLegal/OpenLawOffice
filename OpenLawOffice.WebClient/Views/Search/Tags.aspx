<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Search.TagSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search Tags
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Search Tags</h2>
    
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
    
    <div style="width: 100%; text-align: center;">
        <div style="width: 100%; text-align: center;">
            <%: Html.TextBoxFor(model => model.Query, new { style = "width: 250px;" })%><input type="submit" value="Search" style="width: 15%;"/>
        </div>
        Matters: <%: Html.CheckBoxFor(model => model.SearchMatters) %>
        Tasks: <%: Html.CheckBoxFor(model => model.SearchTasks) %>
    </div>

    <% } %>

    <% if (Model.MatterTags != null && Model.MatterTags.Count > 0)
       {%>
        <br />
        <h3>Matters</h3>
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
                <th style="width: 150px;"></th>
            </tr>

            <% foreach (var item in Model.MatterTags) { %>
    
                <tr>
                    <td>
                        <%: item.Matter.Title %>
                    </td>
                    <td>
                        <%: item.TagCategory.Name %>
                    </td>
                    <td>
                        <%: item.Tag %>
                    </td>
                    <td>
                        <%: Html.ActionLink("Edit", "Edit", "MatterTags", new { id = item.Id.Value }, null)%> |
                        <%: Html.ActionLink("Details", "Details", "MatterTags", new { id = item.Id.Value }, null)%> |
                        <%: Html.ActionLink("Delete", "Delete", "MatterTags", new { id = item.Id.Value }, null)%>
                    </td>
                </tr>
    
            <% } %>

        </table>
    <% } %>
    
    <% if (Model.TaskTags != null && Model.TaskTags.Count > 0)
       {%>
        <br />
        <h3>Tasks</h3>
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
                <th style="width: 150px;"></th>
            </tr>

            <% foreach (var item in Model.TaskTags) { %>
    
                <tr>
                    <td>
                        <%: item.Task.Title %>
                    </td>
                    <td>
                        <%: item.TagCategory.Name %>
                    </td>
                    <td>
                        <%: item.Tag %>
                    </td>
                    <td>
                        <%: Html.ActionLink("Edit", "Edit", "MatterTags", new { id = item.Id.Value }, null)%> |
                        <%: Html.ActionLink("Details", "Details", "MatterTags", new { id = item.Id.Value }, null)%> |
                        <%: Html.ActionLink("Delete", "Delete", "MatterTags", new { id = item.Id.Value }, null)%>
                    </td>
                </tr>
    
            <% } %>

        </table>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
