using System;
using System.Windows.Controls;
using System.Windows;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for MasterDetailWindow.xaml
    /// </summary>
    public partial class MasterDetailWindow : UserControl, IDockableWindow
    {
        public Action<IDockableWindow> OnActivated { get; set; }
        public Action<IDockableWindow> OnDeactivated { get; set; }
        public Action<IDockableWindow> OnSelected { get; set; }
        public Action<IDockableWindow> OnDeselected { get; set; }
        public Action<IDockableWindow> OnClose { get; set; }
        public Action<IDockableWindow> OnDispose { get; set; }

        public Action<IDockableWindow> OnRefresh { get; set; }
        public Action<IDockableWindow> OnRequestDetailDataContextUpdate { get; set; }
        
        private UserControl _detailControl;

        public bool CanHaveMultipleInstances { get { return false; } }
        public AvalonDock.Layout.LayoutDocument DockingWindow { get; set; }
        public string Title { get; set; }

        public UserControl MasterControl { get; set; }
        public UserControl DetailControl 
        { 
            get { return _detailControl; }
            set
            {
                if (value == null)
                {
                    UIGrid.ColumnDefinitions.RemoveRange(1, 2);
                    UIGridSplitter.Visibility = System.Windows.Visibility.Collapsed;
                    UIRightPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (UIGrid.ColumnDefinitions.Count == 1)
                {
                    UIGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5) });
                    UIGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    UIGridSplitter.Visibility = System.Windows.Visibility.Visible;
                    UIRightPanel.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public object MasterDataContext { get { return DataContext; } set { DataContext = value; } }
        public object DetailDataContext { get; set; }

        public MasterDetailWindow()
        {
            InitializeComponent();
            DockingWindow = new AvalonDock.Layout.LayoutDocument();
            DataContextChanged += (sender, args) =>
                {
                    if (OnRequestDetailDataContextUpdate != null) OnRequestDetailDataContextUpdate(this);
                };
            Title = "Not Set";
        }

        public void Load()
        {
            WindowManager.Instance.RegisterWindow(this);
        }

        public void Activate()
        {
            DockingWindow.IsSelected = true;
        }

        public void Refresh()
        {
            if (OnRefresh != null) OnRefresh(this);
        }

        public void Close()
        {
            // Do not fire OnClose, it will get fired by the window manager, this method is ONLY
            // for anything that needs handled specific to the MasterDetailWindow
        }

        public void Dispose()
        {
            // Do not fire OnDispose, it will get fired by the window manager, this method is ONLY
            // for anything that needs handled specific to the MasterDetailWindow
        }

        public void UpdateMasterDataContext(object obj)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (UILeftPanel.Children.Count <= 0)
                    UILeftPanel.Children.Add(MasterControl);
                DataContext = obj;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        public void UpdateDetailDataContext(object obj)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                DetailDataContext = obj;
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
