using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for UserEdit.xaml
    /// </summary>
    public partial class UserEdit : UserControl, Controls.IDetail
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        private void UIPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.Security.User user = (ViewModels.Security.User)DataContext;
            user.Password = UIPassword.Password;
        }
    }
}