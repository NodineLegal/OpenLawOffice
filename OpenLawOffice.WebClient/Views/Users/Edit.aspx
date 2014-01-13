<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <table class="detail_table">
            <tr>
                <td class="display-label">Id</td>
                <td class="display-field"><%: Model.Id %></td>
            </tr>
            <tr>
                <td class="display-label">Username</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Username) %>
                    <%: Html.ValidationMessageFor(model => model.Username)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Password</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Password) %>
                    <%: Html.ValidationMessageFor(model => model.Password)%>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New User", "Create") %></li>
        <li><%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>

