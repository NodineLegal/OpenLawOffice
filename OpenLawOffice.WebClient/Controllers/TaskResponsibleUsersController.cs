// -----------------------------------------------------------------------
// <copyright file="TaskResponsibleUsersController.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    [HandleError(View = "Errors/", Order = 10)]
    public class TaskResponsibleUsersController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Tasks.TaskResponsibleUserViewModel viewModel;
            Common.Models.Tasks.TaskResponsibleUser model;

            model = Data.Tasks.TaskResponsibleUser.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.User = Data.Security.User.Get(model.User.Id.Value);

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
            List<ViewModels.Security.UserViewModel> userViewModelList;
            Common.Models.Tasks.Task task;
            ViewModels.Tasks.TaskViewModel taskViewModel;

            userViewModelList = new List<ViewModels.Security.UserViewModel>();

            task = OpenLawOffice.Data.Tasks.Task.Get(id);

            taskViewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task);

            Data.Security.User.List().ForEach(x =>
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
            Common.Models.Tasks.TaskResponsibleUser model;
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskResponsibleUser currentResponsibleUser;
            List<ViewModels.Security.UserViewModel> userViewModelList;
            Common.Models.Tasks.Task task;
            ViewModels.Tasks.TaskViewModel taskViewModel;

            model = Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(viewModel);
            currentUser = UserCache.Instance.Lookup(Request);

            // Is there already an entry for this user?
            currentResponsibleUser = Data.Tasks.TaskResponsibleUser.GetIgnoringDisable(
                long.Parse(RouteData.Values["Id"].ToString()), currentUser.Id.Value);

            if (currentResponsibleUser != null)
            { // Update
                if (!currentResponsibleUser.UtcDisabled.HasValue)
                {
                    ModelState.AddModelError("User", "This user already has a responsibility.");

                    userViewModelList = new List<ViewModels.Security.UserViewModel>();

                    task = Data.Tasks.Task.Get(currentResponsibleUser.Task.Id.Value);

                    taskViewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task);

                    Data.Security.User.List().ForEach(x =>
                    {
                        userViewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
                    });

                    ViewData["UserList"] = userViewModelList;

                    return View(new ViewModels.Tasks.TaskResponsibleUserViewModel() { Task = taskViewModel });
                }

                model.Id = currentResponsibleUser.Id;
                model.Responsibility = model.Responsibility;

                // Remove disability
                model = Data.Tasks.TaskResponsibleUser.Enable(model, currentUser);

                // Update responsibility
                model = Data.Tasks.TaskResponsibleUser.Edit(model, currentUser);
            }
            else
            { // Insert
                model = Data.Tasks.TaskResponsibleUser.Create(model, currentUser);
            }

            return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value.ToString() });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Tasks.TaskResponsibleUserViewModel viewModel;
            List<ViewModels.Security.UserViewModel> userViewModelList;
            Common.Models.Tasks.TaskResponsibleUser model;

            userViewModelList = new List<ViewModels.Security.UserViewModel>();

            model = Data.Tasks.TaskResponsibleUser.Get(id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.User = Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskResponsibleUserViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);

            Data.Security.User.List().ForEach(x =>
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
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskResponsibleUser model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Tasks.TaskResponsibleUser>(viewModel);

            model = Data.Tasks.TaskResponsibleUser.Edit(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value });
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
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskResponsibleUser model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Data.Tasks.TaskResponsibleUser.Get(id);

            model = Data.Tasks.TaskResponsibleUser.Disable(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Tasks", new { Id = model.Task.Id.Value });
        }
    }
}