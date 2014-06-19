<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Account.UsersViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <h2>Administration<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <table class="listing_table">
        <tr>
            <th>
                Username
            </th>
            <th>
                Email
            </th>
            <th>
                Approved
            </th>
            <th>
                Locked Out
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink(item.Username, "DetailsUser", new { Username = item.Username })%>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td>
                <% if (item.IsApproved)
                   { %>Yes<% }
                   else
                   { %> No <% } %>
            </td>
            <td>
                <% if (item.IsLockedOut)
                   { %>Yes<% }
                   else
                   { %> No <% } %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "EditUser", new { Username = item.Username })%><br />
                <%: Html.ActionLink("Roles", "UserRoles", new { Username = item.Username })%><br />

                <% 
                    if (item.IsLockedOut)
                { %>
                <%: Html.ActionLink("Unlock", "UnlockUser", new { Username = item.Username })%><br />
                <% } %>

                <% 
                    if (item.IsApproved)
                { %>
                <%: Html.ActionLink("Disable", "DisableUser", new { Username = item.Username })%><br />
                <% } %>
                
                <%: Html.ActionLink("Reset Password", "ResetPassword", new { Username = item.Username })%>
            </td>
        </tr>
    
    <% } %>

    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page shows a list of users and allows administration of their accounts.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the user's account.  To make changes to a user's
        account, click 'Edit'.  If the user's account is locked, clicking 'Unlock' will allow the
        user to attempt login again (Unlock only appears if the account is locked out).  Clicking
        'Reset Password' will send the user an email allowing them to reset their account password.  
        To prevent access, click 'Disable', this can be changed later by editing the account
        and clicking the approved checkbox (Disable only appears if the account is Approved).
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "CreateUser") %></li>
    </ul>
</asp:Content>

