// -----------------------------------------------------------------------
// <copyright file="ExpensesController.cs" company="Nodine Legal, LLC">
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
    public class ExpensesController : Controller
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id)
        {
            Common.Models.Billing.Expense model;
            Common.Models.Matters.Matter matter;
            ViewModels.Billing.ExpenseViewModel viewModel;

            model = Data.Billing.Expense.Get(id);
            matter = Data.Billing.Expense.GetMatter(id);

            viewModel = Mapper.Map<ViewModels.Billing.ExpenseViewModel>(model);
            
            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Edit(Guid id, ViewModels.Billing.ExpenseViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Matters.Matter matter;
            Common.Models.Billing.Expense model;

            currentUser = Data.Account.Users.Get(User.Identity.Name);
            matter = Data.Billing.Expense.GetMatter(id);

            model = Mapper.Map<Common.Models.Billing.Expense>(viewModel);

            model = Data.Billing.Expense.Edit(model, currentUser);

            return RedirectToAction("Details", "Matters", new { Id = matter.Id });
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Create()
        {
            Common.Models.Matters.Matter matter = null;

            matter = Data.Matters.Matter.Get(Guid.Parse(Request["MatterId"]));

            ViewData["MatterId"] = matter.Id.Value;
            ViewData["Matter"] = matter.Title;

            return View(new ViewModels.Billing.ExpenseViewModel() 
                { 
                    Incurred = DateTime.Now, 
                    Paid = DateTime.Now, 
                });
        }

        [HttpPost]
        [Authorize(Roles = "Login, User")]
        public ActionResult Create(ViewModels.Billing.ExpenseViewModel viewModel)
        {
            Common.Models.Account.Users currentUser;
            Common.Models.Billing.Expense model;
            Guid matterid;

            currentUser = Data.Account.Users.Get(User.Identity.Name);

            model = Mapper.Map<Common.Models.Billing.Expense>(viewModel);

            model = Data.Billing.Expense.Create(model, currentUser);

            matterid = Guid.Parse(Request["MatterId"]);

            Data.Billing.Expense.RelateMatter(model, matterid, currentUser);

            return RedirectToAction("Details", "Matters", new { Id = matterid });
        }
    }
}
