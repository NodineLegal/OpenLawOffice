using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.Area))]
    public class Area : ControllerBase
    {
        private MainWindow MainWindow = Globals.Instance.MainWindow;
        private Type _masterControlType = typeof(Controls.TreeGridView);
        private Type _detailControlType = typeof(Views.Security.AreaDetail);
        private Controls.MasterDetailWindow _window;
        private Consumers.Security.Area _consumer;
        private Common.Rest.Requests.Security.Area _lastRequest;
        private RestSharp.IRestResponse _lastRestSharpResponse;

        private Controls.TreeGridView _masterControl
        {
            get { return (Controls.TreeGridView)_window.MasterControl; }
            set { _window.MasterControl = value; }
        }

        private Views.Security.AreaDetail _detailControl
        {
            get { return (Views.Security.AreaDetail)_window.DetailControl; }
            set { _window.DetailControl = value; }
        }

        public override void LoadUI()
        {
            _window = new Controls.MasterDetailWindow() { Title = "Security Areas" };
            _consumer = new Consumers.Security.Area();
            _lastRequest = null;

            // set controls
            _masterControl = (Controls.TreeGridView)_masterControlType.GetConstructor(new Type[] { }).Invoke(null);
            _detailControl = (Views.Security.AreaDetail)_detailControlType.GetConstructor(new Type[] { }).Invoke(null);

            _masterControl
                .AddResource(typeof(ViewModels.Security.Area), new System.Windows.HierarchicalDataTemplate()
                {
                    DataType = typeof(ViewModels.Security.Area),
                    ItemsSource = new System.Windows.Data.Binding("Children")
                })
                .SetExpanderColumnTemplate("Name", "Name", 225)
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "Description",
                    DisplayMemberBinding = new System.Windows.Data.Binding("Description")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });
            
            // Show the security tab
            MainWindow.SecurityAreaTab.Visibility = System.Windows.Visibility.Visible;
            MainWindow.SecurityAreaTab.IsSelected = true;

            // wireup deselection of window
            _window.OnDeselected += iwin =>
            {
                MainWindow.SecurityAreas_List.IsEnabled = true;
                MainWindow.SecurityAreas_List_Name.IsEnabled = false;
                MainWindow.SecurityAreas_Relationships.IsEnabled = false;
                MainWindow.SecurityAreas_Actions.IsEnabled = false;
            };

            // wireup selection of window
            _window.OnSelected += iwin =>
            {
                MainWindow.SecurityAreas_List.IsEnabled = false;
                MainWindow.SecurityAreas_List_Name.IsEnabled = true;
                MainWindow.SecurityAreas_Edit.IsEnabled = true;
                MainWindow.SecurityAreas_Create.IsEnabled = true;
                if (_masterControl.GetSelectedItem() != null)
                { // If something is selected then
                    MainWindow.SecurityAreas_Save.IsEnabled = true;
                    MainWindow.SecurityAreas_Cancel.IsEnabled = true;
                }
                else
                {
                    MainWindow.SecurityAreas_Save.IsEnabled = false;
                    MainWindow.SecurityAreas_Cancel.IsEnabled = false;
                }                
            };

            _window.OnActivated += iwin =>
            {
                MainWindow.SecurityAreas_List.IsEnabled = false;
                MainWindow.SecurityAreas_List_Name.IsEnabled = true;
            };

            _window.OnClose += iwin =>
            {
                MainWindow.SecurityAreaTab.Visibility = System.Windows.Visibility.Hidden;
            };

            // load window
            _window.Load();

            GetData();
        }

        public override void GetData(ViewModels.IViewModel obj = null)
        {
            bool pushToDataContext = false;
            ViewModels.Security.Area areaVM = null;
            Common.Rest.Requests.Security.Area request;            
            
            request = new Common.Rest.Requests.Security.Area()
            {
                AuthToken = Globals.Instance.AuthToken
            };
            
            if (obj != null && obj.GetType() == typeof(ViewModels.Security.Area))
            {
                areaVM = (ViewModels.Security.Area)obj;
                if (areaVM != null && areaVM.Id.HasValue)
                    request.ParentId = areaVM.Id.Value;
                areaVM.Children.Clear();
            }            
            
            // Could use a UI progress overlay or something here

            _consumer.GetList(request,
            result =>
            {
                EnhancedObservableCollection<ViewModels.Security.Area> viewModels;

                // Put the last request updating here, while it could go outside the callback,
                // I am putting it in here to be certain we never have a race condition
                _lastRequest = result.Request;
                _lastRestSharpResponse = result.RestSharpResponse;

                if (areaVM != null)
                {
                    viewModels = areaVM.Children;
                    
                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        viewModels.Clear();
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                }
                else
                {
                    viewModels = new EnhancedObservableCollection<ViewModels.Security.Area>();
                    pushToDataContext = true;
                }

                if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Need error handling!!!");
                }

                foreach (Common.Rest.Responses.Security.Area area in result.Response)
                {
                    Common.Models.Security.Area sysModel = Mapper.Map<Common.Models.Security.Area>(area);
                    ViewModels.Security.Area viewModel = new ViewModels.Security.Area();
                    viewModel.AttachModel(sysModel);
                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        viewModels.Add(viewModel);
                        viewModel.AddChild(new ViewModels.Security.Area()
                        {
                            IsDummy = true
                        });
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                }

                if (pushToDataContext)
                    _window.UpdateMasterDataContext(viewModels);
            });
        }
    }
}
