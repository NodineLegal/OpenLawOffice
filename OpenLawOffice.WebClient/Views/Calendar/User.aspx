<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Calendar.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        var calFormatTime = function (datetime) {
            var hours = datetime.getHours();
            var meridiem;
            var minutes = datetime.getMinutes();
            if (hours >= 12) {
                // PM
                hours -= 12;
                meridiem = 'pm';
            } else {
                // AM
                meridiem = 'am';
            }
            if (hours == 0) hours = '12';
            if (minutes < 10 && minutes != 0) minutes = '0' + minutes;
            if (minutes == 0)
                return hours + meridiem;
            return hours + ':' + minutes + meridiem;
        }

        var calFormatDate = function (datetime) {
            return $.datepicker.formatDate('D, MM d', datetime);
        }

        var calFormatDateTime = function (datetime) {
            return $.datepicker.formatDate('D, MM d', datetime) + ', ' + calFormatTime(datetime);
        }

        var calFormatStartStop = function (start, end) {
            if (end == null)
                return calFormatDateTime(start);

            if (start.getYear() == end.getYear() &&
                start.getMonth() == end.getMonth() &&
                start.getDay() == end.getDay()) {
                // Starts and stops in same day
                return calFormatDateTime(start) + ' - ' + calFormatTime(end);
            } else {
                return calFormatDateTime(start) + ' - ' + calFormatDateTime(end);
            }
        }

        $(document).ready(function () {
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: true,
                eventSources: [
                    {
                        name: 'Event',
                        url: '/Calendar/ListEventsForUser/<%: RouteData.Values["Id"] %>'
                    },
                    {
                        name: 'Task',
                        url: '/Tasks/TodoListForUser/<%: RouteData.Values["Id"] %>',
                        color: 'rgb(22, 167, 101)'
                    }
                ],
                eventClick: function (calEvent, jsEvent, view) {
                    if (calEvent.source.name == 'Event') {
                        if ($("#taskViewDialog").dialog("isOpen")) $("#taskViewDialog").dialog("close");
                        $("#eventViewDialog_Date").html(calFormatStartStop(calEvent.start, calEvent.end));
                        $("#eventViewDialog_Location").html(calEvent.location);
                        $("#eventViewDialog").dialog("open");
                        $("#eventViewDialog").dialog("option", "title", calEvent.title);
                    } else if (calEvent.source.name == 'Task') {
                        if ($("#eventViewDialog").dialog("isOpen")) $("#eventViewDialog").dialog("close");
                        $("#taskViewDialog_Date").html(calFormatDate(calEvent.start));
                        if (calEvent.description.length > 120)
                            $("#taskViewDialog_Description").html(calEvent.description.substring(0, 120) + "...");
                        else
                            $("#taskViewDialog_Description").html(calEvent.description);
                        $("#taskViewDialog").dialog("open");
                        $("#taskViewDialog").dialog("option", "title", calEvent.title);
                    }
                }
            });

            $("#eventViewDialog").dialog({
                autoOpen: false,
                width: 400,
                show: {
                    effect: "blind",
                    duration: 100
                },
                hide: {
                    effect: "fade",
                    duration: 100
                }
            });

            $("#taskViewDialog").dialog({
                autoOpen: false,
                width: 400,
                show: {
                    effect: "blind",
                    duration: 100
                },
                hide: {
                    effect: "fade",
                    duration: 100
                }
            });

        });

    </script>

    <style>

	    #calendar {
		    width: 900px;
		    margin: 0 auto;
		    }

    </style>
    <div id='calendar'></div>
    
    <div id="eventViewDialog" title="View Event" style="font-size: 14px;">
        <div id="eventViewDialog_Date" style="padding: 5px 0 10px 0;"></div>
        <div style="font-weight: bold;">
            Where <span id="eventViewDialog_Location" style="padding: 5px 0 20px 0; font-weight: normal;"></span><br />
            Type <span style="padding: 5px 0 20px 0; font-weight:normal;">Event</span>
        </div>
        <hr style="line-height: 1px;" />
        <span>View event »</span><span style="float: right;">Edit event »</span>
    </div>
    
    <div id="taskViewDialog" title="View Task" style="font-size: 14px;">
        <div style="font-weight: bold;">
            Due <span id="taskViewDialog_Date" style="padding: 10px 0 10px 0; font-weight:normal;"></span><br />
            Type <span style="padding: 5px 0 20px 0; font-weight:normal;">Task</span><br /><br />
            <div id="taskViewDialog_Description" style="font-weight: normal; font-size: 12px;"></div>
        </div>
        <hr style="line-height: 1px;" />
        <span>View task »</span><span style="float: right;">Edit task »</span>
    </div>
</asp:Content>
