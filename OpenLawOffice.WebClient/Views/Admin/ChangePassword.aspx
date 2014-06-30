<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.UsersViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Change User Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change User Password<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>

    <table class="detail_table">
        <tr>
            <td class="display-label">
                Username
            </td>
            <td class="display-field">
                <%: Model.Username %>
                <%: Html.HiddenFor(model => model.Username) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Password<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.PasswordFor(model => model.Password)%>
                <%: Html.ValidationMessageFor(model => model.Password)%>
            </td>
        </tr>
    </table>

    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>   
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Modify the password on this page to change the user's password.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> (display name).<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "CreateUser") %></li>
        <li>
            <%: Html.ActionLink("Details", "DetailsUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Edit", "EditUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Roles", "UserRoles", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Disable", "DisableUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Unlock", "UnlockUser", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Change Password", "ChangePassword", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Reset Password", "ResetPassword", new { id = Model.Username })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("List", "Index") %></li>
</asp:Content>
