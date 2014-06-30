// -----------------------------------------------------------------------
// <copyright file="AdminController.cs" company="Nodine Legal, LLC">
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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using AutoMapper;
    using System.Web.Routing;

    public class AdminController : BaseController
    {
        public AccountMembershipService MembershipService { get; set; }
        
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            int totalRecords = 0;
            MembershipUserCollection users = null;
            List<ViewModels.Account.UsersViewModel> vmList;

            users = MembershipService.GetAllUsers(0, 100, out totalRecords);

            vmList = new List<ViewModels.Account.UsersViewModel>();

            foreach (MembershipUser mu in users)
            {
                vmList.Add(new ViewModels.Account.UsersViewModel()
                {
                    Username = mu.UserName,
                    Email = mu.Email,
                    IsApproved = mu.IsApproved,
                    IsLockedOut = mu.IsLockedOut
                });
            }

            return View(vmList);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id)
        {
            ViewModels.Account.SelectableUserRoleViewModel viewModel;
            List<ViewModels.Account.SelectableUserRoleViewModel> rolesList;

            rolesList = new List<ViewModels.Account.SelectableUserRoleViewModel>();

            Roles.GetAllRoles().ToList().ForEach(x =>
            {
                bool a = Roles.IsUserInRole(id, x);
                viewModel = new ViewModels.Account.SelectableUserRoleViewModel()
                {
                    Username = id,
                    Rolename = x,
                    IsSelected = Roles.IsUserInRole(id, x)
                };

                rolesList.Add(viewModel);
            });

            return View(rolesList);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id, List<ViewModels.Account.SelectableUserRoleViewModel> nullViewModel)
        {
            List<ViewModels.Account.SelectableUserRoleViewModel> rolesList;

            rolesList = new List<ViewModels.Account.SelectableUserRoleViewModel>();

            Roles.GetAllRoles().ToList().ForEach(x =>
            {
                if (Request["CB_" + id + "_" + x] == "on")
                {
                    if (!Roles.IsUserInRole(id, x))
                        Roles.AddUserToRole(id, x);
                }
                else
                {
                    if (Roles.IsUserInRole(id, x))
                        Roles.RemoveUserFromRole(id, x);
                }
            });

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DetailsUser(string username)
        {
            Common.Models.Account.Users model;
            ViewModels.Account.UsersViewModel viewModel;

            model = Data.Account.Users.Get(username);

            viewModel = Mapper.Map<ViewModels.Account.UsersViewModel>(model);

            viewModel.Password = null;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser(ViewModels.Account.RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(viewModel.UserName, viewModel.Password, viewModel.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("DetailsUser", "Admin", new { Username = viewModel.UserName });
                }
                else
                {
                    ModelState.AddModelError("", ViewModels.AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            Common.Models.Account.Users model;
            ViewModels.Account.UsersViewModel viewModel;

            model = Data.Account.Users.Get(id);

            viewModel = Mapper.Map<ViewModels.Account.UsersViewModel>(model);

            viewModel.Password = null;

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id, ViewModels.Account.UsersViewModel viewModel)
        {
            MembershipUser user;            
            
            user = MembershipService.GetUser(id);

            user.Email = viewModel.Email;
            user.Comment = viewModel.Comment;
            user.IsApproved = viewModel.IsApproved;

            MembershipService.UpdateUser(user);

            return RedirectToAction("DetailsUser", new { Username = id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisableUser(string id)
        {
            Common.Models.Account.Users model;
            ViewModels.Account.UsersViewModel viewModel;

            model = Data.Account.Users.Get(id);

            viewModel = Mapper.Map<ViewModels.Account.UsersViewModel>(model);

            viewModel.Password = null;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DisableUser(string id, ViewModels.Account.UsersViewModel viewModel)
        {
            MembershipUser user;

            user = MembershipService.GetUser(id);

            user.IsApproved = false;

            MembershipService.UpdateUser(user);

            return RedirectToAction("DetailsUser", new { Username = id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UnlockUser(string id)
        {
            Common.Models.Account.Users model;
            ViewModels.Account.UsersViewModel viewModel;

            model = Data.Account.Users.Get(id);

            viewModel = Mapper.Map<ViewModels.Account.UsersViewModel>(model);

            viewModel.Password = null;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UnlockUser(string id, ViewModels.Account.UsersViewModel viewModel)
        {
            MembershipUser user;

            user = MembershipService.GetUser(id);

            user.UnlockUser();

            return RedirectToAction("DetailsUser", new { Username = id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassword(string id)
        {
            ViewModels.Account.UsersViewModel viewModel;

            viewModel = Mapper.Map<ViewModels.Account.UsersViewModel>(Data.Account.Users.Get(id));

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassword(string id, ViewModels.Account.UsersViewModel viewModel)
        {
            MembershipUser user;

            user = MembershipService.GetUser(id);

            user.ChangePassword(user.ResetPassword(), viewModel.Password);

            return RedirectToAction("DetailsUser", new { Username = id });
        }
    }
}
