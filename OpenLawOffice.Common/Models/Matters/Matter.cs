using System;
using AutoMapper;
using System.Collections.Generic;

namespace OpenLawOffice.Common.Models.Matters
{
    [MapMe]
    [Can(CanFlags.Get | CanFlags.Create | CanFlags.Update | CanFlags.Delete)]
    public class Matter : Core, Security.ISecuredResource, IHasGuidId
    {
        public Guid? Id { get; set; }
        [ShowInList]
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public List<MatterTag> Tags { get; set; }
        public Security.SecuredResource SecuredResource { get; set; }

        public Matter()
        {
        }
        
        public override void BuildMappings()
        {
            Mapper.CreateMap<Rest.Requests.Matters.Matter, Matter>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcModified, opt => opt.UseValue(null))
                .ForMember(dst => dst.UtcDisabled, opt => opt.UseValue(null))
                .ForMember(dst => dst.CreatedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.ModifiedBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.DisabledBy, opt => opt.UseValue(null))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Tags, opt => opt.Ignore())
                .ForMember(dst => dst.SecuredResource, opt => opt.Ignore());

            Mapper.CreateMap<Matter, Rest.Requests.Matters.Matter>()
                .ForMember(dst => dst.Received, opt => opt.Ignore())
                .ForMember(dst => dst.AuthToken, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.TagQuery, opt => opt.ResolveUsing(model =>
                {
                    if (model.Tags != null && model.Tags.Count > 0)
                        return string.Join(";", model.Tags);
                    return null;
                }));

            Mapper.CreateMap<Matter, Rest.Responses.Matters.Matter>()
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
                    List<Rest.Responses.Matters.MatterTag> respTags = 
                        new List<Rest.Responses.Matters.MatterTag>();

                    if (model.Tags != null && model.Tags.Count > 0)
                    {
                        model.Tags.ForEach(tag =>
                        {
                            respTags.Add(Mapper.Map<Rest.Responses.Matters.MatterTag>(tag));
                        });
                    }

                    return respTags;
                }));

            Mapper.CreateMap<Rest.Responses.Matters.Matter, Matter>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dst => dst.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dst => dst.DisabledBy, opt => opt.MapFrom(src => src.DisabledBy))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
                .ForMember(dst => dst.Tags, opt => opt.ResolveUsing(response =>
                {
                    List<MatterTag> modelTags =
                        new List<MatterTag>();

                    if (response.Tags != null && response.Tags.Count > 0)
                    {
                        response.Tags.ForEach(tag =>
                        {
                            modelTags.Add(Mapper.Map<MatterTag>(tag));
                        });
                    }

                    return modelTags;
                }))
                .ForMember(dst => dst.SecuredResource, opt => opt.Ignore());
        }
    }
}
