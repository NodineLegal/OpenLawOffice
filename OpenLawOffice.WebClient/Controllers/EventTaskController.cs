// -----------------------------------------------------------------------
// <copyright file="EventTaskController.cs" company="Nodine Legal, LLC">
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

    public class EventTaskController : BaseController
    {
        public ActionResult SelectTask(Guid id)
        {
            return View();
        }

        public ActionResult SelectEvent(Guid id)
        {
            List<ViewModels.Events.EventViewModel> list;

            list = new List<ViewModels.Events.EventViewModel>();

            Data.Events.Event.List().ForEach(x =>
            {
                list.Add(Mapper.Map<ViewModels.Events.EventViewModel>(x));
            });

            return View(list);
        }

        public ActionResult AssignTask(Guid id)
        {
            long taskId;
            Common.Models.Tasks.Task task;
            Common.Models.Events.Event evnt;

            taskId = long.Parse(Request["TaskId"]);

            task = Data.Tasks.Task.Get(taskId);

            evnt = Data.Events.Event.Get(id);

            return View(new ViewModels.Events.EventTaskViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(evnt)
            });
        }

        public ActionResult AssignEvent(long id)
        {
            Guid eventId;
            Common.Models.Tasks.Task task;
            Common.Models.Events.Event evnt;

            eventId = Guid.Parse(Request["EventId"]);

            evnt = Data.Events.Event.Get(eventId);

            task = Data.Tasks.Task.Get(id);

            return View(new ViewModels.Events.EventTaskViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(task),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(evnt)
            });
        }

        [HttpPost]
        public ActionResult AssignTask(Guid id, ViewModels.Events.EventTaskViewModel viewModel)
        {
            long taskId;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            taskId = long.Parse(Request["TaskId"]);

            Data.Events.EventTask.Create(new Common.Models.Events.EventTask()
            {
                Event = new Common.Models.Events.Event()
                {
                    Id = id
                },
                Task = new Common.Models.Tasks.Task()
                {
                    Id = taskId
                },
            }, currentUser);

            return RedirectToAction("Tasks", "Events", new { id = id });
        }

        [HttpPost]
        public ActionResult AssignEvent(long id, ViewModels.Events.EventTaskViewModel viewModel)
        {
            Guid eventId;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            eventId = Guid.Parse(Request["EventId"]);

            Data.Events.EventTask.Create(new Common.Models.Events.EventTask()
            {
                Event = new Common.Models.Events.Event()
                {
                    Id = eventId
                },
                Task = new Common.Models.Tasks.Task()
                {
                    Id = id
                },
            }, currentUser);

            return RedirectToAction("Events", "Tasks", new { id = id });
        }

        public ActionResult UnlinkMatter(long id)
        {
            Guid eventId;
            Common.Models.Events.EventTask model;

            eventId = Guid.Parse(Request["EventId"]);

            model = Data.Events.EventTask.Get(id, eventId);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            return View("Unlink", new ViewModels.Events.EventTaskViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event)
            });
        }

        [HttpPost]
        public ActionResult UnlinkMatter(long id, ViewModels.Events.EventTaskViewModel viewModel)
        {
            Guid eventId;
            Common.Models.Events.EventTask model;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            eventId = Guid.Parse(Request["EventId"]);

            model = Data.Events.EventTask.Get(id, eventId);

            Data.Events.EventTask.Delete(model, currentUser);

            return RedirectToAction("Tasks", "Events", new { id = model.Event.Id.Value });
        }

        public ActionResult UnlinkEvent(Guid id)
        {
            long taskId;
            Common.Models.Events.EventTask model;

            taskId = long.Parse(Request["TaskId"]);

            model = Data.Events.EventTask.Get(taskId, id);
            model.Task = Data.Tasks.Task.Get(model.Task.Id.Value);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            return View("Unlink", new ViewModels.Events.EventTaskViewModel()
            {
                Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(model.Task),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event)
            });
        }

        [HttpPost]
        public ActionResult UnlinkEvent(Guid id, ViewModels.Events.EventTaskViewModel viewModel)
        {
            long taskId;
            Common.Models.Events.EventTask model;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            taskId = long.Parse(Request["TaskId"]);

            model = Data.Events.EventTask.Get(taskId, id);

            Data.Events.EventTask.Delete(model, currentUser);

            return RedirectToAction("Events", "Tasks", new { id = model.Task.Id.Value });
        }
    }
}
