<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Matter", "Create") %></li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/grid.locale-en.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery.jqGrid.min.js"></script>

    <style type="text/css">
    div.ui-jqgrid-titlebar 
    {
        height: 16px;
    }
    </style>

    <h2>Matters</h2>

    <table id="list"></table>
    <div id="pager"></div>

    <script language="javascript">
        $(function () {
            $("#list").jqGrid({
                treeGrid: true,
                autowidth: true,
                url: '../Matters/ListChildrenJqGrid',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Title', 'Synopsis', 'Actions'],
                colModel: [
                    { name: 'Id', width:1,hidden:true,key:true },
                    { name: 'Title', width: 250 },
                    { name: 'Synopsis', width: 400 },
                    { name: 'act', width: 100}
                ],
                pager: '#pager',
                gridview: true,
                treedatatype: 'json',
                treeGridModel: 'adjacency',
                ExpandColumn:'Title',
                caption: 'Matters',
                gridComplete: function () {
                    var ids = jQuery("#list").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        id = ids[i];
                        detailButton = "<a href=\"../Matters/Details/" + ids[i] + "\">Details</a>";
                        editButton = "<a href=\"../Matters/Edit/" + ids[i] + "\">Edit</a>";
                        jQuery("#list").jqGrid('setRowData', ids[i], { act: detailButton + " | " + editButton });
                    }
                }
            });
        });
    </script>

</asp:Content>

