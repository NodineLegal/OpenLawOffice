<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Timing.TimeViewModel>>" %>

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
        });
    </script>
    
    <p>
        Date: <input type="text" id="date" name="date" value="<%: ((DateTime)ViewData["Date"]).ToString("MM/dd/yyyy") %>" />
    </p>

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
            <th></th>
        </tr>

    <% 
        double totalMinutes = 0;
        DateTime lastTimestampStart = DateTime.Today;
        DateTime lastTimestampStop = DateTime.Today;
        
        foreach (var item in Model) {

            totalMinutes += item.Duration.TotalMinutes;
                
            if (item.Start.Hour > lastTimestampStart.Hour && lastTimestampStart > DateTime.Today)
            { %>
            <tr>
                <td style="background-color: Black; height: 0px; padding: 2px;" colspan="5" />
            </tr>
            <%
            }
            if (item.Start.Subtract(lastTimestampStop).TotalMinutes > 10)
            {
            %>
            <tr>
                <td style="background-color: #D6F8DE; padding: 2px;" colspan="4" />
                <td style="background-color: #D6F8DE; padding: 2px; text-align:center;">
                    <%: Html.ActionLink("QuickEnter", "QuickEnter", "Timing", new { ContactId = RouteData.Values["Id"] }, new { @class = "btn-addtimeentry", title = "QuickEnter" })%>
                </td>
            </tr>
            <% } %>
            
            <tr <% if (lastTimestampStop > item.Start) { %>style="background-color: #FFCECE;"<% } %>>
                <td>
                    <%: String.Format("{0:g}", item.Start) %>
                </td>
                <% if (item.Stop.HasValue)
                   { %>
                    <td>
                        <%: String.Format("{0:g}", item.Stop)%>
                    </td>
                    <% }
                   else
                   { %>
                   <td style="background-color: #FFFFC8;">
                   ???
                   </td>
                   <% } %>
                <td>
                    <%: item.Duration %>
                </td>
                <td>
                    <%: item.Details %>
                </td>
                <td style="text-align:center;">
                    <%: Html.ActionLink("Edit", "Edit", "Timing", new { Id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                </td>
            </tr>
    
            <% 
            lastTimestampStart = item.Start;
            if (item.Stop.HasValue)
                lastTimestampStop = item.Stop.Value;
        } %>
        
        <tr>
            <td colspan="2" style="text-align: right; font-weight: bold;">
                Total Time:
            </td>
            <td style="text-align: center; font-weight: bold;">
                <%: TimeSpan.FromMinutes(Math.Round(totalMinutes, 0)).ToString(@"d'd 'hh'h 'mm'm'") %>
            </td>
            <td colspan="2">
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

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

