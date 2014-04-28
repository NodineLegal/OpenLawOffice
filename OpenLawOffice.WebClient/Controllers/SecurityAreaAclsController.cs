// -----------------------------------------------------------------------
// <copyright file="SecurityAreaAclsController.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.WebClient.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class SecurityAreaAclsController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.AreaAclViewModel> modelList;
            ViewModels.Security.AreaAclViewModel vm;

            modelList = new List<ViewModels.Security.AreaAclViewModel>();

            Data.Security.AreaAcl.List().ForEach(x =>
            {
                x.User = OpenLawOffice.Data.Security.User.Get(x.User.Id.Value);
                x.Area = OpenLawOffice.Data.Security.Area.Get(x.Area.Id.Value);

                vm = Mapper.Map<ViewModels.Security.AreaAclViewModel>(x);
                vm.User = Mapper.Map<ViewModels.Security.UserViewModel>(x.User);
                vm.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(x.Area);

                modelList.Add(vm);
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Security.AreaAclViewModel viewModel;
            Common.Models.Security.AreaAcl model;

            model = Data.Security.AreaAcl.Get(id);
            model.User = Data.Security.User.Get(model.User.Id.Value);
            model.Area = Data.Security.Area.Get(model.Area.Id.Value);

            viewModel = Mapper.Map<ViewModels.Security.AreaAclViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            viewModel.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(model.Area);

            PopulateCoreDetails(viewModel);

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            List<ViewModels.Security.UserViewModel> userList;

            userList = new List<ViewModels.Security.UserViewModel>();

            Data.Security.User.List().ForEach(x =>
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

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Security.AreaAclViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Security.AreaAcl model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Security.AreaAcl>(viewModel);

            model = Data.Security.AreaAcl.Create(model, currentUser);

            return RedirectToAction("Index");
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.AreaAclViewModel viewModel;
            Common.Models.Security.AreaAcl model;
            List<SelectListItem> selectList;
            SelectListItem slItem;

            model = Data.Security.AreaAcl.Get(id);
            model.User = Data.Security.User.Get(model.User.Id.Value);
            model.Area = Data.Security.Area.Get(model.Area.Id.Value);

            viewModel = Mapper.Map<ViewModels.Security.AreaAclViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            viewModel.Area = Mapper.Map<ViewModels.Security.AreaViewModel>(model.Area);

            selectList = new List<SelectListItem>();

            Data.Security.User.List().ForEach(x =>
            {
                slItem = new SelectListItem()
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

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Security.AreaAclViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Security.AreaAcl model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Security.AreaAcl>(viewModel);

            model = Data.Security.AreaAcl.Edit(model, currentUser);

            return RedirectToAction("Index");
        }
    }
}