// -----------------------------------------------------------------------
// <copyright file="SecurityAreasController.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.Controllers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class SecurityAreasController : BaseController
    {
        //
        // GET: /Area/
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(int? id)
        {
            ViewModels.JqGridObject jqObject;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = int.Parse(Request["nodeid"]);
            }

            List<ViewModels.Security.AreaViewModel> modelList = GetChildrenList(id);
            List<object> anonList = new List<object>();

            if (!string.IsNullOrEmpty(Request["n_level"]))
                level = int.Parse(Request["n_level"]) + 1;

            modelList.ForEach(x =>
            {
                if (x.Parent == null)
                    anonList.Add(new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        level = level,
                        isLeaf = false,
                        expanded = false
                    });
                else
                    anonList.Add(new
                    {
                        Id = x.Id,
                        parent = x.Parent.Id,
                        Name = x.Name,
                        Description = x.Description,
                        level = level,
                        isLeaf = false,
                        expanded = false
                    });
            });

            jqObject = new ViewModels.JqGridObject()
            {
                TotalPages = 1,
                CurrentPage = 1,
                TotalRecords = modelList.Count,
                Rows = anonList.ToArray()
            };

            return Json(jqObject, JsonRequestBehavior.AllowGet);
        }

        private List<ViewModels.Security.AreaViewModel> GetChildrenList(int? id)
        {
            List<ViewModels.Security.AreaViewModel> viewModelList = new List<ViewModels.Security.AreaViewModel>();

            List<Common.Models.Security.Area> modelList = OpenLawOffice.Data.Security.Area.ListChildren(id);

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Security.AreaViewModel>(modelList));
            });

            return viewModelList;
        }

        //
        // GET: /Area/Details/5
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Security.AreaViewModel viewModel = null;
            Common.Models.Security.Area model = OpenLawOffice.Data.Security.Area.Get(id);
            viewModel = Mapper.Map<ViewModels.Security.AreaViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(model);
        }
        
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Permissions(int id)
        {
            List<ViewModels.Security.AreaAclViewModel> viewModelList = new List<ViewModels.Security.AreaAclViewModel>();
            List<Common.Models.Security.AreaAcl> modelList = OpenLawOffice.Data.Security.AreaAcl.ListForArea(id);
            modelList.ForEach(x =>
            {
                x.User = OpenLawOffice.Data.Security.User.Get(x.User.Id.Value);
                ViewModels.Security.AreaAclViewModel vm = Mapper.Map<ViewModels.Security.AreaAclViewModel>(x);
                vm.User = Mapper.Map<ViewModels.Security.UserViewModel>(x.User);
                viewModelList.Add(vm);
            });

            return View(modelList);
        }
    }
}
