using System;
using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    [Common.Models.MapMe]
    public class User : ModelWithDatesOnly<Common.Models.Security.User>
    {
        public int? Id
        {
            get
            {
                if (_model == null) return null;
                return _model.Id;
            }
            set
            {
                if (_model == null) AttachModel(new Common.Models.Security.User());
                _model.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Username
        {
            get
            {
                if (_model == null) return null;
                return _model.Username;
            }
            set
            {
                if (_model == null) AttachModel(new Common.Models.Security.User());
                _model.Username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get
            {
                if (_model == null) return null;
                return _model.Password;
            }
            set
            {
                if (_model == null) AttachModel(new Common.Models.Security.User());
                _model.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public Guid? UserAuthToken
        {
            get
            {
                if (_model == null) return null;
                return _model.UserAuthToken;
            }
            set
            {
                if (_model == null) AttachModel(new Common.Models.Security.User());
                _model.UserAuthToken = value;
                OnPropertyChanged("UserAuthToken");
            }
        }

        public DateTime? UserAuthTokenExpiry
        {
            get
            {
                if (_model == null) return null;
                return _model.UserAuthTokenExpiry;
            }
            set
            {
                if (_model == null) AttachModel(new Common.Models.Security.User());
                _model.UserAuthTokenExpiry = value;
                OnPropertyChanged("UserAuthTokenExpiry");
            }
        }

        public bool IsDummy { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Security.User, User>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<User, Common.Models.Security.User>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.Ignore())
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry));
        }
    }
}
