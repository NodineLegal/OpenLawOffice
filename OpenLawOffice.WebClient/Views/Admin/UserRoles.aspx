<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Account.SelectableUserRoleViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User's Roles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Roles of <%: Request["Username"] %><a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    
    <div style="width: 70px; padding-bottom: 5px;">
        <a id="SelectAll" href="javascript:void(0);">Select All</a><br />
        <a id="DeselectAll" href="javascript:void(0);">Deselect All</a>
<script language="javascript">

    $(function () {    
        var cbs = [];

        <% 
        foreach (var item in Model)
        { %>
            <%= "cbs.push('" + item.Username + "_" + item.Rolename + "');" %>
          <%
        }
        %>

        $("#SelectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', true);
            }
        });

        $("#DeselectAll").click(function () {
            for (var i = 0; i < cbs.length; i++)
            {
                $("#CB_" + cbs[i]).prop('checked', false);
            }
        });
    });
</script>
    </div>
    

    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="listing_table">
        <tr>
            <th style="text-align: center; width: 20px;" />
            <th style="text-align: center;">Role</th>
        </tr>

    <% foreach (var item in Model) { %>    
        <tr>
            <td>
                <input type="checkbox" id="CB_<%: item.Username %>_<%: item.Rolename %>" 
                    name="CB_<%: item.Username %>_<%: item.Rolename %>" <% if (item.IsSelected) { %>checked<% } %>/>
            </td>
            <td>
                <%: item.Rolename %>
            </td>
        </tr>
        <% } %>
    </table>

    <input type="submit" value="Save" style="margin-top: 5px;" />
    
    <% } %>

    </table>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page shows a list of roles and indicates those roles that the user has with checkmarks.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        A checkmark means the user has the role.  To update the user check or uncheck the desired roles
        and then click save.
        </p>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New User", "CreateUser") %></li>
        <li>
            <%: Html.ActionLink("Details", "DetailsUser", new { id = Request["Username"] })%></li>
        <li>
            <%: Html.ActionLink("Edit", "EditUser", new { id = Request["Username"] })%></li>
        <li>
            <%: Html.ActionLink("Disable", "DisableUser", new { id = Request["Username"] })%></li>
        <li>
            <%: Html.ActionLink("Unlock", "UnlockUser", new { id = Request["Username"] })%></li>
        <li>
            <%: Html.ActionLink("Change Password", "ChangePassword", new { id = Model.Username })%></li>
        <li>
            <%: Html.ActionLink("Reset Password", "ResetPassword", new { id = Model.Username })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("List", "Index") %></li>
</asp:Content>
