<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Matters.MatterContactViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contacts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Contacts</h2>
    
    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                Role
            </th>
            <th style="width: 175px;"></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Contact.DisplayName %>
            </td>
            <td>
                <%: item.Role %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "MatterContact", new { id = item.Id }, null) %> |
                <%: Html.ActionLink("Details", "Details", "MatterContact", new { id = item.Id }, null)%> |
                <%: Html.ActionLink("Unassign", "Delete", "MatterContact", new { id = item.Id }, null)%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Contact", "Create", "Contacts", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Assign Contact", "SelectContactToAssign", "MatterContact", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>

