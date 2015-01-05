<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Timing.DayViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Timesheet
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>    
    #opt_selected {
      margin-top: 20px;
      font-size: 20px;
    }

    .print_container {
      margin: 20px 30px 10px 30px ;
      display: inline;
    }
 
    .print_menu {
      position: absolute;
      width: 240px !important;
      margin-top: 3px !important;
    }
 
    /* fix for jquery-ui-bootstrap theme */
    #print_launcher span {
      display: inline;
    }
    </style>

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Timesheet for Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
        
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>

    <script>
        $(function () {
            $("#from").datepicker({
                autoSize: true,
                onSelect: function (date) {
                    $("form").submit();
                }
            });
            $("#to").datepicker({
                autoSize: true,
                onSelect: function (date) {
                    $("form").submit();
                }
            });
            $('#Employee_Id').change(function () {
                $("form").submit();
            });
            $("#print_drop").jui_dropdown({
                launcher_id: 'print_launcher',
                launcher_container_id: 'print_container',
                menu_id: 'print_menu',
                containerClass: 'print_container',
                menuClass: 'print_menu',
                launcher_is_UI_button: false,
                onSelect: function (event, data) {
                    if (data.id == 'print_internal') {
                        window.open('/Matters/Timesheet_PrintInternal/<%: ViewData["MatterId"] %>?empid=' + encodeURIComponent($('#Employee_Id').val()) + '&from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_allempinternal') {
                        window.open('/Matters/Timesheet_PrintInternal/<%: ViewData["MatterId"] %>?from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_client') {
                        window.open('/Matters/Timesheet_PrintClient/<%: ViewData["MatterId"] %>?empid=' + encodeURIComponent($('#Employee_Id').val()) + '&from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_allempclient') {
                        window.open('/Matters/Timesheet_PrintClient/<%: ViewData["MatterId"] %>?from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_3rdparty') {
                        window.open('/Matters/Timesheet_Print3rdParty/<%: ViewData["MatterId"] %>?empid=' + encodeURIComponent($('#Employee_Id').val()) + '&from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_allemp3rdparty') {
                        window.open('/Matters/Timesheet_Print3rdParty/<%: ViewData["MatterId"] %>?from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    }
                }
            });
        });
    </script>
    

    <div class="options_div" style="height: 22px;">
        <div style="width: 200px; display: inline;">
            From: <input type="text" id="from" name="from" style="width: 75px;" <% if (ViewData["From"] != null ) { %>value="<%: ((DateTime)ViewData["From"]).ToString("MM/dd/yyyy") %>"<% } %> />
        </div>
        <div style="width: 200px; display: inline;">
            To: <input type="text" id="to" name="to" style="width: 75px;" <% if (ViewData["To"] != null ) { %>value="<%: ((DateTime)ViewData["To"]).ToString("MM/dd/yyyy") %>"<% } %> />
        </div>
        <div style="width: 200px; display: inline;">
            Employee:
            <%: Html.DropDownListFor(x => x.Employee.Id,
                new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                new { @size = 1, @style = "width: 200px" })%>
        </div>
        <div style="width: 200px; display: inline; float: right; text-align: right;"> 
            <div id="print_drop" style="text-align: left; display: inline;">
                <div id="print_container" style="display: inline;">
                    <button id="print_launcher" style="background-image: url('/Content/fugue-icons-3.5.6/icons-shadowless/printer.png'); 
                        background-position: left center; background-repeat: no-repeat; padding-left: 20px;">Print</button>
                </div>
                <ul id="print_menu">
                    <div style="font-weight:bold; text-align:center;">Office Views</div>
                    <li id="print_internal"><a href="javascript:void(0);">Single Employee</a></li>
                    <li id="print_allempinternal"><a href="javascript:void(0);">All Employees</a></li>
                    <hr>
                    <div style="font-weight:bold; text-align:center;">Client Views</div>
                    <li id="print_client"><a href="javascript:void(0);">Single Employee</a></li>
                    <li id="print_allempclient"><a href="javascript:void(0);">All Employees</a></li>
                    <hr>
                    <div style="font-weight:bold; text-align:center;">3rd Party Views</div>
                    <li id="print_3rdparty"><a href="javascript:void(0);">Single Employee</a></li>
                    <li id="print_allemp3rdparty"><a href="javascript:void(0);">All Employees</a></li>
                </ul>
            </div>
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
        bool altRow = true;
        double totalMinutes = 0;
        DateTime lastTimestampStart = DateTime.MinValue;// DateTime.Today;
        DateTime lastTimestampStop = DateTime.MinValue;// DateTime.Today;
        
        foreach (var item in Model.Items) {

            altRow = !altRow;
            totalMinutes += item.Time.Duration.TotalMinutes;

            %>
            
            <tr <% 
                if (lastTimestampStop > item.Time.Start) { 
                    %>style="background-color: #FFCECE;"<% 
                } else if (altRow) {
                    %>class="tr_alternate"<% } %> >
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
                <td style="width: 100px;">
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
