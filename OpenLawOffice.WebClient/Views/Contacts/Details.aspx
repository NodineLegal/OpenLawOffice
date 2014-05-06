<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.ContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contact Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Contact Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="detail_table">
        <thead style="font-weight: bold;">
            <tr>
                <td colspan="2">
                    General
                </td>
            </tr>
        </thead>
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Is an Organization
            </td>
            <td class="display-field">
                <%: Model.IsOrganization%>
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
            <td class="display-label">
                Nickname
            </td>
            <td class="display-field">
                <%: Model.Nickname%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Generation
            </td>
            <td class="display-field">
                <%: Model.Generation%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Display Name Prefix
            </td>
            <td class="display-field">
                <%: Model.DisplayNamePrefix %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Display Name
            </td>
            <td class="display-field">
                <%: Model.DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Surname (Last)
            </td>
            <td class="display-field">
                <%: Model.Surname%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Middle Name
            </td>
            <td class="display-field">
                <%: Model.MiddleName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Given (First) Name
            </td>
            <td class="display-field">
                <%: Model.GivenName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Initials
            </td>
            <td class="display-field">
                <%: Model.Initials%>
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
            <td class="display-label">
                Email 1 Display Name
            </td>
            <td class="display-field">
                <%: Model.Email1DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 1 Email Address
            </td>
            <td class="display-field">
                <%: Model.Email1EmailAddress%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 2 Display Name
            </td>
            <td class="display-field">
                <%: Model.Email2DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 2 Email Address
            </td>
            <td class="display-field">
                <%: Model.Email2EmailAddress%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 3 Display Name
            </td>
            <td class="display-field">
                <%: Model.Email3DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Email 3 Email Address
            </td>
            <td class="display-field">
                <%: Model.Email3EmailAddress%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 1 Display Name
            </td>
            <td class="display-field">
                <%: Model.Fax1DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 1 Fax Number
            </td>
            <td class="display-field">
                <%: Model.Fax1FaxNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 2 Display Name
            </td>
            <td class="display-field">
                <%: Model.Fax2DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 2 Fax Number
            </td>
            <td class="display-field">
                <%: Model.Fax2FaxNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 3 Display Name
            </td>
            <td class="display-field">
                <%: Model.Fax3DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Fax 3 Fax Number
            </td>
            <td class="display-field">
                <%: Model.Fax3FaxNumber%>
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
            <td class="display-label">
                Address 1 Display Name
            </td>
            <td class="display-field">
                <%: Model.Address1DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Street
            </td>
            <td class="display-field">
                <%: Model.Address1AddressStreet%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 City
            </td>
            <td class="display-field">
                <%: Model.Address1AddressCity%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 State/Province
            </td>
            <td class="display-field">
                <%: Model.Address1AddressStateOrProvince%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Postal Code
            </td>
            <td class="display-field">
                <%: Model.Address1AddressPostalCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Country
            </td>
            <td class="display-field">
                <%: Model.Address1AddressCountry%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Country Code
            </td>
            <td class="display-field">
                <%: Model.Address1AddressCountryCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 1 Post Office Box
            </td>
            <td class="display-field">
                <%: Model.Address1AddressPostOfficeBox%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Display Name
            </td>
            <td class="display-field">
                <%: Model.Address2DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Street
            </td>
            <td class="display-field">
                <%: Model.Address2AddressStreet%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 City
            </td>
            <td class="display-field">
                <%: Model.Address2AddressCity%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 State/Province
            </td>
            <td class="display-field">
                <%: Model.Address2AddressStateOrProvince%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Postal Code
            </td>
            <td class="display-field">
                <%: Model.Address2AddressPostalCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Country
            </td>
            <td class="display-field">
                <%: Model.Address2AddressCountry%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Country Code
            </td>
            <td class="display-field">
                <%: Model.Address2AddressCountryCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 2 Post Office Box
            </td>
            <td class="display-field">
                <%: Model.Address2AddressPostOfficeBox%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Display Name
            </td>
            <td class="display-field">
                <%: Model.Address3DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Street
            </td>
            <td class="display-field">
                <%: Model.Address3AddressStreet%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 City
            </td>
            <td class="display-field">
                <%: Model.Address3AddressCity%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 State/Province
            </td>
            <td class="display-field">
                <%: Model.Address3AddressStateOrProvince%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Postal Code
            </td>
            <td class="display-field">
                <%: Model.Address3AddressPostalCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Country
            </td>
            <td class="display-field">
                <%: Model.Address3AddressCountry%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Country Code
            </td>
            <td class="display-field">
                <%: Model.Address3AddressCountryCode%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Address 3 Post Office Box
            </td>
            <td class="display-field">
                <%: Model.Address3AddressPostOfficeBox%>
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
            <td class="display-label">
                Telephone 1 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone1DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 1 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone1TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 2 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone2DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 2 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone2TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 3 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone3DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 3 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone3TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 4 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone4DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 4 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone4TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 5 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone5DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 5 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone5TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 6 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone6DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 6 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone6TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 7 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone7DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 7 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone7TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 8 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone8DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 8 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone8TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 9 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone9DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 9 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone9TelephoneNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 10 Display Name
            </td>
            <td class="display-field">
                <%: Model.Telephone10DisplayName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Telephone 10 Telephone Number
            </td>
            <td class="display-field">
                <%: Model.Telephone10TelephoneNumber%>
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
            <td class="display-label">
                Birthday
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy}", Model.Birthday) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Wedding
            </td>
            <td class="display-field">
                <%: string.Format("{0:MMM d, yyyy}", Model.Wedding) %>
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
            <td class="display-label">
                Title
            </td>
            <td class="display-field">
                <%: Model.Title%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Company Name
            </td>
            <td class="display-field">
                <%: Model.CompanyName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Department Name
            </td>
            <td class="display-field">
                <%: Model.DepartmentName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Office Location
            </td>
            <td class="display-field">
                <%: Model.OfficeLocation%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Manager Name
            </td>
            <td class="display-field">
                <%: Model.ManagerName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Assistant Name
            </td>
            <td class="display-field">
                <%: Model.AssistantName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Profession
            </td>
            <td class="display-field">
                <%: Model.Profession%>
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
            <td class="display-label">
                Spouse's Name
            </td>
            <td class="display-field">
                <%: Model.SpouseName%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Language
            </td>
            <td class="display-field">
                <%: Model.Language%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Instant Messaging Address
            </td>
            <td class="display-field">
                <%: Model.InstantMessagingAddress%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Personal Home Page
            </td>
            <td class="display-field">
                <%: Model.PersonalHomePage%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Business Home Page
            </td>
            <td class="display-field">
                <%: Model.BusinessHomePage%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Gender
            </td>
            <td class="display-field">
                <%: Model.Gender%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Referred By
            </td>
            <td class="display-field">
                <%: Model.ReferredByName%>
            </td>
        </tr>
    </table>
    <table class="detail_table">
        <tr>
            <td colspan="5" style="font-weight: bold;">
                Core Details
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Created By
            </td>
            <td class="display-field">
                <%: Model.CreatedBy.Username %>
            </td>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Created At
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcCreated.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Modified By
            </td>
            <td class="display-field">
                <%: Model.ModifiedBy.Username %>
            </td>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Modified At
            </td>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcModified.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Disabled By
            </td>
            <% if (Model.DisabledBy != null)
               { %>
            <td class="display-field">
                <%: Model.DisabledBy.Username%>
            </td>
            <% }
               else
               { %>
            <td />
            <% } %>
            <td style="width: 10px;">
            </td>
            <td class="display-label">
                Disabled At
            </td>
            <% if (Model.UtcDisabled.HasValue)
               { %>
            <td class="display-field">
                <%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcDisabled.Value, DateTimeKind.Utc).ToLocalTime())%>
            </td>
            <% }
               else
               { %>
            <td class="display-field">
            </td>
            <% } %>
        </tr>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the contact.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create") %></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
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