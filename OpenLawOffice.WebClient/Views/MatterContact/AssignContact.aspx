<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Assign Contact to Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Assign Contact to Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
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
                <%: Html.HiddenFor(model => model.Matter.Id)%>
                <%: Html.ActionLink(Model.Matter.Title, "Details", "Matters", new { id = Model.Matter.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Contact
            </td>
            <td class="display-field">
                <%: Html.HiddenFor(model => model.Contact.Id) %>
                <%: Html.ActionLink(Model.Contact.DisplayName, "Details", "Contacts", new { id = Model.Contact.Id }, null)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Role<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Role) %>
                <%: Html.ValidationMessageFor(model => model.Role)%><br />
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
        Fill in the information on this page to assign a contact to a matter.  Required fields are indicated with an
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
            <%: Html.ActionLink("New Contact", "Create", "Contacts")%></li>
    </ul>
    <li><%: Html.ActionLink("Matter", "Details", "Matters", new { id = Request["MatterId"] }, null) %></li>
    <li><%: Html.ActionLink("Contacts of Matter", "Contacts", "Matters", new { id = Request["MatterId"] }, null)%></li>
</asp:Content>