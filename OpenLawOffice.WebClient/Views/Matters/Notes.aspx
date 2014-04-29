<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Notes.NoteViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matter Notes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Matter Notes<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                Body
            </th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Notes", new { id = item.Id }, null)%>
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
                <%: Html.ActionLink("Edit", "Edit", "Notes", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Notes allow users to make comments or record information on matters.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the note.  Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the note.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Note", "Create", "Notes", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li>
            <%: Html.ActionLink("Matter", "Details", "Matters", new { Id  = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>