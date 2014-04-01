<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.ContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Classification</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Our Employee</td>
                <td class="display-field">
                    Check to indicate that this contact is employed by your company giving them rights to bill within this system.<br />
                    <%: Html.CheckBoxFor(model => model.IsOurEmployee)%>
                    <%: Html.ValidationMessageFor(model => model.IsOurEmployee)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Organization</td>
                <td class="display-field">
                    Check to indicate that this contact is for an organization, not an individual.<br />
                    <%: Html.CheckBoxFor(model => model.IsOrganization)%>
                    <%: Html.ValidationMessageFor(model => model.IsOrganization)%>
                </td>
            </tr>
        </table>

        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Contact Name</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Nickname</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Nickname)%>
                    <%: Html.ValidationMessageFor(model => model.Nickname)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Generation</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Generation)%>
                    <%: Html.ValidationMessageFor(model => model.Generation)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Display Name Prefix</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.DisplayNamePrefix)%>
                    <%: Html.ValidationMessageFor(model => model.DisplayNamePrefix)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Surname (Last)</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Surname)%>
                    <%: Html.ValidationMessageFor(model => model.Surname)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Middle Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.MiddleName)%>
                    <%: Html.ValidationMessageFor(model => model.MiddleName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Given (First) Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.GivenName)%>
                    <%: Html.ValidationMessageFor(model => model.GivenName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Initials</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Initials)%>
                    <%: Html.ValidationMessageFor(model => model.Initials)%>
                </td>
            </tr>
        </table>

        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Electronic Addresses</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Email 1 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email1DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Email1DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Email 1 Email Address</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email1EmailAddress)%>
                    <%: Html.ValidationMessageFor(model => model.Email1EmailAddress)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Email 2 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email2DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Email2DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Email 2 Email Address</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email2EmailAddress)%>
                    <%: Html.ValidationMessageFor(model => model.Email2EmailAddress)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Email 3 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email3DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Email3DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Email 3 Email Address</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Email3EmailAddress)%>
                    <%: Html.ValidationMessageFor(model => model.Email3EmailAddress)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 1 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax1DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Fax1DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 1 Fax Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax1FaxNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Fax1FaxNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 2 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax2DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Fax2DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 2 Fax Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax2FaxNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Fax2FaxNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 3 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax3DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Fax3DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Fax 3 Fax Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Fax3FaxNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Fax3FaxNumber)%>
                </td>
            </tr>
        </table>

        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Physical Addresses</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Address 1 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Address1DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 Street</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressStreet)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressStreet)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 City</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressCity)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressCity)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 State/Province</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressStateOrProvince)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressStateOrProvince)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 Postal Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressPostalCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressPostalCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 Country</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressCountry)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressCountry)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 Country Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressCountryCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressCountryCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 1 Post Office Box</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address1AddressPostOfficeBox)%>
                    <%: Html.ValidationMessageFor(model => model.Address1AddressPostOfficeBox)%>
                </td>
            </tr>        
            <tr>
                <td class="display-label">Address 2 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Address2DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 Street</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressStreet)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressStreet)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 City</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressCity)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressCity)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 State/Province</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressStateOrProvince)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressStateOrProvince)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 Postal Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressPostalCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressPostalCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 Country</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressCountry)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressCountry)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 Country Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressCountryCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressCountryCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 2 Post Office Box</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address2AddressPostOfficeBox)%>
                    <%: Html.ValidationMessageFor(model => model.Address2AddressPostOfficeBox)%>
                </td>
            </tr>        
            <tr>
                <td class="display-label">Address 3 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Address3DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 Street</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressStreet)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressStreet)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 City</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressCity)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressCity)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 State/Province</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressStateOrProvince)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressStateOrProvince)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 Postal Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressPostalCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressPostalCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 Country</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressCountry)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressCountry)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 Country Code</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressCountryCode)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressCountryCode)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Address 3 Post Office Box</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Address3AddressPostOfficeBox)%>
                    <%: Html.ValidationMessageFor(model => model.Address3AddressPostOfficeBox)%>
                </td>
            </tr>
        </table>

        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Telephone Numbers</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Telephone 1 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone1DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone1DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 1 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone1TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone1TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 2 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone2DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone2DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 2 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone2TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone2TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 3 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone3DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone3DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 3 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone3TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone3TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 4 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone4DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone4DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 4 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone4TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone4TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 5 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone5DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone5DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 5 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone5TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone5TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 6 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone6DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone6DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 6 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone6TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone6TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 7 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone7DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone7DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 7 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone7TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone7TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 8 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone8DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone8DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 8 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone8TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone8TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 9 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone9DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone9DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 9 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone9TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone9TelephoneNumber)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 10 Display Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone10DisplayName)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone10DisplayName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Telephone 10 Telephone Number</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Telephone10TelephoneNumber)%>
                    <%: Html.ValidationMessageFor(model => model.Telephone10TelephoneNumber)%>
                </td>
            </tr>
        </table>
    
        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Events</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Birthday</td>
                <td class="display-field">
                    <%: Html.EditorFor(model => model.Birthday)%>
                    <%: Html.ValidationMessageFor(model => model.Birthday)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Wedding</td>
                <td class="display-field">
                    <%: Html.EditorFor(model => model.Wedding)%>
                    <%: Html.ValidationMessageFor(model => model.Wedding)%>
                </td>
            </tr>
        </table>
    
        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Professional Details</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Title</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Title)%>
                    <%: Html.ValidationMessageFor(model => model.Title)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Company Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.CompanyName)%>
                    <%: Html.ValidationMessageFor(model => model.CompanyName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Department Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.DepartmentName)%>
                    <%: Html.ValidationMessageFor(model => model.DepartmentName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Office Location</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.OfficeLocation)%>
                    <%: Html.ValidationMessageFor(model => model.OfficeLocation)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Manager Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.ManagerName)%>
                    <%: Html.ValidationMessageFor(model => model.ManagerName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Assistant Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.AssistantName)%>
                    <%: Html.ValidationMessageFor(model => model.AssistantName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Profession</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Profession)%>
                    <%: Html.ValidationMessageFor(model => model.Profession)%>
                </td>
            </tr>
        </table>
    
        <table class="detail_table">
            <thead style="font-weight: bold;">
                <tr>
                    <td colspan="2">Other Details</td>
                </tr>
            </thead>
            <tr>
                <td class="display-label">Spouse's Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.SpouseName)%>
                    <%: Html.ValidationMessageFor(model => model.SpouseName)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Language</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Language)%>
                    <%: Html.ValidationMessageFor(model => model.Language)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Instant Messaging Address</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.InstantMessagingAddress)%>
                    <%: Html.ValidationMessageFor(model => model.InstantMessagingAddress)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Personal Home Page</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.PersonalHomePage)%>
                    <%: Html.ValidationMessageFor(model => model.PersonalHomePage)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Business Home Page</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.BusinessHomePage)%>
                    <%: Html.ValidationMessageFor(model => model.BusinessHomePage)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Gender</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Gender)%>
                    <%: Html.ValidationMessageFor(model => model.Gender)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Referred By</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.ReferredByName)%>
                    <%: Html.ValidationMessageFor(model => model.ReferredByName)%>
                </td>
            </tr>
        </table>

        <p>
            <input type="submit" value="Save" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

