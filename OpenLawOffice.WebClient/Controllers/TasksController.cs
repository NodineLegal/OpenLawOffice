// -----------------------------------------------------------------------
// <copyright file="TasksController.cs" company="Nodine Legal, LLC">
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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;
    using System.Configuration;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class TasksController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        [HttpGet]
        public ActionResult ListChildrenJqGrid(long? id)
        {
            List<ViewModels.Tasks.TaskViewModel> modelList;
            ViewModels.JqGridObject jqObject;
            List<object> anonList;
            int level = 0;

            if (id == null)
            {
                // jqGrid uses nodeid by default
                if (!string.IsNullOrEmpty(Request["nodeid"]))
                    id = long.Parse(Request["nodeid"]);
            }

            anonList = new List<object>();

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
            List<ViewModels.Tasks.TaskViewModel> viewModelList;

            viewModelList = new List<ViewModels.Tasks.TaskViewModel>();
            ViewModels.Tasks.TaskViewModel viewModel;

            Data.Tasks.Task.ListChildren(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x);

                if (viewModel.IsGroupingTask)
                {
                    if (Data.Tasks.Task.GetTaskForWhichIAmTheSequentialPredecessor(x.Id.Value) != null)
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
            List<ViewModels.Tasks.TaskViewModel> viewModelList;
            ViewModels.Tasks.TaskViewModel viewModel;

            viewModelList = new List<ViewModels.Tasks.TaskViewModel>();

            Data.Tasks.Task.ListForMatter(matterid).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x);

                if (viewModel.IsGroupingTask)
                {
                    if (Data.Tasks.Task.GetTaskForWhichIAmTheSequentialPredecessor(x.Id.Value)
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

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(long id)
        {
            ViewModels.Tasks.TaskViewModel viewModel;
            Common.Models.Tasks.Task model;
            Common.Models.Matters.Matter matter;

            model = Data.Tasks.Task.Get(id);

            viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model);

            PopulateCoreDetails(viewModel);

            if (model.Parent != null && model.Parent.Id.HasValue)
            {
                model.Parent = Data.Tasks.Task.Get(model.Parent.Id.Value);
                viewModel.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Parent);
            }

            if (model.SequentialPredecessor != null && model.SequentialPredecessor.Id.HasValue)
            {
                model.SequentialPredecessor = Data.Tasks.Task.Get(model.SequentialPredecessor.Id.Value);
                viewModel.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.SequentialPredecessor);
            }

            matter = Data.Tasks.Task.GetRelatedMatter(model.Id.Value);

            ViewData["MatterId"] = matter.Id.Value;

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(long id)
        {
            ViewModels.Tasks.TaskViewModel viewModel;
            Common.Models.Tasks.Task model;
            Common.Models.Matters.Matter matter;

            model = Data.Tasks.Task.Get(id);
            viewModel = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model);

            if (model.Parent != null && model.Parent.Id.HasValue)
            {
                model.Parent = Data.Tasks.Task.Get(model.Parent.Id.Value);
                viewModel.Parent = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Parent);
            }

            if (model.SequentialPredecessor != null && model.SequentialPredecessor.Id.HasValue)
            {
                model.SequentialPredecessor = Data.Tasks.Task.Get(model.SequentialPredecessor.Id.Value);
                viewModel.SequentialPredecessor = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.SequentialPredecessor);
            }

            matter = Data.Tasks.Task.GetRelatedMatter(model.Id.Value);

            ViewData["MatterId"] = matter.Id.Value;

            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(long id, ViewModels.Tasks.TaskViewModel viewModel)
        {
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.Task model;
            Common.Models.Matters.Matter matterModel;
            Common.Models.Tasks.Task currentModel;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Tasks.Task>(viewModel);

            matterModel = Data.Tasks.Task.GetRelatedMatter(id);

            currentModel = Data.Tasks.Task.Get(id);

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

            model = Data.Tasks.Task.Edit(model, currentUser);

            return RedirectToAction("Details", new { Id = id });
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
            Common.Models.Security.User currentUser;
            Common.Models.Tasks.Task model;
            Guid matterid = Guid.Empty;

            currentUser = UserCache.Instance.Lookup(Request);

            model = Mapper.Map<Common.Models.Tasks.Task>(viewModel);

            model = Data.Tasks.Task.Create(model, currentUser);

            matterid = Guid.Parse(Request["MatterId"]);

            Data.Tasks.Task.RelateMatter(model, matterid, currentUser);

            return RedirectToAction("Details", new { Id = model.Id });
        }

        public ActionResult TodoListForAll()
        {
            DateTime? start = null;
            DateTime? stop = null;
            List<dynamic> jsonList;
            List<Common.Models.Settings.TagFilter> tagFilters;
            if (Request["start"] != null)
                start = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["start"]));
            if (Request["stop"] != null)
                stop = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["stop"]));

            tagFilters = Common.Settings.Manager.Instance.System.GlobalTaskTagFilters.ToUserSettingsModel();

            jsonList = new List<dynamic>();

            Data.Tasks.Task.GetTodoListForAll(tagFilters, start, stop).ForEach(x =>
            {
                if (x.DueDate.HasValue)
                {
                    jsonList.Add(new
                    {
                        id = x.Id.Value,
                        title = x.Title,
                        allDay = true,
                        start = Common.Utilities.DateTimeToUnixTimestamp(x.DueDate.Value.ToLocalTime()),
                        description = x.Description
                    });
                }
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TodoListForUser(int? id)
        {
            DateTime? start = null;
            DateTime? stop = null;
            List<dynamic> jsonList;
            Common.Models.Security.User user;
            List<Common.Models.Settings.TagFilter> tagFilter;
            if (Request["start"] != null)
                start = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["start"]));
            if (Request["stop"] != null)
                stop = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["stop"]));

            if (!id.HasValue)
            {
                if (string.IsNullOrEmpty(Request["UserId"]))
                    return null;
                else
                    id = int.Parse(Request["UserId"]);
            }

            user = Data.Security.User.Get(id.Value);

            tagFilter = Data.Settings.UserTaskSettings.ListTagFiltersFor(user);

            jsonList = new List<dynamic>();

            Data.Tasks.Task.GetTodoListFor(user, tagFilter, start, stop).ForEach(x =>
            {
                if (x.DueDate.HasValue)
                {
                    jsonList.Add(new
                    {
                        id = x.Id.Value,
                        title = x.Title,
                        allDay = true,
                        start = Common.Utilities.DateTimeToUnixTimestamp(x.DueDate.Value.ToLocalTime()),
                        description = x.Description
                    });
                }
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TodoListForContact(int? id)
        {
            DateTime? start = null;
            DateTime? stop = null;
            List<dynamic> jsonList;
            Common.Models.Contacts.Contact contact;
            List<Common.Models.Settings.TagFilter> tagFilters;
            if (Request["start"] != null)
                start = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["start"]));
            if (Request["stop"] != null)
                stop = Common.Utilities.UnixTimeStampToDateTime(double.Parse(Request["stop"]));

            if (!id.HasValue)
            {
                if (string.IsNullOrEmpty(Request["ContactId"]))
                    return null;
                else
                    id = int.Parse(Request["ContactId"]);
            }

            contact = Data.Contacts.Contact.Get(id.Value);

            tagFilters = Common.Settings.Manager.Instance.System.GlobalTaskTagFilters.ToUserSettingsModel();

            jsonList = new List<dynamic>();

            Data.Tasks.Task.GetTodoListFor(contact, tagFilters, start, stop).ForEach(x =>
            {
                if (x.DueDate.HasValue)
                {
                    jsonList.Add(new
                    {
                        id = x.Id.Value,
                        title = x.Title,
                        allDay = true,
                        start = Common.Utilities.DateTimeToUnixTimestamp(x.DueDate.Value.ToLocalTime()),
                        description = x.Description
                    });
                }
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        [SecurityFilter(SecurityAreaName = "Timing", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Time(long id)
        {
            List<ViewModels.Timing.TimeViewModel> viewModelList;
            ViewModels.Timing.TimeViewModel viewModel;
            Common.Models.Contacts.Contact contact;

            viewModelList = new List<ViewModels.Timing.TimeViewModel>();

            Data.Timing.Time.ListForTask(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Timing.TimeViewModel>(x);

                contact = Data.Contacts.Contact.Get(viewModel.Worker.Id.Value);

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
            List<ViewModels.Tasks.TaskAssignedContactViewModel> viewModelList;
            ViewModels.Tasks.TaskAssignedContactViewModel viewModel;
            Common.Models.Contacts.Contact contact;

            viewModelList = new List<ViewModels.Tasks.TaskAssignedContactViewModel>();

            Data.Tasks.TaskAssignedContact.ListForTask(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Tasks.TaskAssignedContactViewModel>(x);

                contact = Data.Contacts.Contact.Get(x.Contact.Id.Value);

                viewModel.Contact = Mapper.Map<ViewModels.Contacts.ContactViewModel>(contact);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Tags(long id)
        {
            List<ViewModels.Tasks.TaskTagViewModel> viewModelList;

            viewModelList = new List<ViewModels.Tasks.TaskTagViewModel>();

            Data.Tasks.TaskTag.ListForTask(id).ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(x));
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Tasks", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ResponsibleUsers(long id)
        {
            List<ViewModels.Tasks.TaskResponsibleUserViewModel> viewModelList;
            Common.Models.Security.User user;
            ViewModels.Tasks.TaskResponsibleUserViewModel viewModel;

            viewModelList = new List<ViewModels.Tasks.TaskResponsibleUserViewModel>();

            Data.Tasks.TaskResponsibleUser.ListForTask(id).ForEach(x =>
            {
                user = Data.Security.User.Get(x.User.Id.Value);

                viewModel = Mapper.Map<ViewModels.Tasks.TaskResponsibleUserViewModel>(x);

                viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(user);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Notes", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Notes(long id)
        {
            List<ViewModels.Notes.NoteViewModel> viewModelList;
            ViewModels.Notes.NoteViewModel viewModel;

            viewModelList = new List<ViewModels.Notes.NoteViewModel>();

            Data.Notes.Note.ListForTask(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(x);

                viewModelList.Add(viewModel);
            });

            return View(viewModelList);
        }

        [SecurityFilter(SecurityAreaName = "Documents", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Documents(long id)
        {
            List<ViewModels.Documents.DocumentViewModel> viewModelList;
            ViewModels.Documents.DocumentViewModel viewModel;

            viewModelList = new List<ViewModels.Documents.DocumentViewModel>();

            Data.Documents.Document.ListForTask(id).ForEach(x =>
            {
                viewModel = Mapper.Map<ViewModels.Documents.DocumentViewModel>(x);

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

        #endregion Commented Out Sequential Predecessor Code
    }
}