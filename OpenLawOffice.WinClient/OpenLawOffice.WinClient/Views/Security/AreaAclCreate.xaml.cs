using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaAclCreate.xaml
    /// </summary>
    public partial class AreaAclCreate : UserControl, Controls.IDetail
    {
        public AreaAclCreate()
        {
            InitializeComponent();

            if (_viewModel == null)
                DataContext = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(new Common.Models.Security.AreaAcl());

            UIAreaSelector.OnNodeExpanded += (sender, args) =>
            {
                DW.WPFToolkit.TreeListViewItem treeItem = (DW.WPFToolkit.TreeListViewItem)((RoutedEventArgs)args).OriginalSource;
                ViewModels.Security.Area viewModel = (ViewModels.Security.Area)treeItem.DataContext;
                ICollection<ViewModels.IViewModel> collection = viewModel.Children.Cast<ViewModels.IViewModel>().ToList();
                ViewModels.Security.Area filter = ViewModels.Creator.Create<ViewModels.Security.Area>(
                    new Common.Models.Security.Area()
                    {
                        Parent = new Common.Models.Security.Area()
                        {
                            Id = viewModel.Id
                        }
                    });

                UIAreaSelector.IsBusy = true;

                ControllerManager.Instance.LoadItems<Common.Models.Security.Area>(filter, collection, (results, error) =>
                {
                    viewModel.Children.Clear();
                    foreach (ViewModels.Security.Area viewModelToAdd in results)
                    {
                        viewModel.AddChild(viewModelToAdd);
                    }
                    UIAreaSelector.IsBusy = false;
                });
            };

            UIAreaSelector.OnSelect += (sender, args) =>
            {
                _viewModel.Area = (ViewModels.Security.Area)UIAreaSelector.SelectedItem;
                UIGrid.IsEnabled = true;
                UIGrid.Visibility = System.Windows.Visibility.Visible;
                UIAreaSelectorOverlay.Visibility = System.Windows.Visibility.Collapsed;
            };

            UIAreaSelector
                .AddResource(typeof(ViewModels.Security.Area), new System.Windows.HierarchicalDataTemplate()
                {
                    DataType = typeof(ViewModels.Security.Area),
                    ItemsSource = new System.Windows.Data.Binding("Children")
                })
                .SetExpanderColumnTemplate("Name", "Name", 225);

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

        private ViewModels.Security.AreaAcl _viewModel { get { return (ViewModels.Security.AreaAcl)DataContext; } }

        private void UIArea_Click(object sender, RoutedEventArgs e)
        {
            UIGrid.Visibility = System.Windows.Visibility.Hidden;
            UIAreaSelectorOverlay.Visibility = System.Windows.Visibility.Visible;
            UIGrid.IsEnabled = false;

            UIAreaSelector.IsBusy = true;

            ControllerManager.Instance.LoadItems<Common.Models.Security.Area>(
                ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area()),
                (ICollection<ViewModels.IViewModel>)new List<ViewModels.Security.Area>().Cast<ViewModels.IViewModel>().ToList(),
                (viewModels, error) =>
                {
                    List<ViewModels.Security.Area> results = viewModels.Cast<ViewModels.Security.Area>().ToList();
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UIAreaSelector.DataContext = results;
                        UIAreaSelector.IsBusy = false;
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