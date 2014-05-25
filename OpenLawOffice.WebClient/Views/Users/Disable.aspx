<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Disable User
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Disable User<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <h3>
        Are you sure you want to disable this user?</h3>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Username
            </td>
            <td class="display-field">
                <%: Model.Username%>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <p>
        <input type="submit" value="Delete" />
    </p>
    <% } %>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a user to be disabled.  This will prevent login and creating of any new relations
        for this user.  However, the user will still exist within the database to maintain necessary
        relationships including audit trails.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        To disable the user, click the disable button.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "Create") %></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Details ", "Details", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("User List", "Index") %></li>
</asp:Content>
