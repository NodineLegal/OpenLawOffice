using System;
using AutoMapper;

namespace OpenLawOffice.Common.Models.Security
{
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class User : ModelWithDatesOnly, IHasIntId
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Guid? UserAuthToken { get; set; }
        public DateTime? UserAuthTokenExpiry { get; set; }

        public User()
        {
        }
        
        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Security.User, User>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcModified, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcDisabled, opt => opt.UseValue(null))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.UseValue(null))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));

            Mapper.CreateMap<User, Rest.Responses.Security.User>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));

            Mapper.CreateMap<Rest.Responses.Security.User, User>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.UseValue(null))
                .ForMember(dst => dst.PasswordSalt, opt => opt.UseValue(null))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));
        }
    }
}
