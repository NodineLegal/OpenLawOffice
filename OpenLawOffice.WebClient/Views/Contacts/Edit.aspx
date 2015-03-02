<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.ContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Contacts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script language="javascript">
        $(function () {
            $("#IsOurEmployee").change(function () {
                if ($(this).is(":checked")) {
                    $("#BillingPane").show();
                } else {
                    $("#BillingPane").hide();
                    $("#BillingRate_Id").val([]);
                }
            });
            <% if (Model.IsOurEmployee) { %>
                $("#BillingPane").show();
            <% } else { %>
                $("#BillingPane").hide();
            <% } %>
        });
    
    </script>

    <h2>
        Edit Contact<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Classification
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label">
                Our Employee
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.IsOurEmployee)%>Check to indicate that this contact is employed by your company giving them rights
                to bill within this system.
                <%: Html.ValidationMessageFor(model => model.IsOurEmployee)%>
                <table class="detail_table" style="margin-top: 5px;" id="BillingPane">
                    <thead style="font-weight: bold;">
                        <tr>
                            <td colspan="2">
                                Billing Details
                            </td>
                        </tr>
                    </thead>                        
                    <tr>
                        <td class="display-label" style="width: 150px;">
                            Billing Rate
                        </td>
                        <td class="display-field">
                        <% if (Model.BillingRate != null)
                           { %>
                            <%: Html.DropDownListFor(x => x.BillingRate.Id,
                                    new SelectList((IList)ViewData["BillingRateList"], "Id", "Title", Model.BillingRate.Id),
                                    new { @size = 5, @style = "width: 100%" })%>
                        <% }
                           else
                           { %>
                            <%: Html.DropDownListFor(x => x.BillingRate.Id,
                                    new SelectList((IList)ViewData["BillingRateList"], "Id", "Title"),
                                    new { @size = 5, @style = "width: 100%" })%>
                        <% } %>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Organization
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.IsOrganization)%>Check to indicate that this contact is for an organization, not an individual.
                <%: Html.ValidationMessageFor(model => model.IsOrganization)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Contact Name
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Nickname
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Nickname, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Nickname)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Generation
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Generation, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Generation)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Display Name Prefix
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.DisplayNamePrefix, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.DisplayNamePrefix)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Display Name<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Surname (Last)
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Surname, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Surname)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Middle Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.MiddleName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.MiddleName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Given (First) Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.GivenName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.GivenName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Initials
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Initials, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Initials)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Electronic Addresses
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Email 1 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email1DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email1DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 1 Email Address
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email1EmailAddress, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email1EmailAddress)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 2 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email2DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email2DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 2 Email Address
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email2EmailAddress, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email2EmailAddress)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 3 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email3DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email3DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 3 Email Address
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Email3EmailAddress, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Email3EmailAddress)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 1 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax1DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax1DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 1 Fax Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax1FaxNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax1FaxNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 2 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax2DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax2DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 2 Fax Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax2FaxNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax2FaxNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 3 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax3DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax3DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 3 Fax Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Fax3FaxNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Fax3FaxNumber)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Physical Addresses
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Address 1 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Street
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressStreet, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressStreet)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 City
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressCity, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressCity)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 State/Province
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressStateOrProvince, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressStateOrProvince)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Postal Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressPostalCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressPostalCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Country
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressCountry, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressCountry)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Country Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressCountryCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressCountryCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Post Office Box
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address1AddressPostOfficeBox, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address1AddressPostOfficeBox)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Street
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressStreet, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressStreet)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 City
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressCity, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressCity)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 State/Province
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressStateOrProvince, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressStateOrProvince)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Postal Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressPostalCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressPostalCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Country
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressCountry, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressCountry)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Country Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressCountryCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressCountryCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Post Office Box
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address2AddressPostOfficeBox, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address2AddressPostOfficeBox)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Street
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressStreet, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressStreet)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 City
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressCity, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressCity)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 State/Province
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressStateOrProvince, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressStateOrProvince)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Postal Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressPostalCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressPostalCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Country
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressCountry, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressCountry)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Country Code
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressCountryCode, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressCountryCode)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Post Office Box
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Address3AddressPostOfficeBox, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Address3AddressPostOfficeBox)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Telephone Numbers
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Telephone 1 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone1DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone1DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 1 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone1TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone1TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 2 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone2DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone2DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 2 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone2TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone2TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 3 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone3DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone3DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 3 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone3TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone3TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 4 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone4DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone4DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 4 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone4TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone4TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 5 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone5DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone5DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 5 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone5TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone5TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 6 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone6DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone6DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 6 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone6TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone6TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 7 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone7DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone7DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 7 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone7TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone7TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 8 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone8DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone8DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 8 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone8TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone8TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 9 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone9DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone9DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 9 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone9TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone9TelephoneNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 10 Display Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone10DisplayName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone10DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 10 Telephone Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Telephone10TelephoneNumber, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Telephone10TelephoneNumber)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Events
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Birthday
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Birthday, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Birthday)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Wedding
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Wedding, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Wedding)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Professional Details
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Title
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Title, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Company Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.CompanyName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.CompanyName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Department Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.DepartmentName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.DepartmentName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Office Location
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.OfficeLocation, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.OfficeLocation)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Manager Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.ManagerName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.ManagerName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Assistant Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.AssistantName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.AssistantName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Profession
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Profession, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Profession)%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    Other Details
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label" style="width: 250px;">
                Spouse's Name
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.SpouseName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.SpouseName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Language
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Language, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Language)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Instant Messaging Address
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.InstantMessagingAddress, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.InstantMessagingAddress)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Personal Home Page
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.PersonalHomePage, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.PersonalHomePage)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Business Home Page
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.BusinessHomePage, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.BusinessHomePage)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Gender
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Gender, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Gender)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Referred By
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.ReferredByName, new { @style = "width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.ReferredByName)%>
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
        Modify the information on this page to make changes to a contact.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> (display name).
        While very little information is required, the more information provided will prove useful later.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.  Its very important that the 
        "Our Employee" checkbox be checked if the contact is an employee of the firm or will be billing time (e.g., of counsel).
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create") %></li>
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Check Conflicts", "Conflicts", new { id = Model.Id }) %></li>
    <li>
        <%: Html.ActionLink("Matters", "Matters", new { ContactId = Model.Id }) %></li>
    <li>
        <%: Html.ActionLink("Tasks", "Tasks", new { ContactId = Model.Id })%></li>
</asp:Content>