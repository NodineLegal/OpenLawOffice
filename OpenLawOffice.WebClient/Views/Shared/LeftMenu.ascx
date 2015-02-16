<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<ul>
    <li>
        My</li>
    <ul>
        <li>
            <%: Html.ActionLink("Dashboard", "Index", "Home") %></li>
        <li>
            <%: Html.ActionLink("Daily Time", "DayView", "Timing") %></li>
       <%-- <li>
            <%: Html.ActionLink("Calendar", "User", "Calendar", new { Id = "" }, null)%></li>
        <li>
            <%: Html.ActionLink("Agenda", "UserAgenda", "Events", new { Id = "" }, null)%></li>--%>
    </ul>
    <ul>
    </ul>
    <% if (Page.User.IsInRole("User"))
       {  %>
    <li>
        <%: Html.ActionLink("Contacts", "", "Contacts")%> <%: Html.ActionLink("New Contact", "Create", "Contacts", null, new { @class = "btn-plus" })%></li>
    <ul>
    </ul>
    <li>
        <%: Html.ActionLink("Matters", "", "Matters")%> <%: Html.ActionLink("New Matter", "Create", "Matters", null, new { @class = "btn-plus" })%></li>
    <ul>
    </ul>
    <%--<li>
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
        Events <%: Html.ActionLink("New Event", "Create", "Events", null, new { @class = "btn-plus" })%></li>
    <ul>
        <li>
            </li>
        <li>
            <%: Html.ActionLink("User Agenda", "SelectUser", "Events", null, null)%></li>
        <li>
            <%: Html.ActionLink("Contact Agenda", "SelectContact", "Events", null, null)%></li>
    </ul>--%>
    <li>Search</li>
    <ul>
        <li>
            <%: Html.ActionLink("Tags", "Tags", "Search")%></li>
        <li>Document Text</li>
    </ul>
    <li><%: Html.ActionLink("Billing", "", "Billing")%> </li>
    <ul>
        <li><%: Html.ActionLink("Rates", "Index", "BillingRates") %> <%: Html.ActionLink("New Rate", "Create", "BillingRates", null, new { @class = "btn-plus" })%></li>
        <li><%: Html.ActionLink("Groups", "Index", "BillingGroups") %> <%: Html.ActionLink("New Group", "Create", "BillingGroups", null, new { @class = "btn-plus" })%></li>
    </ul>
    <ul>
    </ul>
    <li>Settings</li>
    <ul>
        <li>
            <%: Html.ActionLink("My Email", "EditUser", "Account")%></li>
        <li>
            <%: Html.ActionLink("My Profile", "Profile", "Account")%></li>
        <li>
            <%: Html.ActionLink("My Tasks", "Index", "UserTaskSettings")%></li>
    </ul>
    <ul>
    </ul>
    <% } %>
    <% 
        var a = Page.User.IsInRole("User");
        if (Page.User.IsInRole("Admin"))
       { %>
    <li><%: Html.ActionLink("Admin", "Index", "Admin")%></li>
    <ul>
    </ul>
    <% } %>
    <li><%: Html.ActionLink("About", "About", "Home") %></li>
</ul>