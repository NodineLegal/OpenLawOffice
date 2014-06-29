<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Confirm FastTime Assignment
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Confirm FastTime Assignment<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Task
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Task.Title, "Details", "Tasks", new { id = Model.Task.Id }, null) %>
            </td>
        </tr><tr>
            <td class="display-label">
                Start
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", Model.Time.Start)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Stop
            </td>
            <td class="display-field">
                <% if (Model.Time.Stop.HasValue)
                   { %>
                    <%: String.Format("{0:g}", Model.Time.Stop.Value)%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Worker
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Time.Worker.DisplayName, "Details", "Contacts", new { id = Model.Time.Worker.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Details
            </td>
            <td class="display-field">
                <%: Model.Time.Details%>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Confirm" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Click the 'Confirm' button to link the time entry with the task.<br /><br />
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
