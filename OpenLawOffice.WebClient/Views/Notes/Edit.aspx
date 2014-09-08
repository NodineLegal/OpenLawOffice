<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Notes.NoteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Note
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <% if (ViewData["Task"] != null)
           { %>
        <div class="one">Task: [<%: Html.ActionLink((string)ViewData["Task"], "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%>]</div>
        <div id="current" class="two">Edit Note<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
        <% }
           else
           { %>           
        <div id="current" class="one">Edit Note<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
        <% } %>
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
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Date/Time
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Timestamp) %>
                <%: Html.ValidationMessageFor(model => model.Timestamp)%>
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
                <%: Html.TextAreaFor(model => model.Body, new { @style = "width: 100%; height: 100px;" })%>
                <%: Html.ValidationMessageFor(model => model.Body)%>
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
        Modify the information on this page to make changes to the note.  Required fields are indicated with an
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
            <%: Html.ActionLink("New Note", "Create", new { MatterId = ViewData["MatterId"], TaskId = ViewData["TaskId"] })%></li>
        <li>
            <%: Html.ActionLink("Edit", "Edit", new { id = Model.Id })%></li>
    </ul>
        <% if (ViewData["MatterId"] != null)
           { %>
    
    <li>
        <%: Html.ActionLink("Matter", "Details", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes of Matter", "Notes", "Matters", new { id = ViewData["MatterId"] }, null)%></li>
    <% }
           else if (ViewData["TaskId"] != null)
           { %>
    <li>
        <%: Html.ActionLink("Task", "Details", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <li>
        <%: Html.ActionLink("Notes of Task", "Notes", "Tasks", new { id = ViewData["TaskId"] }, null)%></li>
    <% } %>
</asp:Content>