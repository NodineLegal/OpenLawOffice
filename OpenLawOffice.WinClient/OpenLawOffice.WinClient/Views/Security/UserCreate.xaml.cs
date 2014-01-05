using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Linq;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for UserCreate.xaml
    /// </summary>
    public partial class UserCreate : UserControl, Controls.IDetail
    {
        public UserCreate()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        private void UIPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModels.Security.User user = (ViewModels.Security.User)DataContext;
            user.Password = UIPassword.Password;
        }
    }
}
