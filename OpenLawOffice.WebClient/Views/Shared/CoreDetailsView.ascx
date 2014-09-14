<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenLawOffice.WebClient.ViewModels.CoreViewModel>" %>

<table class="detail_table">
    <tr>
        <td colspan="5" class="detail_table_heading">
            Core Details
        </td>
    </tr>
    <tr>
        <td class="display-label" style="width: 135px;">
            Created By
        </td>
        <td class="display-field" style="width: 135px;">
            <%: Model.CreatedBy.Username %>
        </td>
        <td>
        </td>
        <td class="display-label" style="width: 135px;">
            Created At
        </td>
        <td class="display-field" style="width: 135px;">
            <%: String.Format("{0:g}", Model.Created.Value) %>
        </td>
    </tr>
    <tr>
        <td class="display-label" style="width: 135px;">
            Modified By
        </td>
        <td class="display-field" style="width: 135px;">
            <%: Model.ModifiedBy.Username %>
        </td>
        <td>
        </td>
        <td class="display-label" style="width: 135px;">
            Modified At
        </td>
        <td class="display-field" style="width: 135px;">
            <%: String.Format("{0:g}", Model.Modified.Value)%>
        </td>
    </tr>
    <tr>
        <td class="display-label" style="width: 135px;">
            Disabled By
        </td>
        <% if (Model.DisabledBy != null)
            { %>
        <td class="display-field" style="width: 135px;">
            <%: Model.DisabledBy.Username%>
        </td>
        <% }
            else
            { %>
        <td style="width: 135px;" />
        <% } %>
        <td>
        </td>
        <td class="display-label" style="width: 135px;">
            Disabled At
        </td>
        <% if (Model.Disabled.HasValue)
            { %>
        <td class="display-field" style="width: 135px;">
            <%: String.Format("{0:g}", Model.Disabled.Value)%>
        </td>
        <% }
            else
            { %>
        <td class="display-field" style="width: 135px;">
        </td>
        <% } %>
    </tr>
</table>