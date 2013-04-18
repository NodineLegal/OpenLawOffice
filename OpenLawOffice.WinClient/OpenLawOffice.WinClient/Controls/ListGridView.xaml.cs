using System;
using System.Windows.Controls;
using System.Collections;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for ListGridView.xaml
    /// </summary>
    public partial class ListGridView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public Action<ListGridView> OnLoad { get; set; }
        public Action<ListGridView> OnBackClick { get; set; }
        public Action<ListGridView> OnNextClick { get; set; }
        
        public object SelectedItem { get { return UIList.SelectedItem; } }

        public ListGridView()
        {
            InitializeComponent();
        }

        public object GetSelectedItem()
        {
            return UIList.SelectedItem;
        }

        public TViewModel GetSelectedItem<TViewModel, TModel>()
            where TViewModel : ViewModels.ModelBase<TModel>
            where TModel : Common.Models.ModelBase
        {
            return (TViewModel)GetSelectedItem();
        }

        public void SelectItemIfAvailable(object obj)
        {
            UIList.SelectedItem = obj;
        }

        public ListGridView AddColumn(GridViewColumn column)
        {
            UIGrid.Columns.Add(column);
            return this;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnLoad != null) OnLoad(this);
        }

        private void UIList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OnSelectionChanged != null) OnSelectionChanged(this, GetSelectedItem());
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnBackClick != null) OnBackClick(this);
        }

        private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnNextClick != null) OnNextClick(this);
        }

        public void ClearSelected()
        {
            UIList.UnselectAll();
        }
    }
}
