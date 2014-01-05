<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.Common.Models.Security.AreaAcl>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Permissions</h2>
    

        <table>
            <tr>
                <th style="text-align: center;">
                    User
                </th>
                <th style="text-align: center;">
                    Allowed
                </th>
                <th style="text-align: center;">
                    Denied
                </th>
                <th></th>
            </tr>

        <%        
            ViewDataDictionary dict = new ViewDataDictionary();
            dict.Add("Readonly", true);
            foreach (var item in Model) { %>
    
            <tr>
                <td>
                    <%: item.User.Username %>
                </td>
                <td>                
                    <% Html.RenderPartial("PermissionsControl", item.AllowFlags, dict);  %>
                </td>
                <td>                
                    <% Html.RenderPartial("PermissionsControl", item.DenyFlags, dict);  %>
                </td>
                <td>
                    <%: Html.ActionLink("Edit", "Edit", "SecurityAreaAcls", new { id=item.Id }, null) %> |
                    <%: Html.ActionLink("Details", "Details", "SecurityAreaAcls", new { id=item.Id }, null)%>
                </td>
            </tr>
    
        <% } %>

    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

