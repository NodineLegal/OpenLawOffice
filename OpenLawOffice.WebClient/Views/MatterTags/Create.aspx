<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Matters.MatterTagViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Tag for Matter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create Tag for Matter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Matter
            </td>
            <td class="display-field">
                <%: Model.Matter.Title %>
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
        Fill in the information on this page to create a new tag for the matter.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Start typing a category, potential options will appear that you may select.  If an option does not appear for the desired 
        category, then type the full category name and it will be created along with the tag.
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Actions</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li>
            <%: Html.ActionLink("List", "Tags", "Matters", new { id = Request["Id"] }, null)%></li>
    </ul>
</asp:Content>