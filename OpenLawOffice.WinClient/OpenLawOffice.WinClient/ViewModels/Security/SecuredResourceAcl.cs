using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    public class SecuredResourceAcl : CoreModel<Common.Models.Security.SecuredResourceAcl>
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

        private SecuredResource _securedResourceViewModel;
        public SecuredResource SecuredResource
        {
            get { return _securedResourceViewModel; }
            set
            {
                if (_securedResourceViewModel == null)
                {
                    _securedResourceViewModel = new SecuredResource();
                    _securedResourceViewModel.AttachModel(_model.SecuredResource);
                }

                _securedResourceViewModel.Id = value.Id;
                _securedResourceViewModel.UtcCreated = value.UtcCreated;
                _securedResourceViewModel.UtcModified = value.UtcModified;
                _securedResourceViewModel.UtcDisabled = value.UtcDisabled;
                _securedResourceViewModel.CreatedBy = value.CreatedBy;
                _securedResourceViewModel.ModifiedBy = value.ModifiedBy;
                _securedResourceViewModel.DisabledBy = value.DisabledBy;
                OnPropertyChanged("SecuredResource");
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
        //    Mapper.CreateMap<Common.Models.Security.SecuredResourceAcl, SecuredResourceAcl>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.SecuredResource, opt => opt.MapFrom(src => src.SecuredResource))
        //        .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
        //        .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
        //        .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));

        //    Mapper.CreateMap<SecuredResourceAcl, Common.Models.Security.SecuredResourceAcl>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.SecuredResource, opt => opt.MapFrom(src => src.SecuredResource))
        //        .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
        //        .ForMember(dst => dst.AllowFlags, opt => opt.MapFrom(src => src.AllowFlags))
        //        .ForMember(dst => dst.DenyFlags, opt => opt.MapFrom(src => src.DenyFlags));
        //}
    }
}
