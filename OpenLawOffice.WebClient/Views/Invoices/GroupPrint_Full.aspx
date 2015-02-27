<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.GroupInvoiceViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Timesheet</title>
    <style>
    body
    {
        font-size: 8pt;
        font-family: Verdana, Helvetica, Sans-Serif;
    }
    @media print 
    {
        .page-break { display: block; page-break-after: always; }
    }
    </style>
</head>
<body style="background: white; margin: 5px; width: 511pt;">
    
    <div style="border: none; color: Black; padding: 5px;">
        <div style="height: 75px; display: inline-block; font-size: 10pt;">
            <span style="font-weight: bold;"><%: ViewData["FirmName"] %></span><br />
            <%: ViewData["FirmAddress"] %><br />
            <%: ViewData["FirmCity"] %>, <%: ViewData["FirmState"] %> <%: ViewData["FirmZip"] %><br />
            Phone: <%: ViewData["FirmPhone"] %><br />
            Web: <%: ViewData["FirmWeb"] %>
        </div>

        <div style="float: right; font-weight: normal; font-size: 32px; display: inline-block;">
            Invoice
        </div>
        
        <br />

        <div style="display: inline-block; margin-top: 25px; margin-left: 5px; border: 1px solid #c0c0c0; width: 185pt;">
            <div style="display: block; background: #dddddd; font-size: 10pt;">Bill To:</div>
            <div><%: Model.BillTo_NameLine1 %></div>
            <% if (!string.IsNullOrEmpty(Model.BillTo_NameLine2))
                { %>
            <div><%: Model.BillTo_NameLine2 %></div>            
            <% } %>
            <div><%: Model.BillTo_AddressLine1%></div>
            <% if (!string.IsNullOrEmpty(Model.BillTo_AddressLine2))
                { %>
            <div><%: Model.BillTo_AddressLine2%></div>            
            <% } %>
            <div><%: Model.BillTo_City%>, <%: Model.BillTo_State%> <%: Model.BillTo_Zip %></div>
        </div>

        <div style="display: inline-block; vertical-align: top; padding-left: 15px; width: 290pt;">
            <table cellpadding="0" cellspacing="0" style="border: none; padding: 0px;">
                <tr>
                    <td style="padding: 0px; text-align: right;">Invoice No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Id %></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">External Invoice No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.ExternalInvoiceId %></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Invoice Date:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Date.ToString("M/d/yyyy") %></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Due Date:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Due.ToString("M/d/yyyy") %></td>
                </tr>
            </table>
        </div>

        <br />

        
        <div style="border: 1px solid gray; margin: 10px 0 10px 0;">

            <div style="text-align: center; display: inline-block; width: 100%; font-weight: bold;">
                Summary
            </div>

            
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
                    <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                        <tr>
                            <td>
                                Matter
                            </td>
                            <td style="width: 100px;">
                                Case No.
                            </td>
                            <td style="width: 80px;">
                                Expenses
                            </td>
                            <td style="width: 80px;">
                                Fees
                            </td>
                            <td style="width: 100px;">
                                Time
                            </td>
                            <td style="width: 80px;">
                                Amount
                            </td>  
                        </tr>      
                    </thead>
                    <tbody>
                    <%
                    bool altRow = true;
                    decimal expTotal = 0, feeTotal = 0, timeTotalMoney = 0;
                    TimeSpan timeTotal = new TimeSpan();
                    for (int i = 0; i < Model.Matters.Count; i++)
                    {
                        expTotal += Model.Matters[i].ExpensesSum;
                        feeTotal += Model.Matters[i].FeesSum;
                        timeTotal = timeTotal.Add(Model.Matters[i].TimeSum);
                        timeTotalMoney += Model.Matters[i].TimeSumMoney;
                        altRow = !altRow;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                            %>
                            <td><%: Model.Matters[i].Matter.Title %></td>
                            <td style="text-align: center;"><%: Model.Matters[i].Matter.CaseNumber %></td>
                            <td style="text-align: center;"><%: Model.Matters[i].ExpensesSum.ToString("C") %></td>
                            <td style="text-align: center;"><%: Model.Matters[i].FeesSum.ToString("C") %></td>
                            <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(Model.Matters[i].TimeSum, false) %> (<%: Model.Matters[i].TimeSumMoney.ToString("C") %>)</td>
                            <td style="text-align: center;">
                                <%: string.Format("{0:C}", Model.Matters[i].ExpensesSum + Model.Matters[i].FeesSum + Model.Matters[i].TimeSumMoney) %>
                            </td>
                        </tr>
                        <% }
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% } %>
                            <td colspan="2" style="text-align: right; font-weight: bold;">
                                Total:
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <%: expTotal.ToString("C") %>
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <%: feeTotal.ToString("C") %>
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <%: TimeSpanHelper.TimeSpan(timeTotal, false) %> (<%: timeTotalMoney.ToString("C") %>)
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <%: string.Format("{0:C}", expTotal + feeTotal + timeTotalMoney) %>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        
        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px; height: 100px;">
            <table border="0" cellpadding="0" cellspacing="0" style="float: right; border: none;">
                <tr>
                    <td>Subtotal:</td>
                    <td><%: Model.Subtotal.ToString("C") %></td>
                </tr>
                <tr>
                    <td>Base:</td>
                    <td><%: Model.BillingGroup.Amount.ToString("C") %></td>
                </tr>
                <tr>
                    <td>Tax Amount:</td>
                    <td><%: Model.TaxAmount.ToString("C") %></td>
                </tr>
                <tr style="font-weight: bold;">
                    <td>Total Due:</td>
                    <td style="padding-left: 10px;"><%: Model.Total.ToString("C") %></td>
                </tr>
            </table>
        </div>
        
        <div class="page-break"></div>

        <% for (int j = 0; j < Model.Matters.Count; j++)
           { %>
               
        <div style="border: 1px solid gray; margin: 10px 0 10px 0;">

            <div style="text-align: center; display: inline-block; width: 100%; font-weight: bold;">
                <%: Model.Matters[j].Matter.Title%><br />
                <%: Model.Matters[j].Matter.CaseNumber%>
            </div>

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 10px;">Expenses</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
                    <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                        <tr>
                            <td style="width: 80px;">
                                Incurred
                            </td>
                            <td>
                                Vendor
                            </td>
                            <td>
                                Details
                            </td>
                            <td style="width: 80px;">
                                Amount
                            </td>  
                        </tr>      
                    </thead>
                    <tbody>
                    <%
            altRow = true;
            decimal expSum = 0;
            for (int i = 0; i < Model.Matters[j].Expenses.Count; i++)
            {
                OpenLawOffice.WebClient.ViewModels.Billing.InvoiceExpenseViewModel item = Model.Matters[j].Expenses[i];
                altRow = !altRow;
                expSum += item.Amount;
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% }
                    %>
                    <td><%: item.Expense.Incurred.ToShortDateString()%></td>
                    <td><%: item.Expense.Vendor%></td>
                    <td><%: item.Details%></td>
                    <td style="text-align: center;"><%: item.Amount.ToString("C")%></td>
                </tr>
                <% }
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% } %>
                    <td colspan="3" style="text-align: right; font-weight: bold;">
                        Total:
                    </td>
                    <td style="text-align: center; font-weight: bold;">
                        <%: expSum.ToString("C") %>
                    </td>
                </tr>
                    </tbody>
                </table>
            </div>
        
            <br />

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 10px;">Fees</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
                    <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                        <tr>
                            <td style="width: 80px;">
                                Incurred
                            </td>
                            <td>
                                Details
                            </td>
                            <td style="width: 80px;">
                                Amount
                            </td>  
                        </tr>      
                    </thead>
                    <tbody>
                    <%
            altRow = true;
            decimal feeSum = 0;
            for (int i = 0; i < Model.Matters[j].Fees.Count; i++)
            {
                OpenLawOffice.WebClient.ViewModels.Billing.InvoiceFeeViewModel item = Model.Matters[j].Fees[i];
                altRow = !altRow;
                feeSum += item.Amount;
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% }
                    %>
                    <td><%: item.Fee.Incurred.ToShortDateString()%></td>
                    <td><%: item.Details %></td>
                    <td style="text-align: center;"><%: item.Amount.ToString("C") %></td>
                </tr>
        <% } 
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% } %>
                    <td colspan="2" style="text-align: right; font-weight: bold;">
                        Total:
                    </td>
                    <td style="text-align: center; font-weight: bold;">
                        <%: feeSum.ToString("C") %>
                    </td>
                </tr>
                    </tbody>
                </table>
            </div>

            <br />

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 10px;">Time</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
                    <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                        <tr>
                            <td style="width: 80px;">
                                Date
                            </td>
                            <td>
                                Details
                            </td>
                            <td style="width: 100px;">
                                Duration (h:m)
                            </td>
                            <td style="width: 80px;">
                                Rate ($/hr.)
                            </td>  
                            <td style="width: 80px;">
                                Amount
                            </td>  
                        </tr>      
                    </thead>
                    <tbody>
                    <%
            altRow = true;
            decimal timeSum = 0;
            for (int i = 0; i < Model.Matters[j].Times.Count; i++)
            {
                OpenLawOffice.WebClient.ViewModels.Billing.InvoiceTimeViewModel item = Model.Matters[j].Times[i];
                altRow = !altRow;
                timeSum += (decimal)item.Duration.TotalHours * item.PricePerHour;
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% }
                    %>
                    <td><%: item.Time.Start.ToShortDateString()%></td>
                    <td><%: item.Details%></td>
                    <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Duration, false)%></td>
                    <td style="text-align: center;"><%: item.PricePerHour.ToString("C")%></td>
                    <td style="text-align: center;"><%: string.Format("{0:C}", (decimal)item.Duration.TotalHours * item.PricePerHour)%></td>
                </tr>
                <% }
                if (altRow)
                { %> <tr class="tr_alternate"> <% }
                else
                { %> <tr> <% } %>
                    <td colspan="4" style="text-align: right; font-weight: bold;">
                        Total:
                    </td>
                    <td style="text-align: center; font-weight: bold;">
                        <%: timeSum.ToString("C") %>
                    </td>
                </tr>
                    </tbody>
                </table>
            </div>
            
        </div>

        <% if (j + 1 < Model.Matters.Count)
           { %>
        <div class="page-break"></div>
        <% } %>

        <% } %>

    </div>
</body>
</html>
