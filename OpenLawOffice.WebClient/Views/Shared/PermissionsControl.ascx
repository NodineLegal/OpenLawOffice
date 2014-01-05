<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenLawOffice.Common.Models.PermissionType>" %>

    <%
        bool ro = false;
        if (ViewData["Readonly"] != null)
            ro = (bool)ViewData["Readonly"];
        
        using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
<table class="detail_table">
    <tr>
        <th style="text-align: center;">Reading</th>
        <th style="text-align: center;">Writing</th>
        <th style="text-align: center;">Admin.</th>
    </tr>
    <tr>
        <td style="text-align: center;">
            <%: Html.CheckBox("Read", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.Read), new { disabled = ro })%>
        </td>
        <td style="text-align: center;">
            <%: Html.CheckBox("Create", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.Create), new { disabled = ro })%>
        </td>
        <td style="text-align: center;">
            <%: Html.CheckBox("Disable", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.Disable), new { disabled = ro })%>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
            <%: Html.CheckBox("List", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.List), new { disabled = ro })%>
        </td>
        <td style="text-align: center;">
            <%: Html.CheckBox("Modify", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.Modify), new { disabled = ro })%>
        </td>
        <td style="text-align: center;">
            <%: Html.CheckBox("Delete", Model.HasFlag(OpenLawOffice.Common.Models.PermissionType.Delete), new { disabled = ro })%>
        </td>
    </tr>

</table>

    <% } %>