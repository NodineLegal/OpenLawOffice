using AutoMapper;
using DW.SharpTools;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    public class Area : CoreModel<Common.Models.Security.Area>, IHierarchicalModel<Area>
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

        private Area _parentViewModel;
        public Area Parent
        {
            get { return _parentViewModel; }
            set
            {
                if (_parentViewModel == null)
                {
                    _parentViewModel = new Area();
                    _parentViewModel.AttachModel(_model.Parent);
                }

                _parentViewModel.Id = value.Id;
                _parentViewModel.Parent = value.Parent;
                _parentViewModel.Name = value.Name;
                _parentViewModel.Description = value.Description;
                _parentViewModel.UtcCreated = value.UtcCreated;
                _parentViewModel.UtcModified = value.UtcModified;
                _parentViewModel.UtcDisabled = value.UtcDisabled;
                _parentViewModel.CreatedBy = value.CreatedBy;
                _parentViewModel.ModifiedBy = value.ModifiedBy;
                _parentViewModel.DisabledBy = value.DisabledBy;
                OnPropertyChanged("Parent");
            }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _model.Description; }
            set
            {
                _model.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public bool IsDummy { get; set; }
        public EnhancedObservableCollection<Area> Children { get; set; }

        //public void BuildMappings()
        //{
        //    Mapper.CreateMap<Common.Models.Security.Area, Area>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.Parent, opt => opt.MapFrom(src => src.Parent))
        //        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
        //        .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

        //    Mapper.CreateMap<Area, Common.Models.Security.Area>()
        //        .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
        //        .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
        //        .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
        //        .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
        //        .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
        //        .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
        //        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dst => dst.Parent, opt => opt.MapFrom(src => src.Parent))
        //        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
        //        .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        //}


        public void AddChild(Area child)
        {
            Children.Add(child);
        }

        public void RemoveChild(Area child)
        {
            Children.Remove(child);
        }
    }
}
