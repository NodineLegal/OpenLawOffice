// -----------------------------------------------------------------------
// <copyright file="MatterTagViewModel.cs" company="Nodine Legal, LLC">
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
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class MatterTagViewModel : Tagging.TagBaseViewModel
    {
        public MatterViewModel Matter { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.MatterTag, MatterTagViewModel>()
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
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Matters.MatterViewModel()
                    {
                        Id = db.Matter.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.TagCategory, opt => opt.ResolveUsing(db =>
                {
                    if (db.TagCategory == null || db.TagCategory.Id < 1)
                        return null;

                    Tagging.TagCategoryViewModel model = Mapper.Map<Tagging.TagCategoryViewModel>(db.TagCategory);
                    model.IsStub = true;
                    return model;
                }))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));

            Mapper.CreateMap<MatterTagViewModel, Common.Models.Matters.MatterTag>()
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
                .ForMember(dst => dst.Disabled, opt => opt.MapFrom(src => src.Disabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.CreatedBy.PId
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.ModifiedBy.PId
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.PId.HasValue) return null;
                    return new ViewModels.Account.UsersViewModel()
                    {
                        PId = db.DisabledBy.PId.Value
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(model =>
                {
                    if (model.Matter == null) return null;
                    return new Common.Models.Matters.Matter()
                    {
                        Id = model.Matter.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.TagCategory, opt => opt.ResolveUsing(model =>
                {
                    if (model.TagCategory == null)
                        return null;
                    return new Common.Models.Tagging.TagCategory()
                    {
                        Id = model.TagCategory.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));
        }
    }
}