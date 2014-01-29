<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AssignContact
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>AssignContact</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <table class="detail_table">
            <tr>
                <td class="display-label">Matter</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Matter.Id)%>
                    <%: Model.Matter.Title %>
                </td>
            </tr>
            <tr>
                <td class="display-label">Contact</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Contact.Id) %>
                    <%: Model.Contact.DisplayName %>
                </td>
            </tr>
            <tr>
                <td class="display-label">Role</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Role) %>
                    <%: Html.ValidationMessageFor(model => model.Role)%>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
