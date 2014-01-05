namespace OpenLawOffice.WebClient.Controllers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using OpenLawOffice.Server.Core;
    using DBOs = OpenLawOffice.Server.Core.DBOs;
    using AutoMapper;

    public class SecurityAreasController : BaseController
    {
        //
        // GET: /Area/
        [SecurityFilter(SecurityAreaName = "Security.Area", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Security.Area", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult List()
        {
            List<Common.Models.Security.Area> modelList = RecursiveListChildren(null);

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        private List<Models.Security.AciTreeObject> ConvertToAciTreeObjectList(List<Common.Models.Security.Area> list)
        {
            List<Models.Security.AciTreeObject> newList = new List<Models.Security.AciTreeObject>();

            if (list == null) return null;

            list.ForEach(item =>
            {
                Models.Security.AciTreeObject newItem = new Models.Security.AciTreeObject();
                newItem.id = item.Id.Value;
                newItem.label = item.Name;

                if (item.Children != null && item.Children.Count > 0)
                {
                    newItem.inode = true;
                    newItem.branch = ConvertToAciTreeObjectList(item.Children);
                }
                else
                {
                    newItem.inode = false;
                }

                newItem.open = false;

                newList.Add(newItem);
            });

            return newList;
        }

        [SecurityFilter(SecurityAreaName = "Security.Area", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult AciTreeList()
        {
            List<Common.Models.Security.Area> modelList = RecursiveListChildren(null);

            List<Models.Security.AciTreeObject> aciTreeList = ConvertToAciTreeObjectList(modelList);

            return Json(aciTreeList, JsonRequestBehavior.AllowGet);
        }

        private List<Common.Models.Security.Area> RecursiveListChildren(Common.Models.Security.Area parent)
        {
            List<Common.Models.Security.Area> modelList = new List<Common.Models.Security.Area>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.Area> list = null;

                if (parent == null || !parent.Id.HasValue)
                    list = db.Query<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\" is null AND \"utc_disabled\" is null");
                else
                    list = db.Query<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null",
                        new { ParentId = parent.Id.Value });

                if (list == null || list.Count == 0)
                    return null;

                list.ForEach(dbo =>
                {
                    Common.Models.Security.Area model = Mapper.Map<Common.Models.Security.Area>(dbo);
                    model.Children = RecursiveListChildren(model);
                    modelList.Add(model);
                });
            }

            return modelList;
        }

        //
        // GET: /Area/Details/5
        [SecurityFilter(SecurityAreaName = "Security.Area", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            Common.Models.Security.Area model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Security.Area dbo = db.QuerySingle<DBOs.Security.Area>(
                    "SELECT * FROM \"area\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<Common.Models.Security.Area>(dbo);

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Security.Area dboParent = db.QuerySingle<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                        new { Id = dbo.ParentId.Value });
                    model.Parent = Mapper.Map<Common.Models.Security.Area>(dboParent);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }
        
        [SecurityFilter(SecurityAreaName = "Security.AreaAcl", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Permissions(int id)
        {
            List<Common.Models.Security.AreaAcl> modelList = new List<Common.Models.Security.AreaAcl>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                List<DBOs.Security.AreaAcl> dboList = db.Query<DBOs.Security.AreaAcl>(
                    "SELECT * FROM \"area_acl\" WHERE \"security_area_id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                dboList.ForEach(dbo =>
                {
                    Common.Models.Security.AreaAcl model = Mapper.Map<Common.Models.Security.AreaAcl>(dbo);
                    DBOs.Security.User userDbo = db.GetById<DBOs.Security.User>(model.User.Id);
                    model.User = Mapper.Map<Common.Models.Security.User>(userDbo);
                    modelList.Add(model);
                });                
            }

            return View(modelList);
        }
    }
}
