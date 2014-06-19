namespace OpenLawOffice.WebClient.ViewModels
{
    public abstract class CoreViewModel : DateOnlyViewModelBase
    {
        public Account.UsersViewModel CreatedBy { get; set; }

        public Account.UsersViewModel ModifiedBy { get; set; }

        public Account.UsersViewModel DisabledBy { get; set; }
    }
}