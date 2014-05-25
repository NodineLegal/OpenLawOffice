<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Contacts.ContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact Selection for Calendaring
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>View Calendar for:<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                City, State
            </th>
            <th style="width: 40px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.DisplayName, "Contact", "Calendar", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: item.Address1AddressCity + ", " + item.Address1AddressStateOrProvince %>
            </td>
        </tr>
        <% } %>
    </table>    
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        To view a contact's calendar, click that contact's name.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the contact's name will show a calendar for that contact.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
