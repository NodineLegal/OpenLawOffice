using System;
using System.Windows.Controls;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for NullView.xaml
    /// </summary>
    public partial class NullView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public Func<ViewModels.IViewModel, ViewModels.IViewModel> GetItemDetails { get; set; }
        public Func<ViewModels.IViewModel, List<ViewModels.IViewModel>> GetItemChildren { get; set; }

        public NullView()
        {
            InitializeComponent();
        }

        public object SelectedItem
        {
            get { return null; }
        }

        public void ClearSelected()
        {
        }

        public void SelectItem(object obj)
        {
        }
    }
}
