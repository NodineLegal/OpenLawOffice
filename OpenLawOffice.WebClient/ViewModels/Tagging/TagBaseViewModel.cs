namespace OpenLawOffice.WebClient.ViewModels.Tagging
{
    using System;
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    public class TagBaseViewModel : CoreViewModel
    {
        public Guid? Id { get; set; }
        public TagCategoryViewModel TagCategory { get; set; }
        public string Tag { get; set; }
    }
}