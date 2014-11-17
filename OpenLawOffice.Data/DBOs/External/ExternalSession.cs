// -----------------------------------------------------------------------
// <copyright file="ExternalSession.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.External
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class ExternalSession
    {
        [ColumnMapping(Name = "id")]
        public Guid Id { get; set; }

        [ColumnMapping(Name = "user_pid")]
        public Guid UserPId { get; set; }

        [ColumnMapping(Name = "app_name")]
        public string AppName { get; set; }

        [ColumnMapping(Name = "machine_id")]
        public Guid MachineId { get; set; }

        [ColumnMapping(Name = "utc_created")]
        public DateTime UtcCreated { get; set; }

        [ColumnMapping(Name = "utc_expires")]
        public DateTime UtcExpires { get; set; }

        [ColumnMapping(Name = "timeout")]
        public int Timeout { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(ExternalSession), new ColumnAttributeTypeMapper<ExternalSession>());
            Mapper.CreateMap<DBOs.External.ExternalSession, Common.Models.External.ExternalSession>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Account.Users()
                    {
                        PId = db.UserPId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.AppName, opt => opt.MapFrom(src => src.AppName))
                .ForMember(dst => dst.MachineId, opt => opt.MapFrom(src => src.MachineId))
                .ForMember(dst => dst.Created, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcCreated.ToSystemTime();
                }))
                .ForMember(dst => dst.Expires, opt => opt.ResolveUsing(db =>
                {
                    return db.UtcExpires.ToSystemTime();
                }))
                .ForMember(dst => dst.Timeout, opt => opt.MapFrom(src => src.Timeout));

            Mapper.CreateMap<Common.Models.External.ExternalSession, DBOs.External.ExternalSession>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserPId, opt => opt.ResolveUsing(model =>
                {
                    if (model.User == null || !model.User.PId.HasValue)
                        return Guid.Empty;
                    return model.User.PId.Value;
                }))
                .ForMember(dst => dst.AppName, opt => opt.MapFrom(src => src.AppName))
                .ForMember(dst => dst.MachineId, opt => opt.MapFrom(src => src.MachineId))
                .ForMember(dst => dst.UtcCreated, opt => opt.ResolveUsing(db =>
                {
                    return db.Created.ToDbTime();
                }))
                .ForMember(dst => dst.UtcExpires, opt => opt.ResolveUsing(db =>
                {
                    return db.Expires.ToDbTime();
                }))
                .ForMember(dst => dst.Timeout, opt => opt.MapFrom(src => src.Timeout));
        }
    }
}
