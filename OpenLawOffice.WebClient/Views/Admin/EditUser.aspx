<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.UsersViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit User Account
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit User Account<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Email<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email)%>
                <%: Html.ValidationMessageFor(model => model.Email)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Comment
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Comment)%>
                <%: Html.ValidationMessageFor(model => model.Comment)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Approved
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.IsApproved)%>
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
        Modify the information on this page to make changes to a user account.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> (display name).<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
