using System;
using AutoMapper;

namespace OpenLawOffice.Common.Models.Security
{
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class SecuredResourceAcl : Core, IHasGuidId
    {
        public Guid? Id { get; set; }
        public SecuredResource SecuredResource { get; set; }
        public User User { get; set; }
        public Models.PermissionType? AllowFlags { get; set; }
        public Models.PermissionType? DenyFlags { get; set; }

        public SecuredResourceAcl()
        {
        }

        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Security.SecuredResourceAcl, SecuredResourceAcl>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcModified, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcDisabled, opt => opt.UseValue(null))
                .ForMember(dst => dst.CreatedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.ModifiedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.DisabledBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SecuredResource, opt => opt.ResolveUsing(request =>
                {
                    if (request.SecuredResourceId.HasValue)
                        return new SecuredResource()
                        {
                            Id = request.SecuredResourceId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(request =>
                {
                    if (request.UserId.HasValue)
                        return new User()
                        {
                            Id = request.UserId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));

            Mapper.CreateMap<SecuredResourceAcl, Rest.Requests.Security.SecuredResourceAcl>()
                .ForMember(dst => dst.Received, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SecuredResourceId, opt => opt.MapFrom(src => src.SecuredResource.Id))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));

            Mapper.CreateMap<SecuredResourceAcl, Rest.Responses.Security.SecuredResourceAcl>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SecuredResource, opt => opt.MapFrom(src => src.SecuredResource))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));

            Mapper.CreateMap<Rest.Responses.Security.SecuredResourceAcl, SecuredResourceAcl>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SecuredResource, opt => opt.MapFrom(src => src.SecuredResource))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));
        }
    }
}
