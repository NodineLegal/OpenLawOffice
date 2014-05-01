// -----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Nodine Legal, LLC">
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

    [HandleError(View = "Errors/Index", Order = 10)]
    public class UsersController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.UserViewModel> viewModelList;

            viewModelList = new List<ViewModels.Security.UserViewModel>();

            Data.Security.User.List().ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            Common.Models.Security.User model;
            ViewModels.Security.UserViewModel viewModel;

            model = Data.Security.User.Get(id);

            viewModel = Mapper.Map<ViewModels.Security.UserViewModel>(model);

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Security.UserViewModel viewModel)
        {
            Common.Models.Security.User model;

            model = Mapper.Map<Common.Models.Security.User>(viewModel);
            model.PasswordSalt = GetRandomString(10);

            // TODO : This will eventually be done in javascript on the browser
            model.Password = WebClient.Security.ClientHashPassword("12345");
            model.Password = WebClient.Security.ServerHashPassword(
                model.Password, model.PasswordSalt);

            model = Data.Security.User.Create(model);

            return RedirectToAction("Index");
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.UserViewModel viewModel;
            Common.Models.Security.User model;

            model = Data.Security.User.Get(id);

            viewModel = Mapper.Map<ViewModels.Security.UserViewModel>(model);
            viewModel.Password = null;

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Security.UserViewModel viewModel)
        {
            Common.Models.Security.User currentModel;
            Common.Models.Security.User model;

            currentModel = Data.Security.User.Get(id);
            model = Mapper.Map<Common.Models.Security.User>(viewModel);

            // TODO : This will eventually be done in javascript on the browser
            model.Password = WebClient.Security.ClientHashPassword(viewModel.Password);
            model.Password = WebClient.Security.ServerHashPassword(
                model.Password, model.PasswordSalt);

            model = Data.Security.User.Edit(model);

            model = Data.Security.User.SetPassword(model);

            return RedirectToAction("Index");
        }

        public ActionResult Disable(int id)
        {
            return Details(id);
        }

        [HttpPost]
        public ActionResult Disable(int id, ViewModels.Security.UserViewModel viewModel)
        {
            Common.Models.Security.User model;

            model = Data.Security.User.Get(id);

            model = Data.Security.User.Disable(model);

            return RedirectToAction("Index");
        }

        private int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }

        private string GetRandomString(int length)
        {
            string[] array = new string[54]
	        {
		        "0","2","3","4","5","6","8","9",
		        "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
		        "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
	        };

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }
    }
}