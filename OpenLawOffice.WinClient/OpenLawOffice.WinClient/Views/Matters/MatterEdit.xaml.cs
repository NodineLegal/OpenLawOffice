using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Matters
{
    /// <summary>
    /// Interaction logic for MatterEdit.xaml
    /// </summary>
    public partial class MatterEdit : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public MatterEdit()
        {
            InitializeComponent();
        }
    }
}