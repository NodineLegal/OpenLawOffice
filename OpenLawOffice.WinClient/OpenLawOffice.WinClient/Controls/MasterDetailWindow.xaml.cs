using System;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Windows.Controls.Ribbon;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for MasterDetailWindow.xaml
    /// </summary>
    public partial class MasterDetailWindow
        : UserControl, IDockableWindow
    {
        public Action<IDockableWindow> OnActivated { get; set; }
        public Action<IDockableWindow> OnDeactivated { get; set; }
        public Action<IDockableWindow> OnSelected { get; set; }
        public Action<IDockableWindow> OnDeselected { get; set; }
        public Action<IDockableWindow> OnClose { get; set; }
        public Action<IDockableWindow> OnDispose { get; set; }

        public Action<IDockableWindow> OnRefresh { get; set; }
        public Action<IDockableWindow> OnRequestDetailDataContextUpdate { get; set; }

        public Func<ViewModels.IViewModel, ViewModels.IViewModel> GetItemDetails { get; set; }
        public Func<ViewModels.IViewModel, List<ViewModels.IViewModel>> GetItemChildren { get; set; }

        protected Controllers.ControllerBase _controller;
        private UserControl _detailControl;
        protected RelationCollection _relations;

        public bool IsSelected { get; set; }
        public bool CanHaveMultipleInstances { get { return false; } }
        public Xceed.Wpf.AvalonDock.Layout.LayoutDocument DockingWindow { get; set; }
        public string Title { get; set; }
        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        protected DisplayModeType _displayMode;
        public virtual DisplayModeType DisplayMode { get { return _displayMode; } protected set { _displayMode = value; } }
        public virtual bool IsInEditMode { get { return _displayMode == DisplayModeType.Edit; } }
        public virtual bool IsInViewMode { get { return _displayMode == DisplayModeType.View; } }
        public virtual bool IsInCreateMode { get { return _displayMode == DisplayModeType.Create; } }

        public List<Commands.DelegateCommand> RelationshipCommands { get; set; }
        private bool _relationshipsEnabled;
        public bool RelationshipsEnabled
        {
            get { return _relationshipsEnabled; }
            set
            {
                _relationshipsEnabled = value;
                foreach (Commands.DelegateCommand command in RelationshipCommands)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _editEnabled;
        public bool EditEnabled
        {
            get { return _editEnabled; }
            set
            {
                _editEnabled = value;
                if (EditButton.Command != null)
                    ((Commands.DelegateCommand)EditButton.Command).RaiseCanExecuteChanged();
            }
        }

        private bool _disableEnabled;
        public bool DisableEnabled
        {
            get { return _disableEnabled; }
            set
            {
                _disableEnabled = value;
                if (DisableButton.Command != null)
                    ((Commands.DelegateCommand)DisableButton.Command).RaiseCanExecuteChanged();
            }
        }

        private bool _createEnabled;
        public bool CreateEnabled
        {
            get { return _createEnabled; }
            set
            {
                _createEnabled = value;
                if (CreateButton.Command != null)
                    ((Commands.DelegateCommand)CreateButton.Command).RaiseCanExecuteChanged();
            }
        }

        private bool _saveEnabled;
        public bool SaveEnabled
        {
            get { return _saveEnabled; }
            set
            {
                _saveEnabled = value;
                if (SaveButton.Command != null)
                    ((Commands.DelegateCommand)SaveButton.Command).RaiseCanExecuteChanged();
            }
        }

        private bool _cancelEnabled;
        public bool CancelEnabled
        {
            get { return _cancelEnabled; }
            set
            {
                _cancelEnabled = value;
                if (CancelButton.Command != null)
                    ((Commands.DelegateCommand)CancelButton.Command).RaiseCanExecuteChanged();
            }
        }
        
        public RibbonTab RibbonTab { get; private set; }
        public RibbonToggleButton EditButton { get; private set; }
        public RibbonButton CreateButton { get; private set; }
        public RibbonButton DisableButton { get; private set; }
        public RibbonButton SaveButton { get; private set; }
        public RibbonButton CancelButton { get; private set; }
                

        public UserControl MasterControl 
        {
            get
            {
                if (UILeftPanel.Children.Count <= 0)
                    return null;

                return (UserControl)UILeftPanel.Children[0];
            }
            set
            {
                UILeftPanel.Children.Clear();
                UILeftPanel.Children.Add(value);
                ((IMaster)value).GetItemDetails = GetItemDetails;
                ((IMaster)value).GetItemChildren = GetItemChildren;
            }
        }

        public UserControl DetailControl 
        { 
            get { return _detailControl; }
            set
            {
                _detailControl = value;
                
                if (value == null)
                {
                    if (UIGrid.ColumnDefinitions[2].Width.Value > 0)
                    {
                        UIGrid.ColumnDefinitions[1].Width = new GridLength(0);
                        UIGrid.ColumnDefinitions[2].Width = new GridLength(0);
                    }
                    //if (UIGrid.ColumnDefinitions.Count > 1)
                    //    UIGrid.ColumnDefinitions.RemoveRange(1, 2);
                    UIGridSplitter.Visibility = System.Windows.Visibility.Collapsed;
                    UIRightPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    UIRightPanel.Children.Clear();
                    if (UIGrid.ColumnDefinitions[2].Width.Value <= 0)
                    //if (UIGrid.ColumnDefinitions.Count == 1)
                    {
                        UIGrid.ColumnDefinitions[1].Width = new GridLength(5);
                        UIGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                        UIGridSplitter.Visibility = System.Windows.Visibility.Visible;
                        UIRightPanel.Visibility = System.Windows.Visibility.Visible;
                        UIRightPanel.Children.Add(_detailControl);
                    }
                    else
                    {
                        UIRightPanel.Children.Add(_detailControl);
                    }
                }
            }
        }

        public object MasterDataContext { get { return DataContext; } set { DataContext = value; } }
        public object DetailDataContext 
        {
            get
            {
                if (DetailControl == null) return null;
                return DetailControl.DataContext;
            }
            set
            {
                if (DetailControl == null) return;
                DetailControl.DataContext = value;
            }
        }

        public MasterDetailWindow(string title, RibbonTab ribbonTab, RibbonToggleButton editButton,
            RibbonButton createButton, RibbonButton disableButton, RibbonButton saveButton,
            RibbonButton cancelButton, Controllers.ControllerBase controller)
        {
            InitializeComponent();

            _relations = new RelationCollection();
            IsBusy = false;

            RelationshipCommands = new List<Commands.DelegateCommand>();

            DockingWindow = new Xceed.Wpf.AvalonDock.Layout.LayoutDocument();

            DataContextChanged += (sender, args) =>
                {
                    if (OnRequestDetailDataContextUpdate != null) OnRequestDetailDataContextUpdate(this);
                };

            RibbonTab = ribbonTab;
            EditButton = editButton;
            CreateButton = createButton;
            DisableButton = disableButton;
            SaveButton = saveButton;
            CancelButton = cancelButton;
            _controller = controller;

            OnClose += iwin =>
            {
                DisplayMode = DisplayModeType.View;
                RibbonTab.Visibility = System.Windows.Visibility.Hidden;
                Clear();
            };

            OnDeselected += iwin =>
            {
                UpdateCommandStates();
            };

            OnDeactivated += iwin =>
            {
                UpdateCommandStates();
            };

            OnActivated += iwin =>
            {
                RibbonTab.Visibility = System.Windows.Visibility.Visible;
                RibbonTab.IsSelected = true;
                // No need to update the UI, setting a displaymode does that automatically
                DisplayMode = DisplayModeType.View;
            };

            OnSelected += iwin =>
            {
                RibbonTab.IsSelected = true;
                UpdateCommandStates();
            };

            EditButton.Checked += (sender, e) =>
            {
                if (DisplayMode != DisplayModeType.Edit)
                    DisplayMode = DisplayModeType.Edit;
            };

            EditButton.Unchecked += (sender, e) =>
            {
                if (DisplayMode == DisplayModeType.Edit)
                    DisplayMode = DisplayModeType.View;
            };
        }

        public void Load()
        {
            WindowManager.Instance.RegisterWindow(this);
            DetailControl = null;
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

        public void SelectWindow()
        {
            DockingWindow.IsSelected = true;
        }

        public void SetDisplayMode(DisplayModeType displayMode)
        {
            DisplayMode = displayMode;
        }

        public void UpdateMasterDataContext(object obj)
        {
            DataContext = obj;
        }

        public virtual void Clear()
        {
            UpdateMasterDataContext(null);
            DetailControl = null;
        }

        public void UpdateCommandStates()
        {
            switch (DisplayMode)
            {
                case DisplayModeType.Create:
                    SetCreateModeCommands();
                    break;
                case DisplayModeType.Edit:
                    SetEditModeCommands();
                    break;
                default: // view
                    SetViewModeCommands();
                    break;
            }
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        protected virtual void SetCreateModeCommands()
        {
            CreateEnabled = true;
            EditEnabled = false;
            DisableEnabled = false;
            if (IsSelected)
            {
                SaveEnabled = true;
                CancelEnabled = true;
            }
            else
            {
                SaveEnabled = false;
                CancelEnabled = false;
            }
        }

        protected virtual void SetEditModeCommands()
        {
            CreateEnabled = false;
            EditEnabled = true;
            DisableEnabled = false;
            if (!EditButton.IsChecked.HasValue || !EditButton.IsChecked.Value)
                EditButton.IsChecked = true;
        }

        protected virtual void SetViewModeCommands()
        {
            SaveEnabled = false;
            CancelEnabled = false;
            CreateEnabled = true;
        }

        public void ShowRelationView<TModel>()
            where TModel : Common.Models.ModelBase
        {
            DetailControl = _relations[typeof(TModel)];
            ((Views.IRelationView)DetailControl).Load();
        }

        public MasterDetailWindow AddRelationView<TModel>(UserControl uc)
            where TModel : Common.Models.ModelBase
        {
            _relations.Add(typeof(TModel), uc);
            return this;
        }
    }
}
