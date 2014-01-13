namespace OpenLawOffice.WebClient.ViewModels.Security
{
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using DBOs = OpenLawOffice.Server.Core.DBOs;

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
            Mapper.CreateMap<DBOs.Security.AreaAcl, AreaAclViewModel>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.CreatedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.ModifiedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserId.HasValue) return null;
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.DisabledByUserId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Area, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.AreaViewModel()
                    {
                        Id = db.SecurityAreaId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Security.UserViewModel()
                    {
                        Id = db.UserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.AllowPermissions, opt => opt.ResolveUsing(model =>
                {
                    return (PermissionsViewModel)model.AllowFlags;
                }))
                .ForMember(dst => dst.DenyPermissions, opt => opt.ResolveUsing(model =>
                {
                    return (PermissionsViewModel)model.DenyFlags;
                }));

            Mapper.CreateMap<AreaAclViewModel, DBOs.Security.AreaAcl>()
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.CreatedBy == null || !model.CreatedBy.Id.HasValue)
                        return 0;
                    return model.CreatedBy.Id.Value;
                }))
                .ForMember(dst => dst.ModifiedByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.ModifiedBy == null || !model.ModifiedBy.Id.HasValue)
                        return 0;
                    return model.ModifiedBy.Id.Value;
                }))
                .ForMember(dst => dst.DisabledByUserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.DisabledBy == null) return null;
                    return model.DisabledBy.Id;
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.SecurityAreaId, opt => opt.ResolveUsing(model =>
                {
                    return model.Area.Id.Value;
                }))
                .ForMember(dst => dst.UserId, opt => opt.ResolveUsing(model =>
                {
                    return model.User.Id.Value;
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