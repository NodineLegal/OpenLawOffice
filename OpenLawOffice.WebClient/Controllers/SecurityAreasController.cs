namespace OpenLawOffice.WebClient.Controllers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
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
            List<ViewModels.Security.AreaViewModel> modelList = new List<ViewModels.Security.AreaViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.Area> list = null;

                if (!id.HasValue)
                    list = db.SqlList<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\" is null AND \"utc_disabled\" is null");
                else
                    list = db.SqlList<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null",
                        new { ParentId = id.Value });

                if (list == null || list.Count == 0)
                    return null;

                list.ForEach(dbo =>
                {
                    modelList.Add(Mapper.Map<ViewModels.Security.AreaViewModel>(dbo));
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
            ViewModels.Security.AreaViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Security.Area dbo = db.Single<DBOs.Security.Area>(
                    "SELECT * FROM \"area\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Security.AreaViewModel>(dbo);

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Security.Area dboParent = db.Single<DBOs.Security.Area>(
                        "SELECT * FROM \"area\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                        new { Id = dbo.ParentId.Value });
                    model.Parent = Mapper.Map<ViewModels.Security.AreaViewModel>(dboParent);
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
            List<ViewModels.Security.AreaAclViewModel> modelList = new List<ViewModels.Security.AreaAclViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                List<DBOs.Security.AreaAcl> dboList = db.SqlList<DBOs.Security.AreaAcl>(
                    "SELECT * FROM \"area_acl\" WHERE \"security_area_id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                dboList.ForEach(dbo =>
                {
                    ViewModels.Security.AreaAclViewModel model = Mapper.Map<ViewModels.Security.AreaAclViewModel>(dbo);
                    DBOs.Security.User userDbo = db.SingleById<DBOs.Security.User>(model.User.Id);
                    model.User = Mapper.Map<ViewModels.Security.UserViewModel>(userDbo);
                    modelList.Add(model);
                });                
            }

            return View(modelList);
        }
    }
}
