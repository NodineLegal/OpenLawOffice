//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Collections.Generic;

//namespace OpenLawOffice.WinClient.Windows
//{
//    /// <summary>
//    /// Interaction logic for MasterDetailWindow.xaml
//    /// </summary>
//    public abstract partial class MasterDetailWindow : UserControl, IDockableWindow
//    {
//        public Action<IDockableWindow> OnActivated { get; set; }
//        public Action<IDockableWindow> OnDeactivated { get; set; }
//        public Action<IDockableWindow> OnSelected { get; set; }
//        public Action<IDockableWindow> OnDeselected { get; set; }
//        public Action<IDockableWindow> OnClose { get; set; }
//        public Action<IDockableWindow> OnDispose { get; set; }


//        private UserControl _masterPanelControl;
//        private Dictionary<Type, UserControl> _availableDetailViews;
//        private UserControl _currentDetailPanelControl;
//        private object _detailPanelDataContext;

//        public Type CurrentDetailPanelControlType
//        {
//            get
//            {
//                if (_currentDetailPanelControl == null) return null;
//                return _currentDetailPanelControl.GetType();
//            }
//        }

//        public object DetailPanelDataContext { get { return _detailPanelDataContext; } }
//        public abstract void Refresh();
//        public abstract void Dispose();    

//        public MasterDetailWindow()
//        {
//            InitializeComponent();
//            DockingWindow = new AvalonDock.Layout.LayoutDocument();
//            _availableDetailViews = new Dictionary<Type, UserControl>();
//            this.DataContextChanged += (sender, e) =>
//                {
//                    string a = "";
//                };
//        }

//        public virtual void Activate()
//        {
//            DockingWindow.IsSelected = true;
//        }

//        public virtual void Load()
//        {
//            WindowManager.Instance.RegisterWindow(this);
//        }
        
//        public virtual void Close()
//        {
//        }

//        public void SetMasterPanel(UserControl uc)
//        {
//            UILeftPanel.Children.Clear();
//            UILeftPanel.Children.Add(_masterPanelControl = uc);
//        }

//        public void ShowDetailPanel(Type type)
//        {
//            UserControl uc;

//            if (!_availableDetailViews.ContainsKey(type))
//                uc = (UserControl)type.GetConstructor(new Type[] { }).Invoke(null);
//            else
//                uc = _availableDetailViews[type];

//            ShowDetailPanel(uc);
//        }

//        public void ShowDetailPanel(UserControl uc)
//        {
//            if (_availableDetailViews.ContainsKey(uc.GetType()))
//                _availableDetailViews.Add(uc.GetType(), uc);

//            if (UIGrid.ColumnDefinitions.Count == 1)
//            {
//                UIGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5) });
//                UIGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
//                UIGridSplitter.Visibility = System.Windows.Visibility.Visible;
//                UIRightPanel.Visibility = System.Windows.Visibility.Visible;
//            }

//            UIRightPanel.Children.Clear();
//            UIRightPanel.Children.Add(_currentDetailPanelControl = uc);

//            _currentDetailPanelControl.DataContext = _detailPanelDataContext;
//        }

//        public void SetDetailPanelDataContext(object obj)
//        {
//            _detailPanelDataContext = obj;
//            //if (_currentDetailPanelControl != null)
//            //    _currentDetailPanelControl.DataContext = _detailPanelDataContext;
//        }

//        public void DisableDetailPanel()
//        {
//            _currentDetailPanelControl = null;
//            UIGrid.ColumnDefinitions.RemoveRange(1, 2);
//            UIGridSplitter.Visibility = System.Windows.Visibility.Collapsed;
//            UIRightPanel.Visibility = System.Windows.Visibility.Collapsed;
//        }

//    }
//}
