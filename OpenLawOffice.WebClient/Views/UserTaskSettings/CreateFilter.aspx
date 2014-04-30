<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Settings.TagFilterViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create Tag Filter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create Tag Filter<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>
    <table class="detail_table">
        <tr>
            <td class="display-label">
                Category
            </td>
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
        Fill in the information on this page to create a tag filter.  Required fields are indicated with an
        <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span><br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span>
        Fields marked with an <span style="color: #ee0000;font-size: 12px;cursor:help;" title="Required Field">*</span> are required.
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>
        <%: Html.ActionLink("Task Settings", "Index") %></li>
</asp:Content>