using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    public class AreaAcl : CoreModel<Common.Models.Security.AreaAcl>
    {
        public int? Id
        {
            get { return _model.Id; }
            set
            {
                _model.Id = value;
                OnPropertyChanged("Id");
            }
        }

        private Area _areaViewModel;
        public Area Area
        {
            get { return _areaViewModel; }
            set
            {
                if (_areaViewModel == null)
                {
                    _areaViewModel = new Area();
                    _areaViewModel.AttachModel(_model.Area);
                }

                _areaViewModel.Id = value.Id;
                _areaViewModel.Parent = value.Parent;
                _areaViewModel.Name = value.Name;
                _areaViewModel.Description = value.Description;
                _areaViewModel.UtcCreated = value.UtcCreated;
                _areaViewModel.UtcModified = value.UtcModified;
                _areaViewModel.UtcDisabled = value.UtcDisabled;
                _areaViewModel.CreatedBy = value.CreatedBy;
                _areaViewModel.ModifiedBy = value.ModifiedBy;
                _areaViewModel.DisabledBy = value.DisabledBy;
                OnPropertyChanged("Area");
            }
        }

        private User _userViewModel;
        public User User
        {
            get { return _userViewModel; }
            set
            {
                if (_userViewModel == null)
                {
                    _userViewModel = new Security.User();
                    _userViewModel.AttachModel(_model.User);
                }

                _userViewModel.Id = value.Id;
                _userViewModel.Username = value.Username;
                _userViewModel.Password = value.Password;
                _userViewModel.UserAuthToken = value.UserAuthToken;
                _userViewModel.UserAuthTokenExpiry = value.UserAuthTokenExpiry;
                _userViewModel.UtcCreated = value.UtcCreated;
                _userViewModel.UtcModified = value.UtcModified;
                _userViewModel.UtcDisabled = value.UtcDisabled;
                OnPropertyChanged("User");
            }
        }

        public Common.Models.PermissionType? AllowFlags
        {
            get { return _model.AllowFlags; }
            set
            {
                _model.AllowFlags = value;
                OnPropertyChanged("AllowFlags");
            }
        }

        public Common.Models.PermissionType? DenyFlags
        {
            get { return _model.DenyFlags; }
            set
            {
                _model.DenyFlags = value;
                OnPropertyChanged("DenyFlags");
            }
        }

        //public void BuildMappings()
        //{
        //    Mapper.CreateMap<Common.Models.Security.AreaAcl, AreaAcl>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.Area, opt => opt.MapFrom(src => src.Area))
        //        .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
        //        .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
        //        .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));

        //    Mapper.CreateMap<AreaAcl, Common.Models.Security.AreaAcl>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.Area, opt => opt.MapFrom(src => src.Area))
        //        .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
        //        .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
        //        .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));
        //}
    }
}
