<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Profile</h2>

    <p>
        You may change any profile information displayed below.
    </p>
    
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true) %>
        <div>
            <fieldset>
                <legend>Profile Information</legend>
                
                <div class="editor-label">
                    Email Address:
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email)%>
                </div>
                
                <p>
                    To change your password, click <%: Html.ActionLink("Change Password", "ChangePassword") %>
                </p>

                <p>
                    <input type="submit" value="Save Changes" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
