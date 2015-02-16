<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Billing.BillingGroupViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Billing Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript">
        $(document).ready(function () {
            $('#BillTo_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#BillTo_Id").val(ui.item.Id);
                    $("#BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#BillTo_Id").val(ui.item.Id);
                    $("#BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.DisplayName + "</a>")
                .appendTo(ul);
            };
        });
    </script>

    <div id="roadmap">
        <div id="current" class="zero">Create Billing Group<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td colspan="3" class="listing_table_heading">
                Billing Rate Details
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Title) %>
                <%: Html.ValidationMessageFor(model => model.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Next Run<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.NextRun)%>
                <%: Html.ValidationMessageFor(model => model.NextRun)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Amount<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Amount)%>
                <%: Html.ValidationMessageFor(model => model.Amount)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Bill To<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.BillTo.Id) %>
                <%: Html.TextBoxFor(model => model.BillTo.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.BillTo.DisplayName)%>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Fill in the information on this page to create a new billing group.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
