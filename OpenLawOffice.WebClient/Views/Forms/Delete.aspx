<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Forms.FormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete Form
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Delete Form<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <h3>
        Are you sure you want to disable this?</h3>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Model.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Matter Type<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Model.MatterType.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                File<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.ActionLink("Download File", "Download", new { id = Model.Id }) %>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Delete" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a form to be disabled.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        To disable the form, click the Delete button.
        </p>
    </div>
</asp:Content>
