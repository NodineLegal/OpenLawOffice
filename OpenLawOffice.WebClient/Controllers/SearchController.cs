// -----------------------------------------------------------------------
// <copyright file="SearchController.cs" company="Nodine Legal, LLC">
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

    public class SearchController : Controller
    {
        public ActionResult Tags()
        {
            return View(new ViewModels.Search.TagSearchViewModel()
                {
                    SearchMatters = true,
                    SearchTasks = true
                });
        }

        [HttpPost]
        public ActionResult Tags(ViewModels.Search.TagSearchViewModel viewModel)
        {
            List<Common.Models.Matters.MatterTag> matterTags = Data.Matters.MatterTag.Search(viewModel.Query.ToLower());
            List<Common.Models.Tasks.TaskTag> taskTags = Data.Tasks.TaskTag.Search(viewModel.Query.ToLower());

            if (viewModel.SearchMatters)
            {
                viewModel.MatterTags = new List<ViewModels.Matters.MatterTagViewModel>();

                matterTags.ForEach(x =>
                {
                    ViewModels.Matters.MatterTagViewModel mtvm = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(x);
                    mtvm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(x.Matter);
                    viewModel.MatterTags.Add(mtvm);
                });
            }

            if (viewModel.SearchTasks)
            {
                viewModel.TaskTags = new List<ViewModels.Tasks.TaskTagViewModel>();

                taskTags.ForEach(x =>
                {
                    ViewModels.Tasks.TaskTagViewModel mtvm = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(x);
                    mtvm.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x.Task);
                    viewModel.TaskTags.Add(mtvm);
                });
            }

            return View(viewModel);
        }
    }
}