// -----------------------------------------------------------------------
// <copyright file="MatterTagsController.cs" company="Nodine Legal, LLC">
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
    using System;
    using System.Web.Mvc;
    using AutoMapper;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class MatterTagsController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel viewModel;
            Common.Models.Matters.MatterTag model;

            model = Data.Matters.MatterTag.Get(id);

            viewModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(model);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(Guid id)
        {
            Common.Models.Matters.Matter matter;

            matter = Data.Matters.Matter.Get(id);

            return View(new ViewModels.Matters.MatterTagViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter)
            });
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Matters.MatterTagViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterTag model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);

            model.Matter = new Common.Models.Matters.Matter()
            {
                Id = Guid.Parse(RouteData.Values["Id"].ToString())
            };

            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);

            model = Data.Matters.MatterTag.Create(model, currentUser);

            return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Matters.MatterTagViewModel viewModel;
            Common.Models.Matters.MatterTag model;

            model = OpenLawOffice.Data.Matters.MatterTag.Get(id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);

            viewModel = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Matters.MatterTagViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterTag model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);
            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);
            model.Matter = Data.Matters.MatterTag.Get(id).Matter;

            model = Data.Matters.MatterTag.Edit(model, currentUser);

            return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [SecurityFilter(SecurityAreaName = "Matters", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(Guid id, ViewModels.Matters.MatterTagViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Matters.MatterTag model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Matters.MatterTag>(viewModel);

            model = Data.Matters.MatterTag.Disable(model, currentUser);

            return RedirectToAction("Tags", "Matters", new { Id = model.Matter.Id.Value.ToString() });
        }
    }
}