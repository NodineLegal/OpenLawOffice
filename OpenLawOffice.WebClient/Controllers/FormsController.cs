// -----------------------------------------------------------------------
// <copyright file="FormsController.cs" company="Nodine Legal, LLC">
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
    using System.IO;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class FormsController : BaseController
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            List<ViewModels.Forms.FormViewModel> vmList = new List<ViewModels.Forms.FormViewModel>();

            Data.Forms.Form.List().ForEach(x =>
            {
                vmList.Add(Mapper.Map<ViewModels.Forms.FormViewModel>(x));
            });

            return View(vmList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Details(int id)
        {
            ViewModels.Forms.FormViewModel viewModel;
            Common.Models.Forms.Form model;

            model = Data.Forms.Form.Get(id);
            model.MatterType = Data.Matters.MatterType.Get(model.MatterType.Id.Value);

            viewModel = Mapper.Map<ViewModels.Forms.FormViewModel>(model);
            viewModel.MatterType = Mapper.Map<ViewModels.Matters.MatterTypeViewModel>(model.MatterType);

            PopulateCoreDetails(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id)
        {
            List<ViewModels.Matters.MatterTypeViewModel> matterTypeList;
            ViewModels.Forms.FormViewModel viewModel;
            Common.Models.Forms.Form model;

            matterTypeList = new List<ViewModels.Matters.MatterTypeViewModel>();

            model = Data.Forms.Form.Get(id);

            Data.Matters.MatterType.List().ForEach(x =>
            {
                ViewModels.Matters.MatterTypeViewModel vm = Mapper.Map<ViewModels.Matters.MatterTypeViewModel>(x);
                matterTypeList.Add(vm);
            });

            viewModel = Mapper.Map<ViewModels.Forms.FormViewModel>(model);

            ViewData["MatterTypeList"] = matterTypeList;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(int id, ViewModels.Forms.FormViewModel viewModel)
        {
            // TODO : Cleanup - should probably wrap this in a transaction with rollback on 
            // error from the SaveAs method of FileUpload

            Common.Models.Account.Users currentUser;
            Common.Models.Forms.Form model, currentModel;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            currentModel = Data.Forms.Form.Get(viewModel.Id.Value);
            model = Mapper.Map<Common.Models.Forms.Form>(viewModel);

            // Path is based on base path + id, so, during an edit, it should NEVER change under
            // the current model - May change in the future if versioning or something similar 
            // is supported
            model.Path = currentModel.Path;

            model = Data.Forms.Form.Edit(model, currentUser);

            // Only overwrite is the user gave us a file to use, otherwise, keep
            // the existing file - user just wants to update the matter type
            if (viewModel.FileUpload != null && viewModel.FileUpload.ContentLength > 0)
            { // Posted file - overwrites existing
                viewModel.FileUpload.SaveAs(model.Path);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            List<ViewModels.Matters.MatterTypeViewModel> matterTypeList;

            matterTypeList = new List<ViewModels.Matters.MatterTypeViewModel>();

            Data.Matters.MatterType.List().ForEach(x =>
            {
                ViewModels.Matters.MatterTypeViewModel vm = Mapper.Map<ViewModels.Matters.MatterTypeViewModel>(x);
                matterTypeList.Add(vm);
            });

            ViewData["MatterTypeList"] = matterTypeList;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Forms.FormViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Forms.Form model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Forms.Form>(viewModel);

            // File to upload is mandatory in creation, no file -> Fail

            if (viewModel.FileUpload == null || viewModel.FileUpload.ContentLength <= 0)
            { // NO FILE - FAIL
                ModelState.AddModelError("FileUpload", "File to upload is required");
            }

            if (ModelState.IsValid)
            {
                // Determine path

                // Create path if it does not exist - IIS may not like this if it does not have
                // the appropriate permissions
                // TODO : Error handling
                if (!Directory.Exists(Common.Settings.Manager.Instance.FileStorage.FormsPath))
                    Directory.CreateDirectory(Common.Settings.Manager.Instance.FileStorage.FormsPath);

                // Make sure the path ends with a directory separator character
                model.Path = Common.Settings.Manager.Instance.FileStorage.FormsPath;
                if (!model.Path.EndsWith(Path.DirectorySeparatorChar.ToString()) && !model.Path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                    model.Path += Path.DirectorySeparatorChar;

                // Must create in DB so that ID is known.

                // TODO : error checking, transaction rollback, etc.
                model = Data.Forms.Form.Create(model, currentUser);

                // Append ID then the extension (if present)
                model.Path += model.Id.Value.ToString();
                if (Path.HasExtension(viewModel.FileUpload.FileName))
                    model.Path += Path.GetExtension(viewModel.FileUpload.FileName);

                // Must update to update the path to pickup the file name
                model = Data.Forms.Form.Edit(model, currentUser);

                viewModel.FileUpload.SaveAs(model.Path);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(int id)
        {
            ViewModels.Forms.FormViewModel viewModel;
            Common.Models.Forms.Form model;

            model = Data.Forms.Form.Get(id);
            model.MatterType = Data.Matters.MatterType.Get(model.MatterType.Id.Value);

            viewModel = Mapper.Map<ViewModels.Forms.FormViewModel>(model);
            viewModel.MatterType = Mapper.Map<ViewModels.Matters.MatterTypeViewModel>(model.MatterType);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Delete(int id, ViewModels.Forms.FormViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Forms.Form model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Forms.Form>(viewModel);

            model = Data.Forms.Form.Disable(model, currentUser);

            // Don't delete the file - in theory a system admin could enable this again
            // deleting file would leave a form in the DB without a corresponding file

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Login, User")]
        public FileResult Download(int id)
        {
            string ext = "";
            Common.Models.Forms.Form model;

            model = Data.Forms.Form.Get(id);

            if (Path.HasExtension(model.Path))
                ext = Path.GetExtension(model.Path);
            
            return File(model.Path, Common.Utilities.GetMimeType(ext), model.Title + ext);
        }
    }
}
