using AutoMapper;

namespace OpenLawOffice.WinClient.ViewModels.Matters
{
    [Common.Models.MapMe]
    public class ResponsibleUser : ViewModelCore<Common.Models.Matters.ResponsibleUser>
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

        private Security.User _user;
        public Security.User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private string _responsibility;
        public string Responsibility
        {
            get
            {
                return _responsibility;
            }
            set
            {
                _responsibility = value;
                OnPropertyChanged("Responsibility");
            }
        }

        public ResponsibleUser()
        {
        }

        public ResponsibleUser(Common.Models.Matters.ResponsibleUser model)
            : base(model)
        {
        }
        
        public static void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.ResponsibleUser, ResponsibleUser>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.Responsibility, opt => opt.MapFrom(src => src.Responsibility))
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<ResponsibleUser, Common.Models.Matters.ResponsibleUser>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.MapFrom(src => src.Matter))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.Responsibility, opt => opt.MapFrom(src => src.Responsibility))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
