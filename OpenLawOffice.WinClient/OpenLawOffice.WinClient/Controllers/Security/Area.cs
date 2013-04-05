using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;

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
                MainWindow.SecurityAreaTab.IsEnabled = true;
                MainWindow.SecurityAreas_Group.IsEnabled = true;
                MainWindow.SecurityAreas_List.IsEnabled = true;
                //MainWindow.SecurityAreas_List_Name.IsEnabled = true;
            };

            _window.OnClose += iwin =>
            {
                MainWindow.SecurityAreaTab.Visibility = System.Windows.Visibility.Hidden;
            };

            _masterControl.OnSelectionChanged += (treeGridView, viewModel) =>
            {
                _window.UpdateDetailDataContext(viewModel);
            };

            // ribbon controls
            MainWindow.SecurityAreas_List.Command = new Commands.AsyncCommand(x =>
            {
                // Ignores selection

                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    GetData<Common.Models.Security.Area>(data =>
                    {
                        List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                        UpdateUI(null, sysModelList);
                    }, new { Name = MainWindow.SecurityAreas_List_Name.Text.Trim() });
                }), System.Windows.Threading.DispatcherPriority.Normal);
            });
            
            // load window
            _window.Load();
            
            // Ignores selection
            GetData<Common.Models.Security.Area>(data => 
            {
                List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                UpdateUI(null, sysModelList);
            }, null);
        }

        public override void GetData<TModel>(Action<object> onComplete, object obj)
        {
            Common.Rest.Requests.Security.Area request;
            Type requestType = typeof(Common.Rest.Requests.Security.Area);
            Dictionary<string, PropertyInfo> requestPropertyDict = new Dictionary<string,PropertyInfo>();
            
            request = new Common.Rest.Requests.Security.Area()
            {
                AuthToken = Globals.Instance.AuthToken
            };

            if (obj != null)
            {
                foreach (PropertyInfo prop in requestType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    requestPropertyDict.Add(prop.Name, prop);
                }

                foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!requestPropertyDict.ContainsKey(prop.Name))
                        throw new ArgumentException("Property named '" + prop.Name + "' does not exist in the request object.");

                    // Sets the value of the argument "obj.[property name]" as the value of "request.[property name]"
                    requestPropertyDict[prop.Name].SetValue(request, prop.GetValue(obj, null), null);
                }
            }

            // Could use a UI progress overlay or something here

            _consumer.GetList(request,
            result =>
            {
                // Put the last request updating here, while it could go outside the callback,
                // I am putting it in here to be certain we never have a race condition
                _lastRequest = result.Request;
                _lastRestSharpResponse = result.RestSharpResponse;

                List<Common.Models.Security.Area> sysModelList = new List<Common.Models.Security.Area>();

                if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Need error handling!!!");
                }

                foreach (Common.Rest.Responses.Security.Area area in result.Response)
                {
                    sysModelList.Add(Mapper.Map<Common.Models.Security.Area>(area));
                }

                if (onComplete != null) onComplete(sysModelList);
            });
        }

        public override void GetData(Action<object> onComplete, ViewModels.IViewModel filter = null)
        {
            ViewModels.Security.Area areaVM = null;
            Common.Rest.Requests.Security.Area request;
            
            request = new Common.Rest.Requests.Security.Area()
            {
                AuthToken = Globals.Instance.AuthToken
            };
            
            if (filter != null && filter.GetType() == typeof(ViewModels.Security.Area))
            {
                areaVM = (ViewModels.Security.Area)filter;
                if (areaVM != null && areaVM.Id.HasValue)
                    request.ParentId = areaVM.Id.Value;
            }
            
            // Could use a UI progress overlay or something here

            _consumer.GetList(request,
            result =>
            {
                // Put the last request updating here, while it could go outside the callback,
                // I am putting it in here to be certain we never have a race condition
                _lastRequest = result.Request;
                _lastRestSharpResponse = result.RestSharpResponse;

                List<Common.Models.Security.Area> sysModelList = new List<Common.Models.Security.Area>();

                if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Need error handling!!!");
                }

                foreach (Common.Rest.Responses.Security.Area area in result.Response)
                {
                    sysModelList.Add(Mapper.Map<Common.Models.Security.Area>(area));
                }

                if (onComplete != null) onComplete(sysModelList);
            });
        }

        public override void UpdateUI(ViewModels.IViewModel viewModel, object data)
        {
            if (!typeof(ViewModels.Security.Area).IsAssignableFrom(viewModel.GetType()))
                throw new ArgumentException("Invalid ViewModel type.");
            if (!typeof(List<Common.Models.Security.Area>).IsAssignableFrom(data.GetType()))
                throw new ArgumentException("Invalid data type.");

            UpdateUI((ViewModels.Security.Area)viewModel,
                (List<Common.Models.Security.Area>)data);
        }

        public void UpdateUI(ViewModels.Security.Area viewModel, List<Common.Models.Security.Area> sysModelList)
        {
            bool pushToDataContext = false;
            EnhancedObservableCollection<ViewModels.Security.Area> viewModels;

            if (viewModel != null)
            {
                viewModels = viewModel.Children;

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

            foreach (Common.Models.Security.Area sysModel in sysModelList)
            {
                ViewModels.Security.Area childVM = new ViewModels.Security.Area();
                childVM.AttachModel(sysModel);
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    viewModels.Add(childVM);
                    childVM.AddChild(new ViewModels.Security.Area()
                    {
                        IsDummy = true
                    });
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }

            if (pushToDataContext)
                _window.UpdateMasterDataContext(viewModels);
        }

    }
}
