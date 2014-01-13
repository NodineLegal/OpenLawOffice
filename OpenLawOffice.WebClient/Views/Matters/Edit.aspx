<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Matter", "Create") %></li>
        <li><%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li><%: Html.ActionLink("Tags", "Tags", "Matters") %></li>
    <li><%: Html.ActionLink("Responsible Users", "Users", "Matters")%></li>
    <li><%: Html.ActionLink("Contacts", "Contacts", "Matters")%></li>
    <li><%: Html.ActionLink("Tasks", "Tasks", "Matters")%> (<%: Html.ActionLink("Add", "AddTask", new { controller = "Matters", id = Model.Id }) %>)</li>
    <li><%: Html.ActionLink("Notes", "Notes", "Matters")%> (<%: Html.ActionLink("Add", "AddNote", new { controller = "Matters", id = Model.Id }) %>)</li>
    <li><%: Html.ActionLink("Documents", "Documents", "Matters")%> (<%: Html.ActionLink("Add", "AddDocument", new { controller = "Matters", id = Model.Id }) %>)</li>
    <li><%: Html.ActionLink("Time", "Time", "Matters")%> (<%: Html.ActionLink("Add", "AddTime", new { controller = "Matters", id = Model.Id }) %>)</li>
    <li><%: Html.ActionLink("Permissions", "Acls", "Matters")%></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <table class="detail_table">
            <tr>
                <td class="display-label">Id</td>
                <td class="display-field"><%: Model.Id %></td>
            </tr>
            <tr>
                <td class="display-label">Title</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Title) %>
                    <%: Html.ValidationMessageFor(model => model.Title) %>
                </td>
            </tr>
            <tr>
                <td class="display-label">Synopsis</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Synopsis) %>
                    <%: Html.ValidationMessageFor(model => model.Synopsis) %>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

    <% } %>

</asp:Content>

