<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.ResponsibleUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

    <table class="detail_table">
        <tr>
            <td class="display-label">Id</td>
            <td class="display-field"><%: Model.Id %></td>
        </tr>
        <tr>
            <td class="display-label">Matter</td>
            <td class="display-field"><%: Model.Matter.Title %></td>
        </tr>
        <tr>
            <td class="display-label">User</td>
            <td class="display-field"><%: Html.DropDownListFor(x => x.User.Id,
                                new SelectList((IList)ViewData["UserList"], "Id", "Username")) %>
                <%: Html.ValidationMessageFor(x => x.User) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Responsiblity</td>
            <td class="display-field"><%: Html.TextBoxFor(x => x.Responsibility) %>
                <%: Html.ValidationMessageFor(x => x.Responsibility) %>
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
        <li><%: Html.ActionLink("List", "ResponsibleUsers", "Matters", new { id = Model.Matter.Id }, null)%></li>
    </ul>
</asp:Content>
