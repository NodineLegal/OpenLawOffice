<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Timing.DayViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DayView
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Daily Time</h2>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>

    <script>
        $(function () {
            $("#date").datepicker({
                autoSize: true,
                onSelect: function (date) {
                    $("form").submit();
                }
            });
            $('#Employee_Id').change(function () {
                $("form").submit();
            });
        });
    </script>
    
    <div class="options_div">
        <div style="width: 200px; display: inline;">
            Date: <input type="text" id="date" name="date" value="<%: ((DateTime)ViewData["Date"]).ToString("MM/dd/yyyy") %>" />
        </div>
        <div style="width: 200px; display: inline;">
            Employee:
            <%: Html.DropDownListFor(x => x.Employee.Id,
                new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                new { @size = 1, @style = "width: 200px" })%>
        </div>
    </div>

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
                Matter
            </th>
            <th>
                Task
            </th>
            <th>
                Details
            </th>
            <th></th>
        </tr>

    <% 
        double totalMinutes = 0;
        DateTime lastTimestampStart = (DateTime)ViewData["Date"];// DateTime.Today;
        DateTime lastTimestampStop = (DateTime)ViewData["Date"];// DateTime.Today;
        
        foreach (var item in Model.Items) {

            totalMinutes += item.Time.Duration.TotalMinutes;

            bool b = item.Time.Start.Hour > lastTimestampStart.Hour;
            b = lastTimestampStart > DateTime.Today;

            if (item.Time.Start.Hour > lastTimestampStart.Hour && lastTimestampStart > (DateTime)ViewData["Date"])
            {
                string a = "";
                %>
            <tr>
                <td style="background-color: Black; height: 0px; padding: 2px;" colspan="7" />
            </tr>
            <%
            }
            if (item.Time.Start.Subtract(lastTimestampStop).TotalMinutes > 10)
            {
            %>
            <tr>
                <td style="background-color: #D6F8DE; padding: 2px;" colspan="6" />
                <td style="background-color: #D6F8DE; padding: 2px; text-align:center;">
                    <%: Html.ActionLink("QuickEnter", "QuickEnter", "Timing", new { ContactId = RouteData.Values["Id"] }, new { @class = "btn-addtimeentry", title = "QuickEnter" })%>
                </td>
            </tr>
            <% } %>
            
            <tr <% if (lastTimestampStop > item.Time.Start) { %>style="background-color: #FFCECE;"<% } %>>
                <td style="width: 170px;">
                    <%: String.Format("{0:g}", item.Time.Start) %>
                </td>
                <% if (item.Time.Stop.HasValue)
                   { %>
                    <td style="width: 170px;">
                        <%: String.Format("{0:g}", item.Time.Stop)%>
                    </td>
                    <% }
                   else
                   { %>
                   <td style="width: 170px; background-color: #FFFFC8;">
                   ???
                   </td>
                   <% } %>
                <td style="width: 80px;">
                    <%: item.Time.Duration %>
                </td>
                <td>
                    <%: item.Matter.Title %>
                </td>
                <td>
                    <%: item.Task.Title %>
                </td>
                <td>
                    <%: item.Time.Details%>
                </td>
                <td style="text-align:center; width: 16px;">
                    <%: Html.ActionLink("Edit", "Edit", "Timing", new { Id = item.Time.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                </td>
            </tr>
    
            <% 
            lastTimestampStart = item.Time.Start;
            if (item.Time.Stop.HasValue)
                lastTimestampStop = item.Time.Stop.Value;
        } %>
        
        <tr>
            <td colspan="2" style="text-align: right; font-weight: bold;">
                Total Time:
            </td>
            <td style="text-align: center; font-weight: bold;">
                <%: TimeSpan.FromMinutes(Math.Round(totalMinutes, 0)).ToString(@"d'd 'hh'h 'mm'm'") %>
            </td>
            <td colspan="4">
            </td>
        </tr>
    </table>

    <h3>Legend</h3>

    <table>
        <tr>
            <td style="background-color: #FFCECE;">This entry overlaps the previous entry.</td>
        </tr>
        <tr>
            <td style="background-color: #FFFFC8;">This entry does not have a stop time.</td>
        </tr>
        <tr>
            <td style="background-color: #D6F8DE;">Gap of over ten minutes, make sure you are not missing an entry.</td>
        </tr>
    </table>
    <% } %>   

</asp:Content>