// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nodine Legal, LLC">
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
    using System.Web;
    using System.Web.Mvc;
    using OpenLawOffice.WebClient.ViewModels.Account;

    [HandleError]
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            try
            {
                List<Common.Models.Security.User> users = Data.Security.User.List();
                if (users == null || users.Count < 1)
                    return RedirectToAction("Index", "Installation");
            }
            catch
            {
                return RedirectToAction("Index", "Installation");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Apply client hash (ideally this will be done on the client side in javascript eventually)
                string hashedPassword = WebClient.Security.ClientHashPassword(model.Password);

                OpenLawOffice.Data.Authentication.LoginResult result =
                    OpenLawOffice.Data.Authentication.Login(model.Username, hashedPassword);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.FailReason);
                    return View(model);
                }

                HttpCookie cookie = new HttpCookie("UserAuthToken", result.UserAuthToken);
                HttpContext.Response.AppendCookie(cookie);
                HttpContext.Response.AppendCookie(new HttpCookie("Username", model.Username));

                UserCache.Instance.Add(result.User);

                Response.Redirect("~/Home", false);

                return View();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}