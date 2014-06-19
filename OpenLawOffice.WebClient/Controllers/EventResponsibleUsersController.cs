// -----------------------------------------------------------------------
// <copyright file="EventResponsibleUsersController.cs" company="Nodine Legal, LLC">
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
    public class EventResponsibleUsersController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            ViewModels.Events.EventResponsibleUserViewModel viewModel;
            Common.Models.Events.EventResponsibleUser model;

            model = Data.Events.EventResponsibleUser.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);
            model.User = Data.Account.Users.Get(model.User.PId.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventResponsibleUserViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);
            viewModel.User = Mapper.Map<ViewModels.Account.UsersViewModel>(model.User);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create(Guid id)
        {
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Events.Event evnt;
            ViewModels.Events.EventViewModel eventViewModel;

            userViewModelList = new List<ViewModels.Account.UsersViewModel>();

            evnt = OpenLawOffice.Data.Events.Event.Get(id);

            eventViewModel = Mapper.Map<ViewModels.Events.EventViewModel>(evnt);

            Data.Account.Users.List().ForEach(x =>
            {
                userViewModelList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
            });

            ViewData["UserList"] = userViewModelList;

            return View(new ViewModels.Events.EventResponsibleUserViewModel() { Event = eventViewModel });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Events.EventResponsibleUserViewModel viewModel)
        {
            Common.Models.Events.EventResponsibleUser model;
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventResponsibleUser currentResponsibleUser;
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Events.Event evnt;
            ViewModels.Events.EventViewModel eventViewModel;

            model = Mapper.Map<Common.Models.Events.EventResponsibleUser>(viewModel);
            currentUser = Data.Account.Users.Get(User.Identity.Name);

            // Is there already an entry for this user?
            currentResponsibleUser = Data.Events.EventResponsibleUser.GetIgnoringDisable(
                Guid.Parse(RouteData.Values["Id"].ToString()), currentUser.PId.Value);

            if (currentResponsibleUser != null)
            { // Update
                if (!currentResponsibleUser.Disabled.HasValue)
                {
                    ModelState.AddModelError("User", "This user already has a responsibility.");

                    userViewModelList = new List<ViewModels.Account.UsersViewModel>();

                    evnt = Data.Events.Event.Get(currentResponsibleUser.Event.Id.Value);

                    eventViewModel = Mapper.Map<ViewModels.Events.EventViewModel>(evnt);

                    Data.Account.Users.List().ForEach(x =>
                    {
                        userViewModelList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
                    });

                    ViewData["UserList"] = userViewModelList;

                    return View(new ViewModels.Events.EventResponsibleUserViewModel() { Event = eventViewModel });
                }

                model.Id = currentResponsibleUser.Id;
                model.Responsibility = model.Responsibility;

                // Remove disability
                model = Data.Events.EventResponsibleUser.Enable(model, currentUser);

                // Update responsibility
                model = Data.Events.EventResponsibleUser.Edit(model, currentUser);
            }
            else
            { // Insert
                model = Data.Events.EventResponsibleUser.Create(model, currentUser);
            }

            return RedirectToAction("ResponsibleUsers", "Events", new { Id = model.Event.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Events.EventResponsibleUserViewModel viewModel;
            List<ViewModels.Account.UsersViewModel> userViewModelList;
            Common.Models.Events.EventResponsibleUser model;

            userViewModelList = new List<ViewModels.Account.UsersViewModel>();

            model = Data.Events.EventResponsibleUser.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);
            model.User = Data.Account.Users.Get(model.User.PId.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventResponsibleUserViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);
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
        public ActionResult Edit(Guid id, ViewModels.Events.EventResponsibleUserViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventResponsibleUser model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Events.EventResponsibleUser>(viewModel);

            model = Data.Events.EventResponsibleUser.Edit(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Events", new { Id = model.Event.Id.Value });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id, ViewModels.Events.EventResponsibleUserViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventResponsibleUser model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Data.Events.EventResponsibleUser.Get(id);

            model = Data.Events.EventResponsibleUser.Disable(model, currentUser);

            return RedirectToAction("ResponsibleUsers", "Events", new { Id = model.Event.Id.Value });
        }
    }
}
