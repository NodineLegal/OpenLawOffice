<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenLawOffice.WebClient.ViewModels.PermissionsViewModel>" %>
<table style="border: none; width: 150px;">
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Read", Model.Read, new { disabled = "disabled" })%>Read</td>
        <td style="border: none;"><%: Html.CheckBox("List", Model.List, new { disabled = "disabled" })%>List</td>
    </tr>
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Create", Model.Create, new { disabled = "disabled" })%>Create</td>
        <td style="border: none;"><%: Html.CheckBox("Modify", Model.Modify, new { disabled = "disabled" })%>Modify</td>
    </tr>
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Disable", Model.Disable, new { disabled = "disabled" })%>Disable</td>
        <td style="border: none;"><%: Html.CheckBox("Delete", Model.Delete, new { disabled = "disabled" })%>Delete</td>
    </tr>
</table>