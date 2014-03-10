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
                DBOs.Tasks.TaskMatter taskMatter = db.Single<DBOs.Tasks.TaskMatter>(new { TaskId = id });

                if (taskMatter == null)
                    throw new ArgumentException("No matter exists paired to the specified task id");

                List<DBOs.Tasks.Task> list = db.SqlList<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@TaskId AND " +
                    "\"id\" in (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "\"utc_disabled\" is null",
                    new { TaskId = id, MatterId = taskMatter.MatterId });

                list.ForEach(dbo =>
                {
                    ViewModels.Tasks.TaskViewModel model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);
                    
                    if (model.IsGroupingTask)
                    {
                        List<DBOs.Tasks.Task> seq = db.SqlList<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@TaskId",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential Group";
                        else
                            model.Type = "Group";
                    }
                    else
                    {
                        List<DBOs.Tasks.Task> seq = db.SqlList<DBOs.Tasks.Task>(
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

                List<DBOs.Tasks.Task> list = db.SqlList<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\" is null AND " +
                    "\"id\" in (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                    "\"utc_disabled\" is null",
                    new { MatterId = matterid });

                list.ForEach(dbo =>
                {
                    ViewModels.Tasks.TaskViewModel model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);

                    if (model.IsGroupingTask)
                    {
                        List<DBOs.Tasks.Task> seq = db.SqlList<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@TaskId",
                            new { TaskId = model.Id.Value });

                        if (seq != null && seq.Count > 0)
                            model.Type = "Sequential Group";
                        else
                            model.Type = "Group";
                    }
                    else
                    {
                        List<DBOs.Tasks.Task> seq = db.SqlList<DBOs.Tasks.Task>(
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
                DBOs.Tasks.Task dbo = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Tasks.TaskMatter dboTaskMatter = db.Single<DBOs.Tasks.TaskMatter>(
                    "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                    new { TaskId = dbo.Id });

                model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);
                matterid = dboTaskMatter.MatterId;

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Tasks.Task parentDbo = db.SingleById<DBOs.Tasks.Task>(dbo.ParentId);
                    model.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(parentDbo);
                }

                if (dbo.SequentialPredecessorId.HasValue)
                {
                    DBOs.Tasks.Task sequentialPredecessorDbo = db.SingleById<DBOs.Tasks.Task>(dbo.SequentialPredecessorId);
                    model.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(sequentialPredecessorDbo);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            ViewData["MatterId"] = matterid;

            return View(model);
        }

        public static void UpdateGroupingTaskProperties(DBOs.Tasks.Task groupingTaskDbo, IDbConnection db)
        {
            bool groupingTaskChanged = false;

            // Projected Start - can probably clean this up into a single query
            DBOs.Tasks.Task temp = db.Single<DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_start\" DESC limit 1",
                new { ParentId = groupingTaskDbo.Id });

            // If temp.ProjectedStart has a value then we know that there are no rows
            // with null value and so, we may update the grouping task to be the
            // earliest projected start value.  However, if null, then we need to
            // set the grouping task's projected start value to null.

            if (temp.ProjectedStart.HasValue)
            {
                temp = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_start\" ASC limit 1",
                    new { ParentId = groupingTaskDbo.Id });
                if (groupingTaskDbo.ProjectedStart != temp.ProjectedStart)
                {
                    groupingTaskDbo.ProjectedStart = temp.ProjectedStart;
                    groupingTaskChanged = true;
                }
            }
            else
            {
                if (groupingTaskDbo.ProjectedStart.HasValue)
                {
                    groupingTaskDbo.ProjectedStart = null;
                    groupingTaskChanged = true;
                }
            }

            // Due Date
            temp = db.Single<DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"due_date\" DESC limit 1",
                new { ParentId = groupingTaskDbo.Id });
            if (temp.DueDate != groupingTaskDbo.DueDate)
            {
                groupingTaskDbo.DueDate = temp.DueDate;
                groupingTaskChanged = true;
            }

            // Projected End
            temp = db.Single<DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_end\" DESC limit 1",
                new { ParentId = groupingTaskDbo.Id });
            if (temp.ProjectedEnd != groupingTaskDbo.ProjectedEnd)
            {
                groupingTaskDbo.ProjectedEnd = temp.ProjectedEnd;
                groupingTaskChanged = true;
            }

            // Actual End
            temp = db.Single<DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"actual_end\" DESC limit 1",
                new { ParentId = groupingTaskDbo.Id });
            if (temp.ActualEnd != groupingTaskDbo.ActualEnd)
            {
                groupingTaskDbo.ActualEnd = temp.ActualEnd;
                groupingTaskChanged = true;
            }

            // Update grouping task if needed
            if (groupingTaskChanged)
            {
                db.UpdateOnly(groupingTaskDbo,
                    fields => new
                    {
                        fields.ProjectedStart,
                        fields.DueDate,
                        fields.ProjectedEnd,
                        fields.ActualEnd
                    },
                    where => where.Id == groupingTaskDbo.Id);
            }
        }

        public static DBOs.Tasks.Task GetParentTask(long taskId, IDbConnection db)
        {
            return db.Single<DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"id\" in (SELECT \"parent_id\" FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null) AND \"utc_disabled\" is null",
                new { Id = taskId });
        }

        public static DBOs.Tasks.Task GetParentTask(DBOs.Tasks.Task taskDbo, IDbConnection db)
        {
            if (!taskDbo.ParentId.HasValue)
                return null;

            return GetParentTask(taskDbo.Id, db);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(long id)
        {
            Guid matterid;
            ViewModels.Tasks.TaskViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {

                // Load base DBO
                DBOs.Tasks.Task dbo = db.Single<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                DBOs.Tasks.TaskMatter dboTaskMatter = db.Single<DBOs.Tasks.TaskMatter>(
                    "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                    new { TaskId = dbo.Id });

                model = Mapper.Map<ViewModels.Tasks.TaskViewModel>(dbo);
                matterid = dboTaskMatter.MatterId;

                if (dbo.ParentId.HasValue)
                {
                    DBOs.Tasks.Task parentDbo = db.SingleById<DBOs.Tasks.Task>(dbo.ParentId);
                    model.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(parentDbo);
                }

                if (dbo.SequentialPredecessorId.HasValue)
                {
                    DBOs.Tasks.Task sequentialPredecessorDbo = db.SingleById<DBOs.Tasks.Task>(dbo.SequentialPredecessorId);
                    model.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(sequentialPredecessorDbo);
                }

                // Core Details
                PopulateCoreDetails(model);
            }

            ViewData["MatterId"] = matterid;

            return View(model);
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(long id, ViewModels.Tasks.TaskViewModel model)
        {
            /*
             * We need to consider how to handle the relationship modifications
             * 
             * First, basic assumptions:
             * 1) If a task has a sequential predecessor then it cannot independently specify its parent
             * 
             * 
             * Parent - if the parent is modified
             * 
             * Sequential Predecessor
             * 1) If changed, need to cascade changes to all subsequent sequence members
             * 2) If removed from sequence, need to defer to user's parent selection
             * 3) If added to sequence, need to override user's parent selection
             * 
             * 
             * UI should be like:
             * 
             * If sequence member:
             * [Remove from Sequence]
             * 
             * 
             * If NOT sequence member:
             * This task is not currently part of a task sequence.  If you would like to make this
             * task part of a task sequence, click here.
             * -- OnClick -->
             * Please select the task you wish t
             * 
             */
            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                DBOs.Tasks.Task dbo = Mapper.Map<DBOs.Tasks.Task>(model);
                dbo.UtcModified = DateTime.UtcNow;
                dbo.ModifiedByUserId = user.Id.Value;

                /* From the submission point of view, we care about checking for a sequence predecessor first,
                 * if it is not specified, then we can care about the user specified parent.
                 */

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        // We need to pull the current task for compairison
                        DBOs.Tasks.Task currentTaskDbo = db.SingleById<DBOs.Tasks.Task>(id);


                        // When we actually implement sequential lists, we need to remove the following lines
                        // these currently prevent sequential lists

                        model.SequentialPredecessor = null;

                        // Need to test for parent being sequence (not allowed)
                        if (dbo.ParentId.HasValue)
                        {
                            DBOs.Tasks.Task proposedParentTask = db.SingleById<DBOs.Tasks.Task>(dbo.ParentId);
                            DBOs.Tasks.Task seqChild = db.Single<DBOs.Tasks.Task>(
                                "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null ORDER BY \"id\" ASC limit 1",
                                new { Id = proposedParentTask.Id });
                            DBOs.Tasks.TaskMatter dboTaskMatter = db.Single<DBOs.Tasks.TaskMatter>(
                                "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                                new { TaskId = dbo.Id });

                            if (proposedParentTask.Id == dbo.Id)
                            {
                                //  Task is trying to set itself as its parent
                                ModelState.AddModelError("Parent.Id", "Parent cannot be the task itself.");
                                ViewData["MatterId"] = dboTaskMatter.MatterId;
                                return View(model);
                            }

                            if (seqChild != null)
                            {
                                ModelState.AddModelError("Parent.Id", "Parent cannot be a sequential group.");
                                ViewData["MatterId"] = dboTaskMatter.MatterId;
                                return View(model);
                            }

                            db.UpdateOnly(dbo,
                                fields => new
                                {
                                    fields.ActualEnd,
                                    fields.Description,
                                    fields.DueDate,
                                    fields.ModifiedByUserId,
                                    fields.ParentId,
                                    fields.ProjectedEnd,
                                    fields.ProjectedStart,
                                    fields.Title,
                                    fields.UtcModified
                                },
                                where => where.Id == dbo.Id);

                            UpdateGroupingTaskProperties(proposedParentTask, db);
                        }
                        else
                        {
                            db.UpdateOnly(dbo,
                                fields => new
                                {
                                    fields.ActualEnd,
                                    fields.Description,
                                    fields.DueDate,
                                    fields.ModifiedByUserId,
                                    fields.ParentId,
                                    fields.ProjectedEnd,
                                    fields.ProjectedStart,
                                    fields.Title,
                                    fields.UtcModified
                                },
                                where => where.Id == dbo.Id);
                        }


                        #region Commented Out Sequential Predecessor Code
                        //if (model.SequentialPredecessor.Id.HasValue 
                        //    && model.SequentialPredecessor.Id.Value > 0)
                        //{
                        //    // SeqPred exists in user request

                        //    if (!currentTaskDbo.SequentialPredecessorId.HasValue)
                        //    {
                        //        // Did not have value, user adding value -> 
                        //        // Overwrite prior parent with sequence parent
                        //        // Cascade pred ids down
                        //        // Calculate new projected start, due date, projected end and actual end

                        //        // Get the sequential predecessor task
                        //        DBOs.Tasks.Task seqPredTaskDbo = GetSequentialPredecessor(dbo, db);
                                
                        //        // Get the grouping task
                        //        DBOs.Tasks.Task groupingTaskDbo = GetParentTask(seqPredTaskDbo, db);

                        //        InsertTaskIntoSequence(dbo, seqPredTaskDbo.Id, db);
                        //    }
                        //    else
                        //    {
                        //        // Was a sequence, stays a sequence, just relocating, but may be relocating to a different sequence

                        //        if (currentTaskDbo.SequentialPredecessorId.Value
                        //            != model.SequentialPredecessor.Id.Value)
                        //        {
                        //            // Relocate
                        //            RelocateTaskInSequence(dbo, dbo.SequentialPredecessorId.Value, db);
                        //        }
                        //        else
                        //        {
                        //             // Simply update the regular properties

                        //            // Need to test for parent being sequence (not allowed)
                        //            if (dbo.ParentId.HasValue)
                        //            {
                        //                DBOs.Tasks.Task proposedParentTask = db.SingleById<DBOs.Tasks.Task>(dbo.ParentId);
                        //                DBOs.Tasks.Task seqChild = db.Single<DBOs.Tasks.Task>(
                        //                    "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null ORDER BY \"id\" ASC limit 1",
                        //                    new { Id = proposedParentTask.Id });
                        //                DBOs.Tasks.TaskMatter dboTaskMatter = db.Single<DBOs.Tasks.TaskMatter>(
                        //                    "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                        //                    new { TaskId = dbo.Id });
                        //                ModelState.AddModelError("Parent.Id", "Parent cannot be set to a sequential group.");
                        //                ViewData["MatterId"] = dboTaskMatter.MatterId;
                        //                return View(model);
                        //            }


                        //            db.UpdateOnly(dbo,
                        //                fields => new
                        //                {
                        //                    fields.ActualEnd,
                        //                    fields.Description,
                        //                    fields.DueDate,
                        //                    fields.ModifiedByUserId,
                        //                    fields.ParentId,
                        //                    fields.ProjectedEnd,
                        //                    fields.ProjectedStart,
                        //                    fields.Title,
                        //                    fields.UtcModified
                        //                },
                        //                where => where.Id == dbo.Id);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    // SeqPred does not exist in user request

                        //    if (currentTaskDbo.SequentialPredecessorId.HasValue)
                        //    {
                        //        // Had a value, user removing value ->
                        //        // Cascade pred ids down
                        //        // Calculate new projected start, due date, projected end and actual end
                        //        RemoveTaskFromSequence(dbo, db);
                        //    }
                        //    else
                        //    {
                        //        // Simply update the regular properties

                        //        // Need to test for parent being sequence (not allowed)
                        //        if (dbo.ParentId.HasValue)
                        //        {
                        //            DBOs.Tasks.Task proposedParentTask = db.SingleById<DBOs.Tasks.Task>(dbo.ParentId);
                        //            DBOs.Tasks.Task seqChild = db.Single<DBOs.Tasks.Task>(
                        //                "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null ORDER BY \"id\" ASC limit 1",
                        //                new { Id = proposedParentTask.Id });
                        //            DBOs.Tasks.TaskMatter dboTaskMatter = db.Single<DBOs.Tasks.TaskMatter>(
                        //                "SELECT * FROM \"task_matter\" WHERE \"task_id\"=@TaskId",
                        //                new { TaskId = dbo.Id });

                        //            if (proposedParentTask.Id == dbo.Id)
                        //            {
                        //                //  Task is trying to set itself as its parent
                        //                ModelState.AddModelError("Parent.Id", "Parent cannot be the task itself.");
                        //                ViewData["MatterId"] = dboTaskMatter.MatterId;
                        //                return View(model);
                        //            }

                        //            if (seqChild != null)
                        //            {
                        //                ModelState.AddModelError("Parent.Id", "Parent cannot be a sequential group.");
                        //                ViewData["MatterId"] = dboTaskMatter.MatterId;
                        //                return View(model);
                        //            }
                        //        }

                        //        db.UpdateOnly(dbo,
                        //            fields => new
                        //            {
                        //                fields.ActualEnd,
                        //                fields.Description,
                        //                fields.DueDate,
                        //                fields.ModifiedByUserId,
                        //                fields.ParentId,
                        //                fields.ProjectedEnd,
                        //                fields.ProjectedStart,
                        //                fields.Title,
                        //                fields.UtcModified
                        //            },
                        //            where => where.Id == dbo.Id);
                        //    }
                        //}

                        #endregion

                        tran.Commit();
                    }
                }

                return RedirectToAction("Details", new { Id = id });
            }
            catch
            {
                return View(model);
            }
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            ViewData["MatterId"] = Request["MatterId"];
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Tasks.Task", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Tasks.TaskViewModel model)
        {
            Guid matterid = Guid.Empty;

            try
            {
                Common.Models.Security.User user = UserCache.Instance.Lookup(Request);

                // TODO : need to make this verify with DB
                matterid = Guid.Parse(Request["MatterId"]);

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        DBOs.Tasks.Task taskDbo = Mapper.Map<DBOs.Tasks.Task>(model);
                        taskDbo.CreatedByUserId = taskDbo.ModifiedByUserId = user.Id.Value;
                        taskDbo.UtcCreated = taskDbo.UtcModified = DateTime.UtcNow;

                        db.Insert<DBOs.Tasks.Task>(taskDbo);
                        taskDbo.Id = db.LastInsertId();

                        DBOs.Tasks.TaskMatter taskMatterDbo = new DBOs.Tasks.TaskMatter()
                        {
                            Id = Guid.NewGuid(),
                            MatterId = matterid,
                            TaskId = taskDbo.Id,
                            UtcCreated = DateTime.UtcNow,
                            UtcModified = DateTime.UtcNow,
                            CreatedByUserId = user.Id.Value,
                            ModifiedByUserId = user.Id.Value
                        };

                        db.Insert<DBOs.Tasks.TaskMatter>(taskMatterDbo);

                        tran.Commit();

                        return RedirectToAction("Details", new { Id = taskDbo.Id });
                    }
                }
            }
            catch
            {
                ViewData["MatterId"] = Request["MatterId"];
                return View(model);
            }
        }
        
        [SecurityFilter(SecurityAreaName = "Timing.Time", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Time(long id)
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Contacts.Contact", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Contacts(long id)
        {
            List<ViewModels.Tasks.TaskAssignedContactViewModel> modelList = new List<ViewModels.Tasks.TaskAssignedContactViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Tasks.TaskAssignedContact> list = db.SqlList<DBOs.Tasks.TaskAssignedContact>(
                    "SELECT * FROM \"task_assigned_contact\" WHERE \"task_id\"=@TaskId AND \"utc_disabled\" is null",
                    new { TaskId = id });

                list.ForEach(dbo =>
                {
                    ViewModels.Tasks.TaskAssignedContactViewModel vm = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(dbo);

                    DBOs.Contacts.Contact contactDbo = db.SingleById<DBOs.Contacts.Contact>(dbo.ContactId);

                    vm.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contactDbo);
                    modelList.Add(vm);
                });
            }

            return View(modelList);
        }

        #region Commented Out Sequential Predecessor Code

        //public static DBOs.Tasks.Task GetSequentialPredecessor(DBOs.Tasks.Task taskDbo, IDbConnection db)
        //{
        //    return db.Single<DBOs.Tasks.Task>(
        //        "SELECT * FROM \"task\" WHERE \"id\"=@SeqPredId AND \"utc_disabled\" is null",
        //        new { SeqPredId = taskDbo.SequentialPredecessorId });
        //}

        //public static string InsertTaskIntoSequence(DBOs.Tasks.Task taskToInsertDbo, long idOfPredecessor, IDbConnection db)
        //{
        //    DBOs.Tasks.Task nextTask = null, lastTask = null;

        //    // This handles if a member is selected
        //    DBOs.Tasks.Task groupingTaskDbo = GetParentTask(idOfPredecessor, db);

        //    // What if a member is not selected, but the sequential grouping task itself is selected?
        //    if (groupingTaskDbo == null)
        //    {
        //        groupingTaskDbo = db.SingleById<DBOs.Tasks.Task>(idOfPredecessor);
        //        if (!groupingTaskDbo.IsGroupingTask)
        //            return "Predecessor must be either a sequence member or grouping sequence.";
        //    }

        //    // Update the edited dbo
        //    taskToInsertDbo.SequentialPredecessorId = idOfPredecessor;
        //    taskToInsertDbo.ParentId = groupingTaskDbo.Id;
        //    db.UpdateOnly(taskToInsertDbo,
        //        fields => new
        //        {
        //            fields.Title,
        //            fields.Description,
        //            fields.ProjectedStart,
        //            fields.DueDate,
        //            fields.ProjectedEnd,
        //            fields.ActualEnd,
        //            fields.ParentId,
        //            fields.ModifiedByUserId,
        //            fields.UtcModified
        //        },
        //        where => where.Id == taskToInsertDbo.Id);

        //    DBOs.Tasks.Task tempTask;

        //    // Load task currently in the position
        //    tempTask = db.Single<DBOs.Tasks.Task>(
        //        "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null",
        //        new { Id = idOfPredecessor });

        //    // We can improve this later - this layout helps with debugging
        //    if (tempTask != null)
        //    {
        //        // Update the seqpredid of taskToInsert
        //        db.UpdateOnly(new DBOs.Tasks.Task()
        //            {
        //                SequentialPredecessorId = idOfPredecessor
        //            },
        //            fields => new
        //            {
        //                fields.SequentialPredecessorId
        //            },
        //            where => where.Id == taskToInsertDbo.Id);

        //        // Load the next task to update into memory
        //        nextTask = db.Single<DBOs.Tasks.Task>(
        //            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null",
        //            new { Id = tempTask.Id });

        //        // Update tempTask's seqpred
        //        db.UpdateOnly(new DBOs.Tasks.Task()
        //            {
        //                SequentialPredecessorId = taskToInsertDbo.Id
        //            },
        //            fields => new
        //            {
        //                fields.SequentialPredecessorId
        //            },
        //            where => where.Id == tempTask.Id);

        //        while (nextTask != null)
        //        {
        //            // Make next task be temp task
        //            tempTask = nextTask;

        //            // Load new next task
        //            nextTask = db.Single<DBOs.Tasks.Task>(
        //                "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null",
        //                new { Id = tempTask.Id });

        //            // Update tempTask's seqpred
        //            db.UpdateOnly(new DBOs.Tasks.Task()
        //                {
        //                    SequentialPredecessorId = taskToInsertDbo.Id
        //                },
        //                fields => new
        //                {
        //                    fields.SequentialPredecessorId
        //                },
        //                where => where.Id == tempTask.Id);
        //        }
        //    }
        //    else
        //    {
        //        db.UpdateOnly(new DBOs.Tasks.Task()
        //            {
        //                SequentialPredecessorId = idOfPredecessor
        //            },
        //            fields => new
        //            {
        //                fields.SequentialPredecessorId
        //            },
        //            where => where.Id == taskToInsertDbo.Id);
        //    }

        //    // Update group properties
        //    UpdateGroupingTaskProperties(groupingTaskDbo, db);
        //}

        //public static void RelocateTaskInSequence(DBOs.Tasks.Task taskToRelocate, long idOfPredecessor, IDbConnection db)
        //{
        //    // Below can be improved, but it will work for the time being.
        //    RemoveTaskFromSequence(taskToRelocate, db);
        //    InsertTaskIntoSequence(taskToRelocate, idOfPredecessor, db);
        //}

        //public static void RemoveTaskFromSequence(DBOs.Tasks.Task taskToRemove, IDbConnection db)
        //{
        //    // Remove taskToRelocate from its current sequence 
        //    //   Set taskToRelocate.ParentId to null (yes, this must push to the db so the query will work right)
        //    //   Query for nextTask (being the task with taskToRelocate as its sequential predecessor)
        //    //   Update it to taskToRelocate's sequential predecessor
        //    //   Cascade this query and update down the chain to move all tasks after taskToRelocate up one position

        //    DBOs.Tasks.Task nextTask = null, lastTask = null;

        //    DBOs.Tasks.Task groupingTaskDbo = null;
        //    long? parentIdHolder = taskToRemove.ParentId;

        //    taskToRemove.ParentId = null;
        //    db.UpdateOnly(taskToRemove,
        //        fields => new
        //        {
        //            fields.ParentId
        //        },
        //        where => where.Id == taskToRemove.Id);

        //    nextTask = db.Single<DBOs.Tasks.Task>(
        //        "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null",
        //        new { Id = taskToRemove.Id });

        //    lastTask = taskToRemove;

        //    while (nextTask != null)
        //    {
        //        nextTask.SequentialPredecessorId = lastTask.SequentialPredecessorId;

        //        db.UpdateOnly(nextTask,
        //            fields => new
        //            {
        //                fields.SequentialPredecessorId
        //            },
        //            where => where.Id == nextTask.Id);

        //        lastTask = nextTask;
        //        nextTask = db.Single<DBOs.Tasks.Task>(
        //            "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@Id AND \"utc_disabled\" is null",
        //            new { Id = nextTask.Id });
        //    }

        //    db.Delete<DBOs.Tasks.Task>(taskToRemove);

        //    // Update group properties
        //    if (parentIdHolder.HasValue)
        //    {
        //        if ((groupingTaskDbo = GetParentTask(parentIdHolder.Value, db)) != null)
        //            UpdateGroupingTaskProperties(groupingTaskDbo, db);
        //    }
        //}

        #endregion

    }
}
