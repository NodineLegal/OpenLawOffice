using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    public class AreaAcl : ViewModelCore<Common.Models.Security.AreaAcl>
    {
        private int? _id;
        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private Area _area;
        public Area Area
        {
            get { return _area; }
            set
            {
                _area = value;
                OnPropertyChanged("Area");
            }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private Common.Models.PermissionType? _allowFlags;
        public Common.Models.PermissionType? AllowFlags
        {
            get { return _allowFlags; }
            set
            {
                _allowFlags = value;
                OnPropertyChanged("AllowFlags");
            }
        }

        private Common.Models.PermissionType? _denyFlags;
        public Common.Models.PermissionType? DenyFlags
        {
            get { return _denyFlags; }
            set
            {
                _denyFlags = value;
                OnPropertyChanged("DenyFlags");
            }
        }

        public AreaAcl()
        {
        }

        public AreaAcl(Common.Models.Security.AreaAcl model)
            : base(model)
        {
        }

        public static void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Security.AreaAcl, AreaAcl>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags))
                .ForMember(dst => dst.IsHierarchical, opt => opt.Ignore())
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<AreaAcl, Common.Models.Security.AreaAcl>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
                .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
