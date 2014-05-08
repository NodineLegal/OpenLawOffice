<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterTagViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Tag
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Edit Tag<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Id
            </td>
            <td class="display-field">
                <%: Model.Id %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Matter
            </td>
            <td class="display-field">
                <%: Html.ActionLink(Model.Matter.Title, "Details", "Matters", new { id = Model.Matter.Id }, null) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Category
            </td>
            <td class="display-field">
                <script type="text/javascript">
                    $(function () {
                        $("#TagCategory_Name").autocomplete({
                            source: "../../TagCategories/ListNameOnly",
                            minLength: 2,
                            focus: function (event, ui) {
                                $("#TagCategory_Name").val(ui.item.Name);
                                return false;
                            },
                            select: function (event, ui) {
                                $("#TagCategory_Name").val(ui.item.Name);
                                return false;
                            }
                        }).data("ui-autocomplete")._renderItem = function (ul, item) {
                            return $("<li>")
                            .append("<a>" + item.Name + "</a>")
                            .appendTo(ul);
                        };
                    });
                </script>
                <%: Html.TextBoxFor(x => x.TagCategory.Name) %>
            </td>
        </tr>
        <tr>
            <td class="display-label">
                Tag<span class="required-field" title="Required Field">*</span>
            </td>
            <td class="display-field">
                <%: Html.TextBoxFor(x => x.Tag) %>
            </td>
        </tr>
    </table>
    <p>
        <input type="submit" value="Save" />
    </p>
    <% } %>
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        Modify the information on this page to make changes to the tag.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span>.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("New Matter Tag", "Create", new { id = Model.Matter.Id })%></li>
        <li>
            <%: Html.ActionLink("Details", "Details", new { id = Model.Id })%></li>
        <li>
            <%: Html.ActionLink("Delete ", "Delete", new { id = Model.Id })%></li>
    </ul>
    <li>
        <%: Html.ActionLink("Matter Tags", "Tags", "Matters", new { id = Model.Matter.Id }, null)%></li>
</asp:Content>