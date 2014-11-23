<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenLawOffice.WebClient.ViewModels.Matters.MatterViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matters
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server"> 
    <h2>Matters<a id="pageInfo" class="btn-question" style="padding-left: 15px;">Help</a></h2>

    
    <div class="options_div">
        <div style="width: 200px; display: inline;">
            Active: 
            <select id="activeSelector">
                <option value="active">Active</option>
                <option value="inactive">Inactive</option>
                <option value="both">Both</option>
            </select>

            <script language="javascript">
                var vars = [], hash;
                var q = document.URL.split('?')[1];
                if (q != undefined) {
                    q = q.split('&');
                    for (var i = 0; i < q.length; i++) {
                        hash = q[i].split('=');
                        vars.push(hash[1]);
                        vars[hash[0]] = hash[1];
                    }
                }
                $(document).ready(function () {
                    if (vars['active'] != null)
                        $('#activeSelector').val(vars['active']);
                    if (vars['contactFilter'] != null)
                        $('#contactFilter').val(decodeURIComponent(vars['contactFilter']));
                    if (vars['titleFilter'] != null)
                        $('#titleFilter').val(decodeURIComponent(vars['titleFilter']));
                    if (vars['caseNumberFilter'] != null)
                        $('#caseNumberFilter').val(decodeURIComponent(vars['caseNumberFilter']));
                    if (vars['jurisdictionFilter'] != null)
                        $('#jurisdictionFilter').val(decodeURIComponent(vars['jurisdictionFilter']));

                    $("#activeSelector").change(function () {
                        go();
                    });
                    $('#goButton').click(function () {
                        go();
                    });
                    $('#contactFilter').focus(function () {
                        $('#contactFilter').val('');
                    });
                    $('#titleFilter').focus(function () {
                        $('#titleFilter').val('');
                    });
                    $('#caseNumberFilter').focus(function () {
                        $('#caseNumberFilter').val('');
                    });
                    $('#jurisdictionFilter').focus(function () {
                        $('#jurisdictionFilter').val('');
                    });
                    $('#contactFilter').keyup(function (e) {
                        if (e.keyCode == 13) go();
                    });
                    $('#titleFilter').keyup(function (e) {
                        if (e.keyCode == 13) go();
                    });
                    $('#caseNumberFilter').keyup(function (e) {
                        if (e.keyCode == 13) go();
                    });
                    $('#jurisdictionFilter').keyup(function (e) {
                        if (e.keyCode == 13) go();
                    });
                    $('#contactFilter').autocomplete({
                        source: "/Contacts/ListDisplayNameOnly",
                        minLength: 2,
                        focus: function (event, ui) {
                            $("#contactFilter").val(ui.item.DisplayName);
                            return false;
                        },
                        select: function (event, ui) {
                            $("#contactFilter").val(ui.item.DisplayName);
                            go();
                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<a>" + item.DisplayName + "</a>")
                            .appendTo(ul);
                    };
                    $('#titleFilter').autocomplete({
                        source: "/Matters/ListTitleOnly",
                        minLength: 2,
                        focus: function (event, ui) {
                            $("#titleFilter").val(ui.item.Title);
                            return false;
                        },
                        select: function (event, ui) {
                            $("#titleFilter").val(ui.item.Title);
                            go();
                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<a>" + item.Title + "</a>")
                            .appendTo(ul);
                    };
                    $('#caseNumberFilter').autocomplete({
                        source: "/Matters/ListCaseNumberOnly",
                        minLength: 2,
                        focus: function (event, ui) {
                            $("#caseNumberFilter").val(ui.item.CaseNumber);
                            return false;
                        },
                        select: function (event, ui) {
                            $("#caseNumberFilter").val(ui.item.CaseNumber);
                            go();
                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<a>" + item.CaseNumber + "</a>")
                            .appendTo(ul);
                    };
                    $('#jurisdictionFilter').autocomplete({
                        source: "/Matters/ListJurisdictionOnly",
                        minLength: 2,
                        focus: function (event, ui) {
                            $("#jurisdictionFilter").val(ui.item.Jurisdiction);
                            return false;
                        },
                        select: function (event, ui) {
                            $("#jurisdictionFilter").val(ui.item.Jurisdiction);
                            go();
                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li>")
                            .append("<a>" + item.Jurisdiction + "</a>")
                            .appendTo(ul);
                    };
                });
                function go() {
                    var href;
                    var contactFilter = $('#contactFilter').val().trim();
                    var titleFilter = $('#titleFilter').val().trim();
                    var caseNumberFilter = $('#caseNumberFilter').val().trim();
                    var jurisdictionFilter = $('#jurisdictionFilter').val().trim();
                    var base;
                    var qMarkAt = window.location.href.lastIndexOf('?');
                    $('#contactFilterGo').attr('disabled', 'disabled');
                    if (qMarkAt > 0)
                        base = window.location.href.substr(0, qMarkAt);
                    else
                        base = window.location.href;

                    href = base + '?active=' + $("#activeSelector").val();

                    if (contactFilter.length > 0)
                        href += '&contactFilter=' + contactFilter;
                    if (titleFilter.length > 0)
                        href += '&titleFilter=' + titleFilter;
                    if (caseNumberFilter.length > 0)
                        href += '&caseNumberFilter=' + caseNumberFilter;
                    if (jurisdictionFilter.length > 0)
                        href += '&jurisdictionFilter=' + jurisdictionFilter;

                    window.location.href = href;
                };
            </script>
        </div>
        <div style="width: 200px; display: inline;">
            Contact: <input type="text" id="contactFilter" name="contactFilter" value="" />
        </div>
        <div style="width: 200px; display: inline;">
            Title: <input type="text" id="titleFilter" name="titleFilter" value="" />
        </div>
        <div style="width: 200px; display: inline;">
            Case No: <input type="text" id="caseNumberFilter" name="caseNumberFilter" value="" />
        </div>
        <div style="width: 200px; display: inline;">
            Jurisdiction: <input type="text" id="jurisdictionFilter" name="jurisdictionFilter" value="" />
        </div>
        <div style="width: 200px; display: inline;">
            <input id="goButton" type="button" value="Go" />
        </div>
    </div>

    <table class="listing_table">
        <tr>
            <th style="text-align: center;">
                Title
            </th>
            <th style="text-align: center;">
                Synopsis
            </th>
            <th style="text-align: center; width: 25px;">
                
            </th>
        </tr>
        <% bool altRow = true; 
           foreach (var item in Model)
           { 
               altRow = !altRow;
               if (altRow)
               { %> <tr class="tr_alternate"> <% }
               else
               { %> <tr> <% }
                %>
            <td>
                <%: Html.ActionLink(item.Title, "Details", "Matters", new { id = item.Id.Value }, null) %>
            </td>
            <td>
                <%: item.Synopsis %>
            </td>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Matters", new { id = item.Id.Value }, new { @class = "btn-edit", title = "Edit" })%>
            </td>
        </tr>
        <% } %>
    </table>
    
    <div id="pageInfoDialog" title="Help">
        <p>
        <span style="font-weight: bold; text-decoration: underline;">Info:</span>
        A matter is a substantial or essential thing.  They contain documents, notes, time entries and more.<br /><br />
        <span style="font-weight: bold; text-decoration: underline;">Usage:</span> 
        The arrow to the left of the title allows for expanding to view submatters (matters within matters).  
        Clicking the title will show the details of the matter including access to documents, tasks, notes and more.  
        Click the <img src="../../Content/fugue-icons-3.5.6/icons-shadowless/pencil.png" /> (edit icon) to make changes to the matter.
        </p>
    </div>
</asp:Content>