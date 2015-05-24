// -----------------------------------------------------------------------
// <copyright file="NotesController.cs" company="Nodine Legal, LLC">
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
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using System.Collections.Generic;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class NotesController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            ViewModels.Notes.NoteViewModel viewModel;
            Common.Models.Notes.Note model;
            Common.Models.Matters.Matter noteMatter;
            Common.Models.Tasks.Task noteTask;

            model = Data.Notes.Note.Get(id);

            viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(model);

            noteMatter = Data.Notes.NoteMatter.GetRelatedMatter(id);

            noteTask = Data.Notes.NoteTask.GetRelatedTask(id);

            if (noteTask != null)
            { // Note belongs to a task
                noteMatter = Data.Tasks.Task.GetRelatedMatter(noteTask.Id.Value);
                ViewData["TaskId"] = noteTask.Id.Value;
                ViewData["Task"] = noteTask.Title;
            }
            
            viewModel.NoteNotifications = new List<ViewModels.Notes.NoteNotificationViewModel>();
            Data.Notes.NoteNotification.ListForNote(id).ForEach(x =>
            {
                x.Contact = Data.Contacts.Contact.Get(x.Contact.Id.Value);
                ViewModels.Notes.NoteNotificationViewModel vm = Mapper.Map<ViewModels.Notes.NoteNotificationViewModel>(x);
                vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(x.Contact);
                viewModel.NoteNotifications.Add(vm);
            });

            PopulateCoreDetails(viewModel);

            ViewData["MatterId"] = noteMatter.Id.Value;
            ViewData["Matter"] = noteMatter.Title;
            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ClearNotification(Guid id)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Notes.NoteNotification model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Data.Notes.NoteNotification.Get(id);

            model = Data.Notes.NoteNotification.Clear(model, currentUser);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;
            Common.Models.Notes.Note model;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;
            ViewModels.Notes.NoteViewModel viewModel;

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            model = Data.Notes.Note.Get(id);
            matter = Data.Notes.Note.GetMatter(id);
            task = Data.Notes.Note.GetTask(id);

            viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(model);

            if (task != null)
            {
                matter = Data.Tasks.Task.GetRelatedMatter(task.Id.Value);
                ViewData["TaskId"] = task.Id.Value;
                ViewData["Task"] = task.Title;
            }

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            List<Common.Models.Notes.NoteNotification> notesNotifications = Data.Notes.NoteNotification.ListForNote(id);
            viewModel.NotifyContactIds = new string[notesNotifications.Count];
            for (int i = 0; i < notesNotifications.Count; i++)
            {
                viewModel.NotifyContactIds[i] = notesNotifications[i].Contact.Id.Value.ToString();
            }

            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;
            ViewData["EmployeeContactList"] = employeeContactList;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Notes.NoteViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Notes.Note model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Notes.Note>(viewModel);

            model = Data.Notes.Note.Edit(model, currentUser);

            if (viewModel.NotifyContactIds != null)
            {
                viewModel.NotifyContactIds.Each(x =>
                {
                    Data.Notes.NoteNotification.Create(new Common.Models.Notes.NoteNotification()
                    {
                        Contact = new Common.Models.Contacts.Contact() { Id = int.Parse(x) },
                        Note = model,
                        Cleared = null
                    }, currentUser);
                });
            }

            return RedirectToAction("Details", new { Id = id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            List<ViewModels.Contacts.ContactViewModel> employeeContactList;
            Common.Models.Matters.Matter matter = null;
            Common.Models.Tasks.Task task = null;

            employeeContactList = new List<ViewModels.Contacts.ContactViewModel>();

            if (Request["MatterId"] != null)
            {
                matter = Data.Matters.Matter.Get(Guid.Parse(Request["MatterId"]));
            }
            else if (Request["TaskId"] != null)
            {
                task = Data.Tasks.Task.Get(long.Parse(Request["TaskId"]));
                matter = Data.Tasks.Task.GetRelatedMatter(task.Id.Value);
                ViewData["TaskId"] = task.Id.Value;
                ViewData["Task"] = task.Title;
            }

            Data.Contacts.Contact.ListEmployeesOnly().ForEach(x =>
            {
                employeeContactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;
            ViewData["EmployeeContactList"] = employeeContactList;

            return View(new ViewModels.Notes.NoteViewModel() { Timestamp = DateTime.Now });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Notes.NoteViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Notes.Note model;
            Guid matterid, eventid;
            long taskid;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Notes.Note>(viewModel);

            model = Data.Notes.Note.Create(model, currentUser);

            if (viewModel.NotifyContactIds != null)
            {
                viewModel.NotifyContactIds.Each(x =>
                {
                    Data.Notes.NoteNotification.Create(new Common.Models.Notes.NoteNotification()
                    {
                        Contact = new Common.Models.Contacts.Contact() { Id = int.Parse(x) },
                        Note = model,
                        Cleared = null
                    }, currentUser);
                });
            }

            if (Request["MatterId"] != null)
            {
                matterid = Guid.Parse(Request["MatterId"]);

                Data.Notes.Note.RelateMatter(model, matterid, currentUser);

                return RedirectToAction("Details", "Matters", new { Id = matterid });
            }
            else if (Request["TaskId"] != null)
            {
                taskid = long.Parse(Request["TaskId"]);

                Data.Notes.Note.RelateTask(model, taskid, currentUser);

                return RedirectToAction("Details", "Tasks", new { Id = taskid });
            }
            else if (Request["EventId"] != null)
            {
                eventid = Guid.Parse(Request["EventId"]);

                Data.Notes.Note.RelateEvent(model, eventid, currentUser);

                return RedirectToAction("Details", "Events", new { Id = eventid });
            }
            else
                throw new HttpRequestValidationException("Must specify a MatterId, TaskId or EventId");
        }
    }
}