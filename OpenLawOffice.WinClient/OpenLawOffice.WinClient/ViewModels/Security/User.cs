﻿using System;
using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    [Common.Models.MapMe]
    public class User : ViewModelWithDatesOnly<Common.Models.Security.User>
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

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private Guid? _userAuthToken;
        public Guid? UserAuthToken
        {
            get
            {
                return _userAuthToken;
            }
            set
            {
                _userAuthToken = value;
                OnPropertyChanged("UserAuthToken");
            }
        }

        private DateTime? _userAuthTokenExpiry;
        public DateTime? UserAuthTokenExpiry
        {
            get
            {
                return _userAuthTokenExpiry;
            }
            set
            {
                _userAuthTokenExpiry = value;
                OnPropertyChanged("UserAuthTokenExpiry");
            }
        }

        public User()
        {
        }

        public User(Common.Models.Security.User model)
            : base(model)
        {
        }

        public static void BuildMappings()
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
                .ForMember(dst => dst.IsHierarchical, opt => opt.Ignore())
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));
            

            Mapper.CreateMap<User, Common.Models.Security.User>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.PasswordSalt, opt => opt.Ignore())
                .ForMember(dst => dst.UserAuthToken, opt => opt.MapFrom(src => src.UserAuthToken))
                .ForMember(dst => dst.UserAuthTokenExpiry, opt => opt.MapFrom(src => src.UserAuthTokenExpiry))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
