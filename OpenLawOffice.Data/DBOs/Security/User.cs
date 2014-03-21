using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace OpenLawOffice.Data.DBOs.Security
{
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
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));

            Mapper.CreateMap<Common.Models.Security.User, DBOs.Security.User>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));
        }
    }
}