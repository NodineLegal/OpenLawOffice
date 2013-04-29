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
    /// Interaction logic for AreaAclEdit.xaml
    /// </summary>
    public partial class AreaAclEdit : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public AreaAclEdit()
        {
            InitializeComponent();
        }
    }
}
