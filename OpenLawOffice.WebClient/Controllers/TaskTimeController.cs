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
using System.Web.Security;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError(View = "Errors/Index", Order = 10)]
    public class TaskTimeController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult SelectContactToAssign()
        {
            List<ViewModels.Contacts.SelectableContactViewModel> modelList;

            modelList = new List<ViewModels.Contacts.SelectableContactViewModel>();

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                modelList.Add(Mapper.Map<ViewModels.Contacts.SelectableContactViewModel>(x));
            });

            return View(modelList);
        }

        [Authorize(Roles = "Login, User")]
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

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Tasks.TaskTimeViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Tasks.TaskTime taskTime;

            currentUser = Data.Account.Users.Get(User.Identity.Name);
            taskTime = Mapper.Map<Common.Models.Tasks.TaskTime>(viewModel);
            taskTime.Time = Mapper.Map<Common.Models.Timing.Time>(viewModel.Time);

            if (viewModel.Time.Stop.HasValue)
            {
                if (Data.Timing.Time.ListConflictingTimes(viewModel.Time.Start, viewModel.Time.Stop.Value).Count > 0)
                { // conflict found
                    long taskId;
                    int contactId;
                    Common.Models.Tasks.Task task;
                    Common.Models.Contacts.Contact contact;

                    taskId = long.Parse(Request["TaskId"]);
                    contactId = int.Parse(Request["ContactId"]);
                    task = Data.Tasks.Task.Get(taskId);
                    contact = Data.Contacts.Contact.Get(contactId);

                    ModelState.AddModelError(String.Empty, "Time conflicts with other time entries.");

                    viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task);
                    viewModel.Time.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

                    return View(viewModel);
                }
            }

            taskTime.Time = Data.Timing.Time.Create(taskTime.Time, currentUser);
            taskTime = Data.Tasks.TaskTime.Create(taskTime, currentUser);

            return RedirectToAction("Time", "Tasks", new { Id = Request["TaskId"] });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult RelateTask()
        {
            return View();
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult AssignFastTime(Guid id)
        {
            // Id is TimeId
            long taskId;
            Common.Models.Tasks.TaskTime model;
            ViewModels.Tasks.TaskTimeViewModel viewModel;
            Common.Models.Account.Users currentUser;

            currentUser = Data.Account.Users.Get((Guid)Membership.GetUser().ProviderUserKey);

            taskId = long.Parse(Request["TaskId"]);

            model = new Common.Models.Tasks.TaskTime()
            {
                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedBy = currentUser,
                ModifiedBy = currentUser
            };
            model.Task = Data.Tasks.Task.Get(taskId);
            model.Time = Data.Timing.Time.Get(id);
            model.Time.Worker = Data.Contacts.Contact.Get(model.Time.Worker.Id.Value);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskTimeViewModel>(model);
            viewModel.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task);
            viewModel.Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(model.Time);
            viewModel.Time.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(model.Time.Worker);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult AssignFastTime(Guid id, ViewModels.Tasks.TaskTimeViewModel viewModel)
        {
            // Id is TimeId
            long taskId;
            Common.Models.Timing.Time model;
            Common.Models.Account.Users currentUser;

            currentUser = Data.Account.Users.Get((Guid)Membership.GetUser().ProviderUserKey);

            taskId = long.Parse(Request["TaskId"]);

            model = Data.Timing.Time.Get(id);

            Data.Timing.Time.RelateTask(model, taskId, currentUser);

            return RedirectToAction("FastTimeList", "Timing");
        }
    }
}