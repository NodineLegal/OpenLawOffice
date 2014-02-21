<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/grid.locale-en.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.5.4/jquery.jqGrid.min.js"></script>

    <h2>Edit</h2>
    
    <table class="detail_table">
        <tr>
            <td class="display-label">Id</td>
            <td class="display-field"><%: Model.Id %></td>
        </tr>
        <tr>
            <td class="display-label">Title</td>
            <td class="display-field"><%: Html.TextBoxFor(model => model.Title) %></td>
        </tr>
        <tr>
            <td class="display-label">Description</td>
            <td class="display-field"><%: Html.TextBoxFor(model => model.Description)%></td>
        </tr>
        <tr>
            <td class="display-label">Projected Start</td>
            <td class="display-field"><%: Html.EditorFor(model => model.ProjectedStart)%></td>
        </tr>
        <tr>
            <td class="display-label">Due Date</td>
            <td class="display-field"><%: Html.EditorFor(model => model.DueDate)%></td>
        </tr>
        <tr>
            <td class="display-label">Projected End</td>
            <td class="display-field"><%: Html.EditorFor(model => model.ProjectedEnd)%></td>
        </tr>
        <tr>
            <td class="display-label">Actual End</td>
            <td class="display-field"><%: Html.EditorFor(model => model.ActualEnd)%></td>
        </tr>
        <tr>
            <td class="display-label">Is a Grouping Task</td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.IsGroupingTask, new { @readonly=true }) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Parent</td>
            <td class="display-field">
                Parent: <%: Html.TextBoxFor(model => model.Parent.Id, new { @readonly = true })%>
                    
                <br /><br /> 
                <table id="parentlist"></table>
                <div id="parentpager"></div>
                <input id="parentclear" type="button" style="width:200px;" value="clear" />

                <script language="javascript">
                    $(function () {
                        $("#parentlist").jqGrid({
                            treeGrid: true,
                            width: 250,
                            url: '../../Tasks/ListChildrenJqGrid',
                            datatype: 'json',
                            jsonReader: {
                                root: 'Rows',
                                page: 'CurrentPage',
                                total: 'TotalRecords',
                                id: 'Id',
                                rows: 'Rows'
                            },
                            colNames: ['id', 'Title', 'Type', 'Due'],
                            colModel: [
                                { name: 'Id', width: 1, hidden: true, key: true },
                                { name: 'Title', width: 350 },
                                { name: 'Type', width: 250 },
                                { name: 'DueDate', width: 120, formatter: 'date' }
                            ],
                            pager: '#parentpager',
                            gridview: true,
                            treedatatype: 'json',
                            treeGridModel: 'adjacency',
                            ExpandColumn: 'Title',
                            caption: 'Parent Task',
                            onSelectRow: function (id) {
                                $("#Parent_Id").val(id);
                            }
                        });
                    });

                    $("#parentclear").click(function () {
                        $("#parentlist").jqGrid('resetSelection');
                        $("#Parent_Id").val(null);
                    });
                </script>
            </td>
        </tr>
        <tr>
            <td class="display-label">Sequential Predecessor</td>
            <td class="display-field">
            
                Sequential Predecessor: <%: Html.TextBoxFor(model => model.SequentialPredecessor.Id, new { @readonly = true })%>
                    
                <br /><br /> 
                <table id="splist"></table>
                <div id="sppager"></div>
                <input id="spclear" type="button" style="width:200px;" value="clear" />

                <script language="javascript">
                    $(function () {
                        $("#splist").jqGrid({
                            treeGrid: true,
                            width: 250,
                            url: '../../Tasks/ListChildrenJqGrid',
                            datatype: 'json',
                            jsonReader: {
                                root: 'Rows',
                                page: 'CurrentPage',
                                total: 'TotalRecords',
                                id: 'Id',
                                rows: 'Rows'
                            },
                            colNames: ['id', 'Title', 'Type', 'Due'],
                            colModel: [
                                { name: 'Id', width: 1, hidden: true, key: true },
                                { name: 'Title', width: 350 },
                                { name: 'Type', width: 250 },
                                { name: 'DueDate', width: 120, formatter: 'date' }
                            ],
                            pager: '#sppager',
                            gridview: true,
                            treedatatype: 'json',
                            treeGridModel: 'adjacency',
                            ExpandColumn: 'Title',
                            caption: 'Parent Task',
                            onSelectRow: function (id) {
                                $("#Parent_Id").val(id);
                            }
                        });
                    });

                    $("#spclear").click(function () {
                        $("#splist").jqGrid('resetSelection');
                        $("#SequentialPredecessor_Id").val(null);
                    });
                </script>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Task", "Create", new { MatterId = ViewData["MatterId"] })%></li>
        <li><%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("Matter ", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    </ul>
    <li><%: Html.ActionLink("Time", "ForTask", "Times", new { id = Model.Id }, null)%></li>
</asp:Content>
