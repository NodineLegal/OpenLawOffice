<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Contacts.ContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contacts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Contacts<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                <%: Html.ActionLink(item.DisplayName, "Details", new { id = item.Id })%>
            </td>
            <td>
                <%: item.Address1AddressCity + ", " + item.Address1AddressStateOrProvince %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Conflict Check", "Conflicts", new { id = item.Id }, new { @class = "btn-conflictcheck", title = "Conflicts Check" })%>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Contacts are the people (individuals or organizations) that do things or have things done to or for them.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the contact.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/exclamation-shield.png" /> 
        (conflicts check icon) to show potential conflicts.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the contact.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create") %></li>
    </ul>
</asp:Content>