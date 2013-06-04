using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for HierarchicalSelector.xaml
    /// </summary>
    public partial class HierarchicalSelector : UserControl
    {
        public event EventHandler<EventArgs> OnSelect;
        public event EventHandler<EventArgs> OnNodeExpanded;

        public object SelectedItem { get { return GetSelectedItem(); } }
        public bool IsBusy 
        { 
            get { return UIBusyIndicator.IsBusy; } 
            set { UIBusyIndicator.IsBusy = value; } 
        }

        public HierarchicalSelector()
        {
            InitializeComponent();
            IsBusy = false;
            UITree.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(TreeViewItemExpanded));
        }

        private void UIClear_Click(object sender, RoutedEventArgs e)
        {
            ClearSelected();
        }

        private void UISelect_Click(object sender, RoutedEventArgs e)
        {
            if (OnSelect != null) OnSelect(sender, e);
        }

        public HierarchicalSelector SetExpanderColumnTemplate(string header, string textBindingPath, double width = Double.NaN)
        {
            string xaml = string.Format(@"<DataTemplate 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:toolkit=""clr-namespace:DW.WPFToolkit;assembly=DW.WPFToolkit"">
                    <DockPanel>
                        <toolkit:TreeListViewExpander DockPanel.Dock=""Left"" />
                        <TextBlock Text=""{{Binding {0}}}"" Margin=""5,0,0,0"" />
                    </DockPanel>
                </DataTemplate>", textBindingPath);

            ExpanderColumn.Header = header;
            ExpanderColumn.CellTemplate = (DataTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
            ExpanderColumn.Width = width;

            return this;
        }

        public HierarchicalSelector AddColumn(GridViewColumn column)
        {
            UIGridView.Columns.Add(column);
            return this;
        }

        public HierarchicalSelector AddResource(Type dataType, object resource)
        {
            UITree.Resources.Add(new DataTemplateKey(dataType), resource);
            return this;
        }

        public object GetSelectedItem()
        {
            return UITree.SelectedItem;
        }

        public void ClearSelected()
        {
            foreach (TreeViewItem item in UITree.SelectedTreeViewItems)
            {
                item.IsSelected = false;
            }
        }

        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            if (OnNodeExpanded != null) OnNodeExpanded(this, e);
        }
    }
}
