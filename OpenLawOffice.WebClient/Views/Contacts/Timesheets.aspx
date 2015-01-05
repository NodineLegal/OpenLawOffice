<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteNoRightBar.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Contacts.TimesheetsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Timesheets
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>    
    #opt_selected {
      margin-top: 20px;
      font-size: 20px;
    }

    .print_container {
      margin: 20px 30px 10px 30px ;
      display: inline;
    }
 
    .print_menu {
      position: absolute;
      width: 240px !important;
      margin-top: 3px !important;
    }
 
    /* fix for jquery-ui-bootstrap theme */
    #print_launcher span {
      display: inline;
    }
    </style>
            
    <% using (Html.BeginForm())
       {%>
    <%: Html.ValidationSummary(true) %>

    <script>
        $(function () {
            $("#from").datepicker({
                autoSize: true,
                onSelect: function (date) {
                    $("form").submit();
                }
            });
            $("#to").datepicker({
                autoSize: true,
                onSelect: function (date) {
                    $("form").submit();
                }
            });
            $("#print_drop").jui_dropdown({
                launcher_id: 'print_launcher',
                launcher_container_id: 'print_container',
                menu_id: 'print_menu',
                containerClass: 'print_container',
                menuClass: 'print_menu',
                launcher_is_UI_button: false,
                onSelect: function (event, data) {
                    if (data.id == 'print_client') {
                        window.open('/Contacts/Timesheets_PrintClient/<%: Model.Contact.Id %>?from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    } else if (data.id == 'print_3rdparty') {
                        window.open('/Contacts/Timesheets_Print3rdParty/<%: Model.Contact.Id %>?from=' + encodeURIComponent($('#from').val()) + '&to=' + encodeURIComponent($('#to').val()),
                            'PrintWindow', 'width=1024,height=768,scrollbars=yes');
                    }
                }
            });
        });
    </script>
    
    <div class="options_div" style="height: 22px;">
        <div style="width: 200px; display: inline;">
            From: <input type="text" id="from" name="from" style="width: 75px;" <% if (ViewData["From"] != null ) { %>value="<%: ((DateTime)ViewData["From"]).ToString("MM/dd/yyyy") %>"<% } %> />
        </div>
        <div style="width: 200px; display: inline;">
            To: <input type="text" id="to" name="to" style="width: 75px;" <% if (ViewData["To"] != null ) { %>value="<%: ((DateTime)ViewData["To"]).ToString("MM/dd/yyyy") %>"<% } %> />
        </div>
        <div style="width: 200px; display: inline; float: right; text-align: right;"> 
            <div id="print_drop" style="text-align: left; display: inline;">
                <div id="print_container" style="display: inline;">
                    <button id="print_launcher" style="background-image: url('/Content/fugue-icons-3.5.6/icons-shadowless/printer.png'); 
                        background-position: left center; background-repeat: no-repeat; padding-left: 20px;">Print</button>
                </div>
                <ul id="print_menu">
                    <li id="print_client"><a href="javascript:void(0);">Client View</a></li>
                    <li id="print_3rdparty"><a href="javascript:void(0);">3rd Party View</a></li>
                </ul>
            </div>
        </div>
    </div>


    <% } %>
    
</asp:Content>
