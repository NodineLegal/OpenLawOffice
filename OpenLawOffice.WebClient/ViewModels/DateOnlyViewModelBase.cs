namespace OpenLawOffice.WebClient.ViewModels
{
    using System;

    public abstract class DateOnlyViewModelBase : ViewModelBase
    {
        public DateTime? UtcCreated { get; set; }
        public DateTime? UtcModified { get; set; }
        public DateTime? UtcDisabled { get; set; }
    }
}