<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.Common.Models.Security.Area>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciPlugin.min.js"></script>
    <script type="text/javascript" src="../../Scripts/aciTree/js/jquery.aciTree.min.js"></script>
    <h2>Index</h2>

    <div id="tree" class="aciTree" style="height: 300px; width: 400px;"></div>
    
    <script type="text/javascript">
        $(function () {

            var selectedItemId;

            $('#tree').on('acitree', function (event, api, item, eventName, options) {
                // get the item id
                var itemId = api.getId(item);
                if (eventName == 'selected') {
                    selectedItemId = itemId;
                }
            });

            // init the tree
            $('#tree').aciTree({
                ajax: {
                    url: '../SecurityAreas/AciTreeList'
                },
                selectable: true
            });

            $('#Details').click(function () {
                if (selectedItemId != undefined)
                    window.location.href = '../SecurityAreas/Details/' + selectedItemId;
                else
                    alert('Please select an item from the list.');
            });

            $('#Permissions').click(function () {
                if (selectedItemId != undefined)
                    window.location.href = '../SecurityAreas/Permissions/' + selectedItemId;
                else
                    alert('Please select an item from the list.');
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <li>Navigation</li>
    <ul style="list-style: none outside none; padding-left: 1em;">
        <li><a id="Details" href="#">Details</a></li>
    </ul>
    <li><a id="Permissions" href="#">Permissions</a></li>
</asp:Content>
