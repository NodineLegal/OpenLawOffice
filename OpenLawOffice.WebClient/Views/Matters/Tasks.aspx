<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Tasks.TaskViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tasks
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="/Scripts/jqGrid-4.5.4/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="/Scripts/jqGrid-4.5.4/grid.locale-en.js"></script>
    <script type="text/javascript" src="/Scripts/jqGrid-4.5.4/jquery.jqGrid.min.js"></script>

    <style type="text/css">
    div.ui-jqgrid-titlebar 
    {
        height: 16px;
    }
    </style>

    <h2>Tasks</h2>

    <table id="list"></table>
    <div id="pager"></div>
    
    <script language="javascript">
        $(function () {
            $("#list").jqGrid({
                treeGrid: true,
                autowidth: true,
                url: '/Tasks/ListChildrenJqGrid?MatterId=<%: RouteData.Values["Id"].ToString() %>',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Title', 'Type', 'Due', 'Actions'],
                colModel: [
                    { name: 'Id', width: 1, hidden: true, key: true },
                    { name: 'Title', width: 350 },
                    { name: 'Type', width: 250 },
                    { name: 'DueDate', width: 120, formatter:'date' },
                    { name: 'act', width: 120 }
                ],
                pager: '#pager',
                gridview: true,
                treedatatype: 'json',
                treeGridModel: 'adjacency',
                ExpandColumn: 'Title',
                caption: 'Tasks',
                gridComplete: function () {
                    var ids = jQuery("#list").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        id = ids[i];
                        detailButton = "<a href=\"/Tasks/Details/" + ids[i] + "\">Details</a>";
                        editButton = "<a href=\"/Tasks/Edit/" + ids[i] + "\">Edit</a>";
                        jQuery("#list").jqGrid('setRowData', ids[i], { act: detailButton + " | " + editButton });
                    }
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Task", "Create", "Tasks", new { MatterId = RouteData.Values["Id"].ToString() }, null)%></li>
        <li><%: Html.ActionLink("Matter", "Details", "Matters", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>

