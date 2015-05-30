// -----------------------------------------------------------------------
// <copyright file="TaskTemplate.cs" company="Nodine Legal, LLC">
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
    public class TaskTemplate : Core
    {
        [ColumnMapping(Name = "id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "task_template_title")]
        public string TaskTemplateTitle { get; set; }

        [ColumnMapping(Name = "title")]
        public string Title { get; set; }

        [ColumnMapping(Name = "description")]
        public string Description { get; set; }

        [ColumnMapping(Name = "projected_start")]
        public string ProjectedStart { get; set; }

        [ColumnMapping(Name = "due_date")]
        public string DueDate { get; set; }

        [ColumnMapping(Name = "projected_end")]
        public string ProjectedEnd { get; set; }

        [ColumnMapping(Name = "actual_end")]
        public string ActualEnd { get; set; }

        [ColumnMapping(Name = "active")]
        public bool Active { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(TaskTemplate), new ColumnAttributeTypeMapper<TaskTemplate>());
            Mapper.CreateMap<DBOs.Tasks.TaskTemplate, Common.Models.Tasks.TaskTemplate>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Created, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcCreated.ToSystemTime();
                }))
                .ForMember(dst => dst.Modified, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcModified.ToSystemTime();
                }))
                .ForMember(dst => dst.Disabled, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcDisabled.ToSystemTime();
                }))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.CreatedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.ModifiedByUserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserPId.HasValue) return null;
                    return new Common.Models.Account.Users()
                    {
                        PId = db.DisabledByUserPId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.TaskTemplateTitle, opt => opt.MapFrom(src => src.TaskTemplateTitle))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ProjectedStart, opt => opt.MapFrom(src => src.ProjectedStart))
                .ForMember(dst => dst.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dst => dst.ProjectedEnd, opt => opt.MapFrom(src => src.ProjectedEnd))
                .ForMember(dst => dst.ActualEnd, opt => opt.MapFrom(src => src.ActualEnd))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active));

            Mapper.CreateMap<Common.Models.Tasks.TaskTemplate, DBOs.Tasks.TaskTemplate>()
                .ForMember(dst => dst.UtcCreated, opt => opt.ResolveUsing(db =>
                {
                    return db.Created.ToDbTime();
                }))
                .ForMember(dst => dst.UtcModified, opt => opt.ResolveUsing(db =>
                {
                    return db.Modified.ToDbTime();
                }))
                .ForMember(dst => dst.UtcDisabled, opt => opt.ResolveUsing(db =>
                {
                    return db.Disabled.ToDbTime();
                }))
                .ForMember(dst => dst.CreatedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.CreatedBy.PId.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.PId.HasValue)
                        return Guid.Empty;
                    return model.ModifiedBy.PId.Value;
                }))
                .ForMember(dst => dst.DisabledByUserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.PId;
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.TaskTemplateTitle, opt => opt.MapFrom(src => src.TaskTemplateTitle))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ProjectedStart, opt => opt.MapFrom(src => src.ProjectedStart))
                .ForMember(dst => dst.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dst => dst.ProjectedEnd, opt => opt.MapFrom(src => src.ProjectedEnd))
                .ForMember(dst => dst.ActualEnd, opt => opt.MapFrom(src => src.ActualEnd))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active));
        }
    }
}
