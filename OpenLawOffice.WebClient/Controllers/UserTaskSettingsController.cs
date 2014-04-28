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
        public ActionResult Index()
        {
            List<Common.Models.Settings.TagFilter> taskTagFilterList;
            ViewModels.Settings.UserTaskSettingsViewModel viewModel;
            Common.Models.Security.User currentUser;

            viewModel = new ViewModels.Settings.UserTaskSettingsViewModel();

            currentUser = UserCache.Instance.Lookup(Request);

            taskTagFilterList = Data.Settings.UserTaskSettings.ListTagFiltersFor(currentUser);

            viewModel.MyTasksFilter = new List<ViewModels.Settings.TagFilterViewModel>();

            taskTagFilterList.ForEach(x =>
            {
                viewModel.MyTasksFilter.Add(Mapper.Map<ViewModels.Settings.TagFilterViewModel>(x));
            });

            return View(viewModel);
        }

        public ActionResult DetailsFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel;
            Common.Models.Settings.TagFilter model;

            model = Data.Settings.UserTaskSettings.GetTagFilter(id);

            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        public ActionResult CreateFilter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFilter(ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = UserCache.Instance.Lookup(Request);

            viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
            model.User = currentUser;

            model = Data.Settings.UserTaskSettings.CreateTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }

        public ActionResult EditFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel;
            Common.Models.Settings.TagFilter model;

            model = Data.Settings.UserTaskSettings.GetTagFilter(id);
            model.User = Data.Security.User.Get(model.User.Id.Value);

            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = UserCache.Instance.Lookup(Request);

            viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
            model.User = currentUser;

            model = Data.Settings.UserTaskSettings.EditTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteFilter(long id)
        {
            return DetailsFilter(id);
        }

        [HttpPost]
        public ActionResult DeleteFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Settings.TagFilter model;

            currentUser = UserCache.Instance.Lookup(Request);

            viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };

            model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);

            Data.Settings.UserTaskSettings.DeleteTagFilter(model, currentUser);

            return RedirectToAction("Index");
        }
    }
}