<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Tasks.TaskAssignedContactViewModel>>" %>

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
                Assignment
            </th>
            <th style="width: 175px;"></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Contact.DisplayName %>
            </td>
            <td>
                <%: item.AssignmentType %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "TaskAssignedContacts", new { id = item.Id }, null) %> |
                <%: Html.ActionLink("Details", "Details", "TaskAssignedContacts", new { id = item.Id }, null)%> |
                <%: Html.ActionLink("Unassign", "Delete", "TaskAssignedContacts", new { id = item.Id }, null)%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Contact", "Create", "TaskAssignedContacts", new { TaskId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Assign Contact", "SelectContactToAssign", "TaskAssignedContacts", new { id = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Task", "Details", "Tasks", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>