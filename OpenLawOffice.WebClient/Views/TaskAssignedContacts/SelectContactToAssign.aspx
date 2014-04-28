<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Contacts.SelectableContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Select Contact for Task
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Select Contact for Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                City, State
            </th>
            <th style="width: 20px;">
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
                <%: Html.ActionLink("Assign", "AssignContact", new { id = item.Id, TaskId = RouteData.Values["Id"].ToString() }, new { @class = "btn-assigncontact", title = "Assign Contact" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page allows a contact to be assigned to a task.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        To assign a contact to a task, click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/user-arrow.png" /> (assign contact icon)
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("List", "Contacts", "Tasks", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>