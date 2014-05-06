<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.ConflictViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Conflicts Check for <%: Model.Contact.DisplayName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Conflicts Check for
        <%: Model.Contact.DisplayName %><a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    Below are the contacts which have been involved in matters with
    <%: Model.Contact.DisplayName %>. Please review each entry for conflicts.
    <% foreach (var mRelation in Model.Matters)
       { %>
    <h3>
        <%: Html.ActionLink(mRelation.Matter.Title, "Details", "Matters", new { id = mRelation.Matter.Id }, null)%></h3>
    <table class="listing_table">
        <tr>
            <th>
                Display Name
            </th>
            <th>
                City, State
            </th>
        </tr>
        <% foreach (var matterContact in mRelation.MatterContacts)
           { %>
        <tr>
            <td>
                <%: Html.ActionLink(matterContact.Contact.DisplayName, "Details", new { id = matterContact.Contact.Id })%>                
            </td>
            <td>
                <%: matterContact.Role%>
            </td>
        </tr>
        <% } %>
    </table>
    <% } %>

    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        This page shows contacts having been involved (either previously or currently) in matters with
        <%: Model.Contact.DisplayName %>.  Each matter and contact should be reviewed to determine if
        an actual conflict exists.  Note, we do not determine conflicts, we just determine potential 
        conflicts leaving the actual determination to the attorney.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        Click the name of the matter to go to the details of that matter.  Click the name of the contact
        to view his/her information.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("Contact", "Details", new { id = RouteData.Values["Id"].ToString() })%></li>
</asp:Content>