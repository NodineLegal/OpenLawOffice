<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.Common.Models.Security.Area>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><%: Html.ActionLink("List", "Index") %></li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciPlugin.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciTree.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciTree.selectable.js"></script>

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
                

        <table class="detail_table">
            <tr>
                <td class="display-label">Name</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Name)%>
                    <%: Html.ValidationMessageFor(model => model.Name)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Description</td>
                <td class="display-field">
                    <%: Html.TextBoxFor(model => model.Description)%>
                    <%: Html.ValidationMessageFor(model => model.Description)%>
                </td>
            </tr>
            <tr>
                <td class="display-label">Parent</td>
                <td class="display-field">
                    <%: Html.HiddenFor(model => model.Parent.Id) %>
                    Active Parent: <%: Html.TextBoxFor(model => model.Parent.Name, new { style = "width: 190px;" })%>
                    <input name="clear" type="button" value="clear" />
                    <div id="tree" class="aciTree" style="height: 200px; width: 315px;"></div>
                </td>
            </tr>
        </table>
            
        <p>
            <input type="submit" value="Save" />
        </p>

    <script type="text/javascript">
        $(function () {
            $('#tree').on('acitree', function (event, api, item, eventName, options) {
                // get the item id
                var itemId = api.getId(item);
                var itemLabel = api.getLabel(item);
                if (eventName == 'selected') {
                    $('#Parent_Id').val(itemId);
                    $('#Parent_Name').val(itemLabel);
                }
            });

            // init the tree
            $('#tree').aciTree({
                ajax: {
                    url: 'AciTreeList'
                },
                selectable: true
            });

            $('[name=clear]').click(function () {
                $('#Parent_Id').val(null);
                $('#Parent_Name').val(null);
            });

        });
    </script>

    <% } %>

</asp:Content>


