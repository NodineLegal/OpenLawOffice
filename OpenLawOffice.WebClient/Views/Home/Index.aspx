<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Home.DashboardViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    OpenLawOffice
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <% using (Html.BeginForm())
       {%>
    <script>
        $(function () {
            $('#Employee_Id').change(function () {
                window.location = "/Home/Index/" + $("#Employee_Id").val();
                //$("form").submit();
            });
        });
    </script>
    <h4>
        Dashboard for <%: Html.DropDownListFor(x => x.Employee.Id,
                new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                new { @size = 1, @style = "width: 200px" })%></h4>
                
    <table class="listing_table">   
        <tr>
            <td colspan="3" class="listing_table_heading">
                Notifications
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Body
            </th>
            <th style="text-align: center; width: 25px;">
                
            </th>
        </tr>
        <% bool altRow = true;
           foreach (OpenLawOffice.WebClient.ViewModels.Notes.NoteNotificationViewModel item in Model.NotificationList)
           {
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Note.Title, "Details", "Notes", new { id = item.Note.Id.Value }, null)%>
            </td>
            <td>
                <%: item.Note.Body %>
            </td>
            <td>
                <%: Html.ActionLink("Clear", "ClearNotification", "Notes", new { id = item.Id.Value, EmployeeId = Model.Employee.Id }, new { @class = "btn-remove", title = "Clear" })%>   
            </td>
        </tr>
        <% } %>
    </table>
    <br />
    <table class="listing_table">   
        <tr>
            <td colspan="3" class="listing_table_heading">
                Todo List
            </td>
        </tr>
        <tr>
            <th style="text-align: center;">
                Matter
            </th>
            <th style="text-align: center;">
                Task
            </th>
            <th style="text-align: center;">
                Due Date
            </th>
        </tr>
        <% foreach (var item in Model.MyTodoList)
           {
               %>         
        <% if (item.Item2.DueDate.HasValue &&
               item.Item2.DueDate.Value.Date < DateTime.Now.Date)
           { %>
        <tr style="background-color: #FFCECE"> 
        <% }
           else if (item.Item2.DueDate.HasValue &&
             item.Item2.DueDate.Value.Date == DateTime.Now.Date)
           { %>
        <tr style="background-color: #FFFFC8"> 
        <% }
           else
           { %>
        <tr>
        <% } %>
            <td>
                <%: Html.ActionLink(item.Item1.Title, "Details", "Matters", new { id = item.Item1.Id.Value }, null)%>
            </td>
            <td>
                <%: Html.ActionLink(item.Item2.Title, "Details", "Tasks", new { id = item.Item2.Id.Value }, null)%>
                <%: Html.ActionLink("Add Time", "Create", "TaskTime", new { ContactId = Model.Employee.Id, TaskId = item.Item2.Id.Value }, new { @class = "btn-addtimeentry", title = "Add Time" })%>
            </td>
            <td>
                <% if (item.Item2.DueDate.HasValue)
                   { %>
                <%: String.Format("{0:g}", item.Item2.DueDate.Value)%>
                <% } %>
            </td>
        </tr>
        <% } %>
    </table>

    <% } %>
</asp:Content>