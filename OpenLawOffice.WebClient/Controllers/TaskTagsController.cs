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

    [HandleError(View = "Errors/Index", Order = 10)]
    public class TaskTagsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            Common.Models.Matters.Matter matter;
            ViewModels.Tasks.TaskTagViewModel viewModel;
            Common.Models.Tasks.TaskTag model;

            model = Data.Tasks.TaskTag.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);

            PopulateCoreDetails(viewModel);

            matter = Data.Tasks.Task.GetRelatedMatter(model.Task.Id.Value);
            ViewData["Task"] = model.Task.Title;
            ViewData["TaskId"] = model.Task.Id;
            ViewData["Matter"] = matter.Title;
            ViewData["MatterId"] = matter.Id;

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create(long id)
        {
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task model = Data.Tasks.Task.Get(id);

            matter = Data.Tasks.Task.GetRelatedMatter(model.Id.Value);
            ViewData["Task"] = model.Title;
            ViewData["TaskId"] = model.Id;
            ViewData["Matter"] = matter.Title;
            ViewData["MatterId"] = matter.Id;

            return View(new ViewModels.Tasks.TaskTagViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model)
            });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Tasks.TaskTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);

            model.Task = new Common.Models.Tasks.Task()
            {
                Id = long.Parse(RouteData.Values["Id"].ToString())
            };

            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);

            model = Data.Tasks.TaskTag.Create(model, currentUser);

            return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Matters.Matter matter;
            ViewModels.Tasks.TaskTagViewModel viewModel;
            Common.Models.Tasks.TaskTag model;

            model = Data.Tasks.TaskTag.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);

            PopulateCoreDetails(viewModel);

            matter = Data.Tasks.Task.GetRelatedMatter(model.Task.Id.Value);
            ViewData["Task"] = model.Task.Title;
            ViewData["TaskId"] = model.Task.Id;
            ViewData["Matter"] = matter.Title;
            ViewData["MatterId"] = matter.Id;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Tasks.TaskTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);
            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);
            model.Task = Data.Tasks.TaskTag.Get(id).Task;

            model = Data.Tasks.TaskTag.Edit(model, currentUser);

            return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id, ViewModels.Tasks.TaskTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Tasks.TaskTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Tasks.TaskTag>(viewModel);

            model = Data.Tasks.TaskTag.Disable(model, currentUser);

            return RedirectToAction("Tags", "Tasks", new { Id = model.Task.Id.Value.ToString() });
        }
    }
}