using System;
using System.Windows.Controls;
using System.Windows;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for TreeGridView.xaml
    /// </summary>
    public partial class TreeGridView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public event RoutedEventHandler OnNodeExpanded;

        public object SelectedItem { get { return UITree.SelectedItem; } }

        public TreeGridView()
        {
            InitializeComponent();
            UITree.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(TreeViewItemExpanded));
        }

        public TreeGridView SetExpanderColumnTemplate(string header, string textBindingPath, double width = Double.NaN)
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

        public TreeGridView AddColumn(GridViewColumn column)
        {
            UIGridView.Columns.Add(column);
            return this;
        }

        public TreeGridView AddResource(Type dataType, object resource)
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
            if (OnNodeExpanded != null) OnNodeExpanded(sender, e);
        }

        private void UITree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (OnSelectionChanged != null) OnSelectionChanged(this, SelectedItem);
        }
    }
}
