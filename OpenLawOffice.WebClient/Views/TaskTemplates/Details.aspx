<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master"
    Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTemplateViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Task Template Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>
        Task Template Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Title
            </td>
            <td class="display-field">
                <%: Model.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Projected Start
            </td>
            <td class="display-field">
                <%: Model.ProjectedStart %>
            </td>            
            <td></td>
            <td class="display-label" style="width: 125px;">
                Projected End
            </td>
            <td class="display-field">
                <%: Model.ProjectedEnd %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Due Date
            </td>
            <td class="display-field">
                <%: Model.DueDate %>
            </td>      
            <td></td>
            <td class="display-label" style="width: 125px;">
                Actual End
            </td>
            <td class="display-field">
                <%: Model.ActualEnd %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Active
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Active, new { disabled = true })%>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Description
            </td>
            <td class="display-field" colspan="4">
                <%: Model.Description%>
            </td>
        </tr>
    </table>
    
</asp:Content>
