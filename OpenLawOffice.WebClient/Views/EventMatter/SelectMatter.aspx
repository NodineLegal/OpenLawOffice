<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Select Matter
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
    <h2>Select Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <table id="list">
    </table>
    <div id="pager">
    </div>
    <script language="javascript">
        var eventId = '<%: RouteData.Values["Id"] %>';
        $(function () {
            $("#list").jqGrid({
                treeGrid: true,
                autowidth: true,
                url: '/Matters/ListChildrenJqGrid',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Title', 'Synopsis', ''],
                colModel: [
                    { name: 'Id', width: 1, hidden: true, key: true },
                    { name: 'Title', width: 250, formatter: titleFormat },
                    { name: 'Synopsis', width: 400 },
                    { name: 'act', width: 25 }
                ],
                pager: '#pager',
                gridview: true,
                treedatatype: 'json',
                treeGridModel: 'adjacency',
                ExpandColumn: 'Title',
                caption: 'Matters',
                gridComplete: function () {
                    var ids = jQuery("#list").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        id = ids[i];
                        editButton = "<a href=\"../Matters/Edit/" + ids[i] + "\" class=\"btn-edit\" title=\"Edit\">Edit</a>";
                        jQuery("#list").jqGrid('setRowData', ids[i], { act: editButton });
                    }
                }
            });
        });

        function titleFormat(cellvalue, options, rowObject) {
            return '<a href="../AssignMatter/' + eventId + '?MatterId=' + options.rowId + '">' + cellvalue + '</a>';
        }
    </script>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        A matter is a substantial or essential thing.  They contain documents, notes, time entries and more.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        The arrow to the left of the title allows for expanding to view submatters (matters within matters).  
        Clicking the title will relate the matter and the event.
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make changes to the matter.
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
