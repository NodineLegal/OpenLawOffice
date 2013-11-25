using AutoMapper;

namespace OpenLawOffice.Common.Models.Matters
{
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class ResponsibleUser : Core, IHasIntId
    {
        public int? Id { get; set; }
        public Matter Matter { get; set; }
        public Security.User User { get; set; }
        public string Responsibility { get; set; }

        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Matters.ResponsibleUser, ResponsibleUser>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcModified, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcDisabled, opt => opt.UseValue(null))
                .ForMember(dst => dst.CreatedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.ModifiedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.DisabledBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(request =>
                {
                    if (request.MatterId.HasValue)
                        return new Matter()
                        {
                            Id = request.MatterId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(request =>
                {
                    if (request.UserId.HasValue)
                        return new Security.User()
                        {
                            Id = request.UserId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }));

            Mapper.CreateMap<ResponsibleUser, Rest.Requests.Matters.ResponsibleUser>()
                .ForMember(dst => dst.Received, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.MatterId, opt => opt.MapFrom(src => src.Matter.Id))
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.User.Id));

            Mapper.CreateMap<ResponsibleUser, Rest.Responses.Matters.ResponsibleUser>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User));

            Mapper.CreateMap<Rest.Responses.Matters.ResponsibleUser, ResponsibleUser>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User));
        }
    }
}
