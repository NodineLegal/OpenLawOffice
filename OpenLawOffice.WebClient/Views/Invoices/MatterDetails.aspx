<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.InvoiceViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invoice Details
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
        
    <script>
        $(function () {
            $("#print_launcher").click(function () {
                window.open('/Invoices/MatterPrint/<%: Model.Id %>',
                    'PrintWindow', 'width=1024,height=768,scrollbars=yes');
            });
        });
    </script>
    
    <div class="options_div" style="height: 22px; width: 1200px;">
        <div style="width: 200px; display: inline; float: right; text-align: right;"> 
            <button id="print_launcher" style="background-image: url('/Content/fugue-icons-3.5.6/icons-shadowless/printer.png'); 
                background-position: left center; background-repeat: no-repeat; padding-left: 20px;">Print</button>
        </div>
    </div>

    <div style="width: 1200px; border: 1px solid black; color: Black; padding: 5px;">
        <div style="height: 75px; display: inline-block;">
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

        <div style="display: inline-block; margin-top: 25px; margin-left: 20px; border: 1px solid #c0c0c0; width: 450px;">
            <div style="display: block; background: #dddddd;">Bill To:</div>
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

        <div style="display: inline-block; vertical-align: top; padding-left: 15px;">
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
                <% if (Model.Matter != null && !string.IsNullOrEmpty(Model.Matter.Title))
                   { %>
                <tr>
                    <td style="padding: 0px; text-align: right;">Matter:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Matter.Title%></td>
                </tr>
                <% } 
                   if (Model.Matter != null && !string.IsNullOrEmpty(Model.Matter.Title))
                   { %>
                <tr>
                    <td style="padding: 0px; text-align: right;">Case No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Matter.CaseNumber %></td>
                </tr>
                <% } %>
            </table>
        </div>

        <br />

        <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
            font-size: 14px;">Expenses</div>
        
        <div style="border: none; padding: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%;">
                <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                    <tr>
                        <td style="width: 100px;">
                            Incurred
                        </td>
                        <td>
                            Vendor
                        </td>
                        <td>
                            Details
                        </td>
                        <td style="width: 100px;">
                            Amount
                        </td>  
                    </tr>      
                </thead>
                <tbody>
                <%
                    bool altRow = true;
                    decimal expSum = 0;
                    for (int i=0; i<Model.Expenses.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceExpenseViewModel item = Model.Expenses[i];
                        altRow = !altRow;
                        expSum += item.Amount;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Expense.Incurred.ToShortDateString() %></td>
                        <td><%: item.Expense.Vendor %></td>
                        <td><%: item.Details %></td>
                        <td style="text-align: center;"><%: item.Amount.ToString("C") %></td>
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
            font-size: 14px;">Fees</div>
        
        <div style="border: none; padding: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%;">
                <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                    <tr>
                        <td style="width: 100px;">
                            Incurred
                        </td>
                        <td>
                            Details
                        </td>
                        <td style="width: 100px;">
                            Amount
                        </td>  
                    </tr>      
                </thead>
                <tbody>
                <%
                    altRow = true;
                    decimal feeSum = 0;
                    for (int i=0; i<Model.Fees.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceFeeViewModel item = Model.Fees[i];
                        altRow = !altRow;
                        feeSum += item.Amount;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Fee.Incurred.ToShortDateString() %></td>
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
            font-size: 14px;">Time</div>
        
        <div style="border: none; padding: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%;">
                <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                    <tr>
                        <td style="width: 100px;">
                            Date
                        </td>
                        <td>
                            Details
                        </td>
                        <td style="width: 120px;">
                            Duration (h:m)
                        </td>
                        <td style="width: 100px;">
                            Rate ($/hr.)
                        </td>  
                        <td style="width: 100px;">
                            Amount
                        </td>  
                    </tr>      
                </thead>
                <tbody>
                <%
                    altRow = true;
                    decimal timeSum = 0;
                    for (int i=0; i<Model.Times.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceTimeViewModel item = Model.Times[i];
                        altRow = !altRow;
                        timeSum += (decimal)item.Duration.TotalHours * item.PricePerHour;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Time.Start.ToShortDateString() %></td>
                        <td><%: item.Details %></td>
                        <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Duration, false) %></td>
                        <td style="text-align: center;"><%: item.PricePerHour.ToString("C") %></td>
                        <td style="text-align: center;"><%: string.Format("{0:C}", (decimal)item.Duration.TotalHours * item.PricePerHour) %></td>
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
        
        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px; height: 100px;">
            <table border="0" cellpadding="0" cellspacing="0" style="float: right; border: none;">
                <tr>
                    <td>Subtotal:</td>
                    <td><%: Model.Subtotal.ToString("C") %></td>
                </tr>
                <tr>
                    <td>Tax Amount:</td>
                    <td><%: Model.TaxAmount.ToString("C") %></td>
                </tr>
                <tr style="font-weight: bold;">
                    <td>Total Due:</td>
                    <td><%: Model.Total.ToString("C") %></td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
