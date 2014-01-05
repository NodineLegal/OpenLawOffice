namespace OpenLawOffice.WinClient.Controllers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using AutoMapper;

    [Handle(typeof(Common.Models.Security.AreaAcl))]
    public class AreaAcl
        : MasterDetailControllerCore<Controls.ListGridView, Views.Security.AreaAclDetail,
            Views.Security.AreaAclEdit, Views.Security.AreaAclCreate>
    {
        public AreaAcl()
            : base("Area ACLs",
            Globals.Instance.MainWindow.SecurityAreaAclTab,
            Globals.Instance.MainWindow.SecurityAreaAcls_Edit,
            Globals.Instance.MainWindow.SecurityAreaAcls_Create,
            null,
            Globals.Instance.MainWindow.SecurityAreaAcls_Save,
            Globals.Instance.MainWindow.SecurityAreaAcls_Cancel)
        {
            _consumer = new Consumers.Security.AreaAcl();

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
                })
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "Allow Flags",
                    DisplayMemberBinding = new System.Windows.Data.Binding("AllowFlags")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                })
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "Deny Flags",
                    DisplayMemberBinding = new System.Windows.Data.Binding("DenyFlags")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });
        }

        public override Type ModelType { get { return typeof(Common.Models.Security.AreaAcl); } }

        public override Type RequestType { get { return typeof(Common.Rest.Requests.Security.AreaAcl); } }

        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Security.AreaAcl); } }

        public override Type ViewModelType { get { return typeof(ViewModels.Security.AreaAcl); } }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, (result, error) =>
            {
                if (error != null) return;
                ViewModels.Security.AreaAcl castedResult = (ViewModels.Security.AreaAcl)result;
                ICollection<ViewModels.IViewModel> collection = (ICollection<ViewModels.IViewModel>)MasterDetailWindow.MasterDataContext;
                LoadAreaAndUser(castedResult);
                collection.Add(castedResult);
                if (onComplete != null)
                    onComplete(castedResult, error);
            });
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return ListItems<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        public void LoadAreaAndUser(ViewModels.Security.AreaAcl viewModel)
        {
            Consumers.ConsumerResult<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area> areaResult = null;
            Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User> userResult = null;

            Consumers.Security.Area areaConsumer = new Consumers.Security.Area();
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            Common.Rest.Requests.Security.Area areaRequest = Mapper.Map<Common.Rest.Requests.Security.Area>(
                new Common.Models.Security.Area() { Id = viewModel.Area.Id });

            Common.Rest.Requests.Security.User userRequest = Mapper.Map<Common.Rest.Requests.Security.User>(
                new Common.Models.Security.User() { Id = viewModel.User.Id });

            areaResult = areaConsumer.GetSingle(areaRequest);
            userResult = userConsumer.GetSingle(userRequest);

            Common.Models.Security.Area areaModel = Mapper.Map<Common.Models.Security.Area>(areaResult.Response);
            Common.Models.Security.User userModel = Mapper.Map<Common.Models.Security.User>(userResult.Response);

            viewModel.Area = ViewModels.Creator.Create<ViewModels.Security.Area>(areaModel);
            viewModel.User = ViewModels.Creator.Create<ViewModels.Security.User>(userModel);
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
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

        public Task LoadFilterOptions()
        {
            return Task.Factory.StartNew(() =>
            {
                Task getAreasTask, getUsersTask;

                Consumers.ListConsumerResult<Common.Rest.Responses.Security.Area> areaOptions = null;
                Consumers.ListConsumerResult<Common.Rest.Responses.Security.User> userOptions = null;

                Consumers.Security.Area areaConsumer = new Consumers.Security.Area();
                Consumers.Security.User userConsumer = new Consumers.Security.User();

                getAreasTask = Task.Factory.StartNew(() =>
                {
                    areaOptions = areaConsumer.GetList<Common.Rest.Requests.Security.Area,
                        Common.Rest.Responses.Security.Area>(
                        new Common.Rest.Requests.Security.Area()
                        {
                            AuthToken = Globals.Instance.AuthToken,
                            ShowAll = true
                        });
                });

                getUsersTask = Task.Factory.StartNew(() =>
                {
                    userOptions = userConsumer.GetList<Common.Rest.Requests.Security.User,
                        Common.Rest.Responses.Security.User>(
                        new Common.Rest.Requests.Security.User()
                        {
                            AuthToken = Globals.Instance.AuthToken
                        });
                });

                Task.WaitAll(new Task[] { getAreasTask, getUsersTask });

                List<ViewModels.Security.Area> areaList = new List<ViewModels.Security.Area>();
                List<ViewModels.Security.User> userList = new List<ViewModels.Security.User>();

                foreach (Common.Rest.Responses.Security.Area areaResponse in areaOptions.Response)
                {
                    Common.Models.Security.Area sysArea = Mapper.Map<Common.Models.Security.Area>(areaResponse);
                    ViewModels.Security.Area vmArea = ViewModels.Creator.Create<ViewModels.Security.Area>(sysArea);
                    areaList.Add(vmArea);
                }

                foreach (Common.Rest.Responses.Security.User userResponse in userOptions.Response)
                {
                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(userResponse);
                    ViewModels.Security.User vmUser = ViewModels.Creator.Create<ViewModels.Security.User>(sysUser);
                    userList.Add(vmUser);
                }

                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MainWindow.SecurityAreaAcls_List_Area.ItemsSource = areaList;
                    MainWindow.SecurityAreaAcls_List_Area.DisplayMemberPath = "Name";

                    MainWindow.SecurityAreaAcls_List_User.ItemsSource = userList;
                    MainWindow.SecurityAreaAcls_List_User.DisplayMemberPath = "Username";
                }));
            });
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            MainWindow.SecurityAreaAcls_ClearArea.Click += (sender, e) =>
            {
                MainWindow.SecurityAreaAcls_List_Area.SelectedIndex = -1;
            };

            MainWindow.SecurityAreaAcls_ClearUser.Click += (sender, e) =>
            {
                MainWindow.SecurityAreaAcls_List_User.SelectedIndex = -1;
            };

            // ribbon controls
            MainWindow.SecurityAreaAcls_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    foreach (ViewModels.Security.AreaAcl result in results)
                        LoadAreaAndUser(result);

                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.SecurityAreaAcls_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Security.AreaAcl viewModel = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(new Common.Models.Security.AreaAcl());
                    viewModel.AllowFlags = Common.Models.PermissionType.None;
                    viewModel.DenyFlags = Common.Models.PermissionType.None;
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.SecurityAreaAcls_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.SecurityAreaAcls_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Security.AreaAcl)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Security.AreaAcl)MasterDetailWindow.CreateView.DataContext, null);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.SecurityAreaAcls_Cancel.Command = new Commands.DelegateCommand(x =>
            {
                // Will need to reload the model from the server (easiest way)
                // This needs to be improved - if we want to keep loading from the server instead of
                // doing a deep copy, then it needs to only load a single, not force the whole
                // tree to reload.
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.MasterView.ClearSelected();
                    MasterDetailWindow.Clear();
                }));

                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                        MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);
                });
            }, x => MasterDetailWindow.CancelEnabled);

            _consumer = new Consumers.Security.AreaAcl();

            // Load filter options
            LoadFilterOptions().ContinueWith(task =>
            {
                // load window
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.Load();
                    if (!MasterDetailWindow.IsSelected)
                        MasterDetailWindow.SelectWindow();

                    viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                    LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                    {
                        MasterDetailWindow.MasterDataContext = results;

                        foreach (ViewModels.Security.AreaAcl viewModel in results)
                        {
                            LoadAreaAndUser(viewModel);
                        }
                        if (selected != null) SelectItem(selected);
                        if (callback != null) callback();
                    });
                }));
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                ((Common.Rest.Requests.Security.AreaAcl)request, onComplete);
        }

        private ViewModels.Security.AreaAcl BuildFilter()
        {
            ViewModels.Security.AreaAcl filter = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(
                new Common.Models.Security.AreaAcl());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ViewModels.Security.User userFilter = null;
                ViewModels.Security.Area areaFilter = null;

                if (MainWindow.SecurityAreaAcls_List_User.SelectedItem != null)
                    userFilter = (ViewModels.Security.User)MainWindow.SecurityAreaAcls_List_User.SelectedItem;
                if (MainWindow.SecurityAreaAcls_List_Area.SelectedItem != null)
                    areaFilter = (ViewModels.Security.Area)MainWindow.SecurityAreaAcls_List_Area.SelectedItem;

                if (userFilter != null || areaFilter != null)
                {
                    if (userFilter != null)
                        filter.User = userFilter;
                    if (areaFilter != null)
                        filter.Area = areaFilter;
                }
            }));

            return filter;
        }
    }
}