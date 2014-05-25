<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenLawOffice.WebClient.ViewModels.CoreViewModel>" %>

<table class="detail_table">
    <tr>
        <td colspan="5" style="font-weight: bold;">
            Core Details
        </td>
    </tr>
    <tr>
        <td class="display-label">
            Created By
        </td>
        <td class="display-field">
            <%: Model.CreatedBy.Username %>
        </td>
        <td style="width: 10px;">
        </td>
        <td class="display-label">
            Created At
        </td>
        <td class="display-field">
            <%: String.Format("{0:g}", Model.Created.Value) %>
        </td>
    </tr>
    <tr>
        <td class="display-label">
            Modified By
        </td>
        <td class="display-field">
            <%: Model.ModifiedBy.Username %>
        </td>
        <td style="width: 10px;">
        </td>
        <td class="display-label">
            Modified At
        </td>
        <td class="display-field">
            <%: String.Format("{0:g}", Model.Modified.Value)%>
        </td>
    </tr>
    <tr>
        <td class="display-label">
            Disabled By
        </td>
        <% if (Model.DisabledBy != null)
            { %>
        <td class="display-field">
            <%: Model.DisabledBy.Username%>
        </td>
        <% }
            else
            { %>
        <td />
        <% } %>
        <td style="width: 10px;">
        </td>
        <td class="display-label">
            Disabled At
        </td>
        <% if (Model.Disabled.HasValue)
            { %>
        <td class="display-field">
            <%: String.Format("{0:g}", Model.Disabled.Value)%>
        </td>
        <% }
            else
            { %>
        <td class="display-field">
        </td>
        <% } %>
    </tr>
</table>