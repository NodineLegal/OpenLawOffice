<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Timing.DayViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Timesheet</title>
    <style>
    body
    {
        font-size: 10pt;
        font-family: Verdana, Helvetica, Sans-Serif;
    }
    </style>
</head>
<body style="background: white; margin: 5px; width: 511pt;">
    <div style="margin: 0 0 5px 0;">
    
    <div>Matter: <%: ViewData["Matter"] %></div>
    <% if (ViewData["Jurisdiction"] != null)
       { %><div>Jurisdiction: <%: ViewData["Jurisdiction"]%></div><% } %>    
    <% if (ViewData["CaseNumber"] != null)
       { %><div>Case Number: <%: ViewData["CaseNumber"]%></div><% } %>
    <% if (Model.Employee != null)
       { %><div>Worker: <%: Model.Employee.DisplayName%></div><% } %>
    <div>Date Range: <% if (ViewData["From"] != null)
                       { %><%: ((DateTime)ViewData["From"]).ToString("MM/dd/yyyy")%><% }
                       else
                       { %>*<% } %>
                       -
                       <% if (ViewData["To"] != null)
                       { %><%: ((DateTime)ViewData["To"]).ToString("MM/dd/yyyy")%><% }
                       else
                       { %>*<% } %></div>
    </div>

    
    <table style="font-size: 8pt;border: 1px solid black; width: 511pt;">
        <tr>
            <th>
                Duration
            </th>
            <th>
                Task
            </th>
            <th>
                Details
            </th>
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
                if (altRow) {
                    %>style="background-color: #f5f5f5;"<% } %> >
                <td style="width: 50px; text-align: center;">
                    <%: item.Time.Duration.ToString(@"h\:mm") %>
                </td>
                <td>
                    <%: item.Task.Title %>
                </td>
                <td>
                    <%: item.Time.Details%>
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
                <%: TimeSpan.FromMinutes(Math.Round(totalMinutes, 0)).ToString(@"h\:mm") %>
            </td>
            <td colspan="4">
            </td>
        </tr>
    </table>
</body>
</html>
