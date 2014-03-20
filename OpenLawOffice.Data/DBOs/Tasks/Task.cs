// -----------------------------------------------------------------------
// <copyright file="Task.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Tasks
{
    using System;
    using AutoMapper;

    /// <summary>
    /// Represents a system task
    /// </summary>
    [Common.Models.MapMe]
    public class Task : Core
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? ProjectedStart { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ProjectedEnd { get; set; }

        public DateTime? ActualEnd { get; set; }

        public long? ParentId { get; set; }

        public bool IsGroupingTask { get; set; }

        public long? SequentialPredecessorId { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<DBOs.Tasks.Task, Common.Models.Tasks.Task>()
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
                .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(db =>
                {
                    if (db.ParentId.HasValue)
                        return new Common.Models.Tasks.Task()
                        {
                            Id = db.ParentId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ProjectedStart, opt => opt.MapFrom(src => src.ProjectedStart))
                .ForMember(dst => dst.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dst => dst.ProjectedEnd, opt => opt.MapFrom(src => src.ProjectedEnd))
                .ForMember(dst => dst.ActualEnd, opt => opt.MapFrom(src => src.ActualEnd))
                .ForMember(dst => dst.IsGroupingTask, opt => opt.MapFrom(src => src.IsGroupingTask))
                .ForMember(dst => dst.SequentialPredecessor, opt => opt.ResolveUsing(db =>
                {
                    if (db.SequentialPredecessorId.HasValue)
                        return new Common.Models.Tasks.Task()
                        {
                            Id = db.SequentialPredecessorId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }));

            Mapper.CreateMap<Common.Models.Tasks.Task, DBOs.Tasks.Task>()
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
                .ForMember(dst => dst.ParentId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Parent != null)
                        return model.Parent.Id;
                    else
                        return null;
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ProjectedStart, opt => opt.MapFrom(src => src.ProjectedStart))
                .ForMember(dst => dst.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dst => dst.ProjectedEnd, opt => opt.MapFrom(src => src.ProjectedEnd))
                .ForMember(dst => dst.ActualEnd, opt => opt.MapFrom(src => src.ActualEnd))
                .ForMember(dst => dst.IsGroupingTask, opt => opt.MapFrom(src => src.IsGroupingTask))
                .ForMember(dst => dst.SequentialPredecessorId, opt => opt.ResolveUsing(model =>
                {
                    if (model.SequentialPredecessor != null)
                        return model.SequentialPredecessor.Id;
                    else
                        return null;
                }));
        }
    }
}