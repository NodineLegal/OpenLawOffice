// -----------------------------------------------------------------------
// <copyright file="UserTaskSettingsController.cs" company="Nodine Legal, LLC">
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
    public class UserTaskSettingsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            List<Common.Models.Settings.TagFilter> taskTagFilterList;
            ViewModels.Settings.UserTaskSettingsViewModel viewModel;
            Common.Models.Account.Users currentUser;

            viewModel = new ViewModels.Settings.UserTaskSettingsViewModel();

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            taskTagFilterList = Data.Settings.UserTaskSettings.ListTagFiltersFor(currentUser);

            viewModel.MyTasksFilter = new List<ViewModels.Settings.TagFilterViewModel>();

            taskTagFilterList.ForEach(x =>
            {
                viewModel.MyTasksFilter.Add(Mapper.Map<ViewModels.Settings.TagFilterViewModel>(x));
            });

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult DetailsFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel;
            Common.Models.Settings.TagFilter model;

            model = Data.Settings.UserTaskSettings.GetTagFilter(id);

            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult CreateFilter()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult CreateFilter(ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            viewModel.User = new ViewModels.Account.UsersViewModel() { PId = currentUser.PId };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
            model.User = currentUser;

            model = Data.Settings.UserTaskSettings.CreateTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult EditFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel;
            Common.Models.Settings.TagFilter model;

            model = Data.Settings.UserTaskSettings.GetTagFilter(id);
            model.User = Data.Account.Users.Get(model.User.PId.Value);

            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Account.UsersViewModel>(model.User);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult EditFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            viewModel.User = new ViewModels.Account.UsersViewModel() { PId = currentUser.PId };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
            model.User = currentUser;

            model = Data.Settings.UserTaskSettings.EditTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult DeleteFilter(long id)
        {
            return DetailsFilter(id);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult DeleteFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            viewModel.User = new ViewModels.Account.UsersViewModel() { PId = currentUser.PId };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);

            Data.Settings.UserTaskSettings.DeleteTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }
    }
}