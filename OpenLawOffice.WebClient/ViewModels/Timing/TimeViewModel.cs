namespace OpenLawOffice.WebClient.ViewModels.Timing
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using DBOs = OpenLawOffice.Server.Core.DBOs;

    [MapMe]
    public class TimeViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public Contacts.ContactViewModel Worker { get; set; }

        public new void BuildMappings()
        {
            Mapper.CreateMap<DBOs.Timing.Time, TimeViewModel>()
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
                .ForMember(dst => dst.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dst => dst.Stop, opt => opt.MapFrom(src => src.Stop))
                .ForMember(dst => dst.Worker, opt => opt.ResolveUsing(db =>
                {
                    return new ViewModels.Contacts.ContactViewModel()
                    {
                        Id = db.WorkerContactId,
                        IsStub = true
                    };
                }));

            Mapper.CreateMap<TimeViewModel, DBOs.Timing.Time>()
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
                .ForMember(dst => dst.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dst => dst.Stop, opt => opt.MapFrom(src => src.Stop))
                .ForMember(dst => dst.WorkerContactId, opt => opt.ResolveUsing(model =>
                {
                    if (model.Worker == null) return null;
                    return model.Worker.Id;
                }));
        }
    }
}