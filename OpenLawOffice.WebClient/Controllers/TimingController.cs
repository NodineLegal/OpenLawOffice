// -----------------------------------------------------------------------
// <copyright file="TimingController.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Web.Profile;
    using System.Web.Security;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class TimingController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            Common.Models.Timing.Time model;
            ViewModels.Timing.TimeViewModel viewModel;
            Common.Models.Contacts.Contact contact;
            Common.Models.Tasks.Task task;
            Common.Models.Matters.Matter matter;

            model = Data.Timing.Time.Get(id);

            viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(model);

            contact = Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);

            viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

            task = OpenLawOffice.Data.Timing.Time.GetRelatedTask(model.Id.Value);

            PopulateCoreDetails(viewModel);

            ViewData["IsFastTime"] = Data.Timing.Time.IsFastTime(id);

            matter = Data.Tasks.Task.GetRelatedMatter(task.Id.Value);
            ViewData["Task"] = task.Title;
            ViewData["TaskId"] = task.Id;
            ViewData["Matter"] = matter.Title;
            ViewData["MatterId"] = matter.Id;
            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Timing.Time model;
            ViewModels.Timing.TimeViewModel viewModel;
            Common.Models.Contacts.Contact contact;
            Common.Models.Tasks.Task task;
            Common.Models.Matters.Matter matter;
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            model = Data.Timing.Time.Get(id);

            viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(model);

            contact = Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);

            viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

            task = Data.Timing.Time.GetRelatedTask(model.Id.Value);

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            ViewData["TaskId"] = task.Id.Value;

            matter = Data.Tasks.Task.GetRelatedMatter(task.Id.Value);
            ViewData["Task"] = task.Title;
            ViewData["TaskId"] = task.Id;
            ViewData["Matter"] = matter.Title;
            ViewData["MatterId"] = matter.Id;
            ViewData["EmployeeContactList"] = employeeContactList;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Timing.TimeViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Timing.Time model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Timing.Time>(viewModel);

            if (model.Stop.HasValue)
            {
                List<Common.Models.Timing.Time> conflicts = 
                    Data.Timing.Time.ListConflictingTimes(model.Start, model.Stop.Value, model.Worker.Id.Value);
                
                if (conflicts.Count > 1 || 
                    (conflicts.Count == 1 && conflicts[0].Id != id))
                { // conflict found
                    Common.Models.Contacts.Contact contact;
                    Common.Models.Tasks.Task task;
                    Common.Models.Matters.Matter matter;

                    contact = Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
                    viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

                    task = Data.Timing.Time.GetRelatedTask(model.Id.Value);

                    ModelState.AddModelError(String.Empty, "Time conflicts with other time entries.");

                    matter = Data.Tasks.Task.GetRelatedMatter(task.Id.Value);
                    ViewData["Task"] = task.Title;
                    ViewData["TaskId"] = task.Id;
                    ViewData["Matter"] = matter.Title;
                    ViewData["MatterId"] = matter.Id;
                    return View(viewModel);
                }
            }

            model = Data.Timing.Time.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult FastTime()
        {
            return View(new ViewModels.Timing.TimeViewModel() { Start = DateTime.Now });
        }
        
        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult FastTime(ViewModels.Timing.TimeViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Timing.Time model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Timing.Time>(viewModel);

            model = Data.Timing.Time.Create(model, currentUser);

            return RedirectToAction("Details", new { Id = model.Id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult FastTimeList()
        {
            List<ViewModels.Timing.TimeViewModel> list;

            list = new List<ViewModels.Timing.TimeViewModel>();

            Data.Timing.Time.FastTimeList().ForEach(x =>
            {
                ViewModels.Timing.TimeViewModel viewModel;
                Common.Models.Contacts.Contact worker;

                worker = Data.Contacts.Contact.Get(x.Worker.Id.Value);

                viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);
                viewModel.WorkerDisplayName = worker.DisplayName;

                list.Add(viewModel);
            });

            return View(list);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult DayView(ViewModels.Timing.DayViewModel currentDVM)
        {
            int id;
            DateTime date;
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;
            ViewModels.Timing.DayViewModel dayViewVM = new ViewModels.Timing.DayViewModel();

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            if (RouteData.Values["Id"] != null)
            {
                id = int.Parse((string)RouteData.Values["Id"]);
            }
            else if (currentDVM.Employee != null && currentDVM.Employee.Id.HasValue)
            {
                id = currentDVM.Employee.Id.Value;
            }
            else
            {
                dynamic profile = ProfileBase.Create(Membership.GetUser().UserName);
                if (profile.ContactId != null && !string.IsNullOrEmpty(profile.ContactId))
                    id = int.Parse(profile.ContactId);
                else
                    throw new ArgumentNullException("Must supply an Id or have a ContactId set in profile.");
            }
            dayViewVM.Employee = Mapper.Map<ViewModels.Contacts.ContactViewModel>(Data.Contacts.Contact.Get(id));
            
            if (Request["Date"] != null)
                date = DateTime.Parse(Request["Date"]);
            else
                date = DateTime.Today;
            
            Data.Timing.Time.ListForDay(id, date).ForEach(x =>
            {
                ViewModels.Timing.DayViewModel.Item dayVMItem;

                dayVMItem = new ViewModels.Timing.DayViewModel.Item();

                dayVMItem.Time = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);

                dayVMItem.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(Data.Timing.Time.GetRelatedTask(dayVMItem.Time.Id.Value));

                dayVMItem.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(Data.Tasks.Task.GetRelatedMatter(dayVMItem.Task.Id.Value));

                dayViewVM.Items.Add(dayVMItem);
            });

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            ViewData["Date"] = date;
            ViewData["EmployeeContactList"] = employeeContactList;
            return View(dayViewVM);
        }
    }
}