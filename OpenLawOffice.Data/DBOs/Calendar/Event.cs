// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Calendar
{
    using System;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Common.Models.MapMe]
    public class Event : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "allday")]
        public bool AllDay { get; set; }

        [ColumnMapping(Name = "start")]
        public DateTime Start { get; set; }

        [ColumnMapping(Name = "end")]
        public DateTime? End { get; set; }

        [ColumnMapping(Name = "location")]
        public string Location { get; set; }

        [ColumnMapping(Name = "description")]
        public string Description { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Event), new ColumnAttributeTypeMapper<Event>());
            Mapper.CreateMap<DBOs.Calendar.Event, Common.Models.Calendar.Event>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = db.CreatedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = db.ModifiedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserId.HasValue) return null;
                    return new Common.Models.Security.User()
                    {
                        Id = db.DisabledByUserId.Value,
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

            Mapper.CreateMap<Common.Models.Calendar.Event, DBOs.Calendar.Event>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.Id.HasValue)
                        return 0;
                    return model.CreatedBy.Id.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.Id.HasValue)
                        return 0;
                    return model.ModifiedBy.Id.Value;
                }))
                .ForMember(dst => dst.DisabledByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.Id;
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
