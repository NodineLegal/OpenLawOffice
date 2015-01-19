<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.TimesheetsViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Timesheets for <%: Model.Contact.DisplayName %></title>
    <style>
    body
    {
        font-size: 10pt;
        font-family: Verdana, Helvetica, Sans-Serif;
    }
    @media print 
    {
        .page-break { display: block; page-break-after: always; }
    }
    </style>
</head>
<body style="background: white; margin: 5px; width: 511pt;">
    <div style="margin: 0 0 5px 0;">
    
    <%--Header--%>
    <div style="font-weight: bold; text-align: center;">Timesheets for <%: Model.Contact.DisplayName %></div>
    <div style="font-weight: bold; text-align: center;">Date Range: 
        <% if (ViewData["From"] != null)
        { %><%: ((DateTime)ViewData["From"]).ToString("MM/dd/yyyy")%><% }
        else
        { %>*<% } %>
        -
        <% if (ViewData["To"] != null)
        { %><%: ((DateTime)ViewData["To"]).ToString("MM/dd/yyyy")%><% }
        else
        { %>*<% } %></div>
    </div>

    <%--Matter Loop--%>

    <%
        bool onePrinted = false;
for (int i=0; i<Model.Matters.Count; i++)
{
    OpenLawOffice.WebClient.ViewModels.Contacts.TimesheetsViewModel.MatterTimeList matter = Model.Matters[i];

    if (matter.Times.Count > 0)
    {
        onePrinted = true;
    %>
   
    <div>Matter: <%: matter.Matter.Title%></div>
    <% if (!string.IsNullOrEmpty(matter.Matter.Jurisdiction))
       { %><div>Jurisdiction: <%: matter.Matter.Jurisdiction%></div><% } %>   
    <% if (!string.IsNullOrEmpty(matter.Matter.CaseNumber))
       { %><div>Case Number: <%: matter.Matter.CaseNumber%></div><% } %>  
    <br />
    <table style="font-size: 8pt;border: 1px solid black; width: 511pt;">
        <tr>
            <th>
                Duration
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

       foreach (var item in matter.Times)
       {
           altRow = !altRow;
           totalMinutes += item.Time.Duration.TotalMinutes;

            %>
            
            <tr <% 
                if (altRow) {
                    %>style="background-color: #f5f5f5;"<% } %> >
                <td style="width: 100px; text-align: center;">
                    <%: item.Time.Duration.ToString(@"h\:mm")%>
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
            <td colspan="1" style="text-align: right; font-weight: bold;">
                Total Time:
            </td>
            <td style="text-align: left; font-weight: bold;">
                <%: TimeSpan.FromMinutes(Math.Round(totalMinutes, 0)).ToString(@"h\:mm")%>
            </td>
            <td colspan="4">
            </td>
        </tr>
    </table>
    <br />
    <% if (i < Model.Matters.Count - 1)
       { %>
        <div class="page-break"></div>
    <% }
    }
}
        if (!onePrinted)
        {    
%>
    <div style="text-align: center;"><br />There are no time entries within this timeframe for this contact</div>
<% } %>




    </div>
</body>
</html>
