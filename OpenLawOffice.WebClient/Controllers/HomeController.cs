using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        [SecurityFilter]
        public ActionResult Index()
        {
            ViewModels.Home.DashboardViewModel viewModel = new ViewModels.Home.DashboardViewModel();
            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            viewModel.MyTodoList = new List<ViewModels.Tasks.TaskViewModel>();

            Data.Tasks.Task.GetTodoListFor(currentUser).ForEach(x =>
            {
                viewModel.MyTodoList.Add(Mapper.Map<ViewModels.Tasks.TaskViewModel>(x));
            });

            return View(viewModel);
        }

        [SecurityFilter]
        public ActionResult About()
        {
            return View();
        }
    }
}
