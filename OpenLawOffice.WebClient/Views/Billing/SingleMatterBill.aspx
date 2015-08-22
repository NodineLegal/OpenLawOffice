<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.InvoiceViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SingleBill
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript" src="/Scripts/moment.min.js"></script>
    <script>
        var vars = [], hash;
        var q = document.URL.split('?')[1];
        if (q != undefined) {
            q = q.split('&');
            for (var i = 0; i < q.length; i++) {
                hash = q[i].split('=');
                vars.push(hash[1]);
                vars[hash[0]] = hash[1];
            }
        }
        $(function () {
            if (vars['RateFrom'] != null)
                $('#RateFrom').val(vars['RateFrom']);
            else {
                <% if (Model.Matter.OverrideMatterRateWithEmployeeRate) { %>
                $('#RateFrom').val('Employee');
                <% } else { %>
                $('#RateFrom').val('Matter');
                <% } %>
            }
            if (vars['StartDate'] != null)
                $('#StartDate').val(vars['StartDate']);
            if (vars['StopDate'] != null)
                $('#StopDate').val(vars['StopDate']);
            $("#Date").datepicker({
                autoSize: true
            });
            $("#Due").datepicker({
                autoSize: true
            });
            $("#BillingGroup_NextRun").datepicker({
                autoSize: true
            });
            $("#StartDate").datepicker({
                autoSize: true
            });
            $("#StopDate").datepicker({
                autoSize: true
            });
            $("#RateFrom").change(function () {
                go();
            });
            $("#StartDate").change(function () {
                go();
            });
            $("#StopDate").change(function () {
                go();
            });
            $("#TaxAmount").change(function () {
                updateTotals();
            });

            var i = 0;
            var obj;
            while ((obj = $("#Expenses_" + i + "__Amount")).length > 0) {
                obj.change(function (data) {
                    $("#expenseTotal").text("$" + sumAllExpenses());
                    updateTotals();
                });
                i++;
            }
            i = 0;
            while ((obj = $("#Fees_" + i + "__Amount")).length > 0) {
                obj.change(function (data) {
                    $("#feeTotal").text("$" + sumAllFees());
                    updateTotals();
                });
                i++;
            }
            i = 0;
            while ((obj = $("#Times_" + i + "__PricePerHour")).length > 0) {
                obj.change(function (data) {
                    $("#timeTotal").text("$" + sumAllTimes());
                    updateTotals();
                });
                i++;
            }
           
        });
        function go() {
            var href;
            var startDate = $('#StartDate').val().trim();
            var stopDate = $('#StopDate').val().trim();
            var base;
            var qMarkAt = window.location.href.lastIndexOf('?');
            if (qMarkAt > 0)
                base = window.location.href.substr(0, qMarkAt);
            else
                base = window.location.href;

            href = base + '?RateFrom=' + $("#RateFrom").val();

            if (startDate.length > 0)
                href += '&StartDate=' + startDate;
            if (stopDate.length > 0)
                href += '&StopDate=' + stopDate;

            window.location.href = href;
        };
        function sumAllExpenses() {
            var sum = 0;
            var i = 0;
            var obj;
            while ((obj = $("#Expenses_" + i + "__Amount")).length > 0) {
                sum += Number(obj.val());
                i++;
            }
            return sum;
        }
        function sumAllFees() {
            var sum = 0;
            var i = 0;
            var obj;
            while ((obj = $("#Fees_" + i + "__Amount")).length > 0) {
                sum += Number(obj.val());
                i++;
            }
            return sum;
        }
        function sumAllTimes() {
            var sum = 0;
            var i = 0;
            var dur, pph;
            while ((dur = $("#Times_" + i + "__Duration")).length > 0) {
                pph = $("#Times_" + i + "__PricePerHour");
                sum += (moment.duration(dur.val()).asMinutes() / 60) * Number(pph.val());
                i++;
            }
            return sum;
        }
        function updateTotals() {
            var taxAmount = Number($("#TaxAmount").val());
            var subtotal = sumAllExpenses() + sumAllFees() + sumAllTimes();
            $("#subtotal").text(subtotal);
            $("#total").text(subtotal + taxAmount);
        }
    </script>
    
    <% using (Html.BeginForm())
       {%>
    <% if (!ViewContext.ViewData.ModelState.IsValid)
       { %>

       <div style="color: #ff0000; font-weight: bold;">Please correct the fields with errors below.  The fields with errors are denoted with red shading.  Once corrected, please click save to again attempt to save the invoice.</div>
       <br />
    <% } %>

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
        <%: Html.HiddenFor(x => x.Matter.Id) %>
        <br />

        <div style="display: inline-block; margin-top: 25px; margin-left: 20px; border: 1px solid #c0c0c0; width: 450px;">
            <div style="display: block; background: #dddddd;">Bill To:</div>
            <%: Html.HiddenFor(x => x.BillTo.Id) %>
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; padding: 0; margin: 0;">
                <tr>
                    <td style="padding: 3px;">Name:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_NameLine1, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">Name 2:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_NameLine2, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">Address:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_AddressLine1, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">Address 2:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_AddressLine2, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">City:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_City, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">State:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_State, new { @style = "width: 100%;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 3px;">Zip:</td>
                    <td style="width: 365px; padding: 3px;"><%: Html.TextBoxFor(x => x.BillTo_Zip, new { @style = "width: 100%;" })%></td>
                </tr>
            </table>
        </div>

        <div style="display: inline-block; vertical-align: top; padding-left: 15px;">
            <table cellpadding="0" cellspacing="0" style="border: none; padding: 0px;">
                <tr>
                    <td style="padding: 0px; text-align: right;">Invoice No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Id %><%: Html.HiddenFor(x => x.Id) %></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">External Invoice No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Html.TextBoxFor(x => x.ExternalInvoiceId, new { @style = "width: 300px;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Invoice Date:</td>
                    <td style="padding: 0 0 0 5px;"><%: Html.TextBoxFor(x => x.Date, new { @Value = Model.Date.ToString("M/d/yyyy"), @style = "width: 300px;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Due Date:</td>
                    <td style="padding: 0 0 0 5px;"><%: Html.TextBoxFor(x => x.Due, new { @Value = Model.Due.ToString("M/d/yyyy"), @style = "width: 300px;" })%></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Matter:</td>
                    <td style="padding: 0 0 0 5px;"><%: ViewData["MatterTitle"] %></td>
                </tr>
                <tr>
                    <td style="padding: 0px; text-align: right;">Case No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: ViewData["CaseNumber"] %></td>
                </tr>
            </table>            
            
            <div style="display: inline-block; margin-top: 25px; margin-left: 20px; border: 1px solid #c0c0c0; width: 450px;">
                <div style="display: block; background: #dddddd;">Billing Options:</div>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; padding: 0; margin: 0;">
                    <tr>
                        <td style="padding: 3px; font-size: 10px;">Rate From:</td>
                        <td style="width: 365px; padding: 3px;">
                            <select id="RateFrom" style="font-size: 10px;">
                                <option value="Employee">Employee</option>
                                <option value="Matter">Matter</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 3px; font-size: 10px;">Start Date:</td>
                        <td style="width: 365px; padding: 3px;"><input type="text" name="StartDate" id="StartDate" style="width: 100px; font-size: 10px;" /></td>
                    </tr>
                    <tr>
                        <td style="padding: 3px; font-size: 10px;">Stop Date:</td>
                        <td style="width: 365px; padding: 3px;"><input type="text" name="StopDate" id="StopDate" style="width: 100px; font-size: 10px;" /></td>
                    </tr>
                </table>
            </div>

        </div>
        
            
        <br />

        <div style="text-align: left; margin: 5px 0 0 0; padding: 2px 0px 2px 5px;
            font-size: 12px; font-weight: bold; border-collapse: collapse;
            border-top-left-radius: 5px; border-top-right-radius: 5px; -moz-border-top-left-radius: 5px;
            -moz-border-top-right-radius: 5px; background: #f5f5f5;">Expenses</div>
        
        <div style="border: none; padding: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
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
                        object o = ViewContext.ViewData.ModelState;
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
                        <td><%: Html.TextBoxFor(x => x.Expenses[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Expenses[i].Amount, new { @style = "width: 75px; font-size: 11px;" })%><%: Html.HiddenFor(x => x.Expenses[i].Expense.Id) %></td>
                    </tr>
                    <% }
                        if (Model.Expenses.Count <= 0)
                        {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                            %>
                            <td colspan="4" style="text-align: center;">
                                No Expenses
                            </td>
                        </tr>
                    <% } 
                        else {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                            %>
                            <td colspan="3" style="text-align: right; font-weight: bold;">
                                Total:
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <span id="expenseTotal"><%: expSum.ToString("C") %></span>
                            </td>
                        </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        
        <br />

        <div style="text-align: left; margin: 5px 0 0 0; padding: 2px 0px 2px 5px;
            font-size: 12px; font-weight: bold; border-collapse: collapse;
            border-top-left-radius: 5px; border-top-right-radius: 5px; -moz-border-top-left-radius: 5px;
            -moz-border-top-right-radius: 5px; background: #f5f5f5;">Fees</div>
        
        <div style="border: none; padding: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;">
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
                    decimal feeSum = 0;
                    altRow = true;
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
                        <td><%: Html.TextBoxFor(x => x.Fees[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Fees[i].Amount, new { @style = "width: 75px; font-size: 11px;" })%><%: Html.HiddenFor(x => x.Fees[i].Fee.Id) %></td>
                    </tr>
                    <% }
                        if (Model.Fees.Count <= 0)
                        {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                        %>
                        <td colspan="3" style="text-align: center;">
                            No Fees
                        </td>
                        </tr>
                    <% } 
                        else {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                            %>
                            <td colspan="2" style="text-align: right; font-weight: bold;">
                                Total:
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <span id="feeTotal"><%: feeSum.ToString("C") %></span>
                            </td>
                        </tr>
                    <% } %>
                </tbody>
            </table>
        </div>

        <br />

        <div style="text-align: left; margin: 5px 0 0 0; padding: 2px 0px 2px 5px;
            font-size: 12px; font-weight: bold; border-collapse: collapse;
            border-top-left-radius: 5px; border-top-right-radius: 5px; -moz-border-top-left-radius: 5px;
            -moz-border-top-right-radius: 5px; background: #f5f5f5;">Time
        </div>
                
        <div style="border: none; padding: 0; margin: 0;">            
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px; margin: 0;">
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
                        <td><%: Html.TextBoxFor(x => x.Times[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                        <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Time.Duration, false) %><%: Html.HiddenFor(x => x.Times[i].Duration) %></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Times[i].PricePerHour, new { @style = "width: 75px; font-size: 11px;" })%><%: Html.HiddenFor(x => x.Times[i].Time.Id) %></td>
                    </tr>
                    <% }
                        if (Model.Times.Count <= 0)
                        {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                        %>
                        <td colspan="4" style="text-align: center;">
                            No Time
                        </td>
                        </tr>
                    <% } 
                        else {
                            altRow = !altRow;
                            if (altRow)
                            { %> <tr class="tr_alternate"> <% }
                            else
                            { %> <tr> <% }
                            %>
                            <td colspan="3" style="text-align: right; font-weight: bold;">
                                Total:
                            </td>
                            <td style="text-align: center; font-weight: bold;">
                                <span id="timeTotal"><%: timeSum.ToString("C") %></span>
                            </td>
                        </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        
        <%
           decimal subtotal = expSum + feeSum + timeSum;
           decimal total = subtotal;
             %>

        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px; height: 100px; font-size: 10px;">
            <table border="0" cellpadding="0" cellspacing="0" style="float: right; border: none;">
                <tr>
                    <td>Subtotal:</td>
                    <td id="subtotal"><%: subtotal.ToString("C") %></td>
                </tr>
                <tr>
                    <td>Tax Amount:</td>
                    <td>$<%: Html.TextBoxFor(x => x.TaxAmount, new { @style = "width: 75px; font-size: 11px;" })%></td>
                </tr>
                <tr style="font-weight: bold;">
                    <td>Total Due:</td>
                    <td id="total"><%: total.ToString("C") %></td>
                </tr>
            </table>
        </div>

        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px;">
            <input type="submit" value="Save" style="width: 100px;" />
        </div>
    </div>

    <% } %>
</asp:Content>
