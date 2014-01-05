using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for SecuredResourceAclEdit.xaml
    /// </summary>
    public partial class SecuredResourceAclEdit : UserControl
    {
        public SecuredResourceAclEdit()
        {
            InitializeComponent();

            UIUserSelector.OnSelect += (sender, args) =>
            {
                _viewModel.User = (ViewModels.Security.User)UIUserSelector.SelectedItem;
                UIGrid.IsEnabled = true;
                UIGrid.Visibility = System.Windows.Visibility.Visible;
                UIUserSelectorOverlay.Visibility = System.Windows.Visibility.Collapsed;
            };

            UIUserSelector.AddColumn(new System.Windows.Controls.GridViewColumn()
            {
                Header = "Username",
                DisplayMemberBinding = new System.Windows.Data.Binding("Username")
                {
                    Mode = System.Windows.Data.BindingMode.TwoWay
                },
                Width = 200
            });
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        private ViewModels.Security.SecuredResourceAcl _viewModel { get { return (ViewModels.Security.SecuredResourceAcl)DataContext; } }

        private void UIUser_Click(object sender, RoutedEventArgs e)
        {
            UIGrid.Visibility = System.Windows.Visibility.Hidden;
            UIUserSelectorOverlay.Visibility = System.Windows.Visibility.Visible;
            UIGrid.IsEnabled = false;

            UIUserSelector.IsBusy = true;

            ControllerManager.Instance.LoadItems<Common.Models.Security.User>(
                ViewModels.Creator.Create<ViewModels.Security.User>(new Common.Models.Security.User()),
                (ICollection<ViewModels.IViewModel>)new List<ViewModels.Security.User>().Cast<ViewModels.IViewModel>().ToList(),
                (viewModels, error) =>
                {
                    List<ViewModels.Security.User> results = viewModels.Cast<ViewModels.Security.User>().ToList();
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UIUserSelector.DataContext = results;
                        UIUserSelector.IsBusy = false;
                    }));
                });
        }
    }
}