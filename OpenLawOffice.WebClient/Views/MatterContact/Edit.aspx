<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Contact Assignment to Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Edit Contact Assignment to Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
            
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Matter
            </td>
            <td class="display-field">
                <%: Model.Matter.Title %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                User
            </td>
            <td class="display-field">
                <%: Model.Contact.DisplayName %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Role<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(x => x.Role) %>
                <%: Html.ValidationMessageFor(x => x.Role)%>
                Special Roles:  <a href="#" id="LeadAttorney">Lead Attorney</a>, 
                                <a href="#" id="Attorney">Attorney</a>, 
                                <a href="#" id="OpposingAttorney">Opposing Attorney</a>, 
                                <a href="#" id="Client">Client</a>, 
                                <a href="#" id="AppointedClient">Appointed Client</a>, 
                                <a href="#" id="OpposingParty">Opposing Party</a>
                <script language="javascript">
                    $("#LeadAttorney").click(function () {
                        $("#Role").val("Lead Attorney");
                        return false;
                    });
                    $("#Attorney").click(function () {
                        $("#Role").val("Attorney");
                        return false;
                    });
                    $("#OpposingAttorney").click(function () {
                        $("#Role").val("Opposing Attorney");
                        return false;
                    });
                    $("#Client").click(function () {
                        $("#Role").val("Client");
                        return false;
                    });
                    $("#AppointedClient").click(function () {
                        $("#Role").val("Appointed Client");
                        return false;
                    });
                    $("#OpposingParty").click(function () {
                        $("#Role").val("Opposing Party");
                        return false;
                    });
                </script>
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
        Modify the information on this page to make changes to the assignmnet of a contact to a matter.  Required fields are indicated with an
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
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete", "Delete", new { id = Model.Id })%></li>
    </ul>    
    <li><%: Html.ActionLink("Contacts of Matter", "Contacts", "Matters", new { id = Model.Matter.Id.Value }, null)%></li>
</asp:Content>