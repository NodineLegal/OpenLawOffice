using System;
using AutoMapper;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.ViewModels.Matters
{
    [Common.Models.MapMe]
    public class Matter : ViewModelCore<Common.Models.Matters.Matter>
    {
        private Guid? _id;
        public Guid? Id
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

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _synopsis;
        public string Synopsis
        {
            get
            {
                return _synopsis;
            }
            set
            {
                _synopsis = value;
                OnPropertyChanged("Synopsis");
            }
        }

        private ObservableCollection<MatterTag> _tags;
        public ObservableCollection<MatterTag> Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                OnPropertyChanged("Tags");
            }
        }

        public Matter()
        {
        }

        public Matter(Common.Models.Matters.Matter model)
            : base(model)
        {
        }

        public static void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Matters.Matter, Matter>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Tags, opt => opt.ResolveUsing(model =>
                {
                    ObservableCollection<MatterTag> tags = new ObservableCollection<MatterTag>();

                    if (model.Tags != null && model.Tags.Count > 0)
                    {
                        model.Tags.ForEach(tag =>
                        {
                            tags.Add(Mapper.Map<MatterTag>(tag));
                        });
                    }

                    return tags;
                }))
                .ForMember(dst => dst.Model, opt => opt.Ignore())
                .ForMember(dst => dst.State, opt => opt.UseValue<StateType>(StateType.Synchronized))
                .ForMember(dst => dst.IsDummy, opt => opt.UseValue(false));

            Mapper.CreateMap<Matter, Common.Models.Matters.Matter>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Tags, opt => opt.ResolveUsing(model =>
                {
                    List<MatterTag> tags = new List<MatterTag>();

                    if (model.Tags != null && model.Tags.Count > 0)
                    {
                        foreach (MatterTag tag in model.Tags)
                        {
                            tags.Add(Mapper.Map<MatterTag>(tag));
                        };
                    }

                    return tags;
                }))
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.SecuredResource, opt => opt.Ignore());
        }
    }
}
