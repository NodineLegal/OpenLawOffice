<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Documents.DocumentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm("Create", "Documents", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
        <%: Html.ValidationSummary(true)%>
        
        <% if (Request["MatterId"] != null) { %>
            <%: Html.Hidden("MatterId", Request["MatterId"]) %>
        <% }
           else if (Request["TaskId"] != null)
           { %>
            <%: Html.Hidden("TaskId", Request["TaskId"])%>
        <% } %>

        <table class="detail_table">
            <tr>
                <td class="display-label">Title</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Title)%>
                    <%: Html.ValidationMessageFor(model => model.Title)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">File</td>
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
