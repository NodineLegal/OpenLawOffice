namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class ResponsibleUsersController : BaseController
    {
        //
        // GET: /ResponsibleUsers/Details/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Matters.ResponsibleUserViewModel viewModel = null;

            Common.Models.Matters.ResponsibleUser model = OpenLawOffice.Data.Matters.ResponsibleUser.Get(id);
            model.Matter = OpenLawOffice.Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            PopulateCoreDetails(viewModel);

            return View(model);
        }

        //
        // GET: /ResponsibleUsers/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(Guid id)
        {
            List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();
            Common.Models.Matters.Matter matter = OpenLawOffice.Data.Matters.Matter.Get(id);
            ViewModels.Matters.MatterViewModel matterViewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);
            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;
            return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matterViewModel });
        }

        //
        // POST: /ResponsibleUsers/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Matters.ResponsibleUser model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);

                // Is there already an entry for this user?
                Common.Models.Matters.ResponsibleUser currentResponsibleUser =
                    OpenLawOffice.Data.Matters.ResponsibleUser.GetIgnoringDisable(
                    Guid.Parse(RouteData.Values["Id"].ToString()), currentUser.Id.Value);
                
                if (currentResponsibleUser != null)
                { // Update
                    if (!currentResponsibleUser.UtcDisabled.HasValue)
                    {
                        ModelState.AddModelError("User", "This user already has a responsibility.");
                        List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();
                        Common.Models.Matters.Matter matter = OpenLawOffice.Data.Matters.Matter.Get(currentResponsibleUser.Matter.Id.Value);
                        ViewModels.Matters.MatterViewModel matterViewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);
                        OpenLawOffice.Data.Security.User.List().ForEach(x =>
                        {
                            userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                        });

                        ViewData["UserList"] = userViewModelList;
                        return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matterViewModel });
                    }

                    // Remove disability
                    model = OpenLawOffice.Data.Matters.ResponsibleUser.Enable(model, currentUser);

                    // Update responsibility
                    model.Responsibility = model.Responsibility;
                    model = OpenLawOffice.Data.Matters.ResponsibleUser.Edit(model, currentUser);
                }
                else
                { // Insert
                    model = OpenLawOffice.Data.Matters.ResponsibleUser.Create(model, currentUser);
                }

                return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value.ToString() });
            }
            catch (Exception)
            {
                return Create(Guid.Parse(RouteData.Values["Id"].ToString()));
            }
        }
        
        //
        // GET: /ResponsibleUsers/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.ResponsibleUserViewModel viewModel = null;
            List<ViewModels.Security.UserViewModel> userViewModelList = new List<ViewModels.Security.UserViewModel>();
            
            Common.Models.Matters.ResponsibleUser model = OpenLawOffice.Data.Matters.ResponsibleUser.Get(id);
            model.Matter = OpenLawOffice.Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);

            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;
            return View(model);
        }
        
        //
        // POST: /ResponsibleUsers/Edit/5
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.ResponsibleUser model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);
                model = OpenLawOffice.Data.Matters.ResponsibleUser.Edit(model, currentUser);

                return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value });
            }
            catch
            {
                return Edit(id);
            }
        }

        //
        // GET: /ResponsibleUsers/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        //
        // POST: /ResponsibleUsers/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.ResponsibleUser", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.ResponsibleUser model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);
                model = OpenLawOffice.Data.Matters.ResponsibleUser.Disable(model, currentUser);

                return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value });
            }
            catch
            {
                return Details(id);
            }
        }
    }
}
