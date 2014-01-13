<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenLawOffice.WebClient.ViewModels.PermissionsViewModel>" %>
<table style="border: none; width: 150px;">
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Read", Model.Read) %>Read</td>
        <td style="border: none;"><%: Html.CheckBox("List", Model.List)%>List</td>
    </tr>
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Create", Model.Create)%>Create</td>
        <td style="border: none;"><%: Html.CheckBox("Modify", Model.Modify)%>Modify</td>
    </tr>
    <tr>
        <td style="border: none;"><%: Html.CheckBox("Disable", Model.Disable)%>Disable</td>
        <td style="border: none;"><%: Html.CheckBox("Delete", Model.Delete)%>Delete</td>
    </tr>
</table>