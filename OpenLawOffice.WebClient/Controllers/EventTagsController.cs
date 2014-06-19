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
    public class EventTagsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            ViewModels.Events.EventTagViewModel viewModel;
            Common.Models.Events.EventTag model;

            model = Data.Events.EventTag.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventTagViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create(Guid id)
        {
            Common.Models.Events.Event model = Data.Events.Event.Get(id);

            return View(new ViewModels.Events.EventTagViewModel()
            {
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(model)
            });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Events.EventTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Events.EventTag>(viewModel);

            model.Event = new Common.Models.Events.Event()
            {
                Id = Guid.Parse(RouteData.Values["Id"].ToString())
            };

            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);

            model = Data.Events.EventTag.Create(model, currentUser);

            return RedirectToAction("Tags", "Events", new { Id = model.Event.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Events.EventTagViewModel viewModel;
            Common.Models.Events.EventTag model;

            model = Data.Events.EventTag.Get(id);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            viewModel = Mapper.Map<ViewModels.Events.EventTagViewModel>(model);
            viewModel.Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Events.EventTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Events.EventTag>(viewModel);
            model.TagCategory = Mapper.Map<Common.Models.Tagging.TagCategory>(viewModel.TagCategory);
            model.Event = Data.Events.EventTag.Get(id).Event;

            model = Data.Events.EventTag.Edit(model, currentUser);

            return RedirectToAction("Tags", "Events", new { Id = model.Event.Id.Value.ToString() });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id)
        {
            return Details(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(Guid id, ViewModels.Events.EventTagViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Events.EventTag model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Events.EventTag>(viewModel);

            model = Data.Events.EventTag.Disable(model, currentUser);

            return RedirectToAction("Tags", "Events", new { Id = model.Event.Id.Value.ToString() });
        }
    }
}
