<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Settings.UserTaskSettingsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Task Settings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Task Settings<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <h3>
        My Tasks Filters</h3>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Category
            </th>
            <th style="text-align: center;">
                Tag
            </th>
            <th style="width: 60px;">
            </th>
        </tr>
        <% foreach (var item in Model.MyTasksFilter)
           { %>
        <tr>
            <td>
                <%: item.Category %>
            </td>
            <td>
                <%: item.Tag %>
            </td>
            <td>
                <%: Html.ActionLink("Details", "DetailsFilter", "UserTaskSettings", new { id = item.Id.Value }, new { @class = "btn-settinggo", title = "Details" })%>
                <%: Html.ActionLink("Edit", "EditFilter", "UserTaskSettings", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Delete", "DeleteFilter", "UserTaskSettings", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Task settings allow each user to customize certain aspects of the system.  "My Task Filters" 
        are used to determine which tasks are shown on your dashboard under "My Todo List".  Users
        should specify a category/tag pair indicating the task is not complete.  This is going to 
        depend on how your system administrator has setup the system.  If you have questions, you should
        contact your system administrator.  We encourage the use of the "Status" category with "Pending", but it
        is customizable.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/gear-arrow.png" /> 
        (setting icon) to show details of the setting tag filter. Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the filter.  Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/cross.png" /> 
        (remove icon) to remove the filter.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("New Filter", "CreateFilter")%></li>
</asp:Content>