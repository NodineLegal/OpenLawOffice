<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.DocumentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Document
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Edit Document<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>  
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Modify the information on this page to make changes to a document.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span>.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Version", "Create", "Versions", new { DocumentId = Model.Id }, null)%></li>
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
    <% if (ViewData["MatterId"] != null)
        { %>
        <li><%: Html.ActionLink("List", "Documents", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <%  }
       else if (ViewData["TaskId"] != null)
        { %>
        <li><%: Html.ActionLink("List", "Documents", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <%  } %>
    </ul>
<% if (ViewData["MatterId"] != null)
    { %>
    <li><%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
<%  }
    if (ViewData["TaskId"] != null)
    { %>
    <li><%: Html.ActionLink("Task", "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
<%  } %>
</asp:Content>