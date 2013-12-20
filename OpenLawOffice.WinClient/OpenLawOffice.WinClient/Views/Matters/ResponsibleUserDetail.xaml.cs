using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views.Matters
{
    /// <summary>
    /// Interaction logic for ResponsibleUserDetail.xaml
    /// </summary>
    public partial class ResponsibleUserDetail : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public ResponsibleUserDetail()
        {
            InitializeComponent();
        }
    }
}