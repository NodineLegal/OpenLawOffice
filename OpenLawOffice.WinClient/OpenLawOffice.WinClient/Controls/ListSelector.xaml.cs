using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for ListSelector.xaml
    /// </summary>
    public partial class ListSelector : UserControl
    {
        public event EventHandler<EventArgs> OnSelect;

        public object SelectedItem { get { return GetSelectedItem(); } }
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public ListSelector()
        {
            InitializeComponent();
            IsBusy = false;
        }

        private void UIClear_Click(object sender, RoutedEventArgs e)
        {
            ClearSelected();
        }

        private void UISelect_Click(object sender, RoutedEventArgs e)
        {
            if (OnSelect != null) OnSelect(sender, e);
        }

        public ListSelector AddColumn(GridViewColumn column)
        {
            UIGrid.Columns.Add(column);
            return this;
        }
        
        public object GetSelectedItem()
        {
            return UIList.SelectedItem;
        }

        public void ClearSelected()
        {
            UIList.SelectedIndex = -1;
        }
    }
}
