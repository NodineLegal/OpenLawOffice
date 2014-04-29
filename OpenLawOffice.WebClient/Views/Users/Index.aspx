<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Security.UserViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Users<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Username
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Username, "Details", new { id = item.Id })%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Users", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Users are individuals with system access.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the username will show the details of the user.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the user.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "Create") %></li>
    </ul>
</asp:Content>