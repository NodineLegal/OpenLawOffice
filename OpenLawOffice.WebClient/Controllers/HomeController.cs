// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nodine Legal, LLC">
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
    public class HomeController : BaseController
    {
        [SecurityFilter]
        public ActionResult Index()
        {
            ViewModels.Home.DashboardViewModel viewModel;
            Common.Models.Security.User currentUser;
            List<Common.Models.Settings.TagFilter> tagFilter;            

            viewModel = new ViewModels.Home.DashboardViewModel();

            currentUser = UserCache.Instance.Lookup(Request);

            viewModel.MyTodoList = new List<ViewModels.Tasks.TaskViewModel>();

            tagFilter = Data.Settings.UserTaskSettings.ListTagFiltersFor(currentUser);

            Data.Tasks.Task.GetTodoListFor(currentUser, tagFilter).ForEach(x =>
            {
                viewModel.MyTodoList.Add(Mapper.Map<ViewModels.Tasks.TaskViewModel>(x));
            });

            return View(viewModel);
        }

        [SecurityFilter]
        public ActionResult About()
        {
            return View();
        }
    }
}