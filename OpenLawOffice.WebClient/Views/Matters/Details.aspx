<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>" %>
<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matter Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter", "Create") %></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
       <%-- <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>--%>
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Tags", "Tags", new { id = Model.Id })%>
        (<%: Html.ActionLink("Add", "Create", "MatterTags", new { id = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", new { id = Model.Id })%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", new { id = Model.Id })%></li>
    <li>
        <%: Html.ActionLink("Tasks", "Tasks", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Tasks", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Form Fields", "FormFields", "Matters", new { id = Model.Id }, null)%></li>
   <%-- <li>
        <%: Html.ActionLink("Events", "Events", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Events", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>--%>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Documents", "Documents", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Documents", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Times by Task", "Time", "Matters", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Timesheet", "Timesheet", "Matters", new { id = Model.Id }, null)%></li>
    <%--<li>
        <%: Html.ActionLink("Permissions", "Acls", "Matters")%></li>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="roadmap">
        <div id="current" class="zero">Matter: <%: Model.Title %><a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
            
    <% using (Html.BeginForm("Close", "Matters", new { Id = RouteData.Values["Id"] }, FormMethod.Post, new { id = "CloseForm" }))
       {%>

    <% if (Model.Active || ViewData["AlertText"] != null)
       { %>
    <div class="options_div" style="text-align: right;">
    <% if (Model.Active)
       { %>
        <div style="width: 200px; display: inline;">  
            <input type="submit" value="Close" 
                style="background-image: url('/Content/fugue-icons-3.5.6/icons-shadowless/cross.png'); 
                background-position: left center; background-repeat: no-repeat; padding-left: 20px;" />
        </div>
        
    <% }
       if (ViewData["AlertText"] != null)
       { %>
        <div style="width: 200px; display: inline;">
            <a class="btn-alert" title="Alerts" id="alertInfo" style="padding-left: 15px;"></a>
            <div id="alertInfoDialog" title="Alerts">
                <p>
                <span style="font-weight: bold; text-decoration: underline;">Alerts:</span>
                <%= ViewData["AlertText"]%>
                </p>
            </div>  
            <script language="javascript">
                $(function () {
                    $("#alertInfoDialog").dialog({
                        autoOpen: false,
                        width: 400,
                        show: {
                            effect: "blind",
                            duration: 100
                        },
                        hide: {
                            effect: "fade",
                            duration: 100
                        }
                    });

                    $("#alertInfo").click(function () {
                        $("#alertInfoDialog").dialog("open");
                    });
                });
            </script>
        </div>
    <% } %>
    </div>
    
            <% }
       }%>
    <table class="detail_table">    
        <tr>
            <td colspan="5" class="detail_table_heading">
                Court Information
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Jurisdiction
            </td>
            <td class="display-field">
                <%: Model.Jurisdiction %>
            </td>
            <td></td>
            <td class="display-label" style="width: 125px;">
                Case Number
            </td>
            <td class="display-field">
                <%: Model.CaseNumber %>
            </td>
        </tr>
    </table>

    <table class="detail_table">  
        <tr>
            <td colspan="5" class="detail_table_heading">
                Matter Details
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Lead Attorney
            </td>
            <td class="display-field">
                <% if (Model.LeadAttorney != null)
                   { %>
                    <%: Html.ActionLink(Model.LeadAttorney.DisplayName, "Details", "Contacts", new { Id = Model.LeadAttorney.Id }, null) %>
                <% } %>
            </td>
            <td>
            </td>
            <td class="display-label" style="width: 125px;">
                Bill To
            </td>
            <td class="display-field">
                <% if (Model.BillTo != null)
                   { %>
                    <%: Html.ActionLink(Model.BillTo.DisplayName, "Details", "Contacts", new { Id = Model.BillTo.Id }, null) %>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Default Billing Rate:
            </td>
            <td class="display-field">
                <% if (Model.DefaultBillingRate != null)
                   { %>
                    <%: Model.DefaultBillingRate.Title %> (<%: Model.DefaultBillingRate.PricePerUnit.ToString("C") %>)
                <% } %>
                Employee Rate Override: 
                <% if (Model.OverrideMatterRateWithEmployeeRate)
                   { %>
                    On
                <% }
                   else
                   { %>
                    Off
                <% } %>
            </td>
            <td>
            </td>
            <td class="display-label" style="width: 125px;">
                Billing Group:
            </td>
            <td class="display-field">
                <% if (Model.BillingGroup != null)
                   { %>
                    <%: Html.ActionLink(Model.BillingGroup.Title, "Details", "BillingGroups", new { Id = Model.BillingGroup.Id }, null) %>
                        (Next Bill: <%: Model.BillingGroup.NextRun.ToShortDateString()%>)
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Matter Type:
            </td>
            <td class="display-field">
                <% if (Model.MatterType != null)
                   { %>
                    <%: Model.MatterType.Title%>
                <% } %>
            </td>
            <td></td>
            <td class="display-label" style="width: 125px;">
                Client(s):
            </td>
            <td class="display-field">
                <%
                foreach (var item in Model.Clients)
                {
                    if (item != null && item.Id.HasValue)
                    { %>
                        <%: Html.ActionLink(item.DisplayName, "Details", "Contacts", new { Id = item.Id }, new { id = "link_" + item.Id.Value })%>
                    <div id="Contact_<%: item.Id.Value %>" title="Contact Details">
                        <div>Phone: <%: item.Telephone1TelephoneNumber %></div>
                        <div>Email: <%: item.Email1EmailAddress %></div>
                        <div>
                            <div style="display:inline-block; vertical-align: top;">Address:</div>
                            <div style="display:inline-block; padding: 0px; margin-left: 5px;">
                                <div><%: item.Address1AddressStreet %></div>
                                <div>
                                    <% if (item.Address1AddressPostOfficeBox != null) { %>
                                    PO Box <%: item.Address1AddressPostOfficeBox %>
                                    <% } %>
                                    <%: item.Address1AddressCity %>, <%: item.Address1AddressStateOrProvince %> <%: item.Address1AddressPostalCode %>
                                </div>
                            </div>
                        </div>
                    </div>  
                    <script language="javascript">
                        $(function () {
                            $("#Contact_<%: item.Id.Value %>").dialog({
                                autoOpen: false,
                                width: 400,
                                show: {
                                    effect: "clip",
                                    duration: 100
                                },
                                hide: {
                                    effect: "fade",
                                    duration: 100
                                }
                            });

                            $("#link_<%: item.Id.Value %>").hoverIntent(function () {
                                $("#Contact_<%: item.Id.Value %>").dialog("open");
                            }, function () {
                                $("#Contact_<%: item.Id.Value %>").dialog("close");
                            });
                        });
                    </script>
                    <% }
                }
                %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Synopsis:
            </td>
            <td class="display-field" colspan="4">
                <%: Model.Synopsis %>
            </td>
        </tr>
    </table>
    
    <table class="detail_table">    
        <tr>
            <td colspan="4" class="detail_table_heading">
                Financial Information
                <div style="float: right;">[
                <% if (Model.BillingGroup == null)
                   { %>
                    <%: Html.ActionLink("Invoices", "Invoices", "Matters", new { Id = Model.Id }, new { @style = "color: white;" })%>
                <% }
                   else
                   { %>
                    <%: Html.ActionLink("Invoices", "Invoices", "BillingGroups", new { Id = Model.BillingGroup.Id }, new { @style = "color: white;" })%>
                <% } %>]
                </div>
            </td>
        </tr>

        <!-- 
        
        Minumum Charge:                 Maximum Charge:
        Nonbillable Time:               Expenses:
        Billed:                         Billable:
        Total Value:                    Effective Hourly Rate:

        Expenses: Billed, Unbilled, Total
        Fees: Billed, Unbilled, Total
        Time: Billed (h:m), Unbilled (h:m), Total (h:m)
        
        -->
        
        <tr>
            <td class="display-label" style="width: 125px;">
                Minimum Charge:
            </td>
            <td class="display-field">
                <% if (Model.MinimumCharge.HasValue) { %>
                    <%: Model.MinimumCharge.Value.ToString("C") %> 
                <%} %>
            </td>
            <td class="display-label" style="width: 125px;">
                Estimated Charge:
            </td>
            <td class="display-field">
                <% if (Model.EstimatedCharge.HasValue) { %>
                    <%: Model.EstimatedCharge.Value.ToString("C") %> 
                <%} %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Maximum Charge:
            </td>
            <td class="display-field">
                <% if (Model.MaximumCharge.HasValue) { %>
                    <%: Model.MaximumCharge.Value.ToString("C") %> 
                <%} %>
            </td>
            <td class="display-label" style="width: 125px;">
                Nonbillable Time:
            </td>
            <td class="display-field">
                <%: TimeSpanHelper.TimeSpan(ViewData["NonBillableTime"], false) %>
            </td>
        </tr>
        <tr>
            <td class="display-label" style="width: 125px;">
                Total Value:
            </td>
            <td class="display-field">
                <%: ViewData["TotalValue"] %>
            </td>
            <td class="display-label" style="width: 125px;">
                Eff. Hourly Rate:
            </td>
            <td class="display-field">
                <% if (ViewData["EffHourlyRate"].ToString() != "NaN")
                   { %><%: ViewData["EffHourlyRate"]%><% } %>
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                
            </th>
            <th style="text-align: center;">
                Billed
            </th>
            <th style="text-align: center;">
                Unbilled & Billable
            </th>
            <th style="text-align: center;">
                Total
            </th>
        </tr>
        <tr>
            <td style="font-weight: bold;">
                <%: Html.ActionLink("Expenses", "Expenses", "Matters", new { id = Model.Id }, null) %>
                <%: Html.ActionLink("New Expense", "Create", "Expenses", new { MatterId = Model.Id }, new { @class = "btn-plus" })%>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["ExpensesBilled"]) %>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["ExpensesUnbilled"])%>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["Expenses"])%>
            </td>
        </tr>
        <tr class="tr_alternate">
            <td style="font-weight: bold;">
                <%: Html.ActionLink("Fees", "Fees", "Matters", new { id = Model.Id }, null) %>
                <%: Html.ActionLink("New Fee", "Create", "Fees", new { MatterId = Model.Id }, new { @class = "btn-plus" })%>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["FeesBilled"])%>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["FeesUnbilled"])%>
            </td>
            <td style="text-align: center;">
                <%: string.Format("{0:C}", ViewData["Fees"])%>
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">
                Time
            </td>
            <td style="text-align: center;">
                <%: ViewData["TimeBilled"]%>
            </td>
            <td style="text-align: center;">
                <%: ViewData["TimeUnbilled"]%>
            </td>
            <td style="text-align: center;">
                <%: ViewData["TotalValue"]%>
            </td>
        </tr>
    </table>

    <table class="listing_table">    
        <tr>
            <td colspan="3" class="listing_table_heading">
                Active Tasks
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center; width: 170px; padding-right: 10px;">
                Due Date
            </th>
            <th style="text-align: center; width: 45px;">
                
            </th>
        </tr>
        <% bool altRow = true;
           foreach (var item in Model.Tasks)
           {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Tasks", new { id = item.Id.Value }, null) %>
            </td>
            <td>
                <%: item.DueDate %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Tasks", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Close", "Close", "Tasks", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Close" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <br />
    
    <table class="listing_table">    
        <tr>
            <td colspan="3" class="listing_table_heading">
                Matter Notes
            </td>
        </tr> 
        <tr>
            <th style="text-align: center; width: 170px;">
                Date/Time
            </th>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center; width: 20px;">
                
            </th>
        </tr>
        <% altRow = true; 
           foreach (var item in Model.Notes)
           {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: item.Timestamp %>
            </td>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Notes", new { id = item.Id.Value }, null) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Notes", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <br />
    
    <table class="listing_table">    
        <tr>
            <td colspan="4" class="listing_table_heading">
                Task Notes
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Task
            </th>
            <th style="text-align: center; width: 170px;">
                Date/Time
            </th>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center; width: 20px;">
                
            </th>
        </tr>
        <% altRow = true; 
           foreach (var item in Model.TaskNotes)
           {
               foreach (var note in item.Value)
               {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Key.Title, "Details", "Tasks", new { id = item.Key.Id.Value }, null)%>
            </td>
            <td>
                <%: note.Timestamp %>
            </td>
            <td>
                <%: Html.ActionLink(note.Title, "Details", "Notes", new { id = note.Id.Value }, null)%>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Notes", new { id = note.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <%     }
           }%>
    </table>
    
    <br />
    
    <% Html.RenderPartial("CoreDetailsView"); %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information about the matter.
        </p>
    </div>
</asp:Content>