<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.Common.Models.Matters.Matter>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Matter", "Create") %></li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="float:left;">Matters</h2>

    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="width: 150px;text-align: center;">
                Created
            </th>
            <th style="width: 150px;text-align: center;">
                Modified
            </th>
            <th style="width: 150px;"></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Title %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.UtcCreated) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.UtcModified) %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id = item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
        </tr>
    
    <% } %>

    </table>


</asp:Content>

