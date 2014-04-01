<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Timing.TimeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Time
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

    <h2>Time</h2>
    
    <table id="list"></table>
    <div id="pager"></div>
    
    <script language="javascript">
        $(function () {
            $("#list").jqGrid({
                autowidth: true,
                url: '/Timing/ListChildrenJqGrid?TaskId=<%: RouteData.Values["Id"].ToString() %>',
                datatype: 'json',
                jsonReader: {
                    root: 'Rows',
                    page: 'CurrentPage',
                    total: 'TotalRecords',
                    id: 'Id',
                    rows: 'Rows'
                },
                colNames: ['id', 'Start', 'Stop', 'Worker', 'Actions'],
                colModel: [
                    { name: 'Id', width: 1, hidden: true, key: true },
                    { name: 'Start', width: 200, formatter: 'date', formatoptions: { newformat: 'm/d/Y H:i:s'} },
                    { name: 'Stop', width: 200, formatter: 'date', formatoptions: { newformat: 'm/d/Y H:i:s'} },
                    { name: 'Worker', width: 200 },
                    { name: 'act', width: 120 }
                ],
                pager: '#pager',
                gridview: true,
                caption: 'Tasks',
                gridComplete: function () {
                    var ids = jQuery("#list").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        id = ids[i];
                        detailButton = "<a href=\"/Timing/Details/" + ids[i] + "\">Details</a>";
                        editButton = "<a href=\"/Timing/Edit/" + ids[i] + "\">Edit</a>";
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
        <li><%: Html.ActionLink("New Entry", "SelectContactToAssign", "TaskTime", new { TaskId = RouteData.Values["Id"].ToString() }, null)%></li>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { Id = RouteData.Values["Id"].ToString() }, null)%></li>
    </ul>
</asp:Content>
