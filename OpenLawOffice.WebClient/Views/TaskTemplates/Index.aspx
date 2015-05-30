<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" 
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Tasks.TaskTemplateViewModel>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Task Templates
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>
        Task Templates<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
        
    <table class="listing_table">
        <tr>
            <th>
                Template Title
            </th>
            <th style="width: 45px;">
            </th>
        </tr>
        <% bool altRow = true; 
           foreach (var item in Model)
           { 
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.TaskTemplateTitle, "Details", new { id = item.Id })%>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Tasks templates allow to easy and quick task creation.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make changes to the task.
        </p>
    </div>
</asp:Content>
