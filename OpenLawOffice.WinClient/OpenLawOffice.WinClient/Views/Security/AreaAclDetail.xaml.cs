using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaAclDetail.xaml
    /// </summary>
    public partial class AreaAclDetail : UserControl, Controls.IDetail
    {
        public AreaAclDetail()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }
    }
}