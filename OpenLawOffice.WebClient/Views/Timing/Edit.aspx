<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Timing.TimeViewModel>" %>

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
    <h2>
        Edit</h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Start
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.Start) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Stop
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.Stop)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Worker
            </td>
            <td class="display-field">
                Worker:
                <%: Html.TextBoxFor(model => model.Worker.Id, new { @readonly = true })%>
                <%: Html.ValidationMessageFor(model => model.Worker.Id)%>
                <table id="list">
                </table>
                <div id="pager">
                </div>
                <input id="clear" type="button" style="width: 200px;" value="clear" />
                <script language="javascript">
                    $(function () {
                        $("#list").jqGrid({
                            autowidth: true,
                            url: '../../Contacts/ListJqGrid',
                            datatype: 'json',
                            jsonReader: {
                                root: 'Rows',
                                page: 'CurrentPage',
                                total: 'TotalRecords',
                                id: 'Id',
                                rows: 'Rows'
                            },
                            colNames: ['id', 'Display Name', 'City', 'State'],
                            colModel: [
                                { name: 'Id', width: 1, hidden: true, key: true },
                                { name: 'DisplayName', width: 350 },
                                { name: 'City', width: 200 },
                                { name: 'State', width: 200 }
                            ],
                            pager: '#pager',
                            gridview: true,
                            caption: 'Contacts',
                            onSelectRow: function (id) {
                                $("#Worker_Id").val(id);
                            }
                        });
                    });

                    $("#clear").click(function () {
                        $("#list").jqGrid('resetSelection');
                        $("#Worker_Id").val(null);
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
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
        <% if (ViewData["TaskId"] != null)
           { %>
        <li>
            <%: Html.ActionLink("Task ", "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
        <% } %>
    </ul>
</asp:Content>