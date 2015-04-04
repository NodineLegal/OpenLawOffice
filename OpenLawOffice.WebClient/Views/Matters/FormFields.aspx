<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<List<OpenLawOffice.WebClient.ViewModels.Forms.FormFieldMatterViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Matter Form Fields
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="roadmap">
        <div class="zero">Matter: [<%: Html.ActionLink((string)ViewData["Matter"], "Details", "Matters", new { id = ViewData["MatterId"] }, null) %>]</div>
        <div id="current" class="one">Matter Form Fields<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></div>
    </div>
    
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Value
            </th>
        </tr>
        <% bool altRow = true; 
           for (int i=0; i<Model.Count; i++)
           {
               OpenLawOffice.WebClient.ViewModels.Forms.FormFieldMatterViewModel item = Model[i];
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: item.FormField.Title %>
            </td>
            <td>
                <%: Html.TextBoxFor(x => x[i].Value, new { @style = "width: 100%;" })%>
                <%: Html.HiddenFor(x => x[i].FormField.Id) %>
            </td>
        </tr>
        <% } %>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Make changes to form fields for the matter.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Make changes then click save to save them.  
        </p>
    </div>
</asp:Content>
