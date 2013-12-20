using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenLawOffice.WinClient.Views.Matters
{
    /// <summary>
    /// Interaction logic for ResponsibleUserEdit.xaml
    /// </summary>
    public partial class ResponsibleUserEdit : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        private ViewModels.Matters.ResponsibleUser _viewModel { get { return (ViewModels.Matters.ResponsibleUser)DataContext; } }
        
        public ResponsibleUserEdit()
        {
            InitializeComponent();

            UIMatterSelector.OnSelect += (sender, args) =>
            {
                _viewModel.Matter = (ViewModels.Matters.Matter)UIMatterSelector.SelectedItem;
                UIGrid.IsEnabled = true;
                UIGrid.Visibility = System.Windows.Visibility.Visible;
                UIMatterSelector.Visibility = System.Windows.Visibility.Collapsed;
            };

            UIMatterSelector.AddColumn(new System.Windows.Controls.GridViewColumn()
            {
                Header = "Title",
                DisplayMemberBinding = new System.Windows.Data.Binding("Title")
                {
                    Mode = System.Windows.Data.BindingMode.TwoWay
                },
                Width = 200
            });

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

        private void UIMatter_Click(object sender, RoutedEventArgs e)
        {
            UIGrid.Visibility = System.Windows.Visibility.Hidden;
            UIUserSelectorOverlay.Visibility = System.Windows.Visibility.Visible;
            UIGrid.IsEnabled = false;

            UIMatterSelector.IsBusy = true;

            ControllerManager.Instance.LoadItems<Common.Models.Matters.Matter>(
                ViewModels.Creator.Create<ViewModels.Matters.Matter>(new Common.Models.Matters.Matter()),
                (ICollection<ViewModels.IViewModel>)new List<ViewModels.Matters.Matter>().Cast<ViewModels.IViewModel>().ToList(),
                (viewModels, error) =>
                {
                    List<ViewModels.Matters.Matter> results = viewModels.Cast<ViewModels.Matters.Matter>().ToList();
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UIMatterSelector.DataContext = results;
                        UIMatterSelector.IsBusy = false;
                    }));
                });
        }

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
