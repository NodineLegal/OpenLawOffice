<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.GroupInvoiceViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SingleGroupBill
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
            $("#BillingGroup_NextRun").datepicker({
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
                    <td style="padding: 0px; text-align: right;">Next Run:</td>
                    <td style="padding: 0 0 0 5px;"><%: Html.TextBoxFor(x => x.BillingGroup.NextRun, new { @Value = Model.BillingGroup.NextRun.ToString("M/d/yyyy"), @style = "width: 300px;" })%></td>
                </tr>
            </table>
        </div>

        <br />
        

        <% for (int j = 0; j < Model.Matters.Count; j++)
            { %>
               
        <div style="border: 1px solid gray; margin: 10px 0 10px 0;">

            <div style="text-align: center; display: inline-block; width: 100%; font-weight: bold;">
                <%: Model.Matters[j].Matter.Title%><br />
                <%: Model.Matters[j].Matter.CaseNumber%>
            </div>

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 12px;">Expenses</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;"">
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
                for (int i = 0; i < Model.Matters[j].Expenses.Count; i++)
                {
                    OpenLawOffice.WebClient.ViewModels.Billing.InvoiceExpenseViewModel item = Model.Matters[j].Expenses[i];
                    altRow = !altRow;
                    if (altRow)
                    { %> <tr class="tr_alternate"> <% }
                    else
                    { %> <tr> <% }
                            %>
                            <td><%: item.Expense.Incurred.ToShortDateString()%></td>
                            <td><%: item.Expense.Vendor%></td>
                            <td><%: Html.TextBoxFor(x => x.Matters[j].Expenses[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                            <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Matters[j].Expenses[i].Amount, new { @style = "width: 75px; font-size: 11px;" })%><%: Html.HiddenFor(x => x.Matters[j].Expenses[i].Expense.Id)%></td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>
        
            <br />

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 12px;">Fees</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;"">
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
                for (int i = 0; i < Model.Matters[j].Fees.Count; i++)
                {
                    OpenLawOffice.WebClient.ViewModels.Billing.InvoiceFeeViewModel item = Model.Matters[j].Fees[i];
                    altRow = !altRow;
                    if (altRow)
                    { %> <tr class="tr_alternate"> <% }
                    else
                    { %> <tr> <% }
                            %>
                            <td><%: item.Fee.Incurred.ToShortDateString()%></td>
                            <td><%: Html.TextBoxFor(x => x.Matters[j].Fees[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                            <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Matters[j].Fees[i].Amount, new { @style = "width: 75px; font-size: 11px;" })%><%: Html.HiddenFor(x => x.Matters[j].Fees[i].Fee.Id)%></td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>

            <br />

            <div style="width: 100%; text-align: left; margin: 5px 0 5px 0; 
                font-size: 12px;">Time</div>
        
            <div style="border: none; padding: 0;">            
                <table cellpadding="0" cellspacing="0" style="border: none; width: 100%; font-size: 10px;"">
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
                        </tr>      
                    </thead>
                    <tbody>
                    <%
                altRow = true;
                for (int i = 0; i < Model.Matters[j].Times.Count; i++)
                {
                    OpenLawOffice.WebClient.ViewModels.Billing.InvoiceTimeViewModel item = Model.Matters[j].Times[i];
                    altRow = !altRow;
                    if (altRow)
                    { %> <tr class="tr_alternate"> <% }
                    else
                    { %> <tr> <% }
                            %>
                            <td><%: item.Time.Start.ToShortDateString()%></td>
                            <td><%: Html.TextBoxFor(x => x.Matters[j].Times[i].Details, new { @style = "width: 100%; font-size: 11px;" })%></td>
                            <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Time.Duration, false)%><%: Html.HiddenFor(x => x.Matters[j].Times[i].Duration)%>
                                <%: Html.HiddenFor(x => x.Matters[j].Times[i].Time.Id)%></td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>
    
        </div>
        
        <% } %>


        <div style="display: block; text-align: right; padding-top: 20px; padding-right: 20px;">
            Contract Amount: $<%: Html.TextBoxFor(x => Model.BillingGroup.Amount, new { @style = "width: 75px;" })%>
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
