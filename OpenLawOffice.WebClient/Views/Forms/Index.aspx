<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Forms.FormViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forms
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Forms<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
        
    <table class="listing_table">
        <tr>
            <th>
                Title
            </th>
            <th>
                Matter Type
            </th>
            <th style="width: 65px;">
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
                <%: Html.ActionLink(item.Title, "Details", new { id = item.Id })%>
            </td>
            <td>
                <%: item.MatterType.Title %>
            </td>
            <td style="text-align: center;">
                <%: Html.ActionLink("Download", "Download", new { id = item.Id }, new { @class = "btn-download", title = "Download" })%>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }, new { @class = "btn-edit", title = "Edit" })%>
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id }, new { @class = "btn-remove", title = "Remove" })%>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>
