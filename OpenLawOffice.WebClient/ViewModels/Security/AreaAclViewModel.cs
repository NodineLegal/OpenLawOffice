namespace OpenLawOffice.WebClient.ViewModels.Security
{
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    [MapMe]
    public class AreaAclViewModel : CoreViewModel
    {
        public int? Id { get; set; }
        public AreaViewModel Area { get; set; }
        public UserViewModel User { get; set; }
        public PermissionsViewModel AllowPermissions { get; set; }
        public PermissionsViewModel DenyPermissions { get; set; }

        public void BuildMappings()
        {
            Mapper.CreateMap<Common.Models.Security.AreaAcl, AreaAclViewModel>()
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
                .ForMember(dst => dst.Area, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.AreaViewModel()
                    {
                        Id = db.Area.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.User.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.AllowPermissions, opt => opt.ResolveUsing(model =>
                {
                    return (PermissionsViewModel)(int)model.AllowFlags.Value;
                }))
                .ForMember(dst => dst.DenyPermissions, opt => opt.ResolveUsing(model =>
                {
                    return (PermissionsViewModel)(int)model.DenyFlags.Value;
                }));

            Mapper.CreateMap<AreaAclViewModel, Common.Models.Security.AreaAcl>()
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
                .ForMember(dst => dst.Area, opt => opt.ResolveUsing(model =>
                {
                    return new Common.Models.Security.Area()
                    {
                        Id = model.Area.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(model =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = model.User.Id,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.AllowFlags, opt => opt.ResolveUsing(model =>
                {
                    return (int)model.AllowPermissions;
                }))
                .ForMember(dst => dst.DenyFlags, opt => opt.ResolveUsing(model =>
                {
                    return (int)model.DenyPermissions;
                }));
        }
    }
}