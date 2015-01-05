<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.CreateMatterViewModel>" %>

<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Matter
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">    
    <script language="javascript">
        $(document).ready(function () {
            $('#Matter_BillTo_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#Matter_BillTo_Id").val(ui.item.Id);
                    $("#Matter_BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#Matter_BillTo_Id").val(ui.item.Id);
                    $("#Matter_BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.DisplayName + "</a>")
                .appendTo(ul);
            };
            $('#Contact1_DisplayName').focus(function () {
                $("#Contact1_Id").val('');
                $('#Contact1_DisplayName').val('');
            });
            $('#Contact1_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#Contact1_Id").val(ui.item.Id);
                    $("#Contact1_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#Contact1_Id").val(ui.item.Id);
                    $("#Contact1_DisplayName").val(ui.item.DisplayName);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.DisplayName + "</a>")
                .appendTo(ul);
            };
            $('#Contact2_DisplayName').focus(function () {
                $("#Contact2_Id").val('');
                $('#Contact2_DisplayName').val('');
            });
            $('#Contact2_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#Contact2_Id").val(ui.item.Id);
                    $("#Contact2_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#Contact2_Id").val(ui.item.Id);
                    $("#Contact2_DisplayName").val(ui.item.DisplayName);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.DisplayName + "</a>")
                .appendTo(ul);
            };
            $('#Contact3_DisplayName').focus(function () {
                $("#Contact3_Id").val('');
                $('#Contact3_DisplayName').val('');
            });
            $('#Contact3_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#Contact3_Id").val(ui.item.Id);
                    $("#Contact3_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#Contact3_Id").val(ui.item.Id);
                    $("#Contact3_DisplayName").val(ui.item.DisplayName);
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
        <div id="current" class="zero">Create Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td colspan="3" class="listing_table_heading">
                Matter Details
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.Title) %>
                <%: Html.ValidationMessageFor(model => model.Matter.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Synopsis<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextAreaFor(model => model.Matter.Synopsis, new { style = "height: 50px; width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Matter.Synopsis)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Jurisdiction
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.Jurisdiction) %>
                <%: Html.ValidationMessageFor(model => model.Matter.Jurisdiction)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Case Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.CaseNumber)%>
                <%: Html.ValidationMessageFor(model => model.Matter.CaseNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Lead Attorney<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.ValidationMessageFor(model => model.LeadAttorney)%>
                <%: Html.DropDownListFor(x => x.LeadAttorney.Contact.Id,
                        new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Active<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Matter.Active, new { Checked = true })%>
                Uncheck if the matter is already completed
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Responsible User
            </td>
            <td class="display-field">
                <%: Html.DropDownListFor(x => x.ResponsibleUser.User.PId,
                        new SelectList((IList)ViewData["UserList"], "PId", "Username"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Responsiblity<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.ResponsibleUser.Responsibility) %>
                <%: Html.ValidationMessageFor(model => model.ResponsibleUser.Responsibility)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Bill To<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Matter.BillTo.Id) %>
                <%: Html.TextBoxFor(model => model.Matter.BillTo.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.Matter.BillTo.DisplayName)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <tr>
            <td colspan="5" class="listing_table_heading">
                Assign Contacts
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Contact1.Id)%>
                <%: Html.TextBoxFor(model => model.Contact1.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.Contact1)%>
            </td>
            <td style="width: 25px;"></td>
            <td class="display-label">
                Role
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Role1) %>
                <%: Html.ValidationMessageFor(model => model.Role1)%><br />
                Special Roles:  <a href="#" id="Attorney1">Attorney</a>, 
                                <a href="#" id="OpposingAttorney1">Opposing Attorney</a>, 
                                <a href="#" id="Client1">Client</a>, 
                                <a href="#" id="AppointedClient1">Appointed Client</a>, 
                                <a href="#" id="OpposingParty1">Opposing Party</a>
                <script language="javascript">
                    $("#LeadAttorney1").click(function () {
                        $("#Role1").val("Lead Attorney");
                        return false;
                    });
                    $("#Attorney1").click(function () {
                        $("#Role1").val("Attorney");
                        return false;
                    });
                    $("#OpposingAttorney1").click(function () {
                        $("#Role1").val("Opposing Attorney");
                        return false;
                    });
                    $("#Client1").click(function () {
                        $("#Role1").val("Client");
                        return false;
                    });
                    $("#ThirdPartyPayor1").click(function () {
                        $("#Role1").val("Third-Party Payor");
                        return false;
                    });
                    $("#AppointedClient1").click(function () {
                        $("#Role1").val("Appointed Client");
                        return false;
                    });
                    $("#OpposingParty1").click(function () {
                        $("#Role1").val("Opposing Party");
                        return false;
                    });
                </script>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Contact2.Id)%>
                <%: Html.TextBoxFor(model => model.Contact2.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.Contact2)%>
            </td>
            <td style="width: 25px;"></td>
            <td class="display-label">
                Role
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Role2) %>
                <%: Html.ValidationMessageFor(model => model.Role2)%><br />
                Special Roles:  <a href="#" id="Attorney2">Attorney</a>, 
                                <a href="#" id="OpposingAttorney2">Opposing Attorney</a>, 
                                <a href="#" id="Client2">Client</a>, 
                                <a href="#" id="AppointedClient2">Appointed Client</a>, 
                                <a href="#" id="OpposingParty2">Opposing Party</a>
                <script language="javascript">
                    $("#LeadAttorney2").click(function () {
                        $("#Role2").val("Lead Attorney");
                        return false;
                    });
                    $("#Attorney2").click(function () {
                        $("#Role2").val("Attorney");
                        return false;
                    });
                    $("#OpposingAttorney2").click(function () {
                        $("#Role2").val("Opposing Attorney");
                        return false;
                    });
                    $("#Client2").click(function () {
                        $("#Role2").val("Client");
                        return false;
                    });
                    $("#ThirdPartyPayor2").click(function () {
                        $("#Role2").val("Third-Party Payor");
                        return false;
                    });
                    $("#AppointedClient2").click(function () {
                        $("#Role2").val("Appointed Client");
                        return false;
                    });
                    $("#OpposingParty2").click(function () {
                        $("#Role2").val("Opposing Party");
                        return false;
                    });
                </script>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Contact3.Id)%>
                <%: Html.TextBoxFor(model => model.Contact3.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.Contact3)%>
            </td>
            <td style="width: 25px;"></td>
            <td class="display-label">
                Role
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Role3) %>
                <%: Html.ValidationMessageFor(model => model.Role3)%><br />
                Special Roles:  <a href="#" id="Attorney3">Attorney</a>, 
                                <a href="#" id="OpposingAttorney3">Opposing Attorney</a>, 
                                <a href="#" id="Client3">Client</a>, 
                                <a href="#" id="AppointedClient3">Appointed Client</a>, 
                                <a href="#" id="OpposingParty3">Opposing Party</a>
                <script language="javascript">
                    $("#LeadAttorney3").click(function () {
                        $("#Role3").val("Lead Attorney");
                        return false;
                    });
                    $("#Attorney3").click(function () {
                        $("#Role3").val("Attorney");
                        return false;
                    });
                    $("#OpposingAttorney3").click(function () {
                        $("#Role3").val("Opposing Attorney");
                        return false;
                    });
                    $("#Client3").click(function () {
                        $("#Role3").val("Client");
                        return false;
                    });
                    $("#ThirdPartyPayor3").click(function () {
                        $("#Role3").val("Third-Party Payor");
                        return false;
                    });
                    $("#AppointedClient3").click(function () {
                        $("#Role3").val("Appointed Client");
                        return false;
                    });
                    $("#OpposingParty3").click(function () {
                        $("#Role3").val("Opposing Party");
                        return false;
                    });
                </script>
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
        Fill in the information on this page to create a new matter.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Select a "parent" matter to make this matter be a "submatter" of another matter.  To deselect a parent matter, click "clear". 
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>