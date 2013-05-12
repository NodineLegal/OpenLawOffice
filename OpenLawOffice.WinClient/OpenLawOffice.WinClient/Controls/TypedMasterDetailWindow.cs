using System;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;

namespace OpenLawOffice.WinClient.Controls
{
    public class TypedMasterDetailWindow<TMasterView, TDetailView, TEditView, TCreateView> 
        : MasterDetailWindow
        where TMasterView : UserControl, IMaster, new()
        where TDetailView : UserControl, new()
        where TEditView : UserControl, new()
        where TCreateView : UserControl, new()
    {
        public TMasterView MasterView { get; set; }
        public TDetailView DetailView { get; set; }
        public TEditView EditView { get; set; }
        public TCreateView CreateView { get; set; }

        public override DisplayModeType DisplayMode
        {
            get { return _displayMode; }
            protected set
            {
                _displayMode = value;
                UpdateCommandStates();
            }
        }

        public TypedMasterDetailWindow(string title, RibbonTab ribbonTab, RibbonToggleButton editButton,
            RibbonButton createButton, RibbonButton disableButton, RibbonButton saveButton,
            RibbonButton cancelButton, Controllers.ControllerBase controller)
            : base(title, ribbonTab, editButton, createButton, disableButton, saveButton, cancelButton, controller)
        {
            Title = title;

            MasterView = new TMasterView();
            DetailView = new TDetailView();
            EditView = new TEditView();
            CreateView = new TCreateView();

            MasterControl = MasterView;
            DetailControl = null;
            
            OnActivated += iwin =>
            {
            };

            OnSelected += iwin =>
            {
            };

            MasterView.OnSelectionChanged += (sender, item) =>
            {
                UpdateCommandStates();
                UpdateDetailAndEditDataContext(item);
            };

            UpdateCommandStates();
        }

        public void UpdateDetailAndEditDataContext(object obj)
        {
            if (obj != null)
            {
                _controller.LoadDetails((ViewModels.IViewModel)obj, viewModel =>
                {
                });
            }

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                DetailView.DataContext = EditView.DataContext = obj;
            }));
        }

        public void GoIntoCreateMode(object obj)
        {
            CreateView.DataContext = obj;
            DisplayMode = DisplayModeType.Create;
        }

        public override void Clear()
        {
            CreateView.DataContext = null;
            UpdateDetailAndEditDataContext(null);
            base.Clear();
        }

        protected override void SetCreateModeCommands()
        {
            base.SetCreateModeCommands();
            DetailControl = CreateView;
            if (IsSelected && MasterView.SelectedItem != null)
                RelationshipsEnabled = true;
            else
                RelationshipsEnabled = false;
        }

        protected override void SetEditModeCommands()
        {
            base.SetEditModeCommands();
            if (MasterView.SelectedItem != null)
            {
                DetailControl = EditView;
                if (IsSelected)
                {
                    SaveEnabled = true;
                    CancelEnabled = true;
                    RelationshipsEnabled = true;
                }
                else
                {
                    SaveEnabled = false;
                    CancelEnabled = false;
                    RelationshipsEnabled = false;
                }
            }
            else
            {
                SaveEnabled = false;
                CancelEnabled = false;
                DetailControl = null;
                RelationshipsEnabled = false;
            }
        }

        protected override void SetViewModeCommands()
        {
            base.SetViewModeCommands();
            if (MasterView.SelectedItem != null)
            {
                DetailControl = DetailView;
                EditEnabled = true;
                if (IsSelected)
                {
                    DisableEnabled = true;
                    RelationshipsEnabled = true;
                }
                else
                {
                    DisableEnabled = false;
                    RelationshipsEnabled = false;
                }
            }
            else
            {
                DetailControl = null;
                EditEnabled = false;
                DisableEnabled = false;
                RelationshipsEnabled = false;
            }
        }
    }
}
