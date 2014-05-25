// -----------------------------------------------------------------------
// <copyright file="EventViewModel.cs" company="Nodine Legal, LLC">
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
    public class EventViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public bool AllDay { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Events.Event, EventViewModel>()
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
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.AllDay, opt => opt.MapFrom(src => src.AllDay))
                .ForMember(dst => dst.Start, opt => opt.ResolveUsing(db =>
                {
                    return new DateTime(db.Start.Ticks, DateTimeKind.Utc);
                }))
                .ForMember(dst => dst.End, opt => opt.ResolveUsing(db =>
                {
                    if (!db.End.HasValue) return null;
                    return new DateTime(db.End.Value.Ticks, DateTimeKind.Utc);
                }))
                .ForMember(dst => dst.End, opt => opt.MapFrom(src => src.End))
                .ForMember(dst => dst.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            Mapper.CreateMap<EventViewModel, Common.Models.Events.Event>()
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
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.AllDay, opt => opt.MapFrom(src => src.AllDay))
                .ForMember(dst => dst.Start, opt => opt.ResolveUsing(db =>
                {
                    return new DateTime(db.Start.Ticks, DateTimeKind.Local);
                }))
                .ForMember(dst => dst.End, opt => opt.ResolveUsing(db =>
                {
                    if (!db.End.HasValue) return null;
                    return new DateTime(db.End.Value.Ticks, DateTimeKind.Local);
                }))
                .ForMember(dst => dst.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dst => dst.End, opt => opt.MapFrom(src => src.End))
                .ForMember(dst => dst.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}