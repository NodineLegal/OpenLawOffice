<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>
    
    <table class="detail_table">
        <tr>
            <td class="display-label">Task</td>
            <td class="display-field">
                <%: Model.Task.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Worker</td>
            <td class="display-field">
                <%: Model.Time.Worker.DisplayName %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Start Date/Time</td>
            <td class="display-field">
                <%: Model.Time.Start %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Stop Date/Time</td>
            <td class="display-field">
                <%: Model.Time.Stop %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Duration</td>
            <td class="display-field">
                <%: ((TimeSpan)(Model.Time.Stop - Model.Time.Start)).TotalMinutes %> minutes
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li><%: Html.ActionLink("Tasks", "Details", "Tasks", new { id = Model.Task.Id.Value }, null)%></li>
    <li><%: Html.ActionLink("Times", "Time", "Tasks", new { id = Model.Task.Id.Value }, null)%></li>
</asp:Content>
