namespace OpenLawOffice.WebClient.ViewModels
{
    public abstract class CoreViewModel : DateOnlyViewModelBase
    {
        public Security.UserViewModel CreatedBy { get; set; }

        public Security.UserViewModel ModifiedBy { get; set; }

        public Security.UserViewModel DisabledBy { get; set; }
    }
}