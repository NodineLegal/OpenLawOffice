// -----------------------------------------------------------------------
// <copyright file="EventAssignedContactViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Events
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class EventAssignedContactViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public EventViewModel Event { get; set; }

        public Contacts.ContactViewModel Contact { get; set; }

        public string Role { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Events.EventAssignedContact, EventAssignedContactViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.CreatedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.ModifiedBy.PId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.PId.HasValue) return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.DisabledBy.PId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Event, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Events.EventViewModel()
                    {
                        Id = db.Event.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Contact, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = db.Contact.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.Role));

            Mapper.CreateMap<EventAssignedContactViewModel, Common.Models.Events.EventAssignedContact>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.CreatedBy == null || !x.CreatedBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.CreatedBy.PId
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.CreatedBy == null || !x.CreatedBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.ModifiedBy.PId
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(x =>
                {
                    if (x.DisabledBy == null || !x.DisabledBy.PId.HasValue)
                        return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = x.DisabledBy.PId.Value
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Event, opt => opt.ResolveUsing(model =>
                {
                    if (model.Event == null || !model.Event.Id.HasValue)
                        return null;
                    return new Common.Models.Events.Event()
                    {
                        Id = model.Event.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Contact, opt => opt.ResolveUsing(model =>
                {
                    if (model.Contact == null || !model.Contact.Id.HasValue)
                        return null;
                    return new Common.Models.Contacts.Contact()
                    {
                        Id = model.Contact.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}