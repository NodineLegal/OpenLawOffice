// -----------------------------------------------------------------------
// <copyright file="TaskTagsController.cs" company="Nodine Legal, LLC">
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

    public class TaskTagsController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskTagViewModel viewModel = null;
            Common.Models.Tasks.TaskTag model = OpenLawOffice.Data.Tasks.TaskTag.Get(id);
            viewModel = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create(long id)
        {
            Common.Models.Tasks.Task model = OpenLawOffice.Data.Tasks.Task.Get(id);

            return View(new ViewModels.Tasks.TaskTagViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model)
            });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskTag model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);
                model.Task = new Common.Models.Tasks.Task() { Id = long.Parse(RouteData.Values["Id"].ToString()) };
                model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);
                model = OpenLawOffice.Data.Tasks.TaskTag.Create(model, currentUser);

                return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
            }
            catch (Exception)
            {
                Common.Models.Tasks.Task model = OpenLawOffice.Data.Tasks.Task.Get(
                    long.Parse(RouteData.Values["Id"].ToString()));

                return View(new ViewModels.Tasks.TaskTagViewModel()
                {
                    Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model)
                });
            }
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Tasks.TaskTagViewModel viewModel = null;
            Common.Models.Tasks.TaskTag model = OpenLawOffice.Data.Tasks.TaskTag.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            viewModel = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskTag model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);
                model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);
                model.Task = Data.Tasks.TaskTag.Get(id).Task;
                model = OpenLawOffice.Data.Tasks.TaskTag.Edit(model, currentUser);
                return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
            }
            catch
            {
                return View(viewModel);
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
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.TaskTag model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);
                model = OpenLawOffice.Data.Tasks.TaskTag.Disable(model, currentUser);
                return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
            }
            catch
            {
                return Details(id);
            }
        }
    }
}