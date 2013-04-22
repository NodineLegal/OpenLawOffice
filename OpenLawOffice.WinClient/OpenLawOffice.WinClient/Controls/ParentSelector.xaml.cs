using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for ParentSelector.xaml
    /// </summary>
    public partial class ParentSelector : UserControl
    {
        public event EventHandler<EventArgs> OnSelect;
        public event EventHandler<EventArgs> OnNodeExpanded;

        public object SelectedItem { get { return GetSelectedItem(); } }

        public ParentSelector()
        {
            InitializeComponent();
            UITree.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(TreeViewItemExpanded));
            UITree.DataContextChanged += new DependencyPropertyChangedEventHandler(UITree_DataContextChanged);
        }

        void UITree_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            string a = "";
        }

        private void UIClear_Click(object sender, RoutedEventArgs e)
        {
            ClearSelected();
        }

        private void UISelect_Click(object sender, RoutedEventArgs e)
        {
            if (OnSelect != null) OnSelect(sender, e);
        }

        public ParentSelector SetExpanderColumnTemplate(string header, string textBindingPath, double width = Double.NaN)
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

        public ParentSelector AddColumn(GridViewColumn column)
        {
            UIGridView.Columns.Add(column);
            return this;
        }

        public ParentSelector AddResource(Type dataType, object resource)
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
