<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Security.AreaViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
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

    <h2>Index</h2>

    <table id="list"></table>
    <div id="pager"></div>

    <script language="javascript">
        $(function () {
            $("#list").jqGrid({
                treeGrid: true,
                autowidth: true,
                url: '../SecurityAreas/ListChildrenJqGrid',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Name', 'Description', 'Actions'],
                colModel: [
                    { name: 'Id', width: 1, hidden: true, key: true },
                    { name: 'Name', width: 250 },
                    { name: 'Description', width: 400 },
                    { name: 'act', width: 110 }
                ],
                pager: '#pager',
                gridview: true,
                treedatatype: 'json',
                treeGridModel: 'adjacency',
                ExpandColumn: 'Name',
                caption: 'Security Areas',
                gridComplete: function () {
                    var ids = jQuery("#list").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        id = ids[i];
                        detailButton = "<a href=\"../SecurityAreas/Details/" + ids[i] + "\">Details</a>";
                        permissionsButton = "<a href=\"../SecurityAreas/Permissions/" + ids[i] + "\">Permissions</a>";
                        jQuery("#list").jqGrid('setRowData', ids[i], { act: detailButton + " | " + permissionsButton });
                    }
                }
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
