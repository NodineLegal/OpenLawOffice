using System;
using AutoMapper;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace OpenLawOffice.WebClient.DBOs.Security
{
    [Common.Models.MapMe]
    public class User : DboWithDatesOnly, IHasIntId
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Index(Unique = true)]
        [StringLength(25)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string PasswordSalt { get; set; }

        [Index(Unique = true)]
        public Guid? UserAuthToken { get; set; }

        public DateTime? UserAuthTokenExpiry { get; set; }

        public void BuildMappings()
        {
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