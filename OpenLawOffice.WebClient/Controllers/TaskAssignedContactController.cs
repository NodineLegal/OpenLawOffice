namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class TaskAssignedContactController : BaseController
    {
        // Selects link based on Guid of Matter
        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign(long id)
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();
            List<Common.Models.Contacts.Contact> contactList = OpenLawOffice.Data.Contacts.Contact.List();

            contactList.ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult AssignContact(int id)
        {
            long taskId = 0;

            if (Request["TaskId"] == null)
                return View("InvalidRequest");

            if (!long.TryParse(Request["TaskId"], out taskId))
                return View("InvalidRequest");

            ViewModels.Tasks.TaskAssignedContactViewModel vm = new ViewModels.Tasks.TaskAssignedContactViewModel();

            vm.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(OpenLawOffice.Data.Tasks.Task.Get(taskId));
            vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(OpenLawOffice.Data.Contacts.Contact.Get(id));

            return View(vm);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult AssignContact(ViewModels.Tasks.TaskAssignedContactViewModel model)
        {
            // We need to reset the Id of the model as it is picking up the id from the route, 
            // which is incorrect
            model.Id = null;

            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

            Common.Models.Tasks.TaskAssignedContact taskContact =
                OpenLawOffice.Data.Tasks.TaskAssignedContact.Get(model.Task.Id.Value, model.Contact.Id.Value);

            if (taskContact == null)
            { // Create
                taskContact = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(model);
                taskContact = OpenLawOffice.Data.Tasks.TaskAssignedContact.Create(taskContact, currentUser);
            }
            else
            { // Enable
                taskContact = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(model);
                taskContact = OpenLawOffice.Data.Tasks.TaskAssignedContact.Enable(taskContact, currentUser);
            }

            return RedirectToAction("Contacts", "Tasks",
                new { id = taskContact.Task.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Tasks.TaskAssignedContactViewModel viewModel = null;

            Common.Models.Tasks.TaskAssignedContact model = OpenLawOffice.Data.Tasks.TaskAssignedContact.Get(id);
            model.Task = OpenLawOffice.Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Contact = OpenLawOffice.Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskAssignedContact model = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(viewModel);
                model = OpenLawOffice.Data.Tasks.TaskAssignedContact.Edit(model, currentUser);

                return RedirectToAction("Contacts", "Tasks",
                    new { id = model.Task.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
            }
        }
        
        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskAssignedContactViewModel viewModel = null;

            Common.Models.Tasks.TaskAssignedContact model = OpenLawOffice.Data.Tasks.TaskAssignedContact.Get(id);
            model.Task = OpenLawOffice.Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Contact = OpenLawOffice.Data.Contacts.Contact.Get(model.Contact.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Contact);
            PopulateCoreDetails(viewModel);

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.TaskAssignedContact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskAssignedContactViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskAssignedContact model = Mapper.Map<Common.Models.Tasks.TaskAssignedContact>(viewModel);
                model = OpenLawOffice.Data.Tasks.TaskAssignedContact.Disable(model, currentUser);

                return RedirectToAction("Contacts", "Tasks",
                    new { id = model.Task.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
