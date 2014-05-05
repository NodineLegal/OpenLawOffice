<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matters of Contact
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Matters of Contact<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
        
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                Synopsis
            </th>
            <th style="width: 20px;">
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Matters", new { id = item.Id }, null)%>
            </td>
            <td>
                <% if (item.Synopsis.Length > 100)
                   {
                %>
                <%: item.Synopsis.Substring(0, 100)%>...
                <% }
                   else
                   { %>
                <%: item.Synopsis%>
                <% } %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Matters", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page shows a list of all matters in which the contact is involved.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Clicking the title will show the details of the matter.
        Click the 
        <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make 
        changes to the matter.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Contact", "Create") %></li>
        <li>
            <%: Html.ActionLink("New Matter", "Create", "Matters", null, null) %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Contact", "Details", new { id = Request["ContactId"] })%></li>
</asp:Content>
