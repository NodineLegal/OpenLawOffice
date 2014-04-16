namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class TimingController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
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

        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
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

        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
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
