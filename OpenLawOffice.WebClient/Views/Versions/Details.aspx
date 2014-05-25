<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.VersionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details of Document Version
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details of Document Version<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
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
                Document
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Document.Title, "Details", "Documents", new { id = Model.Document.Id }, null) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Version
            </td>
            <td class="display-field">
                <%: Model.VersionNumber%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Mime
            </td>
            <td class="display-field">
                <%: Model.Mime%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Filename
            </td>
            <td class="display-field">
                <%: Model.Filename%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Extension
            </td>
            <td class="display-field">
                <%: Model.Extension%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Size
            </td>
            <td class="display-field">
                <%: Model.Size %> bytes
            </td>
        </tr>
        <tr>
            <td class="display-label">
                MD5 Checksum
            </td>
            <td class="display-field">
                <%: Model.Md5 %>
            </td>
        </tr>
    </table>
<% Html.RenderPartial("CoreDetailsView"); %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Displays detailed information regarding the document version.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("Document", "Details", "Documents", new { id = Model.Document.Id }, null)%></li>
</asp:Content>