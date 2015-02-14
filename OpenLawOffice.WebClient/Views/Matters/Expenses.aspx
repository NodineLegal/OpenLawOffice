<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Billing.ExpenseViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matter Expenses
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Matter Expenses<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    
    <script language="javascript">
    $(function () {
        $("#add").click(function () {
            window.location = "/Expenses/Create?MatterId=<%: ViewData["MatterId"] %>";
        });
    });
    </script>

    <div class="options_div" style="text-align: right;">
        <div style="width: 200px; display: inline;">  
            <input type="button" value="Add" id="add" name="add"
                style="background-image: url('/Content/fugue-icons-3.5.6/icons-shadowless/plus.png'); 
                background-position: left center; background-repeat: no-repeat; padding-left: 20px;" />
        </div>
    </div>

    <table class="listing_table">
        <tr>
            <th>
                Incurred
            </th>
            <th>
                Paid
            </th>
            <th>
                Vendor
            </th>
            <th>
                Amount
            </th>
            <th>
                Details
            </th>
            <th style="width: 25px;">
            </th>
        </tr>
        <% bool altRow = true; 
           foreach (var item in Model)
           {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% } %>
            <td>
                 <%: String.Format("{0:g}", item.Incurred) %>
            </td>
            <td>
                 <%: String.Format("{0:g}", item.Paid) %>
            </td>
            <td>
                <%: item.Vendor %>
            </td>
            <td>
                 <%: String.Format("{0:C}", item.Amount) %>
            </td>
            <td>
                <%: item.Details %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Expenses", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        An expense allows for tracking of expenses incurred throughout the course of the matter which may
        then be billed to the client at that time or a later date.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the expense.
        </p>
    </div>
</asp:Content>

