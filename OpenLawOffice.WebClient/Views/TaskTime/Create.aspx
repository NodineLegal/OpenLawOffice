<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    New Time Entry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        New Time Entry<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Task
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Task.Id) %>
                <%: Model.Task.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Worker
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Time.Worker.Id) %>
                <%: Model.Time.Worker.DisplayName %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Start Date/Time<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Time.Start)%>
                <%: Html.ValidationMessageFor(model => model.Time.Start)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Stop Date/Time<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Time.Stop)%>
                <%: Html.ValidationMessageFor(model => model.Time.Stop)%>
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
        Fill in the information on this page to create a new time entry for the task.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>