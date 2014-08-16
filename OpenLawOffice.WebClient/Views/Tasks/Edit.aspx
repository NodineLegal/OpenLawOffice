<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Task
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
    <h2>
        Edit Task<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Title) %>
                <%: Html.ValidationMessageFor(model => model.Title) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Description<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Description)%>
                <%: Html.ValidationMessageFor(model => model.Description)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Active<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Active)%>
                A check indicates there is something left to complete.  Once completed, uncheck.
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected Start
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ProjectedStart)%>
                <%: Html.ValidationMessageFor(model => model.ProjectedStart)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Due Date
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.DueDate)%>
                <%: Html.ValidationMessageFor(model => model.DueDate)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Projected End
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ProjectedEnd)%>
                <%: Html.ValidationMessageFor(model => model.ProjectedEnd)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Actual End
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.ActualEnd)%>
                <%: Html.ValidationMessageFor(model => model.ActualEnd)%>
            </td>
        </tr>
        <%--<tr>
            <td class="display-label">
                Parent
            </td>
            <td class="display-field">
                <div id="ParentDiv">
                    Parent:
                    <%: Html.TextBoxFor(model => model.Parent.Id, new { @readonly = true })%>
                    <%: Html.ValidationMessageFor(model => model.Parent.Id)%>
                    <br />
                    <br />
                    <table id="parentlist">
                    </table>
                    <div id="parentpager">
                    </div>
                    <input id="parentclear" type="button" style="width: 200px;" value="clear" />
                    <script language="javascript">
                        $(function () {
                            $("#parentlist").jqGrid({
                                treeGrid: true,
                                autowidth: true,
                                url: '/Tasks/ListChildrenJqGrid?MatterId=<%: ViewData["MatterId"].ToString() %>',
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
                </div>
                <div id="ParentDivHidden">
                    This task's parent cannot be modified as it is part of a sequence. Task sequence
                    members automatically have their parent task managed.
                </div>
            </td>
        </tr>
        <tr>
            <td class="display-label">Sequence</td>
            <td class="display-field">

            <div id="SeqDiv">
                This task is a member of a sequence.

                <br /><br />

                <input id="spclear" type="button" style="width:200px;" value="Remove from Sequence" />

                <br /><br />

                If you would like to change this task's position in the sequence, you may do so by
                selecting it's new predecessor below.

                <br /><br />

                Sequential Predecessor: <%: Html.TextBoxFor(model => model.SequentialPredecessor.Id, new { @readonly = true })%>
                <%: Html.ValidationMessageFor(model => model.SequentialPredecessor.Id)%>

                <br /><br />
                <table id="splist"></table>
                <div id="sppager"></div>

                <script language="javascript">
                    $(function () {
                        $("#splist").jqGrid({
                            treeGrid: true,
                            autowidth: true,
                            url: '/Tasks/ListChildrenJqGrid?MatterId=<%: ViewData["MatterId"].ToString() %>',
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
                                $("#SequentialPredecessor_Id").val(id);
                            }
                        });
                    });

                    $("#spclear").click(function () {
                        $("#splist").jqGrid('resetSelection');
                        $("#SequentialPredecessor_Id").val(null);
                        $("#ParentDiv").show();
                        $("#ParentDivHidden").hide();
                        $("#SeqDiv").hide();
                        $("#SeqDivHidden").show();
                    });
                </script>
            </div>
            <div id="SeqDivHidden">
                This task's is not part of a sequence.  To make this task a sequence member click the button.
                <br />
                <input id="makeSeqMember" type="button" style="width:200px;" value="Add to Sequence" />

                <script language="javascript">
                    $("#makeSeqMember").click(function () {
                        $("#splist").jqGrid('resetSelection');
                        $("#SequentialPredecessor_Id").val(null);
                        $("#ParentDiv").hide();
                        $("#ParentDivHidden").show();
                        $("#SeqDiv").show();
                        $("#SeqDivHidden").hide();
                    });
                </script>
            </div>
            </td>
        </tr>--%>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <%--<script language="javascript">
        $(function () {
            var seqPredId = $("#SequentialPredecessor_Id").val();

            if (seqPredId == null || seqPredId == '') {
                // NOT sequence member
                $("#ParentDiv").show();
                $("#ParentDivHidden").hide();
                //$("#SeqDiv").hide();
                //$("#SeqDivHidden").show();
            } else {
                // Sequence member
                $("#ParentDiv").hide();
                $("#ParentDivHidden").show();
                //$("#SeqDiv").show();
                //$("#SeqDivHidden").hide();
            }
        });
    </script>--%>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Fill in the information on this page to modify the task.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Select a "parent" task to make this task be a "subtask" of another task.  To deselect a parent task, click "clear". 
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Task", "Create", new { MatterId = ViewData["MatterId"] })%></li>
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Tags", "Tags", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", "Tasks", new { id = Model.Id }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Tasks", TaskId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Documents", "Documents", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Documents", new { controller = "Tasks", TaskId = Model.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Time", "Time", "Tasks", new { id = Model.Id }, null)%>
        (<%: Html.ActionLink("Add", "SelectContactToAssign", "TaskTime", new { TaskId = Model.Id }, null)%>)</li>
</asp:Content>