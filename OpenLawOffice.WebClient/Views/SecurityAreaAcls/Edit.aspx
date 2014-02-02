<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaAclViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/grid.locale-en.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery.jqGrid.min.js"></script>
    
    <style type="text/css">
    div.ui-jqgrid-titlebar 
    {
        height: 16px;
    }
    </style>

    <h2>Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        

        <table class="detail_table">
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
            <tr>
                <td class="display-label">Area</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Area.Id) %>
                    Active Parent: <%: Html.TextBoxFor(model => model.Area.Name, new { style = "width: 190px;" })%>
                    
                    <table id="list"></table>
                    <div id="pager"></div>
                    <input id="clear" type="button" style="width:200px;" value="clear" />
                                        
                    <script language="javascript">
                        $(function () {
                            $("#list").jqGrid({
                                treeGrid: true,
                                width: 400,
                                url: '../../SecurityAreas/ListChildrenJqGrid',
                                datatype: 'json',
                                jsonReader: {
                                    root: 'Rows',
                                    page: 'CurrentPage',
                                    total: 'TotalRecords',
                                    id: 'Id',
                                    rows: 'Rows'
                                },
                                colNames: ['id', 'Name', 'Description'],
                                colModel: [
                                    { name: 'Id', width: 1, hidden: true, key: true },
                                    { name: 'Name', width: 300 },
                                    { name: 'Description', width: 400 }
                                ],
                                pager: '#pager',
                                gridview: true,
                                treedatatype: 'json',
                                treeGridModel: 'adjacency',
                                ExpandColumn: 'Name',
                                caption: 'Security Areas',
                                onSelectRow: function (id) {
                                    $("#Area_Id").val(id);
                                    $('#Area_Name').val($("#list").getRowData(id).Name);
                                }
                            });
                        });

                        $("#clear").click(function () {
                            $("#list").jqGrid('resetSelection');
                            $("#Area_Id").val(null);
                            $('#Area_Name').val(null);
                        });
                    </script>
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
        <li><%: Html.ActionLink("Add Area Access", "Create") %></li>
        <li><%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>

