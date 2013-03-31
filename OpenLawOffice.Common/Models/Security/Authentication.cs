using System;
using AutoMapper;

namespace OpenLawOffice.Common.Models.Security
{
    [MapMe]
    [Can(CanFlags.Execute)]
    public class Authentication : ModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? AuthToken { get; set; }

        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Security.Authentication, Authentication>()
                .ForMember(dst => dst.IsStub, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.MapFrom(src => src.AuthToken))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password));
            Mapper.CreateMap<Authentication, Rest.Responses.Security.Authentication>()
                .ForMember(dst => dst.AuthToken, opt => opt.MapFrom(src => src.AuthToken));
            Mapper.CreateMap<Rest.Responses.Security.Authentication, Authentication>()
                .ForMember(dst => dst.IsStub, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.MapFrom(src => src.AuthToken))
                .ForMember(dst => dst.Username, opt => opt.UseValue(null))
                .ForMember(dst => dst.Password, opt => opt.UseValue(null));
        }
    }
}