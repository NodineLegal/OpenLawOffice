namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    public class TaskResponsibleUsersController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskResponsibleUserViewModel viewModel = null;

            Common.Models.Tasks.TaskResponsibleUser model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Get(id);
            model.Task = OpenLawOffice.Data.Tasks.Task.Get(model.Task.Id.Value);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskResponsibleUserViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(long id)
        {
            List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();
            Common.Models.Tasks.Task task = OpenLawOffice.Data.Tasks.Task.Get(id);
            ViewModels.Tasks.TaskViewModel taskViewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task);
            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;
            return View(new ViewModels.Tasks.TaskResponsibleUserViewModel() { Task = taskViewModel });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Tasks.TaskResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Tasks.TaskResponsibleUser model = Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(viewModel);
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                // Is there already an entry for this user?
                Common.Models.Tasks.TaskResponsibleUser currentResponsibleUser =
                    OpenLawOffice.Data.Tasks.TaskResponsibleUser.GetIgnoringDisable(
                    long.Parse(RouteData.Values["Id"].ToString()), currentUser.Id.Value);

                if (currentResponsibleUser != null)
                { // Update
                    if (!currentResponsibleUser.UtcDisabled.HasValue)
                    {
                        ModelState.AddModelError("User", "This user already has a responsibility.");
                        List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();
                        Common.Models.Tasks.Task task = OpenLawOffice.Data.Tasks.Task.Get(currentResponsibleUser.Task.Id.Value);
                        ViewModels.Tasks.TaskViewModel taskViewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task);
                        OpenLawOffice.Data.Security.User.List().ForEach(x =>
                        {
                            userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                        });

                        ViewData["UserList"] = userViewModelList;
                        return View(new ViewModels.Tasks.TaskResponsibleUserViewModel() { Task = taskViewModel });
                    }

                    model.Id = currentResponsibleUser.Id;
                    model.Responsibility = model.Responsibility;

                    // Remove disability
                    model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Enable(model, currentUser);

                    // Update responsibility
                    model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Edit(model, currentUser);
                }
                else
                { // Insert
                    model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Create(model, currentUser);
                }

                return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value.ToString() });
            }
            catch (Exception)
            {
                return Create(long.Parse(RouteData.Values["Id"].ToString()));
            }
        }

        //
        // GET: /ResponsibleUsers/Edit/5
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Tasks.TaskResponsibleUserViewModel viewModel = null;
            List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();

            Common.Models.Tasks.TaskResponsibleUser model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Get(id);
            model.Task = OpenLawOffice.Data.Tasks.Task.Get(model.Task.Id.Value);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskResponsibleUserViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);

            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskResponsibleUser model = Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(viewModel);
                model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Edit(model, currentUser);

                return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value });
            }
            catch
            {
                return Edit(id);
            }
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskResponsibleUser model = Data.Tasks.TaskResponsibleUser.Get(id);
                model = OpenLawOffice.Data.Tasks.TaskResponsibleUser.Disable(model, currentUser);

                return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value });
            }
            catch
            {
                return Details(id);
            }
        }
    }
}