<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.BillingGroupViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Billing Group Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Billing Group Details</h2>
    
    <table class="detail_table">    
        <tr>
            <td colspan="4" class="detail_table_heading">
                Financial Information
                <div style="float: right;">[<%: Html.ActionLink("Invoices", "Invoices", "BillingGroups", new { Id = Model.Id }, new { @style = "color: white;" })%>]</div>
            </td>
        </tr>        
        <tr>
            <td class="display-label" style="width: 125px;">
                Title:
            </td>
            <td class="display-field">
                <%: Model.Title %>
            </td>
            <td class="display-label" style="width: 125px;">
                Last Run:
            </td>
            <td class="display-field">
                <% if (Model.LastRun.HasValue) { %>
                    <%: Model.LastRun.Value.ToShortDateString() %> 
                <%} %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Next Run:
            </td>
            <td class="display-field">
                <%: Model.NextRun.ToShortDateString() %>
            </td>
            <td class="display-label" style="width: 125px;">
                Amount:
            </td>
            <td class="display-field">
                <%: Model.Amount.ToString("C") %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Bill To:
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.BillTo.DisplayName, "Details", "Contacts", new { Id = Model.BillTo.Id }, null) %>
            </td>
            <td colspan="2"></td>
        </tr>
    </table>

    <br />
    
    <table class="listing_table">    
        <tr>
            <td colspan="3" class="listing_table_heading">
                Matter Members
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Case Number
            </th>
        </tr>
        <% bool altRow = true;
           foreach (var item in Model.MatterMembers)
           {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Matters", new { id = item.Id.Value }, null) %>
            </td>
            <td>
                <%: item.CaseNumber %>
            </td>
        </tr>
        <% } %>
    </table>

    <br />

    <% Html.RenderPartial("CoreDetailsView"); %>
</asp:Content>
