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

    public class SecurityAreaAclsController : BaseController
    {
        //
        // GET: /SecurityAreaAcls/
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.AreaAclViewModel> modelList = new List<ViewModels.Security.AreaAclViewModel>();
            OpenLawOffice.Data.Security.AreaAcl.List().ForEach(x =>
            {
                x.User = OpenLawOffice.Data.Security.User.Get(x.User.Id.Value);
                x.Area = OpenLawOffice.Data.Security.Area.Get(x.Area.Id.Value);
                ViewModels.Security.AreaAclViewModel vm = Mapper.Map<ViewModels.Security.AreaAclViewModel>(x);
                vm.User = Mapper.Map<ViewModels.Security.UserViewModel>(x.User);
                vm.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(x.Area);
                modelList.Add(vm);
            });

            return View(modelList);
        }

        //
        // GET: /SecurityAreaAcls/Details/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Security.AreaAclViewModel viewModel = null;

            Common.Models.Security.AreaAcl model = OpenLawOffice.Data.Security.AreaAcl.Get(id);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);
            model.Area = OpenLawOffice.Data.Security.Area.Get(model.Area.Id.Value);

            viewModel = Mapper.Map<ViewModels.Security.AreaAclViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            viewModel.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(model.Area);
            PopulateCoreDetails(viewModel);

            return View(model);
        }

        //
        // GET: /SecurityAreaAcls/Create
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            ViewData["Readonly"] = false;
            ViewData["UserList"] = userList;

            return View(new ViewModels.Security.AreaAclViewModel() 
            {
                AllowPermissions = new ViewModels.PermissionsViewModel(),
                DenyPermissions = new ViewModels.PermissionsViewModel()
            });
        } 

        //
        // POST: /SecurityAreaAcls/Create
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Security.AreaAclViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Security.AreaAcl model = Mapper.Map<Common.Models.Security.AreaAcl>(viewModel);
                model = OpenLawOffice.Data.Security.AreaAcl.Create(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                List<ViewModels.Security.UserViewModel> userList = new List<ViewModels.Security.UserViewModel>();
                OpenLawOffice.Data.Security.User.List().ForEach(x =>
                {
                    userList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                });

                ViewData["Readonly"] = false;
                ViewData["UserList"] = userList;

                return View(viewModel);
            }
        }
        
        //
        // GET: /SecurityAreaAcls/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.AreaAclViewModel viewModel = null;

            Common.Models.Security.AreaAcl model = OpenLawOffice.Data.Security.AreaAcl.Get(id);
            model.User = OpenLawOffice.Data.Security.User.Get(model.User.Id.Value);
            model.Area = OpenLawOffice.Data.Security.Area.Get(model.Area.Id.Value);

            viewModel = Mapper.Map<ViewModels.Security.AreaAclViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            viewModel.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(model.Area);

            List<SelectListItem> selectList = new List<SelectListItem>();

            OpenLawOffice.Data.Security.User.List().ForEach(x =>
            {
                SelectListItem slItem = new SelectListItem()
                {
                    Value = x.Id.Value.ToString(),
                    Text = x.Username
                };
                if (x.Id == model.User.Id.Value)
                    slItem.Selected = true;
                selectList.Add(slItem);
            });

            ViewData["UserList"] = selectList;
            return View(model);
        }

        //
        // POST: /SecurityAreaAcls/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Security.AreaAclViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Security.AreaAcl model = Mapper.Map<Common.Models.Security.AreaAcl>(viewModel);
                model = OpenLawOffice.Data.Security.AreaAcl.Edit(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
