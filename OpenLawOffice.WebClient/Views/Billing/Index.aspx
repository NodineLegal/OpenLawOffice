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
                <%: item.BillingGroup.Title %>
            </td>
            <td>
                <%: item.BillingGroup.BillTo.DisplayName %>
            </td>
            <td>
                <% if (item.BillingGroup.LastRun.HasValue)
                   { %>
                    <%: item.BillingGroup.LastRun.Value.ToShortDateString() %>
                <% } %>
            </td>
            <td>
                <% if (item.BillingGroup.NextRun.HasValue)
                   { %>
                    <%: item.BillingGroup.NextRun.Value.ToShortDateString()%>
                <% } %>
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

    <%--
    <div style="width: 70px; padding-bottom: 5px; margin-top: 20px;">
        <a id="SelectAll_Matters" href="javascript:void(0);">Select All</a><br />
        <a id="DeselectAll_Matters" href="javascript:void(0);">Deselect All</a>
<script language="javascript">

    $(function () {    
        var cbs = [];

        <% 
        foreach (var item in Model.Items)
        { %>
            <%= "cbs.push('" + item.Matter.Id.Value + "');" %>
          <%
        }
        %>

        $("#SelectAll_Matters").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', true);
            }
        });

        $("#DeselectAll_Matters").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', false);
            }
        });
    });
</script>
    </div>--%>

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
                <%: item.BillTo.DisplayName %>
            </td>
            <td>
                <%: item.Matter.Title %>
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
<%--
    <input type="submit" value="Bill all Selected" style="margin-top: 5px;" />--%>
    
    <% } %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This area is used to bill clients.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        
        </p>
    </div>
</asp:Content>
