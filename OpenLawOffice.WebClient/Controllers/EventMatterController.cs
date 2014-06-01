// -----------------------------------------------------------------------
// <copyright file="EventMatterController.cs" company="Nodine Legal, LLC">
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

    public class EventMatterController : BaseController
    {
        public ActionResult SelectMatter(Guid id)
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

        public ActionResult AssignMatter(Guid id)
        {
            Guid matterId;
            Common.Models.Matters.Matter matter;
            Common.Models.Events.Event evnt;

            matterId = Guid.Parse(Request["MatterId"]);

            matter = Data.Matters.Matter.Get(matterId);

            evnt = Data.Events.Event.Get(id);

            return View(new ViewModels.Events.EventMatterViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(evnt)
            });
        }

        public ActionResult AssignEvent(Guid id)
        {
            Guid eventId;
            Common.Models.Matters.Matter matter;
            Common.Models.Events.Event evnt;

            eventId = Guid.Parse(Request["EventId"]);

            evnt = Data.Events.Event.Get(eventId);

            matter = Data.Matters.Matter.Get(id);

            return View(new ViewModels.Events.EventMatterViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(matter),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(evnt)
            });
        }

        [HttpPost]
        public ActionResult AssignMatter(Guid id, ViewModels.Events.EventMatterViewModel viewModel)
        {
            Guid matterId;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            matterId = Guid.Parse(Request["MatterId"]);

            Data.Events.EventMatter.Create(new Common.Models.Events.EventMatter()
            {
                Event = new Common.Models.Events.Event()
                {
                    Id = id
                },
                Matter = new Common.Models.Matters.Matter()
                {
                    Id = matterId
                },
            }, currentUser);

            return RedirectToAction("Matters", "Events", new { id = id });
        }

        [HttpPost]
        public ActionResult AssignEvent(Guid id, ViewModels.Events.EventMatterViewModel viewModel)
        {
            Guid eventId;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            eventId = Guid.Parse(Request["EventId"]);

            Data.Events.EventMatter.Create(new Common.Models.Events.EventMatter()
            {
                Event = new Common.Models.Events.Event()
                {
                    Id = eventId
                },
                Matter = new Common.Models.Matters.Matter()
                {
                    Id = id
                },
            }, currentUser);

            return RedirectToAction("Events", "Matters", new { id = id });
        }

        public ActionResult UnlinkMatter(Guid id)
        {
            Guid eventId;
            Common.Models.Events.EventMatter model;

            eventId = Guid.Parse(Request["EventId"]);

            model = Data.Events.EventMatter.Get(eventId, id);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            return View("Unlink", new ViewModels.Events.EventMatterViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event)
            });
        }

        [HttpPost]
        public ActionResult UnlinkMatter(Guid id, ViewModels.Events.EventMatterViewModel viewModel)
        {
            Guid eventId;
            Common.Models.Events.EventMatter model;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            eventId = Guid.Parse(Request["EventId"]);

            model = Data.Events.EventMatter.Get(eventId, viewModel.Id.Value);

            Data.Events.EventMatter.Delete(model, currentUser);

            return RedirectToAction("Matters", "Events", new { id = model.Event.Id.Value });
        }

        public ActionResult UnlinkEvent(Guid id)
        {
            Guid matterId;
            Common.Models.Events.EventMatter model;

            matterId = Guid.Parse(Request["MatterId"]);

            model = Data.Events.EventMatter.Get(id, matterId);
            model.Matter = Data.Matters.Matter.Get(model.Matter.Id.Value);
            model.Event = Data.Events.Event.Get(model.Event.Id.Value);

            return View("Unlink", new ViewModels.Events.EventMatterViewModel()
            {
                Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(model.Matter),
                Event = Mapper.Map<ViewModels.Events.EventViewModel>(model.Event)
            });
        }

        [HttpPost]
        public ActionResult UnlinkEvent(Guid id, ViewModels.Events.EventMatterViewModel viewModel)
        {
            Guid matterId;
            Common.Models.Events.EventMatter model;
            Common.Models.Security.User currentUser;

            currentUser = UserCache.Instance.Lookup(Request);

            matterId = Guid.Parse(Request["MatterId"]);

            model = Data.Events.EventMatter.Get(id, matterId);

            Data.Events.EventMatter.Delete(model, currentUser);

            return RedirectToAction("Events", "Matters", new { id = model.Matter.Id.Value });
        }
    }
}
