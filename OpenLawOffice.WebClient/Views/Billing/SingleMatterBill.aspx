<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.InvoiceViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SingleBill
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(function () {
            $("#Date").datepicker({
                autoSize: true
            });
            $("#Due").datepicker({
                autoSize: true
            });
        });
    </script>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>

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
                    for (int i=0; i<Model.Expenses.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceExpenseViewModel item = Model.Expenses[i];
                        altRow = !altRow;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Expense.Incurred.ToShortDateString() %></td>
                        <td><%: item.Expense.Vendor %></td>
                        <td><%: Html.TextBoxFor(x => x.Expenses[i].Details, new { @style = "width: 100%;" }) %></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Expenses[i].Amount, new { @style = "width: 75px;" })%><%: Html.HiddenFor(x => x.Expenses[i].Expense.Id) %></td>
                    </tr>
                    <% } %>
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
                    for (int i=0; i<Model.Fees.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceFeeViewModel item = Model.Fees[i];
                        altRow = !altRow;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Fee.Incurred.ToShortDateString() %></td>
                        <td><%: Html.TextBoxFor(x => x.Fees[i].Details, new { @style = "width: 100%;" })%></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Fees[i].Amount, new { @style = "width: 75px;" })%><%: Html.HiddenFor(x => x.Fees[i].Fee.Id) %></td>
                    </tr>
                    <% } %>
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
                    </tr>      
                </thead>
                <tbody>
                <%
                    altRow = true;
                    for (int i=0; i<Model.Times.Count; i++)
                    {
                        OpenLawOffice.WebClient.ViewModels.Billing.InvoiceTimeViewModel item = Model.Times[i];
                        altRow = !altRow;
                        if (altRow)
                        { %> <tr class="tr_alternate"> <% }
                        else
                        { %> <tr> <% }
                        %>
                        <td><%: item.Time.Start.ToShortDateString() %></td>
                        <td><%: Html.TextBoxFor(x => x.Times[i].Details, new { @style = "width: 100%;" })%></td>
                        <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Time.Duration, false) %><%: Html.HiddenFor(x => x.Times[i].Duration) %></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Times[i].PricePerHour, new { @style = "width: 75px;" })%><%: Html.HiddenFor(x => x.Times[i].Time.Id) %></td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        
        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px;">
            Tax Amount: $<%: Html.TextBoxFor(x => x.TaxAmount, new { @style = "width: 75px;" }) %>
        </div>
        
        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px;">
            <input type="submit" value="Save" style="width: 100px;" />
        </div>
    </div>

    <% } %>
</asp:Content>
