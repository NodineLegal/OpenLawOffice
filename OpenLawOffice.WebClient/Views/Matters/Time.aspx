<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterTimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Times
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Times</h2>
    <% foreach (var task in Model.Tasks)
       { %>
    <h4>
        <%: task.Title%></h4>
    <% double totalMinutes = 0; %>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Start
            </th>
            <th style="text-align: center;">
                Stop
            </th>
            <th style="text-align: center;">
                Duration
            </th>
            <th style="text-align: center;">
                Worker
            </th>
            <th style="width: 100px;">
            </th>
        </tr>
        <% foreach (var item in task.Times)
           { %>
        <% totalMinutes += item.Duration.TotalMinutes; %>
        <tr>
            <td>
                <%: item.Start%>
            </td>
            <td>
                <%: item.Stop%>
            </td>
            <td>
                <%: Math.Round(item.Duration.TotalMinutes, 0)%>
                min.
            </td>
            <td>
                <%: item.Worker.DisplayName%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Timing", new { id = item.Id.Value }, null)%>
                |
                <%: Html.ActionLink("Details", "Details", "Timing", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
        <% } %>
        <tr>
            <td colspan="2" style="text-align: right; font-weight: bold;">
                Total Time:
            </td>
            <td style="text-align: center; font-weight: bold;">
                <%: Math.Round(totalMinutes, 0)%>
                min.
            </td>
            <td colspan="2">
            </td>
        </tr>
    </table>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>