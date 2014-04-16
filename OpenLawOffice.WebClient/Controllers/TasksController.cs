namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class TasksController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
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
            List<ViewModels.Tasks.TaskViewModel> viewModelList = new List<ViewModels.Tasks.TaskViewModel>();
            List<Common.Models.Tasks.Task> modelList = OpenLawOffice.Data.Tasks.Task.ListChildren(id);
            
            modelList.ForEach(x =>
            {
                ViewModels.Tasks.TaskViewModel viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x);
                    
                if (viewModel.IsGroupingTask)
                {
                    if (OpenLawOffice.Data.Tasks.Task.GetTaskForWhichIAmTheSequentialPredecessor(x.Id.Value) != null)
                        viewModel.Type = "Sequential Group";
                    else
                        viewModel.Type = "Group";
                }
                else
                {
                    if (x.SequentialPredecessor != null)
                        viewModel.Type = "Sequential";
                    else
                        viewModel.Type = "Standard";
                }

                viewModelList.Add(viewModel);
            });

            return viewModelList;
        }

        public static List<ViewModels.Tasks.TaskViewModel> GetListForMatter(Guid matterid)
        {
            List<ViewModels.Tasks.TaskViewModel> viewModelList = new List<ViewModels.Tasks.TaskViewModel>();
            List<Common.Models.Tasks.Task> modelList = OpenLawOffice.Data.Tasks.Task.ListForMatter(matterid);

            modelList.ForEach(x =>
            {
                ViewModels.Tasks.TaskViewModel viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x);

                if (viewModel.IsGroupingTask)
                {
                    if (OpenLawOffice.Data.Tasks.Task.GetTaskForWhichIAmTheSequentialPredecessor(x.Id.Value) 
                        != null)
                        viewModel.Type = "Sequential Group";
                    else
                        viewModel.Type = "Group";
                }
                else
                {
                    if (x.SequentialPredecessor != null)
                        viewModel.Type = "Sequential";
                    else
                        viewModel.Type = "Standard";
                }

                viewModelList.Add(viewModel);
            });

            return viewModelList;
        }

        //
        // GET: /Matter/Details/9acb1b4f-0442-4c9b-a550-ad7478e36fb2
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(long id)
        {
            ViewModels.Tasks.TaskViewModel viewModel = null;
            Common.Models.Tasks.Task model = OpenLawOffice.Data.Tasks.Task.Get(id);
            viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model);
            PopulateCoreDetails(viewModel);

            if (model.Parent != null && model.Parent.Id.HasValue)
            {
                model.Parent = OpenLawOffice.Data.Tasks.Task.Get(model.Parent.Id.Value);
                viewModel.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Parent);
            }

            if (model.SequentialPredecessor != null && model.SequentialPredecessor.Id.HasValue)
            {
                model.SequentialPredecessor = OpenLawOffice.Data.Tasks.Task.Get(model.SequentialPredecessor.Id.Value);
                viewModel.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.SequentialPredecessor);
            }

            Common.Models.Matters.Matter matter = OpenLawOffice.Data.Tasks.Task.GetRelatedMatter(model.Id.Value);

            ViewData["MatterId"] = matter.Id.Value;

            return View(viewModel);
        }
        
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(long id)
        {
            ViewModels.Tasks.TaskViewModel viewModel = null;
            Common.Models.Tasks.Task model = OpenLawOffice.Data.Tasks.Task.Get(id);
            viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model);

            if (model.Parent != null && model.Parent.Id.HasValue)
            {
                model.Parent = OpenLawOffice.Data.Tasks.Task.Get(model.Parent.Id.Value);
                viewModel.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Parent);
            }

            if (model.SequentialPredecessor != null && model.SequentialPredecessor.Id.HasValue)
            {
                model.SequentialPredecessor = OpenLawOffice.Data.Tasks.Task.Get(model.SequentialPredecessor.Id.Value);
                viewModel.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.SequentialPredecessor);
            }

            Common.Models.Matters.Matter matter = OpenLawOffice.Data.Tasks.Task.GetRelatedMatter(model.Id.Value);

            ViewData["MatterId"] = matter.Id.Value;

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(long id, ViewModels.Tasks.TaskViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.Task model = Mapper.Map<Common.Models.Tasks.Task>(viewModel);
                Common.Models.Matters.Matter matterModel = OpenLawOffice.Data.Tasks.Task.GetRelatedMatter(id);

                Common.Models.Tasks.Task currentModel = OpenLawOffice.Data.Tasks.Task.Get(id);

                if (model.Parent != null && model.Parent.Id.HasValue)
                {
                    if (model.Parent.Id.Value == model.Id.Value)
                    {
                        //  Task is trying to set itself as its parent
                        ModelState.AddModelError("Parent.Id", "Parent cannot be the task itself.");
                        ViewData["MatterId"] = matterModel.Id.Value;
                        return View(viewModel);
                    }
                }

                model = OpenLawOffice.Data.Tasks.Task.Edit(model, currentUser);

                return RedirectToAction("Details", new { Id = id });
            }
            catch
            {
                return View(viewModel);
            }
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            ViewData["MatterId"] = Request["MatterId"];
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Tasks.TaskViewModel viewModel)
        {
            Guid matterid = Guid.Empty;

            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Tasks.Task model = Mapper.Map<Common.Models.Tasks.Task>(viewModel);
                model = OpenLawOffice.Data.Tasks.Task.Create(model, currentUser);
                matterid = Guid.Parse(Request["MatterId"]);
                OpenLawOffice.Data.Tasks.Task.RelateMatter(model, matterid, currentUser);
                return RedirectToAction("Details", new { Id = model.Id });
            }
            catch
            {
                ViewData["MatterId"] = Request["MatterId"];
                return View(viewModel);
            }
        }
        
        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Time(long id)
        {
            List<ViewModels.Timing.TimeViewModel> viewModelList = new List<ViewModels.Timing.TimeViewModel>();
            List<Common.Models.Timing.Time> modelList = OpenLawOffice.Data.Timing.Time.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Timing.TimeViewModel viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);
                Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);
                viewModel.Worker = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                viewModel.WorkerDisplayName = viewModel.Worker.DisplayName;
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Contacts", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Contacts(long id)
        {
            List<ViewModels.Tasks.TaskAssignedContactViewModel> viewModelList = new List<ViewModels.Tasks.TaskAssignedContactViewModel>();
            List<Common.Models.Tasks.TaskAssignedContact> modelList = OpenLawOffice.Data.Tasks.TaskAssignedContact.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Tasks.TaskAssignedContactViewModel viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(x);
                Common.Models.Contacts.Contact contact = OpenLawOffice.Data.Contacts.Contact.Get(x.Contact.Id.Value);
                viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tags(long id)
        {
            List<ViewModels.Tasks.TaskTagViewModel> viewModelList = new List<ViewModels.Tasks.TaskTagViewModel>();
            List<Common.Models.Tasks.TaskTag> modelList = OpenLawOffice.Data.Tasks.TaskTag.ListForTask(id);

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(x));
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ResponsibleUsers(long id)
        {
            List<ViewModels.Tasks.TaskResponsibleUserViewModel> viewModelList = new List<ViewModels.Tasks.TaskResponsibleUserViewModel>();
            List<Common.Models.Tasks.TaskResponsibleUser> modelList = OpenLawOffice.Data.Tasks.TaskResponsibleUser.ListForTask(id);

            modelList.ForEach(x =>
            {
                Common.Models.Security.User user = OpenLawOffice.Data.Security.User.Get(x.User.Id.Value);
                ViewModels.Tasks.TaskResponsibleUserViewModel viewModel = Mapper.Map<ViewModels.Tasks.TaskResponsibleUserViewModel>(x);
                viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(user);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Notes(long id)
        {
            List<ViewModels.Notes.NoteViewModel> viewModelList = new List<ViewModels.Notes.NoteViewModel>();
            List<Common.Models.Notes.Note> modelList = Data.Notes.Note.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Notes.NoteViewModel viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(x);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Documents(long id)
        {
            List<ViewModels.Documents.DocumentViewModel> viewModelList = new List<ViewModels.Documents.DocumentViewModel>();
            List<Common.Models.Documents.Document> modelList = Data.Documents.Document.ListForTask(id);

            modelList.ForEach(x =>
            {
                ViewModels.Documents.DocumentViewModel viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(x);
                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
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
