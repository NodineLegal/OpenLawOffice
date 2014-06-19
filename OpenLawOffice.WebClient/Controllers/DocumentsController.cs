// -----------------------------------------------------------------------
// <copyright file="DocumentsController.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class DocumentsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public FileResult Download(Guid id)
        {
            Common.Models.Documents.Version version = null;

            version = Data.Documents.Document.GetCurrentVersion(id);

            return File(Common.Settings.Manager.Instance.FileStorage.CurrentVersionPath + version.Id.ToString() + "." + version.Extension,
                version.Mime, version.Filename + "." + version.Extension);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(Guid id)
        {
            Common.Models.Documents.Document model;
            List<Common.Models.Documents.Version> versionList;
            ViewModels.Documents.DocumentViewModel viewModel;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;

            model = Data.Documents.Document.Get(id);
            matter = Data.Documents.Document.GetMatter(id);
            task = Data.Documents.Document.GetTask(id);

            viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(model);
            viewModel.Versions = new List<ViewModels.Documents.VersionViewModel>();

            versionList = Data.Documents.Document.GetVersions(id);

            versionList.ForEach(x =>
            {
                viewModel.Versions.Add(Mapper.Map<ViewModels.Documents.VersionViewModel>(x));
            });

            PopulateCoreDetails(viewModel);

            if (matter != null && matter.Id.HasValue)
                ViewData["MatterId"] = matter.Id.Value;

            if (task != null && task.Id.HasValue)
                ViewData["TaskId"] = task.Id.Value;

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            if (Request["TaskId"] != null)
            { // The create request originated from a task
            }
            else if (Request["MatterId"] != null)
            { // The create request originated from a matter
            }
            else
                return View("Errors/InvalidRequest");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Documents.DocumentViewModel viewModel, HttpPostedFileBase file)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Documents.Document model;
            Common.Models.Documents.Version version;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Documents.Document>(viewModel);

            model = Data.Documents.Document.Create(model, currentUser);

            version = new Common.Models.Documents.Version()
            {
                Id = Guid.NewGuid(),
                Document = model,
                Mime = file.ContentType,
                Filename = file.FileName.Split('.')[0],
                Extension = file.FileName.Split('.')[1],
                Size = (long)file.ContentLength,

                // Md5 = md5
            };

            // Save file
            file.SaveAs(Common.Settings.Manager.Instance.FileStorage.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));

            // Calculate the MD5 checksum
            version.Md5 = Common.Settings.FileStorageSettings.CalculateMd5(
                Common.Settings.Manager.Instance.FileStorage.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));

            // Version
            Data.Documents.Document.CreateNewVersion(model.Id.Value, version, currentUser);

            // Matter or Task
            if (Request["MatterId"] != null)
            {
                Data.Documents.Document.RelateMatter(model, Guid.Parse(Request["MatterId"]), currentUser);
                return RedirectToAction("Documents", "Matters", new { Id = Request["MatterId"] });
            }
            else if (Request["TaskId"] != null)
            {
                Data.Documents.Document.RelateTask(model, long.Parse(Request["TaskId"]), currentUser);
                return RedirectToAction("Documents", "Tasks", new { Id = Request["TaskId"] });
            }
            else
                throw new Exception("Must have a matter or task id.");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Documents.DocumentViewModel viewModel;
            Common.Models.Documents.Document model;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;

            model = OpenLawOffice.Data.Documents.Document.Get(id);
            matter = Data.Documents.Document.GetMatter(id);
            task = Data.Documents.Document.GetTask(id);

            viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(model);

            if (matter != null && matter.Id.HasValue)
                ViewData["MatterId"] = matter.Id.Value;

            if (task != null && task.Id.HasValue)
                ViewData["TaskId"] = task.Id.Value;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Documents.DocumentViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Documents.Document model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Documents.Document>(viewModel);

            model = OpenLawOffice.Data.Documents.Document.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
        }
    }
}