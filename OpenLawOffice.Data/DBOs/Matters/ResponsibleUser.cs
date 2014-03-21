using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace OpenLawOffice.Data.DBOs.Matters
{
    [Common.Models.MapMe]
    public class ResponsibleUser : Core
    {
        [ColumnMapping(Name = "id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "matter_id")]
        public Guid MatterId { get; set; }

        [ColumnMapping(Name = "user_id")]
        public int UserId { get; set; }

        [ColumnMapping(Name = "responsibility")]
        public string Responsibility { get; set; }

        public void BuildMappings()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(ResponsibleUser), new ColumnAttributeTypeMapper<ResponsibleUser>());
            Mapper.CreateMap<DBOs.Matters.ResponsibleUser, Common.Models.Matters.ResponsibleUser>()
                .ForMember(dst => dst.IsStub, opt => opt.UseValue(false))
                .ForMember(dst => dst.UtcCreated, opt => opt.MapFrom(src => src.UtcCreated))
                .ForMember(dst => dst.UtcModified, opt => opt.MapFrom(src => src.UtcModified))
                .ForMember(dst => dst.UtcDisabled, opt => opt.MapFrom(src => src.UtcDisabled))
                .ForMember(dst => dst.CreatedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = db.CreatedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.ModifiedBy, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = db.ModifiedByUserId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.DisabledBy, opt => opt.ResolveUsing(db =>
                {
                    if (!db.DisabledByUserId.HasValue) return null;
                    return new Common.Models.Security.User()
                    {
                        Id = db.DisabledByUserId.Value,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Matter, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Matters.Matter()
                    {
                        Id = db.MatterId,
                        IsStub = true
                    };
                }))
                .ForMember(dst => dst.User, opt => opt.ResolveUsing(db =>
                {
                    return new Common.Models.Security.User()
                    {
                        Id = db.UserId,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser>()
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
                .ForMember(dst => dst.MatterId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Matter == null) return null;
                    return model.Matter.Id;
                }))
                .ForMember(dst => dst.UserId, opt => opt.ResolveUsing(model =>
                {
                    if (model.User == null) return null;
                    return model.User.Id;
                }));
        }
    }
}