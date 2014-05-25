// -----------------------------------------------------------------------
// <copyright file="NoteTask.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Notes
{
    using System;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Common.Models.MapMe]
    public class NoteTask : Core
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "note_id")]
        public Guid NoteId { get; set; }

        [ColumnMapping(Name = "task_id")]
        public long TaskId { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(NoteTask), new ColumnAttributeTypeMapper<NoteTask>());
            Mapper.CreateMap<DBOs.Notes.NoteTask, Common.Models.Notes.NoteTask>()
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
                .ForMember(dst => dst.Note, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Notes.Note()
                    {
                        Id = db.NoteId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Task, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Tasks.Task()
                    {
                        Id = db.TaskId,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Notes.NoteTask, DBOs.Notes.NoteTask>()
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
                .ForMember(dst => dst.NoteId, opt => opt.ResolveUsing(db =>
                {
                    return db.Note.Id.Value;
                }))
                .ForMember(dst => dst.TaskId, opt => opt.ResolveUsing(db =>
                {
                    return db.Task.Id.Value;
                }));
        }
    }
}