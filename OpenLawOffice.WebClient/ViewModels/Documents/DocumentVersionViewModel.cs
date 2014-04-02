namespace OpenLawOffice.WebClient.ViewModels.Documents
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class DocumentVersionViewModel : CoreViewModel
    {
        public Guid Id { get; set; }
        public DocumentViewModel Document { get; set; }
        public VersionViewModel Version { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<OpenLawOffice.Common.Models.Documents.DocumentVersion, DocumentVersionViewModel>()
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
                .ForMember(dst => dst.Version, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Documents.VersionViewModel()
                    {
                        Id = db.Version.Id,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<DocumentVersionViewModel, OpenLawOffice.Common.Models.Documents.DocumentVersion>()
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
                    if (model.Document == null) return null;
                    return new Common.Models.Documents.Document()
                    {
                        Id = model.Document.Id.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Version, opt => opt.ResolveUsing(model =>
                {
                    if (model.Version == null) return null;
                    return new Common.Models.Documents.Version()
                    {
                        Id = model.Version.Id.Value,
                        IsStub = true
                    };
                }));
        }
    }
}