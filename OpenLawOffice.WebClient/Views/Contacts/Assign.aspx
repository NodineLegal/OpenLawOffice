<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Assign
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Assign</h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Fields</legend>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.Id) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.Id) %>
            <%: Html.ValidationMessageFor(model => model.Id) %>
        </div>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.Role) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.Role) %>
            <%: Html.ValidationMessageFor(model => model.Role) %>
        </div>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.UtcCreated) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.UtcCreated) %>
            <%: Html.ValidationMessageFor(model => model.UtcCreated) %>
        </div>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.UtcModified) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.UtcModified) %>
            <%: Html.ValidationMessageFor(model => model.UtcModified) %>
        </div>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.UtcDisabled) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.UtcDisabled) %>
            <%: Html.ValidationMessageFor(model => model.UtcDisabled) %>
        </div>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsStub) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.IsStub) %>
            <%: Html.ValidationMessageFor(model => model.IsStub) %>
        </div>
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>