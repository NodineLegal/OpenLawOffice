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
        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public FileResult Download(Guid id)
        {
            Common.Models.Documents.Version version = null;

            version = Data.Documents.Document.GetCurrentVersion(id);

            return File(Data.FileStorage.Instance.CurrentVersionPath + version.Id.ToString() + "." + version.Extension,
                version.Mime, version.Filename + "." + version.Extension);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            Common.Models.Documents.Document model;
            List<Common.Models.Documents.Version> versionList;
            ViewModels.Documents.DocumentViewModel viewModel;

            model = Data.Documents.Document.Get(id);

            viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(model);
            viewModel.Versions = new List<ViewModels.Documents.VersionViewModel>();

            versionList = Data.Documents.Document.GetVersions(id);

            versionList.ForEach(x =>
            {
                viewModel.Versions.Add(Mapper.Map<ViewModels.Documents.VersionViewModel>(x));
            });

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            ViewModels.Documents.DocumentViewModel viewModel;

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

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Documents.DocumentViewModel viewModel, HttpPostedFileBase file)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Documents.Document model;
            Common.Models.Documents.Version version;

            currentUser = UserCache.Instance.Lookup(Request);

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
            file.SaveAs(Data.FileStorage.Instance.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));

            // Calculate the MD5 checksum
            version.Md5 = Data.FileStorage.CalculateMd5(
                Data.FileStorage.Instance.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));

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

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Documents.DocumentViewModel viewModel;
            Common.Models.Documents.Document model;

            model = OpenLawOffice.Data.Documents.Document.Get(id);

            viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(model);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Documents.DocumentViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Documents.Document model;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Documents.Document>(viewModel);

            model = OpenLawOffice.Data.Documents.Document.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
        }
    }
}