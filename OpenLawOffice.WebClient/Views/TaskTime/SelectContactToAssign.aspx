<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Contacts.SelectableContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Select Contact for Time Entry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Select Contact for Time Entry<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <p>
        Select the person that did the work.</p>
    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                City, State
            </th>
            <th style="width: 150px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.DisplayName, "Details", "Contacts", new { id = item.Id }, null)%>
            </td>
            <td>
                <%: item.Address1AddressCity + ", " + item.Address1AddressStateOrProvince %>
            </td>
            <td>
                <%: Html.ActionLink("Select", "Create", "TaskTime", new { ContactId = item.Id, TaskId = Request["TaskId"] }, new { @class = "btn-assigncontact", title = "Assign Contact" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a contact to be assigned to a time entry.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Click the contact to show details of the contact.
        To assign a contact to a time entry, click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/user-arrow.png" /> (assign contact icon)
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create", "Contacts")%></li>
    </ul>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = Request["TaskId"] }, null) %></li>
    <li><%: Html.ActionLink("Time for Task", "Time", "Tasks", new { id = Request["TaskId"] }, null)%></li>
</asp:Content>