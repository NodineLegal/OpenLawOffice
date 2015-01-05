<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.EditMatterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter", "Create") %></li>
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Matter.Id })%></li>
       <%-- <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>--%>
        <li>
            <%: Html.ActionLink("List", "Index") %></li>
    </ul>
    <li>
        <%: Html.ActionLink("Tags", "Tags", new { id = Model.Matter.Id })%></li>
    <li>
        <%: Html.ActionLink("Responsible Users", "ResponsibleUsers", new { id = Model.Matter.Id })%></li>
    <li>
        <%: Html.ActionLink("Contacts", "Contacts", new { id = Model.Matter.Id })%></li>
    <li>
        <%: Html.ActionLink("Tasks", "Tasks", "Matters", new { id = Model.Matter.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Tasks", new { controller = "Matters", MatterId = Model.Matter.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Notes", "Notes", "Matters", new { id = Model.Matter.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Notes", new { controller = "Matters", MatterId = Model.Matter.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Documents", "Documents", "Matters", new { id = Model.Matter.Id }, null)%>
        (<%: Html.ActionLink("Add", "Create", "Documents", new { controller = "Matters", MatterId = Model.Matter.Id }, null)%>)</li>
    <li>
        <%: Html.ActionLink("Times", "Time", "Matters", new { id = Model.Matter.Id }, null)%></li>
    <%--<li>
        <%: Html.ActionLink("Permissions", "Acls", "Matters")%></li>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">   
    <script language="javascript">
        $(document).ready(function () {
            $('#Matter_BillTo_DisplayName').autocomplete({
                source: "/Contacts/ListDisplayNameOnly",
                minLength: 2,
                focus: function (event, ui) {
                    $("#Matter_BillTo_Id").val(ui.item.Id);
                    $("#Matter_BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                },
                select: function (event, ui) {
                    $("#Matter_BillTo_Id").val(ui.item.Id);
                    $("#Matter_BillTo_DisplayName").val(ui.item.DisplayName);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.DisplayName + "</a>")
                .appendTo(ul);
            };
            $('#Matter_BillTo_DisplayName').focus(function () {
                $("#Matter_BillTo_Id").val('');
                $('#Matter_BillTo_DisplayName').val('');
            });
        });
    </script>
    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Edit Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>

    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Matter.Id%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Title<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Matter.Title)%>
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
                <%: Html.TextBoxFor(model => model.Matter.Jurisdiction)%>
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
                Bill To<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Matter.BillTo.Id) %>
                <%: Html.TextBoxFor(model => model.Matter.BillTo.DisplayName) %>
                <%: Html.ValidationMessageFor(model => model.Matter.BillTo.DisplayName)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Active<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.Matter.Active)%>
                Uncheck if the matter is already completed
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Fill in the information on this page to modify the matter.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Select a "parent" matter to make this matter be a "submatter" of another matter.  To deselect a parent matter, click "clear". 
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>