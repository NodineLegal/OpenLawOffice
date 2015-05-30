<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master"
    Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTemplateViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Task Template
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>
        Create Task Template<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true)%>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Title of Template<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.TaskTemplateTitle, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.TaskTemplateTitle)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Title of Task
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Title, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Description
            </td>
            <td class="display-field">
                <%: Html.TextAreaFor(model => model.Description, new { style = "height: 50px; width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Description)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Active
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Active, new { Checked = true })%>
                Uncheck if the task is already completed
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected Start
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ProjectedStart)%>
                <%: Html.ValidationMessageFor(model => model.ProjectedStart)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Due Date
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.DueDate)%>
                <%: Html.ValidationMessageFor(model => model.DueDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected End
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ProjectedEnd)%>
                <%: Html.ValidationMessageFor(model => model.ProjectedEnd)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Actual End
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ActualEnd)%>
                <%: Html.ValidationMessageFor(model => model.ActualEnd)%>
            </td>
        </tr>
    </table>
    <div>
        <div style="font-weight:bold;">Use Tools</div>
        The following may be used in the above fields.<br />
        [NOW] - Insert the current date and time (<%: DateTime.Now.ToString("G") %>).<br />
        [DATE] - Insert the current date (<%: DateTime.Now.ToString("d") %>).<br />
        [DATE+X] where X is a number - Insert the current date plus the given value of X ([DATE+5]=<%: DateTime.Now.AddDays(5).ToString("d") %>).

    </div>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Fill in the information on this page to create a new task template.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
