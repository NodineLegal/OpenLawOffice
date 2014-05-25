namespace OpenLawOffice.WebClient.ViewModels
{
    using System;

    public abstract class DateOnlyViewModelBase : ViewModelBase
    {
        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? Disabled { get; set; }
    }
}