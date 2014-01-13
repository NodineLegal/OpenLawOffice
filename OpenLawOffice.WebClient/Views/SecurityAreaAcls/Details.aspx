<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaAclViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>
    
    <table class="detail_table">
        <tr>
            <td class="display-label">Area</td>
            <td class="display-field">
                <%: Html.LabelFor(model => model.Area.Name)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">User</td>
            <td class="display-field">
                <%: Html.LabelFor(model => model.User) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Allowed</td>
            <td class="display-field"> 
                    <%: Html.DisplayFor(x => Model.AllowPermissions, "PermissionViewModel") %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Denied</td>
            <td class="display-field">                    
                    <%: Html.DisplayFor(x => Model.DenyPermissions, "PermissionViewModel")%>
            </td>
        </tr>
    </table>

    <table class="detail_table">
        <tr>
            <td colspan="5" style="font-weight: bold;">Core Details</td>
        </tr>
        <tr>
            <td class="display-label">Created By</td>
            <td class="display-field"><%: Model.CreatedBy.Username %></td>
            <td style="width: 10px;"></td>
            <td class="display-label">Created At</td>
            <td class="display-field"><%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcCreated.Value, DateTimeKind.Utc).ToLocalTime())%></td>
        </tr>
        <tr>
            <td class="display-label">Modified By</td>
            <td class="display-field"><%: Model.ModifiedBy.Username %></td>
            <td style="width: 10px;"></td>
            <td class="display-label">Modified At</td>
            <td class="display-field"><%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcModified.Value, DateTimeKind.Utc).ToLocalTime())%></td>
        </tr>
        <tr>
            <td class="display-label">Disabled By</td>
            <% if (Model.DisabledBy != null)
               { %>
            <td class="display-field"><%: Model.DisabledBy.Username%></td>
            <% }
               else
               { %>
               <td />
            <% } %>
            <td style="width: 10px;"></td>
            <td class="display-label">Disabled At</td>
            <% if (Model.UtcDisabled.HasValue)
               { %>
            <td class="display-field"><%: String.Format("{0:g}", DateTime.SpecifyKind(Model.UtcDisabled.Value, DateTimeKind.Utc).ToLocalTime())%></td>
            <% }
               else
               { %>
            <td class="display-field"></td>
            <% } %>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>

