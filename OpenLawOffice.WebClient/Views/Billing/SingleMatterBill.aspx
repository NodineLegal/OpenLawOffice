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
            <%: Model.BillTo.DisplayName %><br />
            <%: Model.BillTo.Address1AddressStreet %><br />
            <% if (!string.IsNullOrEmpty(Model.BillTo.Address1AddressPostOfficeBox))
               { %>
            PO Box <%: Model.BillTo.Address1AddressPostOfficeBox%><br />
            <% } %>
            <%: Model.BillTo.Address1AddressCity %>, <%: Model.BillTo.Address1AddressStateOrProvince %> <%: Model.BillTo.Address1AddressPostalCode %>
        </div>

        <div style="display: inline-block; vertical-align: top; padding-left: 15px;">
            <table cellpadding="0" cellspacing="0" style="border: none; padding: 0px;">
                <tr>
                    <td style="padding: 0px; text-align: right;">Invoice No.:</td>
                    <td style="padding: 0 0 0 5px;"><%: Model.Id %></td>
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

        <div style="border: 1px solid #c0c0c0; margin-top: 5px; padding: 0;">
            <table cellpadding="0" cellspacing="0" style="border: none; width: 100%;">
                <thead style="background: #dddddd; text-align: center; font-weight: bold;">
                    <td>
                        Date
                    </td>
                    <td>
                        Details
                    </td>
                    <td>
                        Duration (h:m)
                    </td>
                    <td>
                        Rate ($/hr.)
                    </td>        
                </thead>
                <tbody>
                <%
                    bool altRow = true;
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
                        <td><%: item.Details %></td>
                        <td style="text-align: center;"><%: TimeSpanHelper.TimeSpan(item.Time.Duration, false) %></td>
                        <td style="text-align: center;">$<%: Html.TextBoxFor(x => x.Times[i].PricePerUnit, new { @style = "width: 75px;" })%></td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
