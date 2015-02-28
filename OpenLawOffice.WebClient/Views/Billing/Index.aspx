<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.BillingViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Billing
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Billing<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="listing_table"> 
        <tr>
            <td colspan="10" class="listing_table_heading">
                Billing Groups
            </td>
        </tr>
        <tr>
            <%--<th style="text-align: center; width: 20px;" />--%>
            <th style="text-align: center;">Title</th>
            <th style="text-align: center;">Bill To</th>
            <th style="text-align: center;">Last Run</th>
            <th style="text-align: center;">Next Run</th>
            <th style="text-align: center;">Amount</th>
            <th style="text-align: center;">Expenses</th>
            <th style="text-align: center;">Fees</th>
            <th style="text-align: center;">Time</th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% 
           foreach (var item in Model.GroupItems)
           { %>
        <tr>
            <%--<td>
                <input type="checkbox" id="Checkbox1" name="CB_<%: item.BillingGroup.Id.Value %>" />
            </td>--%>
            <td>
                <%: Html.ActionLink(item.BillingGroup.Title, "Details", "BillingGroups", new { Id = item.BillingGroup.Id }, null) %>
            </td>
            <td>
                <%: Html.ActionLink(item.BillingGroup.BillTo.DisplayName, "Details", "Contacts", new { Id = item.BillingGroup.BillTo.Id }, null)%>
            </td>
            <td>
                <% if (item.BillingGroup.LastRun.HasValue)
                   { %>
                    <%: item.BillingGroup.LastRun.Value.ToShortDateString() %>
                <% } %>
            </td>
            <td>
                <%: item.BillingGroup.NextRun.ToShortDateString()%>
            </td>
            <td>
                <%: string.Format("{0:C}", item.BillingGroup.Amount) %>
            </td>
            <td>
                <%: string.Format("{0:C}", item.Expenses) %>
            </td>
            <td>
                <%: string.Format("{0:C}", item.Fees) %>
            </td>
            <td>
                <%: TimeSpanHelper.TimeSpan(item.Time, false) %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Bill", "SingleGroupBill", "Billing", new { id = item.BillingGroup.Id }, new { @class = "btn-money-arrow", title = "Bill" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div style="height: 50px;"></div>

    <table class="listing_table"> 
        <tr>
            <td colspan="8" class="listing_table_heading">
                Individual Matters
            </td>
        </tr>
        <tr>
           <%-- <th style="text-align: center; width: 20px;" />--%>
            <th style="text-align: center;">Bill To</th>
            <th style="text-align: center;">Matter</th>
            <th style="text-align: center;">Case No.</th>
            <th style="text-align: center;">Expenses</th>
            <th style="text-align: center;">Fees</th>
            <th style="text-align: center;">Time</th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% 
           foreach (var item in Model.Items)
           { %>
        <tr>
            <%--<td>
                <input type="checkbox" id="CB_<%: item.Matter.Id.Value %>" name="CB_<%: item.Matter.Id.Value %>" />
            </td>--%>
            <td>
                <%: Html.ActionLink(item.BillTo.DisplayName, "Details", "Contacts", new { Id = item.BillTo.Id }, null) %>
            </td>
            <td>
                <%: Html.ActionLink(item.Matter.Title, "Details", "Matters", new { Id = item.Matter.Id }, null) %>
            </td>
            <td>
                <%: item.Matter.CaseNumber %>
            </td>
            <td>
                <%: string.Format("{0:C}", item.Expenses) %>
            </td>
            <td>
                <%: string.Format("{0:C}", item.Fees) %>
            </td>
            <td>
                <%: TimeSpanHelper.TimeSpan(item.Time, false) %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Bill", "SingleMatterBill", "Billing", new { id = item.Matter.Id }, new { @class = "btn-money-arrow", title = "Bill" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div style="height: 50px;"></div>

    <table class="listing_table"> 
        <tr>
            <td colspan="8" class="listing_table_heading">
                Recently Created Invoices
            </td>
        </tr>
        <tr>
           <%-- <th style="text-align: center; width: 20px;" />--%>
            <th style="text-align: center;">Bill To</th>
            <th style="text-align: center;">Matter/Group</th>
            <th style="text-align: center;">Case No.</th>
            <th style="text-align: center;">Total</th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% 
           foreach (var item in Model.RecentInvoices)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.BillTo.DisplayName, "Details", "Contacts", new { Id = item.BillTo.Id }, null) %>
            </td>
            <td>
                <% if (item.Matter != null)
                   { %>
                    <%: Html.ActionLink(item.Matter.Title, "Details", "Matters", new { Id = item.Matter.Id }, null)%>
                <% }
                   else
                   { %>
                    <%: Html.ActionLink(item.BillingGroup.Title, "Details", "BillingGroups", new { Id = item.BillingGroup.Id }, null)%>
                <% } %>
            </td>
            <td>
                <% if (item.Matter != null) { %>
                    <%: item.Matter.CaseNumber %>
                <% } %>
            </td>
            <td>
                <%: string.Format("{0:C}", item.Total) %>
            </td>
            <td style="text-align: center;">
                <% if (item.Matter != null)
                   { %>
                    <%: Html.ActionLink("View", "MatterDetails", "Invoices", new { id = item.Id }, new { @class = "btn-money-arrow", title = "Bill" })%>
                <% }
                   else
                   { %>
                    <%: Html.ActionLink("View", "GroupDetails", "Invoices", new { id = item.Id }, new { @class = "btn-money-arrow", title = "Bill" })%>
                <% } %>
            </td>
        </tr>
        <% } %>
    </table>
    
    <% } %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This area is used to bill clients.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        
        </p>
    </div>
</asp:Content>
