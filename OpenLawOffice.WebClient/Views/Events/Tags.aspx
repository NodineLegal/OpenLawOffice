<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Events.EventTagViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Event Tags
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Event Tags<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                <%: Html.ActionLink(item.Tag, "Details", "EventTags", new { id = item.Id.Value }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "EventTags", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Remove", "Delete", "EventTags", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Tags allow users to describe an event and are searchable.  Tags allow for categorization so as to provide 
        additional meaning.  For instance a tag of "High" makes more sense when it has a category of "Priority".<br /><br />
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
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Add Tag", "Create", "EventTags", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Event", "Details", "Events", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>
