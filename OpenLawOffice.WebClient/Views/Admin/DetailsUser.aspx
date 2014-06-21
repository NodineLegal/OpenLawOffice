<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.UsersViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Account Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>User Account Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <table class="detail_table">
        <tr>
            <td class="display-label">
                Username
            </td>
            <td class="display-field">
                <%: Model.Username%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email
            </td>
            <td class="display-field">
                <%: Model.Email%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Comment
            </td>
            <td class="display-field">
                <%: Model.Comment%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Approved
            </td>
            <td class="display-field">
                <% if (Model.IsApproved)
                   { %>Yes<% }
                   else
                   { %> No <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Last Activity
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.LastActivityDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Last Login
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.LastLoginDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Last Password Changed
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.LastPasswordChangedDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Creation
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.CreationDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Online
            </td>
            <td class="display-field">
                <% if (Model.IsOnLine)
                   { %>Yes<% }
                   else
                   { %> No <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Locked Out
            </td>
            <td class="display-field">
                <% if (Model.IsLockedOut)
                   { %>Yes<% }
                   else
                   { %> No <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Last Locked Out
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.LastLockedOutDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Failed Password Attempts
            </td>
            <td class="display-field">
                <%: Model.FailedPasswordAttemptCount%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Failed Password Attempt Window Start
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy hh:mm:ss tt}", Model.FailedPasswordAttemptWindowStart)%>
            </td>
        </tr>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information about the user account.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "CreateUser") %></li>
        <li>
            <%: Html.ActionLink("Edit", "EditUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Roles", "UserRoles", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Disable", "DisableUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Unlock", "UnlockUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>
