using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.AreaAcl))]
    public class AreaAcl : ControllerBase
    {
        private MainWindow MainWindow = Globals.Instance.MainWindow;
        private Type _masterControlType = typeof(Controls.ListGridView);
        private Type _detailControlType = typeof(Views.Security.AreaAclDetail);
        private Controls.MasterDetailWindow _window;
        private Consumers.Security.AreaAcl _consumer;
        private Common.Rest.Requests.Security.AreaAcl _lastRequest;
        private RestSharp.IRestResponse _lastRestSharpResponse;

        private Controls.ListGridView _masterControl
        {
            get { return (Controls.ListGridView)_window.MasterControl; }
            set { _window.MasterControl = value; }
        }

        private Views.Security.AreaAclDetail _detailControl
        {
            get { return (Views.Security.AreaAclDetail)_window.DetailControl; }
            set { _window.DetailControl = value; }
        }

        public override void LoadUI()
        {
            _window = new Controls.MasterDetailWindow() { Title = "Area ACLs" };
            _consumer = new Consumers.Security.AreaAcl();
            _lastRequest = null;

            // set controls
            _masterControl = (Controls.ListGridView)_masterControlType.GetConstructor(new Type[] { }).Invoke(null);
            _detailControl = (Views.Security.AreaAclDetail)_detailControlType.GetConstructor(new Type[] { }).Invoke(null);

            _masterControl
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "User",
                    DisplayMemberBinding = new System.Windows.Data.Binding("User.Username")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });

            // Show the security area acl tab
            MainWindow.SecurityAreaAclTab.Visibility = System.Windows.Visibility.Visible;
            MainWindow.SecurityAreaAclTab.IsSelected = true;

            // wireup deselection of window
            _window.OnDeselected += iwin =>
            {
                MainWindow.SecurityAreaAcls_List.IsEnabled = true;
                MainWindow.SecurityAreaAcls_Relationships.IsEnabled = false;
                MainWindow.SecurityAreaAcls_Actions.IsEnabled = false;
            };

            // wireup selection of window
            _window.OnSelected += iwin =>
            {
                MainWindow.SecurityAreaAclTab.IsSelected = true;
                MainWindow.SecurityAreaAcls_List.IsEnabled = false;
                MainWindow.SecurityAreaAcls_Edit.IsEnabled = true;
                MainWindow.SecurityAreaAcls_Create.IsEnabled = true;
                if (_masterControl.GetSelectedItem() != null)
                { // If something is selected then
                    MainWindow.SecurityAreaAcls_Save.IsEnabled = true;
                    MainWindow.SecurityAreaAcls_Cancel.IsEnabled = true;
                }
                else
                {
                    MainWindow.SecurityAreaAcls_Save.IsEnabled = false;
                    MainWindow.SecurityAreaAcls_Cancel.IsEnabled = false;
                }
            };

            _window.OnActivated += iwin =>
            {
                MainWindow.SecurityAreaAclTab.IsEnabled = true;
                MainWindow.SecurityAreaAcls_Group.IsEnabled = true;
                MainWindow.SecurityAreaAcls_List.IsEnabled = true;
            };

            _window.OnClose += iwin =>
            {
                MainWindow.SecurityAreaAclTab.Visibility = System.Windows.Visibility.Hidden;
            };

            _masterControl.OnSelectionChanged += (treeGridView, viewModel) =>
            {
                _window.UpdateDetailDataContext(viewModel);
            };

            // ribbon controls

            // load window
            _window.Load();
        }

        public override void GetData<TModel>(Action<object> onComplete, object obj)
        {
            throw new NotImplementedException();
        }

        public override void GetData(Action<object> onComplete, ViewModels.IViewModel obj)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUI(ViewModels.IViewModel viewModel, object data)
        {
            throw new NotImplementedException();
        }
    }
}
