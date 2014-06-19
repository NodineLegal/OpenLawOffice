// -----------------------------------------------------------------------
// <copyright file="CalendarController.cs" company="Nodine Legal, LLC">
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

    public class CalendarController : Controller
    {
        [Authorize(Roles = "Login, User")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Login, User")]
        public new ActionResult User()
        {
            return View();
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult Contact()
        {
            return View();
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult SelectUser()
        {
            List<ViewModels.Account.UsersViewModel> usersList;

            usersList = new List<ViewModels.Account.UsersViewModel>();

            Data.Account.Users.List().ForEach(x =>
            {
                usersList.Add(Mapper.Map<ViewModels.Account.UsersViewModel>(x));
            });

            return View(usersList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult SelectContact()
        {
            List<ViewModels.Contacts.ContactViewModel> contactList;

            contactList = new List<ViewModels.Contacts.ContactViewModel>();

            Data.Contacts.Contact.List().ForEach(x =>
            {
                contactList.Add(Mapper.Map<ViewModels.Contacts.ContactViewModel>(x));
            });

            return View(contactList);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ListAllEvents()
        {
            double start = 0;
            double? stop = null;
            List<Common.Models.Events.Event> list;
            List<dynamic> jsonList;

            if (Request["start"] != null)
                start = double.Parse(Request["start"]);
            if (Request["stop"] != null)
                stop = double.Parse(Request["stop"]);

            list = Data.Events.Event.List(start, stop);

            jsonList = new List<dynamic>();

            list.ForEach(x =>
            {
                DateTime? end = null;
                if (x.End.HasValue)
                    end = x.End.Value;

                jsonList.Add(new
                {
                    id = x.Id.Value,
                    title = x.Title,
                    allDay = x.AllDay,
                    start = Common.Utilities.DateTimeToUnixTimestamp(x.Start),
                    end = Common.Utilities.DateTimeToUnixTimestamp(end),
                    location = x.Location,
                    description = x.Description
                });
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ListEventsForUser(Guid id)
        {
            double start = 0;
            double? stop = null;
            List<Common.Models.Events.Event> list;
            List<dynamic> jsonList;

            if (Request["start"] != null)
                start = double.Parse(Request["start"]);
            if (Request["stop"] != null)
                stop = double.Parse(Request["stop"]);

            list = Data.Events.Event.ListForUser(id, start, stop);

            jsonList = new List<dynamic>();

            list.ForEach(x =>
            {
                DateTime? end = null;
                if (x.End.HasValue)
                    end = x.End.Value;

                jsonList.Add(new
                {
                    id = x.Id.Value,
                    title = x.Title,
                    allDay = x.AllDay,
                    start = Common.Utilities.DateTimeToUnixTimestamp(x.Start),
                    end = Common.Utilities.DateTimeToUnixTimestamp(end),
                    location = x.Location,
                    description = x.Description
                });
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Login, User")]
        public ActionResult ListEventsForContact(int id)
        {
            double start = 0;
            double? stop = null;
            List<Common.Models.Events.Event> list;
            List<dynamic> jsonList;

            if (Request["start"] != null)
                start = double.Parse(Request["start"]);
            if (Request["stop"] != null)
                stop = double.Parse(Request["stop"]);

            list = Data.Events.Event.ListForContact(id, start, stop);

            jsonList = new List<dynamic>();

            list.ForEach(x =>
            {
                DateTime? end = null;
                if (x.End.HasValue)
                    end = x.End.Value;

                jsonList.Add(new
                {
                    id = x.Id.Value,
                    title = x.Title,
                    allDay = x.AllDay,
                    start = Common.Utilities.DateTimeToUnixTimestamp(x.Start),
                    end = Common.Utilities.DateTimeToUnixTimestamp(end),
                    location = x.Location,
                    description = x.Description
                });
            });

            return Json(jsonList, JsonRequestBehavior.AllowGet);
        }
    }
}