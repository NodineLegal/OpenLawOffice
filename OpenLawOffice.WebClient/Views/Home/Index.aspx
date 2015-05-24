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
                $("form").submit();
            });
        });
    </script>
    <h4>
        Todo List for <%: Html.DropDownListFor(x => x.Employee.Id,
                new SelectList((IList)ViewData["EmployeeContactList"], "Id", "DisplayName"),
                new { @size = 1, @style = "width: 200px" })%></h4>
    <table class="listing_table">
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
                <%: Html.ActionLink("Add Time", "SelectContactToAssign", "TaskTime", new { TaskId = item.Item2.Id.Value }, new { @class = "btn-addtimeentry" })%>
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