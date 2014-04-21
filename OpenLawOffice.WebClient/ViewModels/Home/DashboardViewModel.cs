using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenLawOffice.WebClient.ViewModels.Home
{
    public class DashboardViewModel
    {
        public List<Tasks.TaskViewModel> MyTodoList { get; set; }
    }
}