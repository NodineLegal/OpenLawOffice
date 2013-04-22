using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaCreate.xaml
    /// </summary>
    public partial class AreaCreate : UserControl
    {
        private ViewModels.Security.Area _viewModel { get { return (ViewModels.Security.Area)DataContext; } }

        public AreaCreate()
        {
            InitializeComponent();

            UIParentSelector.OnNodeExpanded += (sender, args) =>
            {
                DW.WPFToolkit.TreeListViewItem treeItem = (DW.WPFToolkit.TreeListViewItem)((RoutedEventArgs)args).OriginalSource;
                ViewModels.Security.Area viewModel = (ViewModels.Security.Area)treeItem.DataContext;
                
                ControllerManager.Instance.GetData<Common.Models.Security.Area>(matches =>
                {
                    List<Common.Models.Security.Area> modelList = (List<Common.Models.Security.Area>)matches;
                    viewModel.Synchronize(() =>
                    {
                        viewModel.Children.Clear();
                        foreach (Common.Models.Security.Area sysModel in modelList)
                        {
                            ViewModels.Security.Area childVM = new ViewModels.Security.Area();
                            childVM.AttachModel(sysModel);
                            childVM.AddChild(new ViewModels.Security.Area()
                            {
                                IsDummy = true
                            });
                            viewModel.AddChild(childVM);
                        }
                    });
                }, new { ParentId = viewModel.Id });
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

            ControllerManager.Instance.GetData<Common.Models.Security.Area>(matches =>
            {
                List<Common.Models.Security.Area> modelList = (List<Common.Models.Security.Area>)matches;
                List<ViewModels.Security.Area> viewModelList = new List<ViewModels.Security.Area>();
                foreach (Common.Models.Security.Area sysModel in modelList)
                {
                    ViewModels.Security.Area childVM = new ViewModels.Security.Area();
                    childVM.AttachModel(sysModel);
                    childVM.AddChild(new ViewModels.Security.Area()
                    {
                        IsDummy = true
                    });
                    viewModelList.Add(childVM);
                }
                
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    UIParentSelector.DataContext = viewModelList;
                }), System.Windows.Threading.DispatcherPriority.Normal);

            }, null);
        }
    }
}
