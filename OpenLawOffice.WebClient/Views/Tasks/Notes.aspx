<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Notes.NoteViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Notes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Notes</h2>

    <table>
        <tr>
            <th>
                Title
            </th>
            <th>
                Body
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Title %>
            </td>
            <td>
            <% if (item.Body.Length > 100)
               {
            %>
                <%: item.Body.Substring(0, 100)%>...
            <% }
               else
               { %>
                <%: item.Body%>
                <% } %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Notes", new { id = item.Id.Value }, null) %> |
                <%: Html.ActionLink("Details", "Details", "Notes", new { id = item.Id.Value }, null)%>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Note", "Create", "Notes", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Task", "Details", "Tasks", new { Id  = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul> 
</asp:Content>
