<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.VersionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create New Document Version
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create New Document Version<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm("Create", "Versions", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <%: Html.ValidationSummary(true)%>
    <% if (ViewData["MatterId"] != null)
       { %>
    <%: Html.Hidden("MatterId", ViewData["MatterId"])%>
    <% }
       else if (ViewData["TaskId"] != null)
       { %>
    <%: Html.Hidden("TaskId", ViewData["TaskId"])%>
    <% } %>
    <%: Html.HiddenFor(model => model.Document.Id) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                File<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <input type="file" name="file" id="file" />
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
        Fill in the information on this page to create a new version of the document.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("Document", "Details", "Documents", new { id = Request["DocumentId"] }, null)%></li>
</asp:Content>