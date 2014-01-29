namespace OpenLawOffice.Common.Models.Matters
{
    using AutoMapper;

    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class MatterTag : Tagging.TagBase
    {
        public Matter Matter { get; set; }

        public MatterTag()
        {
        }

        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Matters.MatterTag, MatterTag>()
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
                .ForMember(dst => dst.TagCategory, opt => opt.ResolveUsing(request =>
                {
                    if (request.TagCategoryId.HasValue)
                        return new Tagging.TagCategory()
                        {
                            Id = request.TagCategoryId.Value,
                            IsStub = true
                        };
                    else
                        return null;
                }))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));

            Mapper.CreateMap<MatterTag, Rest.Requests.Matters.MatterTag>()
                .ForMember(dst => dst.Received, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.MatterId, opt => opt.ResolveUsing(request =>
                {
                    if (request.Matter != null)
                        return request.Matter.Id;
                    return null;
                }))
                .ForMember(dst => dst.TagCategoryId, opt => opt.ResolveUsing(request =>
                {
                    if (request.TagCategory != null)
                        return request.TagCategory.Id;
                    return null;
                }))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));

            Mapper.CreateMap<MatterTag, Rest.Responses.Matters.MatterTag>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.TagCategory, opt => opt.MapFrom(src => src.TagCategory))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));

            Mapper.CreateMap<Rest.Responses.Matters.MatterTag, MatterTag>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.TagCategory, opt => opt.MapFrom(src => src.TagCategory))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag));
        }
    }
}
