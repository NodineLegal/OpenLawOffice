<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Events.EventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Create Event<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
    <table class="detail_table">
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
                All Day Event<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.CheckBoxFor(model => model.AllDay)%>
                <%: Html.ValidationMessageFor(model => model.AllDay)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Start<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.Start)%>
                <%: Html.ValidationMessageFor(model => model.Start)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                End
            </td>
            <td class="display-field">
                <%: Html.EditorFor(model => model.End)%>
                <%: Html.ValidationMessageFor(model => model.End)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Location
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Location)%>
                <%: Html.ValidationMessageFor(model => model.Location)%>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Description
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(model => model.Description)%>
                <%: Html.ValidationMessageFor(model => model.Description)%>
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
        Fill in the information on this page to create a new event.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server"> 
</asp:Content>

