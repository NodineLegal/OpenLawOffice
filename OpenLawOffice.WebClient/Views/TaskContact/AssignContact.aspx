<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Tasks.TaskAssignedContactViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AssignContact
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>AssignContact</h2>

    <% using (Html.BeginForm()) {
            List<object> list = new List<object>();
            list.Add(new { Id = 1, Name = "Direct" });
            list.Add(new { Id = 2, Name = "Assigned" });
            SelectList assignmentList = new SelectList(list, "Id", "Name");
           
           
           %>
        <%: Html.ValidationSummary(true) %>

        <table class="detail_table">
            <tr>
                <td class="display-label">Task</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Task.Id)%>
                    <%: Model.Task.Title%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Contact</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Contact.Id) %>
                    <%: Model.Contact.DisplayName %>
                </td>
            </tr>
            <tr>
                <td class="display-label">Assignment</td>
                <td class="display-field">
                    <%: Html.DropDownListFor(model => model.AssignmentType, assignmentList) %>
                    <%: Html.ValidationMessageFor(model => model.AssignmentType)%>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
