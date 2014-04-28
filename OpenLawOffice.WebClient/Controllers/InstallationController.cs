// -----------------------------------------------------------------------
// <copyright file="InstallationController.cs" company="Nodine Legal, LLC">
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
    using System.IO;
    using System.Web.Mvc;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class InstallationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private FileInfo GetScriptPath()
        {
            FileInfo fi;
            string path;

            path = Request.PhysicalApplicationPath;

            fi = new FileInfo(path + Path.DirectorySeparatorChar + "Install.sql");
            if (!fi.Exists)
            {
                fi = new FileInfo(path + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Install.sql");
                if (!fi.Exists)
                {
                    fi = new FileInfo(path + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Installation" + Path.DirectorySeparatorChar + "Install.sql");
                    if (!fi.Exists)
                        return null;
                }
            }

            return fi;
        }

        public ActionResult Install()
        {
            FileInfo fi;

            fi = GetScriptPath();

            if (!fi.Exists)
                return RedirectToAction("MissingDbInstallScript");

            OpenLawOffice.Data.Installation.Setup.CreateDb(fi.FullName);

            return View("Complete");
        }

        public ActionResult InstallWithData()
        {
            FileInfo fi;

            fi = GetScriptPath();

            if (!fi.Exists)
                return RedirectToAction("MissingDbInstallScript");

            OpenLawOffice.Data.Installation.Setup.CreateDb(fi.FullName, true);

            return View("Complete");
        }

        public ActionResult MissingDbInstallScript()
        {
            return View();
        }
    }
}