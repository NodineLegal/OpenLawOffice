namespace OpenLawOffice.WebClient.ViewModels.Documents
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class VersionViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }
        public DocumentViewModel Document { get; set; }
        public int VersionNumber { get; set; }
        public string Mime { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public ulong Size { get; set; }
        public string Md5 { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Documents.Version, VersionViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (db.DisabledBy == null || !db.DisabledBy.Id.HasValue) return null;
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.DisabledBy.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Document, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Documents.DocumentViewModel()
                    {
                        Id = db.Document.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber))
                .ForMember(dst => dst.Mime, opt => opt.MapFrom(src => src.Mime))
                .ForMember(dst => dst.Filename, opt => opt.MapFrom(src => src.Filename))
                .ForMember(dst => dst.Extension, opt => opt.MapFrom(src => src.Extension))
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dst => dst.Md5, opt => opt.MapFrom(src => src.Md5));

            Mapper.CreateMap<VersionViewModel, Common.Models.Documents.Version>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.CreatedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.ModifiedBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null || !model.DisabledBy.Id.HasValue)
                        return null;
                    return new Common.Models.Security.User()
                    {
                        Id = model.DisabledBy.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Document, opt => opt.ResolveUsing(model =>
                {
                    if (model.Document == null || !model.Document.Id.HasValue)
                        return null;
                    return new Common.Models.Documents.Document()
                    {
                        Id = model.Document.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber))
                .ForMember(dst => dst.Mime, opt => opt.MapFrom(src => src.Mime))
                .ForMember(dst => dst.Filename, opt => opt.MapFrom(src => src.Filename))
                .ForMember(dst => dst.Extension, opt => opt.MapFrom(src => src.Extension))
                .ForMember(dst => dst.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dst => dst.Md5, opt => opt.MapFrom(src => src.Md5));
        }
    }
}