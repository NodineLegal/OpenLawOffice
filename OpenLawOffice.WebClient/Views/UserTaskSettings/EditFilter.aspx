<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Settings.TagFilterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditFilter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.10.4.custom.min.js"></script>

    <h2>EditFilter</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
    <table class="detail_table">
        <tr>
            <td class="display-label">Category</td>
            <td class="display-field">
                <script type="text/javascript">
                    $(function () {
                        $("#Category").autocomplete({
                            source: "../../TagCategories/ListNameOnly",
                            minLength: 2,
                            focus: function (event, ui) {
                                $("#Category").val(ui.item.Name);
                                return false;
                            },
                            select: function (event, ui) {
                                $("#Category").val(ui.item.Name);
                                return false;
                            }
                        }).data("ui-autocomplete")._renderItem = function (ul, item) {
                            return $("<li>")
                            .append("<a>" + item.Name + "</a>")
                            .appendTo(ul);
                        };
                    });
                </script>
                <%: Html.TextBoxFor(x => x.Category) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">Tag</td>
            <td class="display-field"><%: Html.TextBoxFor(x => x.Tag) %></td>
        </tr>
    </table>
            
    <p>
        <input type="submit" value="Save" />
    </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("New Filter", "CreateFilter")%></li>
        <li><%: Html.ActionLink("Details ", "DetailsFilter", new { id = Model.Id })%></li>
        <li><%: Html.ActionLink("Delete", "DeleteFilter", new { id = Model.Id })%></li>
    </ul>
    <li><%: Html.ActionLink("Task Settings", "Index") %></li>
</asp:Content>
