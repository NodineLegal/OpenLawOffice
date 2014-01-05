using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaDetail.xaml
    /// </summary>
    public partial class AreaDetail : UserControl, Controls.IDetail
    {
        public AreaDetail()
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