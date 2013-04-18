using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.Area))]
    public class Area 
        : MasterDetailController<Controls.TreeGridView, Views.Security.AreaDetail, 
            Views.Security.AreaEdit, Views.Security.AreaCreate>
    {
        private Consumers.Security.Area _consumer;
        private Common.Rest.Requests.Security.Area _lastRequest;
        private RestSharp.IRestResponse _lastRestSharpResponse;

        public Area()
            : base("Security Areas", 
            Globals.Instance.MainWindow.SecurityAreaTab,
            Globals.Instance.MainWindow.SecurityAreas_Edit,
            Globals.Instance.MainWindow.SecurityAreas_Create,
            Globals.Instance.MainWindow.SecurityAreas_Save,
            Globals.Instance.MainWindow.SecurityAreas_Cancel)
        {
            _consumer = new Consumers.Security.Area();
            _lastRequest = null;

            MasterDetailWindow.MasterView
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
        }

        public override void LoadUI()
        {
            // ribbon controls
            MainWindow.SecurityAreas_List.Command = new Commands.DelegateCommand(x =>
            {
                // Ignores selection
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    GetData<Common.Models.Security.Area>(data =>
                    {
                        List<Common.Models.Security.Area> sysModelList = 
                            (List<Common.Models.Security.Area>)data;
                        UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
                    }, new { Name = MainWindow.SecurityAreas_List_Name.Text.Trim() });
                }), System.Windows.Threading.DispatcherPriority.Normal);
            });

            MainWindow.SecurityAreas_Acls.Command = new Commands.DelegateCommand(x =>
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.SecurityAreas_Acls.Command);

            MainWindow.SecurityAreas_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    MasterDetailWindow.GoIntoCreateMode(new ViewModels.Security.Area().AttachModel(new Common.Models.Security.Area()));
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }, x => MasterDetailWindow.CreateEnabled);
            
            MainWindow.SecurityAreas_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    MasterDetailWindow.UpdateDetailAndEditDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.SecurityAreas_Save.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    ViewModels.Security.Area areaVM = (ViewModels.Security.Area)MasterDetailWindow.DetailView.DataContext;
                    Common.Models.Security.Area sysModel = areaVM.Model;
                    Common.Rest.Requests.Security.Area requestModel = Mapper.Map<Common.Rest.Requests.Security.Area>(sysModel);
                    requestModel.AuthToken = Globals.Instance.AuthToken;
                    _consumer.Update(requestModel, result =>
                    {
                        if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new Exception("Need error handling!!!");

                        sysModel = Mapper.Map<Common.Models.Security.Area>(result.Response);
                        areaVM.Synchronize(() =>
                        {
                            areaVM = (ViewModels.Security.Area)new ViewModels.Security.Area().AttachModel(sysModel);
                            MasterDetailWindow.MasterView.ClearSelected();
                            if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                                MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);
                        });
                    });
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.SecurityAreas_Cancel.Command = new Commands.DelegateCommand(x =>
            {
                // Will need to reload the model from the server (easiest way)
                // This needs to be improved - if we want to keep loading from the server instead of 
                // doing a deep copy, then it needs to only load a single, not force the whole
                // tree to reload.
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    MasterDetailWindow.MasterView.ClearSelected();
                    MasterDetailWindow.Clear();
                }), System.Windows.Threading.DispatcherPriority.Normal);

                GetData<Common.Models.Security.Area>(data =>
                {
                    List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                        UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
                    else
                        UpdateUI(null, sysModelList, null);
                }, null);
            }, x => MasterDetailWindow.CancelEnabled);

            // load window
            MasterDetailWindow.Load();
            if (!MasterDetailWindow.IsSelected)
                MasterDetailWindow.SelectWindow();
            
            // Ignores selection
            GetData<Common.Models.Security.Area>(data => 
            {
                List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
            }, null);
        }

        public override void GetDetailData(Action onComplete, ViewModels.IViewModel viewModel)
        {
            if (typeof(ViewModels.Security.Area).IsAssignableFrom(viewModel.GetType()))
                GetDetailData(onComplete, (ViewModels.Security.Area)viewModel);
            else
                throw new ArgumentException("Argument viewModel of incorrect type.");
        }

        public void GetDetailData(Action onComplete, ViewModels.Security.Area viewModel)
        {
            int outCount = 0;
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            outCount++;
            userConsumer.GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.CreatedBy.Id
                },
                result =>
                {
                    if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception("Need error handling!!!");
                    }

                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                    viewModel.CreatedBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);

                    outCount--;

                    if (outCount <= 0)
                        if (onComplete != null) onComplete();
                });

            outCount++;
            userConsumer.GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.ModifiedBy.Id
                },
                result =>
                {
                    if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception("Need error handling!!!");
                    }

                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                    viewModel.ModifiedBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);

                    outCount--;

                    if (outCount <= 0)
                        if (onComplete != null) onComplete();
                });

            if (viewModel.DisabledBy == null || !viewModel.DisabledBy.Id.HasValue) return;

            outCount++;
            userConsumer.GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.DisabledBy.Id
                },
                result =>
                {
                    if (result.RestSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception("Need error handling!!!");
                    }

                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                    viewModel.DisabledBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);

                    outCount--;

                    if (outCount <= 0)
                        if (onComplete != null) onComplete();
                });
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

            GetData(onComplete, request);
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

            GetData(onComplete, request);
        }

        private void GetData(Action<object> onComplete, Common.Rest.Requests.Security.Area request)
        {
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

        public override void UpdateUI(ViewModels.IViewModel viewModel, object data, Controls.DisplayModeType? displayMode)
        {
            if (!typeof(ViewModels.Security.Area).IsAssignableFrom(viewModel.GetType()))
                throw new ArgumentException("Invalid ViewModel type.");
            if (!typeof(List<Common.Models.Security.Area>).IsAssignableFrom(data.GetType()))
                throw new ArgumentException("Invalid data type.");

            UpdateUI((ViewModels.Security.Area)viewModel,
                (List<Common.Models.Security.Area>)data, displayMode);
        }

        public void UpdateUI(ViewModels.Security.Area viewModel, 
            List<Common.Models.Security.Area> sysModelList, Controls.DisplayModeType? displayMode)
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
            
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                foreach (Common.Models.Security.Area sysModel in sysModelList)
                {
                    ViewModels.Security.Area childVM = new ViewModels.Security.Area();
                    childVM.AttachModel(sysModel);
                    viewModels.Add(childVM);
                    childVM.AddChild(new ViewModels.Security.Area()
                    {
                        IsDummy = true
                    });
                }
            }), System.Windows.Threading.DispatcherPriority.Normal);

            if (pushToDataContext)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    MasterDetailWindow.Clear();
                    MasterDetailWindow.UpdateMasterDataContext(viewModels);
                    if (displayMode.HasValue)
                        MasterDetailWindow.SetDisplayMode(displayMode.Value);
                    MasterDetailWindow.UpdateCommandStates();
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }
    }
}
