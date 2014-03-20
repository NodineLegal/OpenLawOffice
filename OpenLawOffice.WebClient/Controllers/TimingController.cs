namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using AutoMapper;

    public class TimingController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(long? id)
        {
            ViewModels.JqGridObject jqObject;
            int level = 0;

            List<ViewModels.Timing.TimeViewModel> modelList = new List<ViewModels.Timing.TimeViewModel>();
            List<object> anonList = new List<object>();

            if (!string.IsNullOrEmpty(Request["n_level"]))
                level = int.Parse(Request["n_level"]) + 1;

            string taskid = Request["TaskId"];
            if (!string.IsNullOrEmpty(taskid))
                modelList = GetTimesForTask(long.Parse(taskid));

            modelList.ForEach(x =>
            {
                anonList.Add(new
                {
                    Id = x.Id,
                    Start = x.Start,
                    Stop = x.Stop,
                    Worker = x.WorkerDisplayName
                });
            });

            jqObject = new ViewModels.JqGridObject()
            {
                TotalPages = 1,
                CurrentPage = 1,
                TotalRecords = modelList.Count,
                Rows = anonList.ToArray()
            };

            return Json(jqObject, JsonRequestBehavior.AllowGet);
        }

        public static List<ViewModels.Timing.TimeViewModel> GetTimesForTask(long id)
        {
            List<ViewModels.Timing.TimeViewModel> viewModelList = new List<ViewModels.Timing.TimeViewModel>();
            List<Common.Models.Timing.Time> modelList = OpenLawOffice.Data.Timing.Time.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Timing.TimeViewModel viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);
                Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
                viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                viewModel.WorkerDisplayName = viewModel.Worker.DisplayName;
            });

            return viewModelList;
        }

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            Common.Models.Timing.Time model = OpenLawOffice.Data.Timing.Time.Get(id);
            ViewModels.Timing.TimeViewModel viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(model);
            Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
            viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
            Common.Models.Tasks.Task task = OpenLawOffice.Data.Timing.Time.GetRelatedTask(model.Id.Value);
            PopulateCoreDetails(viewModel);
            ViewData["TaskId"] = task.Id.Value;
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Timing.Time model = OpenLawOffice.Data.Timing.Time.Get(id);
            ViewModels.Timing.TimeViewModel viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(model);
            Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
            viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
            Common.Models.Tasks.Task task = OpenLawOffice.Data.Timing.Time.GetRelatedTask(model.Id.Value);
            ViewData["TaskId"] = task.Id.Value;
            return View(viewModel);
        }        

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Timing.TimeViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Timing.Time model = Mapper.Map<Common.Models.Timing.Time>(viewModel);
                model = OpenLawOffice.Data.Timing.Time.Edit(model, currentUser);
                return RedirectToAction("Details", new { Id = id });
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
