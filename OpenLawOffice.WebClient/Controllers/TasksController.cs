namespace OpenLawOffice.WebClient.Controllers
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

    public class TasksController : BaseController
    {

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(long? id)
        {
            ViewModels.JqGridObject jqObject;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = long.Parse(Request["nodeid"]);
            }

            List<ViewModels.Tasks.TaskViewModel> modelList;
            List<object> anonList = new List<object>();

            if (!string.IsNullOrEmpty(Request["n_level"]))
                level = int.Parse(Request["n_level"]) + 1;
            
            if (!id.HasValue)
            {
                string matterid = Request["MatterId"];
                if (string.IsNullOrEmpty(matterid))
                    return null;
                modelList = GetListForMatter(Guid.Parse(matterid));
            }
            else
            {
                modelList = GetChildrenList(id.Value);
            }

            modelList.ForEach(x =>
            {
                if (x.IsGroupingTask)
                {
                    // isLeaf = false
                    anonList.Add(new
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Type = x.Type,
                        DueDate = x.DueDate,
                        Description = x.Description,
                        level = level,
                        isLeaf = false,
                        expanded = false
                    });
                }
                else
                {
                    // isLeaf = true
                    anonList.Add(new
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Type = x.Type,
                        DueDate = x.DueDate,
                        Description = x.Description,
                        level = level,
                        isLeaf = true,
                        expanded = false
                    });
                }
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

        public static List<ViewModels.Tasks.TaskViewModel> GetChildrenList(long id)
        {
            List<ViewModels.Tasks.TaskViewModel> modelList = new List<ViewModels.Tasks.TaskViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Get the Matter Id - allows to test against task_matter
                DBOs.Tasks.TaskMatter taskMatter = db.QuerySingle<DBOs.Tasks.TaskMatter>(new { TaskId = id });

                if (taskMatter == null)
                    throw new ArgumentException("No matter exists paired to the specified task id");

                List<DBOs.Tasks.Task> list = db.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@TaskId AND " +
                    "\"id\" in (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "\"utc_disabled\" is null",
                    new { TaskId = id, MatterId = taskMatter.MatterId });

                list.ForEach(dbo =>
                {
                    ViewModels.Tasks.TaskViewModel model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);
                    
                    if (model.IsGroupingTask)
                    {
                        List<DBOs.Tasks.Task> seq = db.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@TaskId",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential Group";
                        else
                            model.Type = "Group";
                    }
                    else
                    {
                        List<DBOs.Tasks.Task> seq = db.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"id\"=@TaskId AND \"sequential_predecessor_id\" is not null",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential";
                        else
                            model.Type = "Standard";
                    }

                    modelList.Add(model);
                });
            }

            return modelList;
        }

        public static List<ViewModels.Tasks.TaskViewModel> GetListForMatter(Guid matterid)
        {
            List<ViewModels.Tasks.TaskViewModel> modelList = new List<ViewModels.Tasks.TaskViewModel>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                /* Standard Tasks are neither hierarchical or sequenced - not is_grouping_task, no parent_id
                 *  We do not need to test for sequential followers as we can infer this from the fact that a
                 *  parent_id must be set for any sequential member.  This is a design mechanism.
                 *  
                 * Grouping Tasks simply contain other tasks and their task properties should be caclulated, not stored.
                 *  Grouping tasks contain standard tasks, sequenced tasks and other grouping tasks.  
                 *  However, a grouping task containing a sequence may only contain members of the sequence,
                 *  but nothing prevents a sequence member from being a grouping task.
                 * 
                 * Sequenced Tasks are members of a sequence and may be either standard tasks or
                 *  grouping tasks.
                 *  
                 * Therefore, when loading the root tasks of a matter, we load standard and 
                 * grouping tasks.  Note, this does not include loading of any task with a non-null
                 * parent_id as that means they are grouped and therefore, not root.  However, this
                 * also means that we may simply load tasks with null parent_id fields to select
                 * all the root tasks.
                 * 
                 * 
                 * Example:
                 * ST1
                 * ST2
                 * GT1
                 *  Seq1
                 *  Seq2-GT
                 *   ST3
                 *   ST4
                 *  Seq3
                 *   GT2
                 *    GT3
                 *     Seq4
                 *     Seq5
                 *     Seq6
                 *    ST5
                 *    ST6
                 *   ST7
                 *  Seq4
                 *  Seq5
                 * ST8
                 * 
                 */

                List<DBOs.Tasks.Task> list = db.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\" is null AND " +
                    "\"id\" in (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "\"utc_disabled\" is null",
                    new { MatterId = matterid });

                list.ForEach(dbo =>
                {
                    ViewModels.Tasks.TaskViewModel model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);

                    if (model.IsGroupingTask)
                    {
                        List<DBOs.Tasks.Task> seq = db.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@TaskId",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential Group";
                        else
                            model.Type = "Group";
                    }
                    else
                    {
                        List<DBOs.Tasks.Task> seq = db.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"id\"=@TaskId AND \"sequential_predecessor_id\" is not null",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential";
                        else
                            model.Type = "Standard";
                    }

                    modelList.Add(model);
                });
            }

            return modelList;
        }

        //
        // GET: /Matter/Details/9acb1b4f-0442-4c9b-a550-ad7478e36fb2
        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(long id)
        {
            Guid matterid;
            ViewModels.Tasks.TaskViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {

                // Load base DBO
                DBOs.Tasks.Task dbo = db.QuerySingle<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Tasks.TaskMatter dboTaskMatter = db.QuerySingle<DBOs.Tasks.TaskMatter>(
                    "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                    new { TaskId = dbo.Id });

                model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);
                matterid = dboTaskMatter.MatterId;

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Tasks.Task parentDbo = db.GetById<DBOs.Tasks.Task>(dbo.ParentId);
                    model.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(parentDbo);
                }

                if (dbo.SequentialPredecessorId.HasValue)
                {
                    DBOs.Tasks.Task sequentialPredecessorDbo = db.GetById<DBOs.Tasks.Task>(dbo.SequentialPredecessorId);
                    model.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(sequentialPredecessorDbo);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            ViewData["MatterId"] = matterid;

            return View(model);
        }
    }
}
