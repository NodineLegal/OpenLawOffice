// -----------------------------------------------------------------------
// <copyright file="TaskTimeController.cs" company="Nodine Legal, LLC">
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

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError(View = "Errors/Index", Order = 10)]
    public class TaskTimeController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult SelectContactToAssign()
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList;

            modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            long taskId;
            int contactId;
            ViewModels.Tasks.TaskTimeViewModel viewModel;
            Common.Models.Tasks.Task task;
            Common.Models.Contacts.Contact contact;

            // Every TaskTime must be created from a task, so we should always know the TaskId
            taskId = long.Parse(Request["TaskId"]);
            contactId = int.Parse(Request["ContactId"]);

            // Load task & contact
            task = Data.Tasks.Task.Get(taskId);

            contact = Data.Contacts.Contact.Get(contactId);

            viewModel = new ViewModels.Tasks.TaskTimeViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task),
                Time = new ViewModels.Timing.TimeViewModel()
                {
                    Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact),
                    Start = DateTime.Now
                }
            };

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Tasks.TaskTimeViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.TaskTime taskTime;

            currentUser = UserCache.Instance.Lookup(Request);
            taskTime = Mapper.Map<Common.Models.Tasks.TaskTime>(viewModel);
            taskTime.Time = Mapper.Map<Common.Models.Timing.Time>(viewModel.Time);

            taskTime.Time = Data.Timing.Time.Create(taskTime.Time, currentUser);
            taskTime = Data.Tasks.TaskTime.Create(taskTime, currentUser);

            return RedirectToAction("Details", new { Id = taskTime.Id });
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Details(Guid id)
        {
            Common.Models.Tasks.TaskTime taskTime;
            ViewModels.Tasks.TaskTimeViewModel viewModel;

            taskTime = Data.Tasks.TaskTime.Get(id);
            taskTime.Task = Data.Tasks.Task.Get(taskTime.Task.Id.Value);
            taskTime.Time = Data.Timing.Time.Get(taskTime.Time.Id.Value);
            taskTime.Time.Worker = Data.Contacts.Contact.Get(taskTime.Time.Worker.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskTimeViewModel>(taskTime);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(taskTime.Task);
            viewModel.Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(taskTime.Time);
            viewModel.Time.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(taskTime.Time.Worker);

            return View(viewModel);
        }
    }
}