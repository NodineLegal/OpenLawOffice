using System;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for ListGridView.xaml
    /// </summary>
    public partial class ListGridView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public Action<ListGridView> OnLoad { get; set; }
        public Func<ViewModels.IViewModel, ViewModels.IViewModel> GetItemDetails { get; set; }
        public Func<ViewModels.IViewModel, List<ViewModels.IViewModel>> GetItemChildren { get; set; }
        //public Action<ListGridView> OnBackClick { get; set; }
        //public Action<ListGridView> OnNextClick { get; set; }

        public object SelectedItem
        {
            get { return UIList.SelectedItem; }
            set { SelectItem(value); }
        }

        public ListGridView()
        {
            InitializeComponent();
        }

        public object GetSelectedItem()
        {
            return UIList.SelectedItem;
        }

        public TViewModel GetSelectedItem<TViewModel, TModel>()
            where TViewModel : ViewModels.ViewModelBase<TModel>
            where TModel : Common.Models.ModelBase, new()
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

        //private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (OnBackClick != null) OnBackClick(this);
        //}

        //private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (OnNextClick != null) OnNextClick(this);
        //}

        public void ClearSelected()
        {
            UIList.UnselectAll();
        }

        public void SelectItem(object obj)
        {
            /* Implementation
             * 1) Visually select the argument ViewModel
             *  a) If the "Select" event is not fired when the programmatic ui selection is made, it will 
             *      need to be programmatically fired.
             */

            foreach (ViewModels.IViewModel viewModel in UIList.ItemsSource)
            {
                if (viewModel.Equals(obj))
                    UIList.SelectedItem = viewModel;
            }
        }
    }
}
