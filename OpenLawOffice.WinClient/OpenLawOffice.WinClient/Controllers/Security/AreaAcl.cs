using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.AreaAcl))]
    public class AreaAcl
        : MasterDetailControllerCore<Controls.ListGridView, Views.Security.AreaAclDetail, 
            Views.Security.AreaAclEdit, Views.Security.AreaAclCreate>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Security.AreaAcl); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Security.AreaAcl); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Security.AreaAcl); } }
        public override Type ModelType { get { return typeof(Common.Models.Security.AreaAcl); } }

        private new Consumers.Security.AreaAcl _consumer;
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
            ViewModels.Security.User userFilter = null;
            ViewModels.Security.Area areaFilter = null;
            Common.Rest.Requests.Security.AreaAcl filter = null;

            //// ribbon controls
            //MainWindow.SecurityAreaAcls_List.Command = new Commands.DelegateCommand(x =>
            //{
            //    App.Current.Dispatcher.Invoke(new Action(() =>
            //    {
            //        userFilter = (ViewModels.Security.User)MainWindow.SecurityAreaAcls_List_User.SelectionBoxItem;
            //        areaFilter = (ViewModels.Security.Area)MainWindow.SecurityAreaAcls_List_Area.SelectionBoxItem;

            //        if (userFilter != null || areaFilter != null)
            //        {
            //            filter = new Common.Rest.Requests.Security.AreaAcl();
            //            if (userFilter != null)
            //                filter.UserId = userFilter.Id;
            //            if (areaFilter != null)
            //                filter.SecurityAreaId = areaFilter.Id;
            //        }
            //    }));

            //    MasterDetailWindow.IsBusy = true;

            //    GetData<Common.Models.Security.AreaAcl>(data =>
            //    {
            //        List<Common.Models.Security.AreaAcl> sysModelList =
            //            (List<Common.Models.Security.AreaAcl>)data;

            //        App.Current.Dispatcher.Invoke(new Action(() =>
            //        {
            //            UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
            //            MasterDetailWindow.IsBusy = false;
            //        }), null);
            //    }, filter);
            //});

            //MainWindow.SecurityAreaAcls_Create.Command = new Commands.DelegateCommand(x =>
            //{
            //    App.Current.Dispatcher.Invoke(new Action(() =>
            //    {
            //        ViewModels.Security.AreaAcl viewModel = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(new Common.Models.Security.AreaAcl());
            //        MasterDetailWindow.GoIntoCreateMode(viewModel);
            //    }));
            //}, x => MasterDetailWindow.CreateEnabled);

            //MainWindow.SecurityAreaAcls_Edit.Command = new Commands.DelegateCommand(x =>
            //{
            //    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            //    {
            //        MasterDetailWindow.UpdateDetailAndEditDataContext(MasterDetailWindow.MasterView.SelectedItem);
            //    }), System.Windows.Threading.DispatcherPriority.Normal);
            //}, x => MasterDetailWindow.EditEnabled);
            
            //MainWindow.SecurityAreas_Disable.Command = new Commands.DelegateCommand(x =>
            //{
            //    System.Windows.MessageBoxResult mbResult = System.Windows.MessageBox.Show(MainWindow,
            //        "Disabling the selected item will make it unusable and prevent it from showing up in searches.  Are you sure you wish to disable the selected item?",
            //        "Confirm",
            //        System.Windows.MessageBoxButton.YesNo,
            //        System.Windows.MessageBoxImage.Question,
            //        System.Windows.MessageBoxResult.No);
            //    if (mbResult == System.Windows.MessageBoxResult.Yes)
            //    {
            //        DisableItem((ViewModels.Security.AreaAcl)MasterDetailWindow.MasterView.SelectedItem);
            //    }
            //}, x => MasterDetailWindow.DisableEnabled);

            //MainWindow.SecurityAreas_Save.Command = new Commands.DelegateCommand(x =>
            //{
            //    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
            //    {
            //        UpdateItem((ViewModels.Security.AreaAcl)MasterDetailWindow.DetailView.DataContext);
            //    }
            //    else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
            //    {
            //        CreateItem((ViewModels.Security.AreaAcl)MasterDetailWindow.CreateView.DataContext);
            //    }
            //    else
            //        throw new Exception("Invalid UI state.");
            //}, x => MasterDetailWindow.SaveEnabled);

            //MainWindow.SecurityAreas_Cancel.Command = new Commands.DelegateCommand(x =>
            //{
            //    // Will need to reload the model from the server (easiest way)
            //    // This needs to be improved - if we want to keep loading from the server instead of 
            //    // doing a deep copy, then it needs to only load a single, not force the whole
            //    // tree to reload.
            //    App.Current.Dispatcher.Invoke(new Action(() =>
            //    {
            //        MasterDetailWindow.MasterView.ClearSelected();
            //        MasterDetailWindow.Clear();
            //    }));

            //    MasterDetailWindow.IsBusy = true;

            //    GetData<Common.Models.Security.AreaAcl>(data =>
            //    {
            //        List<Common.Models.Security.AreaAcl> sysModelList = (List<Common.Models.Security.AreaAcl>)data;
            //        if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
            //            UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
            //        else
            //            UpdateUI(null, sysModelList, null);

            //        App.Current.Dispatcher.Invoke(new Action(() =>
            //        {
            //            MasterDetailWindow.IsBusy = false;
            //        }));
            //    }, null);
            //}, x => MasterDetailWindow.CancelEnabled);

            //// load window
            //MasterDetailWindow.Load();
            //if (!MasterDetailWindow.IsSelected)
            //    MasterDetailWindow.SelectWindow();

            //MasterDetailWindow.IsBusy = true;

            //// update filter
            //App.Current.Dispatcher.Invoke(new Action(() =>
            //{
            //    userFilter = (ViewModels.Security.User)MainWindow.SecurityAreaAcls_List_User.SelectionBoxItem;
            //    areaFilter = (ViewModels.Security.Area)MainWindow.SecurityAreaAcls_List_Area.SelectionBoxItem;

            //    if (userFilter != null || areaFilter != null)
            //    {
            //        filter = new Common.Rest.Requests.Security.AreaAcl();
            //        if (userFilter != null)
            //            filter.UserId = userFilter.Id;
            //        if (areaFilter != null)
            //            filter.SecurityAreaId = areaFilter.Id;
            //    }
            //}));

            //// Ignores selection
            //GetData<Common.Models.Security.AreaAcl>(data =>
            //{
            //    List<Common.Models.Security.AreaAcl> sysModelList = (List<Common.Models.Security.AreaAcl>)data;
            //    UpdateUI(null, sysModelList, Controls.DisplayModeType.View);

            //    App.Current.Dispatcher.Invoke(new Action(() =>
            //    {
            //        MasterDetailWindow.IsBusy = false;
            //    }));
            //}, filter);
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete)
        {
            ViewModels.Security.AreaAcl castViewModel = (ViewModels.Security.AreaAcl)viewModel;

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            return PopulateCoreDetails<Common.Models.Security.AreaAcl>(castViewModel, () =>
            {
                MasterDetailWindow.DetailView.IsBusy = false;
                MasterDetailWindow.EditView.IsBusy = false;
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>> onComplete)
        {
            return ListItems<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.Area)request, onComplete);
        }


        //public override void GetData<TModel>(Action<object> onComplete, object obj)
        //{
        //    Common.Rest.Requests.Security.AreaAcl request;
        //    Type requestType = typeof(Common.Rest.Requests.Security.AreaAcl);
        //    Dictionary<string, PropertyInfo> requestPropertyDict = new Dictionary<string, PropertyInfo>();

        //    request = new Common.Rest.Requests.Security.AreaAcl()
        //    {
        //        AuthToken = Globals.Instance.AuthToken
        //    };

        //    if (obj != null)
        //    {
        //        foreach (PropertyInfo prop in requestType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        //        {
        //            requestPropertyDict.Add(prop.Name, prop);
        //        }

        //        foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        //        {
        //            if (!requestPropertyDict.ContainsKey(prop.Name))
        //                throw new ArgumentException("Property named '" + prop.Name + "' does not exist in the request object.");

        //            // Sets the value of the argument "obj.[property name]" as the value of "request.[property name]"
        //            requestPropertyDict[prop.Name].SetValue(request, prop.GetValue(obj, null), null);
        //        }
        //    }

        //    GetData(onComplete, request);
        //}

        //public override void GetData(Action<object> onComplete, ViewModels.IViewModel filter = null)
        //{
        //    ViewModels.Security.AreaAcl viewModel = null;
        //    Common.Rest.Requests.Security.AreaAcl request;

        //    request = new Common.Rest.Requests.Security.AreaAcl()
        //    {
        //        AuthToken = Globals.Instance.AuthToken
        //    };

        //    if (filter != null && filter.GetType() == typeof(ViewModels.Security.AreaAcl))
        //    {
        //        viewModel = (ViewModels.Security.AreaAcl)filter;
        //        if (viewModel != null)
        //        {
        //            if (viewModel.Area != null && viewModel.Area.Id.HasValue)
        //                request.SecurityAreaId = viewModel.Area.Id;
        //            if (viewModel.User != null && viewModel.User.Id.HasValue)
        //                request.UserId = viewModel.User.Id;
        //        }
        //    }

        //    GetData(onComplete, request);
        //}

        //private void GetData(Action<object> onComplete, Common.Rest.Requests.Security.AreaAcl request)
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        _consumer.GetList(request,
        //        result =>
        //        {
        //            // Put the last request updating here, while it could go outside the callback,
        //            // I am putting it in here to be certain we never have a race condition
        //            _lastRequest = result.Request;
        //            _lastRestSharpResponse = result.RestSharpResponse;

        //            List<Common.Models.Security.AreaAcl> sysModelList = new List<Common.Models.Security.AreaAcl>();

        //            if (!result.ListResponseContainer.WasSuccessful)
        //            {
        //                ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
        //                    new ErrorHandling.ActionableError()
        //                    {
        //                        Level = ErrorHandling.LevelType.Error,
        //                        Title = "Error",
        //                        SimpleMessage = "Failed to retrieve the list of security areas.  Would you like to retry?",
        //                        Message = "Error: " + result.ListResponseContainer.Error.Message,
        //                        Exception = result.ListResponseContainer.Error.Exception,
        //                        Source = "OpenLawOffice.WinClient.Controllers.Security.AreaAcl.GetData()",
        //                        Recover = (error, data, onFail) =>
        //                        {
        //                            GetData(onComplete, request);
        //                        },
        //                        Fail = (error, data) =>
        //                        {
        //                            if (onComplete != null) onComplete(sysModelList);
        //                        }
        //                    });
        //            }
        //            else
        //            {
        //                foreach (Common.Rest.Responses.Security.AreaAcl area in result.Response)
        //                {
        //                    sysModelList.Add(Mapper.Map<Common.Models.Security.AreaAcl>(area));
        //                }

        //                if (onComplete != null) onComplete(sysModelList);
        //            }
        //        });
        //    });
        //}

        //public override void UpdateUI(ViewModels.IViewModel viewModel, object data, Controls.DisplayModeType? displayMode)
        //{
        //    if (!typeof(ViewModels.Security.AreaAcl).IsAssignableFrom(viewModel.GetType()))
        //        throw new ArgumentException("Invalid ViewModel type.");
        //    if (!typeof(List<Common.Models.Security.AreaAcl>).IsAssignableFrom(data.GetType()))
        //        throw new ArgumentException("Invalid data type.");

        //    UpdateUI((ViewModels.Security.AreaAcl)viewModel,
        //        (List<Common.Models.Security.AreaAcl>)data, displayMode);
        //}

        //public void UpdateUI(ViewModels.Security.AreaAcl viewModel,
        //    List<Common.Models.Security.AreaAcl> sysModelList, Controls.DisplayModeType? displayMode = null)
        //{
        //    EnhancedObservableCollection<ViewModels.Security.AreaAcl> viewModels = new EnhancedObservableCollection<ViewModels.Security.AreaAcl>();
        //    App.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        foreach (Common.Models.Security.AreaAcl sysModel in sysModelList)
        //        {
        //            ViewModels.Security.AreaAcl childVM = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(sysModel);
        //            viewModels.Add(childVM);
        //        }

        //        MasterDetailWindow.Clear();
        //        MasterDetailWindow.UpdateMasterDataContext(viewModels);
        //        if (displayMode.HasValue)
        //            MasterDetailWindow.SetDisplayMode(displayMode.Value);
        //        MasterDetailWindow.UpdateCommandStates();
        //    }));
        //}
    }
}
