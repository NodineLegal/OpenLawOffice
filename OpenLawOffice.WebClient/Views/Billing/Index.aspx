<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.BillingViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Billing
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Billing<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <div style="width: 70px; padding-bottom: 5px;">
        <a id="SelectAll" href="javascript:void(0);">Select All</a><br />
        <a id="DeselectAll" href="javascript:void(0);">Deselect All</a>
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

        $("#SelectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', true);
            }
        });

        $("#DeselectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', false);
            }
        });
    });
</script>
    </div>

    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="listing_table">
        <tr>
            <th style="text-align: center; width: 20px;" />
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
            <td>
                <input type="checkbox" id="CB_<%: item.Matter.Id.Value %>" name="CB_<%: item.Matter.Id.Value %>" />
            </td>
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

    <input type="submit" value="Bill all Selected" style="margin-top: 5px;" />
    
    <% } %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This area is used to bill clients.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        
        </p>
    </div>
</asp:Content>
