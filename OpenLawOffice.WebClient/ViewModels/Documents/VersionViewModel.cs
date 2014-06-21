// -----------------------------------------------------------------------
// <copyright file="VersionViewModel.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.ViewModels.Documents
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class VersionViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }

        public DocumentViewModel Document { get; set; }

        public int VersionNumber { get; set; }

        public string Mime { get; set; }

        public string Filename { get; set; }

        public string Extension { get; set; }

        public ulong Size { get; set; }

        public string Md5 { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Documents.Version, VersionViewModel>()
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
                .ForMember(dst => dst.Document, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Documents.DocumentViewModel()
                    {
                        Id = db.Document.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber))
                .ForMember(dst => dst.Mime, opt => opt.MapFrom(src => src.Mime))
                .ForMember(dst => dst.Filename, opt => opt.MapFrom(src => src.Filename))
                .ForMember(dst => dst.Extension, opt => opt.MapFrom(src => src.Extension))
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dst => dst.Md5, opt => opt.MapFrom(src => src.Md5));

            Mapper.CreateMap<VersionViewModel, Common.Models.Documents.Version>()
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
                .ForMember(dst => dst.Document, opt => opt.ResolveUsing(model =>
                {
                    if (model.Document == null || !model.Document.Id.HasValue)
                        return null;
                    return new Common.Models.Documents.Document()
                    {
                        Id = model.Document.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber))
                .ForMember(dst => dst.Mime, opt => opt.MapFrom(src => src.Mime))
                .ForMember(dst => dst.Filename, opt => opt.MapFrom(src => src.Filename))
                .ForMember(dst => dst.Extension, opt => opt.MapFrom(src => src.Extension))
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dst => dst.Md5, opt => opt.MapFrom(src => src.Md5));
        }
    }
}