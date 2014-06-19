// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nodine Legal, LLC">
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
    using System.Web.Mvc;
    using AutoMapper;
    using System.IO;
    using System.Net.Mail;

    public class BaseController : Controller
    {
        public ViewModels.Account.UsersViewModel GetUser(Guid id)
        {
            Common.Models.Account.Users user = null;

            user = Data.Account.Users.Get(id);

            if (user == null) return null;

            return Mapper.Map<ViewModels.Account.UsersViewModel>(user);
        }

        public void PopulateCoreDetails(ViewModels.CoreViewModel model)
        {
            if (model.CreatedBy != null)
            {
                if (model.CreatedBy.PId.HasValue)
                    model.CreatedBy = GetUser(model.CreatedBy.PId.Value);
                else
                    model.CreatedBy = null;
            }
            if (model.ModifiedBy != null)
            {
                if (model.ModifiedBy.PId.HasValue)
                    model.ModifiedBy = GetUser(model.ModifiedBy.PId.Value);
                else
                    model.ModifiedBy = null;
            }
            if (model.DisabledBy != null)
            {
                if (model.DisabledBy.PId.HasValue)
                    model.DisabledBy = GetUser(model.DisabledBy.PId.Value);
                else
                    model.DisabledBy = null;
            }
        }

        protected string RenderViewToString<T>(string viewPath, T model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(viewPath);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(ControllerContext, view, vdd, new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

        protected void EmailView<T>(string viewPath, T model, string to, string subject)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.SendCompleted += (sender, e) =>
            {
                return;
            };
            MailMessage msg = new System.Net.Mail.MailMessage(
                Common.Settings.Manager.Instance.System.PasswordRetrievalFromEmail,
                to, subject,
                RenderViewToString<T>(viewPath, model));
            msg.IsBodyHtml = true;

            smtpClient.SendAsync(msg, null);
        }
    }
}