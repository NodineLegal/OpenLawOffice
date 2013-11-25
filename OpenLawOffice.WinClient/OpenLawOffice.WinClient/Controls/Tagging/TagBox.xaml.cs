using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.Controls.Tagging
{
    /// <summary>
    /// Interaction logic for TagBox.xaml
    /// </summary>
    public partial class TagBox : UserControl//, System.ComponentModel.INotifyPropertyChanged
    {
        public delegate void ActionHandler(TagBox tagbox, CategoryTag tag);
        public delegate void AddActionHandler(TagBox tagbox, ViewModels.Tagging.TagCategory category, string tag);
        public event ActionHandler OnCancel;
        public event ActionHandler OnSave;
        public event ActionHandler OnDelete;
        public event AddActionHandler OnAdd;

        //public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(CategoryTag.DisplayModeType), 
            typeof(TagBox));

        public CategoryTag.DisplayModeType DisplayMode
        {
            get { return (CategoryTag.DisplayModeType)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        public TagBox()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DisplayMode == CategoryTag.DisplayModeType.View ||
                DisplayMode == CategoryTag.DisplayModeType.Edit)
            {
                Editable.Visibility = System.Windows.Visibility.Visible;
                Readonly.Visibility = System.Windows.Visibility.Collapsed;
                Add.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Editable.Visibility = System.Windows.Visibility.Collapsed;
                Readonly.Visibility = System.Windows.Visibility.Visible;
                Add.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void CategoryTag_OnCancel(CategoryTag sender)
        {
            if (OnCancel != null) OnCancel(this, sender);
        }

        private void CategoryTag_OnDelete(CategoryTag sender)
        {
            if (OnDelete != null) OnDelete(this, sender);
        }

        private void CategoryTag_OnSave(CategoryTag sender)
        {
            if (OnSave != null) OnSave(this, sender);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Editable.Visibility = System.Windows.Visibility.Hidden;
            Add.Visibility = System.Windows.Visibility.Hidden;
            UITagCreator.Load();
        }

        private void TagCreator_OnCancel(TagCreator tagcreator)
        {
            Editable.Visibility = System.Windows.Visibility.Visible;
            Add.Visibility = System.Windows.Visibility.Visible;
        }

        private void TagCreator_OnSave(TagCreator tagcreator, ViewModels.Tagging.TagCategory category, string tag)
        {
            Editable.Visibility = System.Windows.Visibility.Visible;
            Add.Visibility = System.Windows.Visibility.Visible;
            if (OnAdd != null) OnAdd(this, category, tag);
        }
    }
}
