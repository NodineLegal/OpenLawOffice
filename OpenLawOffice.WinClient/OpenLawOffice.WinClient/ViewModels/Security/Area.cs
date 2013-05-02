using System;
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
                // value is either (1) null or (2) value

                if (value == null)
                {
                    // set parent model = null
                    if (_parentViewModel != null)
                        _parentViewModel.AttachModel(null);
                    _parentViewModel = null;
                    return;
                }
                else
                {
                    if (_parentViewModel == null)
                        _parentViewModel = new Area();

                    _model.Parent = value.Model;
                    _parentViewModel.AttachModel(value.Model);
                }

                OnPropertyChanged("Parent");

                //if (_parentViewModel == null)
                //{
                //    if (_model.Parent == null)
                //        _model.Parent = new Common.Models.Security.Area();

                //    _parentViewModel = new Area();
                //    _parentViewModel.AttachModel(_model.Parent);
                //}
                //else
                //{
                //    _parentViewModel.Parent = value.Parent;
                //}

                //_parentViewModel.Id = value.Id;
                //_parentViewModel.IsDummy = value.IsDummy;
                //_parentViewModel.Name = value.Name;
                //_parentViewModel.Description = value.Description;
                //_parentViewModel.UtcCreated = value.UtcCreated;
                //_parentViewModel.UtcModified = value.UtcModified;
                //_parentViewModel.UtcDisabled = value.UtcDisabled;
                //_parentViewModel.CreatedBy = value.CreatedBy;
                //_parentViewModel.ModifiedBy = value.ModifiedBy;
                //_parentViewModel.DisabledBy = value.DisabledBy;
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
            get 
            { 
                // this really needs improved
                if (_children.Count == 1 && _children[0].IsDummy)
                    return _children;

                if (_model == null)
                {
                    _children = new EnhancedObservableCollection<Area>();
                    return _children;
                }

                _children.Clear();
                for (int i = 0; i < _model.Children.Count; i++)
                {
                    Common.Models.Security.Area sysModel = _model.Children[i];
                    Area vm = new Area();
                    vm.AttachModel(sysModel);
                    if (sysModel.Children.Count <= 0)
                        vm.AddChild(new ViewModels.Security.Area()
                        {
                            IsDummy = true
                        });
                    _children.Add(vm);
                }
                return _children;
            }
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
                OnPropertyChanged("Children");
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
                            {
                                // improved handling of viewmodel -> model would eliminate the need for this
                                // fix me
                                if (_model.Children.Find(x => x.Id == viewModel.Id) != null)
                                    return;

                                // Should never map ViewModel to Model -> causes all kinds of hierarchical issues
                                //_model.Children.Add(Mapper.Map<Common.Models.Security.Area>(viewModel));
                                _model.AddChild(viewModel.Model);
                            }
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach (Area viewModel in e.OldItems)
                        {
                            _model.Children.RemoveAll(x => x.Id == viewModel.Id);
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        //if (_model != null && _model.Children != null)
                        //    _model.Children.Clear();
                        break;
                    default:
                        throw new System.MethodAccessException("Requested action type is not supported.");
                }
            };
        }

        public override IViewModel AttachModel(Common.Models.ModelBase model)
        {
            if (!typeof(Common.Models.Security.Area).IsAssignableFrom(model.GetType()))
                throw new ArgumentException("Argument 'model' must inherit from Common.Models.Security.Area");

            base.AttachModel(model);

            if (((Common.Models.Security.Area)model).Parent != null)
                _parentViewModel = (Area)new Area().AttachModel(((Common.Models.Security.Area)model).Parent);

            _model.Parent = ((Common.Models.Security.Area)model).Parent;
                        
            return this;
        }

        public override IViewModel AttachModel(Common.Models.Security.Area model)
        {
            if (model.Parent != null)
                _parentViewModel = (Area)new Area().AttachModel(((Common.Models.Security.Area)model).Parent);

            base.AttachModel(model);

            _model.Parent = ((Common.Models.Security.Area)model).Parent;
            
            return this;
        }

        public void AddChild(Area child)
        {
            if (!child.IsDummy)
                child.Parent = this;


            //if (!child.IsDummy)
            //    child.Model.Parent = Model;


            ////child._parentViewModel = this;
            //if (child._parentViewModel == null)
            //    child._parentViewModel = new Area();
            //child._parentViewModel.AttachModel(Model);
            Children.Add(child);
            OnPropertyChanged("Children");
        }

        public void RemoveChild(Area child)
        {
            child._parentViewModel = null;
            child.Model.Parent = null;

            // EnhancedObservableCollection for some reason calls "Reset" on data context change
            // instead of simply removing.  This causes a problem with the commenting out required
            // by our inadequate viewmodel <-> model interfacing.  Yet another reason to fix as once
            // that is fixed, this can more smoothly
            Model.RemoveChild(child.Model);
            OnPropertyChanged("Children");

            //Children.Remove(child);
        }

        public void BuildMappings()
        {
            //Mapper.CreateMap<Common.Models.Security.Area, Area>()
            //    .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
            //    .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
            //    .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
            //    .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            //    .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            //    .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
            //    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dst => dst.Parent, opt => opt.ResolveUsing(sysModel =>
            //    {
            //        if (sysModel == null) return null;
            //        return Mapper.Map<Area>(sysModel.Parent);
            //    }))
            //    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

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
