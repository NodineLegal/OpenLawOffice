using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace OpenLawOffice.WebClient.Controllers
{
    public class InstallationController : Controller
    {
        //
        // GET: /Installation/

        public ActionResult Index()
        {
            return View();
        }

        private FileInfo GetScriptPath()
        {
            string path = Request.PhysicalApplicationPath;

            FileInfo fi = new FileInfo(path + Path.DirectorySeparatorChar + "Install.sql");
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
            FileInfo fi = GetScriptPath();

            if (!fi.Exists)
                return RedirectToAction("MissingDbInstallScript");

            OpenLawOffice.Data.Installation.Setup.CreateDb(fi.FullName);

            return View();
        }

        public ActionResult InstallWithData()
        {
            FileInfo fi = GetScriptPath();

            if (!fi.Exists)
                return RedirectToAction("MissingDbInstallScript");

            OpenLawOffice.Data.Installation.Setup.CreateDb(fi.FullName, true);

            return View();
        }

        public ActionResult MissingDbInstallScript()
        {
            return View();
        }
    }
}
