<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Forms.FormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Form
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Create Form<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <% using (Html.BeginForm("Create", "Forms", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <%: Html.ValidationSummary(true)%>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Title, new { @style = "width: 100%" })%>
                <%: Html.ValidationMessageFor(model => model.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Matter Type<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.DropDownListFor(x => x.MatterType.Id,
                        new SelectList((IList)ViewData["MatterTypeList"], "Id", "Title"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                File<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.FileUpload, new { type = "file", @style = "width: 100%" })%>
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
        Fill in the information on this page to create a new form field.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
