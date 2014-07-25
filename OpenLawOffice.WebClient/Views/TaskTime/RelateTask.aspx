<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Relate Task to FastTime
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../../Scripts/jqGrid-4.6.0/grid.locale-en.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.6.0/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        div.ui-jqgrid-titlebar
        {
            height: 16px;
        }
    </style>
    <h2>Relate Task to FastTime<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table id="list">
    </table>
    <div id="pager">
    </div>
    <script language="javascript">
        $(function () {
            $("#list").jqGrid({
                treeGrid: true,
                height: '100%',
                autowidth: true,
                url: '/Tasks/ListChildrenJqGrid',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Title', 'Type', 'Due', ''],
                colModel: [
                    { name: 'Id', width: 1, hidden: true, key: true },
                    { name: 'Title', width: 350, formatter: titleFormat },
                    { name: 'Type', width: 200 },
                    { name: 'DueDate', width: 120, formatter: 'date' },
                    { name: 'act', width: 35 }
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
                        editButton = "<a href=\"/TaskTime/AssignFastTime/<%: RouteData.Values["Id"] %>?TaskId=" + ids[i] + "\" class=\"btn-assigntask\" title=\"Assign Task\">Assign Task</a>";
                        jQuery("#list").jqGrid('setRowData', ids[i], { act: editButton });
                    }
                }
            });
        });

        function titleFormat(cellvalue, options, rowObject) {
            return '<a href="/Tasks/Details/' + options.rowId + '">' + cellvalue + '</a>';
        }
    </script>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Tasks can contain documents, tasks, notes and more.  Tasks
        can have subtasks and be subtasks<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        The arrow to the left of the title allows for expanding to view subtasks (tasks within tasks).  
        Clicking the title will show the details of the task including access to documents, notes, time entries and more.  
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make changes to the task.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
