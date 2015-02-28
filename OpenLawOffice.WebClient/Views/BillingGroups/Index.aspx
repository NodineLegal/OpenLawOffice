<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Billing.BillingGroupViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Billing Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Billing Groups<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Last Run
            </th>
            <th style="text-align: center;">
                Next Run
            </th>
            <th style="text-align: center;">
                Amount
            </th>
            <th style="text-align: center;">
                Bill To
            </th>
            <th style="text-align: center; width: 25px;">
                
            </th>
        </tr>
        <% bool altRow = true; 
           foreach (var item in Model)
           { 
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "BillingGroups", new { Id = item.Id }, null) %>
            </td>
            <td style="text-align: center;">
                <% if (item.LastRun.HasValue)
                   { %>
                    <%: item.LastRun.Value.ToShortDateString()%>
                <% } %>
            </td>
            <td style="text-align: center;">
                <%: item.NextRun.ToShortDateString()%>
            </td>
            <td style="text-align: center;">
                <%: item.Amount.ToString("C") %>
            </td>
            <td>
                <%: Html.ActionLink(item.BillTo.DisplayName, "Details", "Contacts", new { id = item.BillTo.Id }, null) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "BillingGroups", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        A billing rate is applied to a time entry to be used in an invoice.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make changes to the billing rate.
        </p>
    </div>
</asp:Content>
