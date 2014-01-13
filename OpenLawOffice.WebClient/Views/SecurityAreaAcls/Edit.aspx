<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaAclViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciPlugin.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciTree.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciTree.selectable.js"></script>

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        

        <table class="detail_table">
            <tr>
                <td class="display-label">Area</td>S
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Area.Id) %>
                    Active Parent: <%: Html.TextBoxFor(model => model.Area.Name, new { style = "width: 190px;" })%>
                    <input name="clear" type="button" value="clear" />
                    <div id="tree" class="aciTree" style="height: 200px; width: 315px;" />
                </td>
            </tr>
            <tr>
                <td class="display-label">User</td>
                <td class="display-field">
                    <%: Html.DropDownListFor(model => model.User.Id, (List<SelectListItem>)ViewData["UserList"], "Id", "Username")%>
                    <%: Html.ValidationMessageFor(model => model.User)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Allowed</td>
                <td class="display-field">                    
                    <%: Html.EditorFor(x => Model.AllowPermissions, "PermissionViewModel") %>
                </td>
            </tr>
            <tr>
                <td class="display-label">Denied</td>
                <td class="display-field">                    
                    <%: Html.EditorFor(x => Model.DenyPermissions, "PermissionViewModel") %>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

        <script type="text/javascript">
            $(function () {
                $('#tree').on('acitree', function (event, api, item, eventName, options) {
                    // get the item id
                    var itemId = api.getId(item);
                    var itemLabel = api.getLabel(item);
                    if (eventName == 'selected') {
                        $('#Area_Id').val(itemId);
                        $('#Area_Name').val(itemLabel);
                    }
                });

                // init the tree
                $('#tree').aciTree({
                    ajax: {
                        url: '../../SecurityAreas/AciTreeList'
                    },
                    selectable: true
                });

                $('[name=clear]').click(function () {
                    $('#Area_Id').val(null);
                    $('#Area_Name').val(null);
                });

            });
        </script>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("Add Area Access", "Create") %></li>
        <li><%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>

