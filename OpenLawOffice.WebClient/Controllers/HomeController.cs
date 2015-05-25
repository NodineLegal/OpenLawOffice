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
    using System;
    using System.Net;
    using System.Web.Profile;
    using System.Web.Security;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class HomeController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index(ViewModels.Home.DashboardViewModel currentDVM)
        {
            int id;
            ViewModels.Home.DashboardViewModel viewModel;
            Common.Models.Contacts.Contact employee;
            Common.Models.Account.Users currentUser;
            List<Common.Models.Settings.TagFilter> tagFilter;
            Common.Models.Matters.Matter matter;
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            try
            {
                Data.Account.Users.List();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Installation");
            }


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

            employee = Data.Contacts.Contact.Get(id);
            currentUser = Data.Account.Users.Get(User.Identity.Name);

            viewModel = new ViewModels.Home.DashboardViewModel();
            viewModel.Employee = Mapper.Map<ViewModels.Contacts.ContactViewModel>(employee);

            viewModel.MyTodoList = new List<Tuple<ViewModels.Matters.MatterViewModel, ViewModels.Tasks.TaskViewModel>>();

            tagFilter = Data.Settings.UserTaskSettings.ListTagFiltersFor(currentUser);

            Data.Tasks.Task.GetTodoListFor(employee, tagFilter).ForEach(x =>
            {
                matter = Data.Tasks.Task.GetRelatedMatter(x.Id.Value);
                viewModel.MyTodoList.Add(
                    new Tuple<ViewModels.Matters.MatterViewModel, ViewModels.Tasks.TaskViewModel>(
                    Mapper.Map<ViewModels.Matters.MatterViewModel>(matter),
                    Mapper.Map<ViewModels.Tasks.TaskViewModel>(x)));
            });

            viewModel.NotificationList = new List<ViewModels.Notes.NoteNotificationViewModel>();
            Data.Notes.NoteNotification.ListAllNoteNotificationsForContact(employee.Id.Value).ForEach(x =>
            {
                ViewModels.Notes.NoteNotificationViewModel nnvm = Mapper.Map<ViewModels.Notes.NoteNotificationViewModel>(x);
                nnvm.Note = Mapper.Map<ViewModels.Notes.NoteViewModel>(Data.Notes.Note.Get(x.Note.Id.Value));
                viewModel.NotificationList.Add(nnvm);
            });

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            ViewData["EmployeeContactList"] = employeeContactList;

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult About()
        {
            return View();
        }
    }
}