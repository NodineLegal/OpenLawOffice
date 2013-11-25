using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Matters
{
    [Common.Models.MapMe]
    public class MatterTag : Tagging.TagBase<Common.Models.Matters.MatterTag>
    {
        private Matter _matter;
        public Matter Matter
        {
            get { return _matter; }
            set
            {
                _matter = value;
                OnPropertyChanged("Matter");
            }
        }
        
        public MatterTag()
        {
        }

        public MatterTag(Common.Models.Matters.MatterTag model)
            : base(model)
        {
        }
        
        public static void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.MatterTag, MatterTag>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.TagCategory, opt => opt.MapFrom(src => src.TagCategory))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<MatterTag, Common.Models.Matters.MatterTag>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.TagCategory, opt => opt.MapFrom(src => src.TagCategory))
                .ForMember(dst => dst.Tag, opt => opt.MapFrom(src => src.Tag))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
