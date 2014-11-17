// -----------------------------------------------------------------------
// <copyright file="Users.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.DBOs.Account
{
    using System;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Common.Models.MapMe]
    public class Users
    {
        public Guid? PId { get; set; }

        public string Username { get; set; }

        public string ApplicationName { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsOnLine { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime LastLockedOutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public DateTime FailedPasswordAttemptWindowStart { get; set; }

        public int FailedPasswordAnswerAttemptCount { get; set; }

        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        
        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Users), new ColumnAttributeTypeMapper<Users>());
            Mapper.CreateMap<DBOs.Account.Users, Common.Models.Account.Users>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.PId, opt => opt.MapFrom(src => src.PId))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.ApplicationName, opt => opt.MapFrom(src => src.ApplicationName))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordQuestion, opt => opt.MapFrom(src => src.PasswordQuestion))
                .ForMember(dst => dst.PasswordAnswer, opt => opt.MapFrom(src => src.PasswordAnswer))
                .ForMember(dst => dst.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dst => dst.LastActivityDate, opt => opt.MapFrom(src => src.LastActivityDate))
                .ForMember(dst => dst.LastLoginDate, opt => opt.MapFrom(src => src.LastLoginDate))
                .ForMember(dst => dst.LastPasswordChangedDate, opt => opt.MapFrom(src => src.LastPasswordChangedDate))
                .ForMember(dst => dst.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dst => dst.IsOnLine, opt => opt.MapFrom(src => src.IsOnLine))
                .ForMember(dst => dst.IsLockedOut, opt => opt.MapFrom(src => src.IsLockedOut))
                .ForMember(dst => dst.LastLockedOutDate, opt => opt.MapFrom(src => src.LastLockedOutDate))
                .ForMember(dst => dst.FailedPasswordAttemptCount, opt => opt.MapFrom(src => src.FailedPasswordAttemptCount))
                .ForMember(dst => dst.FailedPasswordAttemptWindowStart, opt => opt.MapFrom(src => src.FailedPasswordAttemptWindowStart))
                .ForMember(dst => dst.FailedPasswordAnswerAttemptCount, opt => opt.MapFrom(src => src.FailedPasswordAnswerAttemptCount))
                .ForMember(dst => dst.FailedPasswordAnswerAttemptWindowStart, opt => opt.MapFrom(src => src.FailedPasswordAnswerAttemptWindowStart));

            Mapper.CreateMap<Common.Models.Account.Users, DBOs.Account.Users>()
                .ForMember(dst => dst.PId, opt => opt.MapFrom(src => src.PId))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.ApplicationName, opt => opt.MapFrom(src => src.ApplicationName))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordQuestion, opt => opt.MapFrom(src => src.PasswordQuestion))
                .ForMember(dst => dst.PasswordAnswer, opt => opt.MapFrom(src => src.PasswordAnswer))
                .ForMember(dst => dst.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
                .ForMember(dst => dst.LastActivityDate, opt => opt.MapFrom(src => src.LastActivityDate))
                .ForMember(dst => dst.LastLoginDate, opt => opt.MapFrom(src => src.LastLoginDate))
                .ForMember(dst => dst.LastPasswordChangedDate, opt => opt.MapFrom(src => src.LastPasswordChangedDate))
                .ForMember(dst => dst.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dst => dst.IsOnLine, opt => opt.MapFrom(src => src.IsOnLine))
                .ForMember(dst => dst.IsLockedOut, opt => opt.MapFrom(src => src.IsLockedOut))
                .ForMember(dst => dst.LastLockedOutDate, opt => opt.MapFrom(src => src.LastLockedOutDate))
                .ForMember(dst => dst.FailedPasswordAttemptCount, opt => opt.MapFrom(src => src.FailedPasswordAttemptCount))
                .ForMember(dst => dst.FailedPasswordAttemptWindowStart, opt => opt.MapFrom(src => src.FailedPasswordAttemptWindowStart))
                .ForMember(dst => dst.FailedPasswordAnswerAttemptCount, opt => opt.MapFrom(src => src.FailedPasswordAnswerAttemptCount))
                .ForMember(dst => dst.FailedPasswordAnswerAttemptWindowStart, opt => opt.MapFrom(src => src.FailedPasswordAnswerAttemptWindowStart));
        }
    }
}
