// -----------------------------------------------------------------------
// <copyright file="MatterViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Matters
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using System.Collections.Generic;

    [MapMe]
    public class MatterViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public MatterViewModel Parent { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public bool Active { get; set; }

        public List<ViewModels.Tasks.TaskViewModel> Tasks { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.Matter, MatterViewModel>()
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
                .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(db =>
                {
                    if (!db.ParentId.HasValue) return null;
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = db.ParentId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dst => dst.Tasks, opt => opt.Ignore());

            Mapper.CreateMap<MatterViewModel, Common.Models.Matters.Matter>()
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
                .ForMember(dst => dst.ParentId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Parent == null || !model.Parent.Id.HasValue)
                        return null;
                    return model.Parent.Id.Value;
                }))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Active, opt => opt.MapFrom(src => src.Active));
        }
    }
}