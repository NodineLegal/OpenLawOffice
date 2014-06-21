<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<ul>
    <li>
        My</li>
    <ul>
        <li>
            <%: Html.ActionLink("Dashboard", "Index", "Home") %></li>
        <li>
            <%: Html.ActionLink("Daily Time", "DayView", "Timing") %></li>
    </ul>
    <ul>
    </ul>
    <% if (Page.User.IsInRole("User"))
       {  %>
    <li>
        Calendar</li>
    <ul>
        <li>
            <%: Html.ActionLink("Master", "Index", "Calendar", null, null)%></li>
        <li>
            <%: Html.ActionLink("User", "SelectUser", "Calendar", null, null)%></li>
        <li>
            <%: Html.ActionLink("Contact", "SelectContact", "Calendar", null, null)%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matters", "", "Matters")%></li>
    <ul>
        <li>
            <%: Html.ActionLink("New Matter", "Create", "Matters")%></li>
    </ul>
    <li>
        Events</li>
    <ul>
        <li>
            <%: Html.ActionLink("New Event", "Create", "Events", null, null)%></li>
        <li>
            <%: Html.ActionLink("User Agenda", "SelectUser", "Events", null, null)%></li>
        <li>
            <%: Html.ActionLink("Contact Agenda", "SelectContact", "Events", null, null)%></li>
    </ul>
    <li>Search</li>
    <ul>
        <li>
            <%: Html.ActionLink("Tags", "Tags", "Search")%></li>
        <li>Document Text</li>
    </ul>
    <li>
        <%: Html.ActionLink("Contacts", "", "Contacts")%></li>
    <ul>
        <li>
            <%: Html.ActionLink("New Contact", "Create", "Contacts")%></li>
    </ul>
    <ul>
    </ul>
    <li>Billing</li>
    <ul>
    </ul>
    <li>Security</li>
    <ul>
        <%--<li>
            <%: Html.ActionLink("Areas", "", "SecurityAreas") %></li>
        <li>
            <%: Html.ActionLink("Area Access", "", "SecurityAreaAcls") %></li>--%>
        <li>
            <%: Html.ActionLink("Users", "", "Users")%></li>
    </ul>
    <ul>
    </ul>
    <li>Settings</li>
    <ul>
        <li>
            <%: Html.ActionLink("Tasks", "Index", "UserTaskSettings")%></li>
    </ul>
    <ul>
    </ul>
    <% } %>
    <% if (Page.User.IsInRole("Admin"))
       { %>
    <li><%: Html.ActionLink("Admin", "Index", "Admin")%></li>
    <ul>
    </ul>
    <% } %>
    <li><%: Html.ActionLink("About", "About", "Home") %></li>
</ul>