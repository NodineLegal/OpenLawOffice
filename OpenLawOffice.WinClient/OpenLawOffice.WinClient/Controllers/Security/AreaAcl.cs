using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.AreaAcl))]
    public class AreaAcl
        : MasterDetailController<Controls.ListGridView, Views.Security.AreaAclDetail, 
            Views.Security.AreaAclEdit, Views.Security.AreaAclCreate>
    {
        private Consumers.Security.AreaAcl _consumer;
        private Common.Rest.Requests.Security.AreaAcl _lastRequest;
        private RestSharp.IRestResponse _lastRestSharpResponse;
        
        public AreaAcl()
            : base("Area ACLs", 
            Globals.Instance.MainWindow.SecurityAreaAclTab,
            Globals.Instance.MainWindow.SecurityAreaAcls_Edit,
            Globals.Instance.MainWindow.SecurityAreaAcls_Create,
            Globals.Instance.MainWindow.SecurityAreas_Disable,
            Globals.Instance.MainWindow.SecurityAreaAcls_Save,
            Globals.Instance.MainWindow.SecurityAreaAcls_Cancel)
        {
            _consumer = new Consumers.Security.AreaAcl();
            _lastRequest = null;

            MasterDetailWindow.MasterView
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "Area",
                    DisplayMemberBinding = new System.Windows.Data.Binding("Area.Name")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                })
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "User",
                    DisplayMemberBinding = new System.Windows.Data.Binding("User.Username")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });
        }

        public override void LoadUI()
        {
            // ribbon controls
            MainWindow.SecurityAreaAcls_List.Command = new Commands.DelegateCommand(x =>
            {
                // Ignores selection
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    GetData<Common.Models.Security.AreaAcl>(data =>
                    {
                        List<Common.Models.Security.AreaAcl> sysModelList =
                            (List<Common.Models.Security.AreaAcl>)data;
                        UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
                    }, null);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            });

            MainWindow.SecurityAreaAcls_Create.Command = new Commands.DelegateCommand(x =>
            {
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.SecurityAreaAcls_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    MasterDetailWindow.UpdateDetailAndEditDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }, x => MasterDetailWindow.EditEnabled);

            // load window
            MasterDetailWindow.Load();
        }

        public override void GetDetailData(Action onComplete, ViewModels.IViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public override void GetData<TModel>(Action<object> onComplete, object obj)
        {
            throw new NotImplementedException();
        }

        public override void GetData(Action<object> onComplete, ViewModels.IViewModel obj)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUI(ViewModels.IViewModel viewModel, object data, Controls.DisplayModeType? displayMode)
        {
            throw new NotImplementedException();
        }
    }
}
