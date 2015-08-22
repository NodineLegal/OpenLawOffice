<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>User Profile</h2>

    <p>
        You may change any profile information displayed below.
    </p>
    
<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                My Contact Info
            </td>
            <td class="display-field">
                <%: Html.DropDownListFor(x => x.ContactId,
                    new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName")) %>
                <%: Html.ValidationMessageFor(m => m.ContactId)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                External App. Key
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(x => x.ExternalAppKey, new { @style = "width: 400px;" }) %>
                <%: Html.ValidationMessageFor(m => m.ContactId)%>
                <a href="?newAppKey=true">Generate New Key</a>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <p>
        To change your password, click <%: Html.ActionLink("Change Password", "ChangePassword") %>
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
