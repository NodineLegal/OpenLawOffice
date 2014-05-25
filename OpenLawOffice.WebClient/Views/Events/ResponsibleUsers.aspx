<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Events.EventResponsibleUserViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Users Responsible for Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Users Responsible for Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                User
            </th>
            <th style="text-align: center;">
                Responsibility
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.User.Username, "Details", "Users", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: Html.ActionLink(item.Responsibility, "Details", "ResponsibleUsers", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "ResponsibleUsers", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Remove", "Delete", "ResponsibleUsers", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Responsible users are the system users (people with login credentials) responsible for managing the system.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the user will show the details of the user.
        Clicking the responsibility will show the details of the responsibility.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the responsibility.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/cross.png" /> 
        (remove icon) to remove the user from the list of responsible users.  
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("Add Resp. User", "Create", "EventResponsibleUsers", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Event", "Details", "Events", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
</asp:Content>
