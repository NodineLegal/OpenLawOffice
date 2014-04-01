<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

    <table class="detail_table">
        <tr>
            <td class="display-label">Task</td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Task.Id) %>
                <%: Model.Task.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Worker</td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Time.Worker.Id) %>
                <%: Model.Time.Worker.DisplayName %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Start Date/Time</td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Time.Start)%>
                <%: Html.ValidationMessageFor(model => model.Time.Start)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">Stop Date/Time</td>
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

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
