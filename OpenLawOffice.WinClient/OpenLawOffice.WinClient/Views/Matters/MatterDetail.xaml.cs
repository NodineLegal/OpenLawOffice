using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Matters
{
    /// <summary>
    /// Interaction logic for MatterDetail.xaml
    /// </summary>
    public partial class MatterDetail : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public MatterDetail()
        {
            InitializeComponent();
        }
    }
}