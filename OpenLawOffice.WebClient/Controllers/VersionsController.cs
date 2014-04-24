// -----------------------------------------------------------------------
// <copyright file="VersionsController.cs" company="Nodine Legal, LLC">
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

    public class VersionsController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Documents.VersionViewModel viewModel;
            Common.Models.Documents.Version model;

            model = Data.Documents.Version.Get(id);
            model.Document = Data.Documents.Document.Get(model.Document.Id.Value);

            viewModel = Mapper.Map<ViewModels.Documents.VersionViewModel>(model);
            viewModel.Document = Mapper.Map<ViewModels.Documents.DocumentViewModel>(model.Document);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public FileResult Download(Guid id)
        {
            Common.Models.Documents.Version version;
            Common.Models.Documents.Version currentVersion;

            version = Data.Documents.Version.Get(id);
            currentVersion = Data.Documents.Document.GetCurrentVersion(version.Document.Id.Value);

            if (currentVersion.VersionNumber == version.VersionNumber)
            {
                return File(Data.FileStorage.Instance.CurrentVersionPath + version.Id.ToString() + "." + version.Extension,
                    version.Mime, version.Filename + "." + version.Extension);
            }
            else
            {
                return File(Data.FileStorage.Instance.PreviousVersionsPath + version.Id.ToString() + "." + version.Extension,
                   version.Mime, version.Filename + "." + version.Extension);
            }
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            Guid documentId;
            Common.Models.Documents.Document docModel;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;

            documentId = Guid.Parse(Request["DocumentId"]);
            docModel = Data.Documents.Document.Get(documentId);
            matter = Data.Documents.Document.GetMatter(documentId);
            task = Data.Documents.Document.GetTask(documentId);

            if (matter != null)
                ViewData["MatterId"] = matter.Id.Value;
            if (task != null)
                ViewData["TaskId"] = task.Id.Value;

            return View(new ViewModels.Documents.VersionViewModel()
                {
                    Document = new ViewModels.Documents.DocumentViewModel()
                    {
                        Id = documentId
                    }
                });
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Documents.VersionViewModel viewModel, HttpPostedFileBase file)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Documents.Document docModel;
            Common.Models.Documents.Version currentVersion;
            Common.Models.Matters.Matter matter;
            Common.Models.Tasks.Task task;
            Common.Models.Documents.Version version;

            currentUser = UserCache.Instance.Lookup(Request);
            docModel = Data.Documents.Document.Get(viewModel.Document.Id.Value);
            currentVersion = Data.Documents.Document.GetCurrentVersion(viewModel.Document.Id.Value);
            matter = Data.Documents.Document.GetMatter(docModel.Id.Value);
            task = Data.Documents.Document.GetTask(docModel.Id.Value);

            version = new Common.Models.Documents.Version()
            {
                Id = Guid.NewGuid(),
                Document = docModel,
                Mime = file.ContentType,
                Filename = file.FileName.Split('.')[0],
                Extension = file.FileName.Split('.')[1],
                Size = (long)file.ContentLength,

                // Md5 = md5
            };

            if (currentVersion == null)
            {
                // Save file
                file.SaveAs(Data.FileStorage.Instance.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));
            }
            else
            {
                // Move current to previous
                Data.FileStorage.Instance.MoveCurrentToPrevious(currentVersion.Id.Value + "." + currentVersion.Extension);

                // Save new
                file.SaveAs(Data.FileStorage.Instance.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));
            }

            // Calculate the MD5 checksum
            version.Md5 = Data.FileStorage.CalculateMd5(
                Data.FileStorage.Instance.GetCurrentVersionFilepathFor(version.Id.Value + "." + version.Extension));

            //Version
            Data.Documents.Document.CreateNewVersion(docModel.Id.Value, version, currentUser);

            // Matter or Task
            if (matter != null)
            {
                return RedirectToAction("Documents", "Matters", new { Id = matter.Id.Value });
            }
            else if (task != null)
            {
                return RedirectToAction("Documents", "Tasks", new { Id = task.Id.Value });
            }
            else
                throw new Exception("Must have a matter or task id.");
        }
    }
}