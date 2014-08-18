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

            PopulateCoreDetails(viewModel);

            ViewData["MatterId"] = noteMatter.Id.Value;
            ViewData["Matter"] = noteMatter.Title;
            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Notes.Note model;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;
            ViewModels.Notes.NoteViewModel viewModel;

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

            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;
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

            return RedirectToAction("Details", new { Id = id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            Common.Models.Matters.Matter matter = null;
            Common.Models.Tasks.Task task = null;

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

            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;

            return View();
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

            if (Request["MatterId"] != null)
            {
                matterid = Guid.Parse(Request["MatterId"]);

                Data.Notes.Note.RelateMatter(model, matterid, currentUser);
            }
            else if (Request["TaskId"] != null)
            {
                taskid = long.Parse(Request["TaskId"]);

                Data.Notes.Note.RelateTask(model, taskid, currentUser);
            }
            else if (Request["EventId"] != null)
            {
                eventid = Guid.Parse(Request["EventId"]);

                Data.Notes.Note.RelateEvent(model, eventid, currentUser);
            }
            else
                throw new HttpRequestValidationException("Must specify a MatterId, TaskId or EventId");

            return RedirectToAction("Details", new { Id = model.Id });
        }
    }
}