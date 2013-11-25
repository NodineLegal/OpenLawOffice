using AutoMapper;

namespace OpenLawOffice.Common.Models.Tagging
{
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class TagCategory : Core
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TagCategory()
        {
        }

        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Tagging.TagCategory, TagCategory>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcModified, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcDisabled, opt => opt.UseValue(null))
                .ForMember(dst => dst.CreatedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.ModifiedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.DisabledBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            Mapper.CreateMap<TagCategory, Rest.Requests.Tagging.TagCategory>()
                .ForMember(dst => dst.Received, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            Mapper.CreateMap<TagCategory, Rest.Responses.Tagging.TagCategory>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));

            Mapper.CreateMap<Rest.Responses.Tagging.TagCategory, TagCategory>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
