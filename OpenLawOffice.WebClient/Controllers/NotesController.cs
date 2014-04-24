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

    [HandleError(View = "Errors/", Order = 10)]
    public class NotesController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
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

            if (noteMatter != null)
            { // Note belongs to a matter
                ViewData["MatterId"] = noteMatter.Id.Value;
            }
            else if (noteTask != null)
            { // Note belongs to a task
                ViewData["TaskId"] = noteTask.Id.Value;
            }
            else
                throw new Exception("Note without relation to a matter or task, orphaned.");

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Notes.Note model;
            ViewModels.Notes.NoteViewModel viewModel;

            model = Data.Notes.Note.Get(id);

            viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(model);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Notes.NoteViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Notes.Note model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Notes.Note>(viewModel);

            model = Data.Notes.Note.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Notes.NoteViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Notes.Note model;
            Guid matterid;
            long taskid;

            currentUser = UserCache.Instance.Lookup(Request);

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
            else
                throw new HttpRequestValidationException("Must specify a MatterId or a TaskId");

            return RedirectToAction("Details", new { Id = model.Id });
        }
    }
}