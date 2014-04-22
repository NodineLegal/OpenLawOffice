<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.VersionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create</h2>
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
                File
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>