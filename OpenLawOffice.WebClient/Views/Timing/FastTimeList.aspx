<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Timing.TimeViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Unrelated FastTime
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Unrelated FastTime<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <table class="listing_table">
        <tr>
            <th>
                Start
            </th>
            <th>
                Stop
            </th>
            <th>
                Duration
            </th>
            <th>
                Details
            </th>
            <th>
                Worker
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: String.Format("{0:g}", item.Start) %>
            </td>
            <td>
            <% if (item.Stop.HasValue)
               { %>
                <%: String.Format("{0:g}", item.Stop)%>
            <% } %>
            </td>
            <td>
                <%: item.Duration %>
            </td>
            <td>
                <%: item.Details %>
            </td>
            <td>
                <%: Html.ActionLink(item.WorkerDisplayName, "Details", "Contacts", new { Id = item.Worker.Id }, null) %>
            </td>
            <td>
                <%: Html.ActionLink("Relate Task", "RelateTask", "TaskTime", new { Id = item.Id }, null) %>
            </td>
        </tr>
    
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        FastTime provides a method to enter working time frames at a rapid pace, later assigning tasks to which the time belongs.
        Here is a list of currently unrelated FastTime entries.  You should relate all the entries here as soon as possible.
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click the relate task link to relate a task to the FastTime entry.
        </p>
    </div>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

