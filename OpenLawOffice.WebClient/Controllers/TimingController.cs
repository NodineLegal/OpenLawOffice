namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using AutoMapper;

    public class TimingController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(long? id)
        {
            ViewModels.JqGridObject jqObject;
            int level = 0;

            List<ViewModels.Timing.TimeViewModel> modelList = new List<ViewModels.Timing.TimeViewModel>();
            List<object> anonList = new List<object>();

            if (!string.IsNullOrEmpty(Request["n_level"]))
                level = int.Parse(Request["n_level"]) + 1;

            string taskid = Request["TaskId"];
            if (!string.IsNullOrEmpty(taskid))
                modelList = GetTimesForTask(long.Parse(taskid));

            modelList.ForEach(x =>
            {
                anonList.Add(new
                {
                    Id = x.Id,
                    Start = x.Start,
                    Stop = x.Stop,
                    Worker = x.WorkerDisplayName
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

        public static List<ViewModels.Timing.TimeViewModel> GetTimesForTask(long id)
        {
            List<ViewModels.Timing.TimeViewModel> modelList = new List<ViewModels.Timing.TimeViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Timing.Time> list = db.SqlList<DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\" in (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\"=@TaskId) AND " +
                    "\"utc_disabled\" is null ORDER BY \"start\" DESC",
                    new { TaskId = id });

                list.ForEach(dbo =>
                {
                    DBOs.Contacts.Contact workerDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.WorkerContactId);
                    ViewModels.Timing.TimeViewModel model = Mapper.Map<ViewModels.Timing.TimeViewModel>(dbo);

                    model.WorkerDisplayName = workerDbo.DisplayName;

                    modelList.Add(model);
                });
            }

            return modelList;
        }

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Timing.TimeViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Timing.Time dbo = db.Single<DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Contacts.Contact workerDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.WorkerContactId);
                    
                model = Mapper.Map<ViewModels.Timing.TimeViewModel>(dbo);
                model.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(workerDbo);

                DBOs.Tasks.Task task = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\" in (SELECT \"task_id\" FROM \"task_time\" WHERE \"time_id\"=@TimeId AND \"utc_disabled\" is null)",
                    new { TimeId = id });

                if (task != null)
                    ViewData["TaskId"] = task.Id;

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Timing.TimeViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Timing.Time dbo = db.Single<DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Timing.TimeViewModel>(dbo);

                DBOs.Tasks.Task task = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\" in (SELECT \"task_id\" FROM \"task_time\" WHERE \"time_id\"=@TimeId AND \"utc_disabled\" is null)",
                    new { TimeId = id });

                if (task != null)
                    ViewData["TaskId"] = task.Id;

                // Core Details
                PopulateCoreDetails(model);
            }

            return View(model);
        }        

        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Timing.TimeViewModel model)
        {
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Timing.Time dbo = Mapper.Map<DBOs.Timing.Time>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = user.Id.Value;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.UpdateOnly(dbo,
                        fields => new
                        {
                            fields.Start,
                            fields.Stop,
                            fields.WorkerContactId,
                            fields.ModifiedByUserId,
                            fields.UtcModified
                        },
                        where => where.Id == dbo.Id);
                }

                return RedirectToAction("Details", new { Id = id });
            }
            catch
            {
                return View(model);
            }
        }
    }
}
