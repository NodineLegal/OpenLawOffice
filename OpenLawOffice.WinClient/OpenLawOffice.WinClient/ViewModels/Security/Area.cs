using System;
using AutoMapper;
using DW.SharpTools;
using System.Linq;

namespace OpenLawOffice.WinClient.ViewModels.Security
{
    [Common.Models.MapMe]
    public class Area : ViewModelCore<Common.Models.Security.Area>, IHierarchicalView<Area>
    {
        public override bool IsHierarchical { get { return true; } }

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

        private Area _parent;
        public Area Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged("Parent");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private EnhancedObservableCollection<Area> _children;
        public EnhancedObservableCollection<Area> Children
        {
            get 
            {                 
                return _children;
            }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }

        public Area()
        {
        }

        public Area(Common.Models.Security.Area model)
            : base(model)
        {
        }

        public void AddChild(Area child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void RemoveChild(Area child)
        {
            Children.Remove(child);
            child.Parent = null;
        }

        public static void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Security.Area, Area>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Parent, opt => opt.MapFrom(src => src.Parent))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.IsHierarchical, opt => opt.Ignore())
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<Area, Common.Models.Security.Area>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Parent, opt => opt.MapFrom(src => src.Parent))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false));
        }
    }
}
