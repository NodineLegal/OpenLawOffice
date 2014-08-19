<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.CreateMatterViewModel>" %>

<%@ Import Namespace="OpenLawOffice.WebClient.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("Matters", "Index") %></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script type="text/javascript" src="../../Scripts/jqGrid-4.6.0/grid.locale-en.js"></script>
    <script type="text/javascript" src="../../Scripts/jqGrid-4.6.0/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        div.ui-jqgrid-titlebar
        {
            height: 16px;
        }
    </style>--%>
    <div id="roadmap">
        <div id="current" class="zero">Create Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.Title) %>
                <%: Html.ValidationMessageFor(model => model.Matter.Title)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Synopsis<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextAreaFor(model => model.Matter.Synopsis, new { style = "height: 50px; width: 100%;" })%>
                <%: Html.ValidationMessageFor(model => model.Matter.Synopsis)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Jurisdiction
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.Jurisdiction) %>
                <%: Html.ValidationMessageFor(model => model.Matter.Jurisdiction)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Case Number
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.CaseNumber)%>
                <%: Html.ValidationMessageFor(model => model.Matter.CaseNumber)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Lead Attorney<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.ValidationMessageFor(model => model.LeadAttorney)%>
                <%: Html.DropDownListFor(x => x.LeadAttorney.Contact.Id,
                        new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Active<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Matter.Active, new { Checked = true })%>
                Uncheck if the matter is already completed
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Responsible User
            </td>
            <td class="display-field">
                <%: Html.DropDownListFor(x => x.ResponsibleUser.User.PId,
                        new SelectList((IList)ViewData["UserList"], "PId", "Username"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Responsiblity<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.ResponsibleUser.Responsibility) %>
                <%: Html.ValidationMessageFor(model => model.ResponsibleUser.Responsibility)%>
            </td>
        </tr>
        <%--<tr>
            <td class="display-label">
                Assigned Contact
            </td>
            <td class="display-field">
                <%: Html.DropDownListFor(x => x.AssignedContact.Contact.Id,
                        new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                        new { @size = 5, @style = "width: 100%" })%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Role<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.AssignedContact.Role)%>
                <%: Html.ValidationMessageFor(model => model.AssignedContact.Role)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Parent
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Matter.Parent.Id) %>
                <table id="list">
                </table>
                <div id="pager">
                </div>
                <input id="clear" type="button" style="width: 200px;" value="clear" />
                <script language="javascript">
                    $(function () {
                        $("#list").jqGrid({
                            treeGrid: true,
                            width: 350,
                            url: '../../Matters/ListChildrenJqGrid',
                            datatype: 'json',
                            jsonReader: {
                                root: 'Rows',
                                page: 'CurrentPage',
                                total: 'TotalRecords',
                                id: 'Id',
                                rows: 'Rows'
                            },
                            colNames: ['id', 'Title', 'Synopsis'],
                            colModel: [
                                    { name: 'Id', width: 1, hidden: true, key: true },
                                    { name: 'Title', width: 250 },
                                    { name: 'Synopsis', hidden: true }
                                ],
                            pager: '#pager',
                            gridview: true,
                            treedatatype: 'json',
                            treeGridModel: 'adjacency',
                            ExpandColumn: 'Title',
                            caption: 'Matters',
                            onSelectRow: function (id) {
                                $("#Parent_Id").val(id);
                            }
                        });
                    });

                    $("#clear").click(function () {
                        $("#list").jqGrid('resetSelection');
                    });
                </script>
            </td>
        </tr>--%>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Fill in the information on this page to create a new matter.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Select a "parent" matter to make this matter be a "submatter" of another matter.  To deselect a parent matter, click "clear". 
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>