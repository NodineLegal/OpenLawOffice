// -----------------------------------------------------------------------
// <copyright file="ResponsibleUsersController.cs" company="Nodine Legal, LLC">
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

    [HandleError(View = "Errors/Index", Order = 10)]
    public class ResponsibleUsersController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(int id)
        {
            Common.Models.Matters.ResponsibleUser model;
            ViewModels.Matters.ResponsibleUserViewModel viewModel;

            model = Data.Matters.ResponsibleUser.Get(id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.User = Data.Account.Users.Get(model.User.PId.Value);

            viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.User = Mapper.Map<ViewModels.Account.UsersViewModel>(model.User);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create(Guid id)
        {
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Matters.Matter matter;
            ViewModels.Matters.MatterViewModel matterViewModel;

            matter = Data.Matters.Matter.Get(id);

            matterViewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);

            userViewModelList = new List<ViewModels.Account.UsersViewModel>();

            Data.Account.Users.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;

            return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matterViewModel });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.ResponsibleUser model;
            Common.Models.Matters.ResponsibleUser currentResponsibleUser;
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Matters.Matter matter;
            ViewModels.Matters.MatterViewModel matterViewModel;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);
            model.Matter = new Common.Models.Matters.Matter() { Id = Guid.Parse(RouteData.Values["Id"].ToString()) };

            // Is there already an entry for this user?
            currentResponsibleUser = Data.Matters.ResponsibleUser.GetIgnoringDisable(
                Guid.Parse(RouteData.Values["Id"].ToString()), currentUser.PId.Value);

            if (currentResponsibleUser != null)
            { // Update
                if (!currentResponsibleUser.Disabled.HasValue)
                {
                    ModelState.AddModelError("User", "This user already has a responsibility.");

                    matter = Data.Matters.Matter.Get(currentResponsibleUser.Matter.Id.Value);

                    matterViewModel = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter);

                    userViewModelList = new List<ViewModels.Account.UsersViewModel>();

                    Data.Account.Users.List().ForEach(x =>
                    {
                        userViewModelList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
                    });

                    ViewData["UserList"] = userViewModelList;

                    return View(new ViewModels.Matters.ResponsibleUserViewModel() { Matter = matterViewModel });
                }

                // Remove disability
                model = Data.Matters.ResponsibleUser.Enable(model, currentUser);

                // Update responsibility
                model.Responsibility = model.Responsibility;
                model = Data.Matters.ResponsibleUser.Edit(model, currentUser);
            }
            else
            { // Insert
                model = Data.Matters.ResponsibleUser.Create(model, currentUser);
            }

            return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id)
        {
            ViewModels.Matters.ResponsibleUserViewModel viewModel;
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Matters.ResponsibleUser model;

            userViewModelList = new List<ViewModels.Account.UsersViewModel>();

            model = Data.Matters.ResponsibleUser.Get(id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.User = Data.Account.Users.Get(model.User.PId.Value);

            viewModel = Mapper.Map<ViewModels.Matters.ResponsibleUserViewModel>(model);
            viewModel.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter);
            viewModel.User = Mapper.Map<ViewModels.Account.UsersViewModel>(model.User);

            Data.Account.Users.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id, ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.ResponsibleUser model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);

            model = Data.Matters.ResponsibleUser.Edit(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(int id)
        {
            return Details(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(int id, ViewModels.Matters.ResponsibleUserViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.ResponsibleUser model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Matters.ResponsibleUser>(viewModel);

            model = Data.Matters.ResponsibleUser.Disable(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Matters", new { Id = model.Matter.Id.Value });
        }
    }
}