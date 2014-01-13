namespace OpenLawOffice.WebClient.ViewModels.Tagging
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;
    using DBOs = OpenLawOffice.Server.Core.DBOs;

    public class TagBaseViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }
        public TagCategoryViewModel TagCategory { get; set; }
        public string Tag { get; set; }
    }
}