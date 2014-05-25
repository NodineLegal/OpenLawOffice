// -----------------------------------------------------------------------
// <copyright file="EventMatterViewModel.cs" company="Nodine Legal, LLC">
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
    public class EventMatterViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public EventViewModel Event { get; set; }

        public Matters.MatterViewModel Matter { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Events.EventMatter, EventMatterViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.Id.HasValue) return null;
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.DisabledBy.Id.Value,
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
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = db.Matter.Id,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<EventMatterViewModel, Common.Models.Events.EventMatter>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null || !model.DisabledBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.DisabledBy.Id,
                        IsStub = true
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
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(model =>
                {
                    if (model.Matter == null) return null;
                    return new Common.Models.Matters.Matter()
                    {
                        Id = model.Matter.Id,
                        IsStub = true
                    };
                }));
        }
    }
}