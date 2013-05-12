using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Linq;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaCreate.xaml
    /// </summary>
    public partial class AreaCreate : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        private ViewModels.Security.Area _viewModel { get { return (ViewModels.Security.Area)DataContext; } }

        public AreaCreate()
        {
            InitializeComponent();

            UIParentSelector.OnNodeExpanded += (sender, args) =>
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

                UIParentSelector.IsBusy = true;

                ControllerManager.Instance.LoadItems(filter, collection, results =>
                {
                    viewModel.Children.Clear();
                    foreach (ViewModels.Security.Area viewModelToAdd in results)
                    {
                        viewModel.AddChild(viewModelToAdd);
                    }
                    UIParentSelector.IsBusy = false;
                });
            };

            UIParentSelector.OnSelect += (sender, args) =>
            {
                _viewModel.Parent = (ViewModels.Security.Area)UIParentSelector.SelectedItem;
                UIGrid.IsEnabled = true;
                UIGrid.Visibility = System.Windows.Visibility.Visible;
                UIParentSelectorOverlay.Visibility = System.Windows.Visibility.Collapsed;
            };

            UIParentSelector
                .AddResource(typeof(ViewModels.Security.Area), new System.Windows.HierarchicalDataTemplate()
                {
                    DataType = typeof(ViewModels.Security.Area),
                    ItemsSource = new System.Windows.Data.Binding("Children")
                })
                .SetExpanderColumnTemplate("Name", "Name", 225);
        }

        private void UIParent_Click(object sender, RoutedEventArgs e)
        {
            UIGrid.Visibility = System.Windows.Visibility.Hidden;
            UIParentSelectorOverlay.Visibility = System.Windows.Visibility.Visible;
            UIGrid.IsEnabled = false;

            UIParentSelector.IsBusy = true;

            ControllerManager.Instance.LoadItems<Common.Models.Security.Area>(
                ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area()), 
                (ICollection<ViewModels.IViewModel>)new List<ViewModels.Security.Area>().Cast<ViewModels.IViewModel>().ToList(), 
                viewModels =>
            {
                List<ViewModels.Security.Area> results = viewModels.Cast<ViewModels.Security.Area>().ToList();
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    UIParentSelector.DataContext = results;
                    UIParentSelector.IsBusy = false;
                }));
            });
        }
    }
}
