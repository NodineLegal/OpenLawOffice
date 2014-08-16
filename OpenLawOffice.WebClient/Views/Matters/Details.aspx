<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matter Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter", "Create") %></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
       <%-- <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>--%>
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Tags", "Tags", new { id = Model.Id })%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", new { id = Model.Id })%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", new { id = Model.Id })%></li>
    <li>
        <%: Html.ActionLink("Tasks", "Tasks", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Tasks", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>        
    <li>
        <%: Html.ActionLink("Events", "Events", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Events", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Documents", "Documents", "Matters", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Documents", new { controller = "Matters", MatterId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Times", "Time", "Matters", new { id = Model.Id }, null)%></li>
    <%--<li>
        <%: Html.ActionLink("Permissions", "Acls", "Matters")%></li>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Matter Details<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <%--<tr>
            <td class="display-label">
                Parent
            </td>
            <td class="display-field">
                <% if (Model.Parent != null)
                   { %>
                <%: Model.Parent.Title%>
                <% } %>
            </td>
        </tr>--%>
        <tr>
            <td class="display-label">
                Title
            </td>
            <td class="display-field">
                <%: Model.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Synopsis
            </td>
            <td class="display-field">
                <%: Model.Synopsis %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Jurisdiction
            </td>
            <td class="display-field">
                <%: Model.Jurisdiction %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Case Number
            </td>
            <td class="display-field">
                <%: Model.CaseNumber %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Lead Attorney
            </td>
            <td class="display-field">
                <% if (Model.LeadAttorney != null)
                   { %>
                    <%: Model.LeadAttorney.DisplayName%>
                <% } %>
            </td>
        </tr>
    </table>
    <% Html.RenderPartial("CoreDetailsView"); %>

    <table class="listing_table">    
        <tr>
            <td colspan="3" style="font-weight: bold;">
                Active Tasks
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Due Date
            </th>
            <th style="text-align: center; width: 40px;">
                
            </th>
        </tr>
        <% foreach (var item in Model.Tasks)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Tasks", new { id = item.Id.Value }, null) %>
            </td>
            <td>
                <%: item.DueDate %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Tasks", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Close", "Close", "Tasks", new { id = item.Id.Value }, new { @class = "btn-remove", title = "Close" })%>
            </td>
        </tr>
        <% } %>
    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information about the matter.
        </p>
    </div>
</asp:Content>