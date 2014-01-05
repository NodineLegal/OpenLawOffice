using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaEdit.xaml
    /// </summary>
    public partial class AreaEdit : UserControl, Controls.IDetail
    {
        public AreaEdit()
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