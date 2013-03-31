using System;
using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    public class SecuredResource : CoreModel<Common.Models.Security.SecuredResource>
    {
        public Guid? Id
        {
            get { return _model.Id; }
            set
            {
                _model.Id = value;
                OnPropertyChanged("Id");
            }
        }

        //public void BuildMappings()
        //{
        //    Mapper.CreateMap<Common.Models.Security.SecuredResource, SecuredResource>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));

        //    Mapper.CreateMap<SecuredResource, Common.Models.Security.SecuredResource>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));
        //}
    }
}
