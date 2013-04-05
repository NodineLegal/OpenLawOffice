using AutoMapper;
using DW.SharpTools;
using System.Linq;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    [Common.Models.MapMe]
    public class Area : CoreModel<Common.Models.Security.Area>, IHierarchicalModel<Area>
    {
        public int? Id
        {
            get 
            {
                if (IsDummy && _model == null) return null;
                return _model.Id; 
            }
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
            get
            {
                if (IsDummy && _model == null) return null; 
                return _model.Name;
            }
            set
            {
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get
            {
                if (IsDummy && _model == null) return null; 
                return _model.Description;
            }
            set
            {
                _model.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public bool IsDummy { get; set; }

        private EnhancedObservableCollection<Area> _children;
        public EnhancedObservableCollection<Area> Children
        {
            get { return _children; }
            set
            {
                // This probably needs redone to be more efficient
                if (_model.Children == null) 
                    _model.Children = new System.Collections.Generic.List<Common.Models.Security.Area>();

                _model.Children.Clear();

                foreach (Area area in value)
                {
                    // Iterate through each Area of the new value - map it to system model
                    _model.Children.Add(Mapper.Map<Common.Models.Security.Area>(area));
                }

                _children = value;
            }
        }

        public override bool IsHierarchical { get { return true; } }

        public Area()
        {
            _children = new EnhancedObservableCollection<Area>();
            _children.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        foreach (Area viewModel in e.NewItems)
                        {
                            if (!viewModel.IsDummy)
                                _model.Children.Add(Mapper.Map<Common.Models.Security.Area>(viewModel));
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach (Area viewModel in e.NewItems)
                        {
                            _model.Children.RemoveAll(x => x.Id == viewModel.Id);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        _model.Children.Clear();
                        break;
                    default:
                        throw new System.MethodAccessException("Requested action type is not supported.");
                }
            };
        }

        public void AddChild(Area child)
        {
            Children.Add(child);
        }

        public void RemoveChild(Area child)
        {
            Children.Remove(child);
        }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Security.Area, Area>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(sysModel =>
                {
                    if (sysModel == null) return null;
                    return Mapper.Map<Area>(sysModel.Parent);
                }))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<Area, Common.Models.Security.Area>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(viewModel =>
                {
                    if (viewModel == null) return null;
                    return Mapper.Map<Common.Models.Security.Area>(viewModel.Parent);
                }))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
