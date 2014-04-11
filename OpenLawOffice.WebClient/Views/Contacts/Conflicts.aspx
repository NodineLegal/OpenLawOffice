<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.ConflictViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Conflicts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Conflicts Check for <%: Model.Contact.DisplayName %></h2>

    Below are the contacts which have been involved in matters with <%: Model.Contact.DisplayName %>.  Please
    review each entry for conflicts.

    <% foreach (var mRelation in Model.Matters)
       { %>
        <h3><%: mRelation.Matter.Title%> (<%: Html.ActionLink("View", "Details", "Matters", new { id = mRelation.Matter.Id }, null)%>)</h3>
        
        <table class="listing_table">
                <tr>
                    <th>
                        Display Name
                    </th>
                    <th>
                        City, State
                    </th>
                    <th style="width: 150px;"></th>
                </tr>

            <% foreach (var matterContact in mRelation.MatterContacts)
               { %>
    
                <tr>
                    <td>
                        <%: matterContact.Contact.DisplayName%>
                    </td>
                    <td>
                        <%: matterContact.Role%>
                    </td>
                    <td>
                        <%: Html.ActionLink("Contact", "Details", new { id = matterContact.Contact.Id })%>
                    </td>
                </tr>
    
            <% } %>

        </table>
    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

