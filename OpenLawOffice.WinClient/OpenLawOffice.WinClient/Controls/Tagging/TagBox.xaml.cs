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

namespace OpenLawOffice.WinClient.Controls.Tagging
{
    /// <summary>
    /// Interaction logic for TagBox.xaml
    /// </summary>
    public partial class TagBox : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

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
            }
            else
            {
                Editable.Visibility = System.Windows.Visibility.Collapsed;
                Readonly.Visibility = System.Windows.Visibility.Visible;
            }

            object o = DataContext;
        }

    }
}
