// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Security
{
    using System;
    using AutoMapper;

    [Common.Models.MapMe]
    public class User : DboWithDatesOnly
    {
        [ColumnMapping(Name = "id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "username")]
        public string Username { get; set; }

        [ColumnMapping(Name = "password")]
        public string Password { get; set; }

        [ColumnMapping(Name = "password_salt")]
        public string PasswordSalt { get; set; }

        [ColumnMapping(Name = "user_auth_token")]
        public Guid? UserAuthToken { get; set; }

        [ColumnMapping(Name = "user_auth_token_expiry")]
        public DateTime? UserAuthTokenExpiry { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(User), new ColumnAttributeTypeMapper<User>());
            Mapper.CreateMap<DBOs.Security.User, Common.Models.Security.User>()
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
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.ResolveUsing(db =>
                {
                    return db.UserAuthTokenExpiry.ToSystemTime();
                }));

            Mapper.CreateMap<Common.Models.Security.User, DBOs.Security.User>()
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
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.ResolveUsing(db =>
                {
                    return db.UserAuthTokenExpiry.ToDbTime();
                }));
        }
    }
}