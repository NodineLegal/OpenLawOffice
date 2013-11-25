using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Views
{
    /// <summary>
    /// Interaction logic for NullView.xaml
    /// </summary>
    public partial class NullView : UserControl, Controls.IDetail
    {
        public bool IsBusy
        {
            get { return false; }
            set { }
        }

        public NullView()
        {
            InitializeComponent();
        }
    }
}
