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

    public class MatterTagsController : BaseController
    {
        //
        // GET: /MatterTags/Details/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel viewModel = null;
            Common.Models.Matters.MatterTag model = OpenLawOffice.Data.Matters.MatterTag.Get(id);
            viewModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        //
        // GET: /MatterTags/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(Guid id)
        {
            Common.Models.Matters.Matter matter = OpenLawOffice.Data.Matters.Matter.Get(id);

            return View(new ViewModels.Matters.MatterTagViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter)
            });
        } 

        //
        // POST: /MatterTags/Create/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.MatterTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.MatterTag model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);
                model = OpenLawOffice.Data.Matters.MatterTag.Create(model, currentUser);
                return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
            }
            catch (Exception)
            {
                Common.Models.Matters.Matter matter = OpenLawOffice.Data.Matters.Matter.Get(
                    Guid.Parse(RouteData.Values["Id"].ToString()));

                return View(new ViewModels.Matters.MatterTagViewModel()
                {
                    Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter)
                });
            }
        }
        
        //
        // GET: /MatterTags/Edit/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel viewModel = null;
            Common.Models.Matters.MatterTag model = OpenLawOffice.Data.Matters.MatterTag.Get(id);
            viewModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        //
        // POST: /MatterTags/Edit/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.MatterTag model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);
                model = OpenLawOffice.Data.Matters.MatterTag.Edit(model, currentUser);
                return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
            }
        }

        //
        // GET: /MatterTags/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        //
        // POST: /MatterTags/Delete/{Guid}
        [SecurityFilter(SecurityAreaName = "Matters.MatterTag", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Matters.MatterTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Matters.MatterTag model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);
                model = OpenLawOffice.Data.Matters.MatterTag.Disable(model, currentUser);
                return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
            }
            catch
            {
                return Details(id);
            }
        }
    }
}
