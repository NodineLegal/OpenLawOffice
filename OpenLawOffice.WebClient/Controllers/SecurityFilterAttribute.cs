// -----------------------------------------------------------------------
// <copyright file="SecurityFilterAttribute.cs" company="Nodine Legal, LLC">
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

    public class SecurityFilterAttribute : ActionFilterAttribute
    {
        public string SecurityAreaName { get; set; }

        public bool IsSecuredResource { get; set; }

        public Common.Models.PermissionType Permission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid authToken = Guid.Empty;
            Guid id = Guid.Empty;
            BaseController controller;
            HttpCookie cookie = null;
            Data.Authorization.AuthorizeResult authResult;

            cookie = filterContext.HttpContext.Request.Cookies["UserAuthToken"];

            if (cookie == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            controller = (BaseController)filterContext.Controller;

            if (!Guid.TryParse(cookie.Value, out authToken))
                throw new Exception("AuthToken could not be parsed.");

            if (string.IsNullOrEmpty(SecurityAreaName))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            authResult = Data.Authorization.AreaAccess(
                new Common.Models.Security.User(),
                new Common.Models.Security.Area() { Name = SecurityAreaName },
                authToken,
                Permission);

            if (!authResult.IsAuthorized)
            {
                filterContext.Result = new RedirectResult("~/Account/InsufficientRights");
                return;
            }

            if (!IsSecuredResource)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!Permission.HasFlag(Common.Models.PermissionType.List) &&
                !Permission.HasFlag(Common.Models.PermissionType.Create))
            {
                if (filterContext.RouteData.Values["id"] != null)
                {
                    if (!Guid.TryParse((string)filterContext.RouteData.Values["id"], out id))
                        throw new Exception("Resource id could not be parsed.");
                }

                authResult = Data.Authorization.SecuredResourceAccess(
                    new Common.Models.Security.User(),
                    new Common.Models.Security.SecuredResource() { Id = id },
                    authToken,
                    Permission);

                if (!authResult.IsAuthorized)
                {
                    filterContext.Result = new RedirectResult("~/Account/InsufficientRights");
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}