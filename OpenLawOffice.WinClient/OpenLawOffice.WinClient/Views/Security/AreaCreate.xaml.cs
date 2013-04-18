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

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaCreate.xaml
    /// </summary>
    public partial class AreaCreate : UserControl
    {
        public AreaCreate()
        {
            InitializeComponent();
        }

        private void UIParent_Click(object sender, RoutedEventArgs e)
        {
            UIGrid.Visibility = System.Windows.Visibility.Hidden;
            UIParentSelector.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
