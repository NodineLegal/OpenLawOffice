using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.Controllers.Matters
{
    [Handle(typeof(Common.Models.Matters.ResponsibleUser))]
    public class ResponsibleUser
        : MasterDetailControllerCore<Controls.ListGridView, Views.Matters.ResponsibleUserDetail,
            Views.Matters.ResponsibleUserEdit, Views.Matters.ResponsibleUserCreate>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Matters.ResponsibleUser); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Matters.ResponsibleUser); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Matters.ResponsibleUser); } }
        public override Type ModelType { get { return typeof(Common.Models.Matters.ResponsibleUser); } }

        public ResponsibleUser()
            : base("Responsible Users",
            Globals.Instance.MainWindow.MatterResponsibleUsersTab,
            Globals.Instance.MainWindow.MatterResponsibleUsers_Edit,
            Globals.Instance.MainWindow.MatterResponsibleUsers_Create,
            Globals.Instance.MainWindow.MatterResponsibleUsers_Disable,
            Globals.Instance.MainWindow.MatterResponsibleUsers_Save,
            Globals.Instance.MainWindow.MatterResponsibleUsers_Cancel)
        {
            _consumer = new Consumers.Matters.ResponsibleUser();

            MasterDetailWindow.MasterView
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
                    Header = "Matter Title",
                    DisplayMemberBinding = new System.Windows.Data.Binding("Matter.Title")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });
        }

        private ViewModels.Matters.ResponsibleUser BuildFilter()
        {
            ViewModels.Matters.ResponsibleUser filter = ViewModels.Creator.Create<ViewModels.Matters.ResponsibleUser>(
                new Common.Models.Matters.ResponsibleUser());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ViewModels.Security.User userFilter = null;
                ViewModels.Matters.Matter matterFilter = ViewModels.Creator.Create<ViewModels.Matters.Matter>(new Common.Models.Matters.Matter());

                if (MainWindow.MatterResponsibleUsers_List_User.SelectedItem != null)
                    userFilter = (ViewModels.Security.User)MainWindow.MatterResponsibleUsers_List_User.SelectedItem;

                string titleFilter = MainWindow.MatterResponsibleUsers_List_User.Text.Trim();
                if (!string.IsNullOrEmpty(titleFilter))
                    matterFilter.Title = titleFilter;

                if (userFilter != null || matterFilter != null)
                {
                    if (userFilter != null)
                        filter.User = userFilter;
                    if (matterFilter != null)
                        filter.Matter = matterFilter;
                }
            }));

            return filter;
        }

        public Task LoadFilterOptions()
        {
            return Task.Factory.StartNew(() =>
            {
                Task getUsersTask;

                Consumers.ListConsumerResult<Common.Rest.Responses.Security.User> userOptions = null;

                Consumers.Security.User userConsumer = new Consumers.Security.User();

                getUsersTask = Task.Factory.StartNew(() =>
                {
                    userOptions = userConsumer.GetList<Common.Rest.Requests.Security.User,
                        Common.Rest.Responses.Security.User>(
                        new Common.Rest.Requests.Security.User()
                        {
                            AuthToken = Globals.Instance.AuthToken
                        });
                });

                Task.WaitAll(new Task[] { getUsersTask });

                List<ViewModels.Security.User> userList = new List<ViewModels.Security.User>();
                
                foreach (Common.Rest.Responses.Security.User userResponse in userOptions.Response)
                {
                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(userResponse);
                    ViewModels.Security.User vmUser = ViewModels.Creator.Create<ViewModels.Security.User>(sysUser);
                    userList.Add(vmUser);
                }

                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MainWindow.SecurityAreaAcls_List_User.ItemsSource = userList;
                    MainWindow.SecurityAreaAcls_List_User.DisplayMemberPath = "Username";
                }));
            });
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            MainWindow.MatterResponsibleUsers_ClearUser.Click += (sender, e) =>
            {
                MainWindow.MatterResponsibleUsers_List_User.SelectedIndex = -1;
            };

            // ribbon controls
            MainWindow.MatterResponsibleUsers_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    foreach (ViewModels.Matters.ResponsibleUser result in results)
                        LoadMatterAndUser(result);

                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.MatterResponsibleUsers_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Matters.ResponsibleUser viewModel = ViewModels.Creator.Create<ViewModels.Matters.ResponsibleUser>(new Common.Models.Matters.ResponsibleUser());
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.MatterResponsibleUsers_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.MatterResponsibleUsers_Disable.Command = new Commands.DelegateCommand(x =>
            {
                System.Windows.MessageBoxResult mbResult = System.Windows.MessageBox.Show(MainWindow,
                    "Disabling the selected item will make it unusable and prevent it from showing up in searches.  Are you sure you wish to disable the selected item?",
                    "Confirm",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question,
                    System.Windows.MessageBoxResult.No);
                if (mbResult == System.Windows.MessageBoxResult.Yes)
                {
                    DisableItem((ViewModels.Matters.ResponsibleUser)MasterDetailWindow.MasterView.SelectedItem, null);
                }
            }, x => MasterDetailWindow.DisableEnabled);

            MainWindow.MatterResponsibleUsers_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Matters.ResponsibleUser)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Matters.ResponsibleUser)MasterDetailWindow.CreateView.DataContext, null);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.MatterResponsibleUsers_Cancel.Command = new Commands.DelegateCommand(x =>
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

            _consumer = new Consumers.Matters.ResponsibleUser();

            // Load filter options
            LoadFilterOptions().ContinueWith(task =>
            {
                // load window
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (WindowManager.Instance.Lookup(MasterDetailWindow) == null)
                        MasterDetailWindow.Load();
                    if (!MasterDetailWindow.IsSelected)
                        MasterDetailWindow.SelectWindow();

                    viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                    LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                    {
                        MasterDetailWindow.MasterDataContext = results;

                        foreach (ViewModels.Matters.ResponsibleUser viewModel in results)
                        {
                            LoadMatterAndUser(viewModel);
                        }
                        if (selected != null) SelectItem(selected);
                        if (callback != null) callback();
                    });
                }));
            });
        }

        public void LoadMatterAndUser(ViewModels.Matters.ResponsibleUser viewModel)
        {
            Consumers.ConsumerResult<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter> matterResult = null;
            Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User> userResult = null;

            Consumers.Matters.Matter matterConsumer = new Consumers.Matters.Matter();
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            Common.Rest.Requests.Matters.Matter matterRequest = Mapper.Map<Common.Rest.Requests.Matters.Matter>(
                new Common.Models.Matters.Matter() { Id = viewModel.Matter.Id });

            Common.Rest.Requests.Security.User userRequest = Mapper.Map<Common.Rest.Requests.Security.User>(
                new Common.Models.Security.User() { Id = viewModel.User.Id });

            matterResult = matterConsumer.GetSingle(matterRequest);
            userResult = userConsumer.GetSingle(userRequest);

            Common.Models.Matters.Matter ruModel = Mapper.Map<Common.Models.Matters.Matter>(matterResult.Response);
            Common.Models.Security.User userModel = Mapper.Map<Common.Models.Security.User>(userResult.Response);

            viewModel.Matter = ViewModels.Creator.Create<ViewModels.Matters.Matter>(ruModel);
            viewModel.User = ViewModels.Creator.Create<ViewModels.Security.User>(userModel);
        }
        
        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            ViewModels.Matters.ResponsibleUser castViewModel = (ViewModels.Matters.ResponsibleUser)viewModel;

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            return PopulateCoreDetails<Common.Models.Matters.ResponsibleUser>(castViewModel, () =>
            {
                MasterDetailWindow.DetailView.IsBusy = false;
                MasterDetailWindow.EditView.IsBusy = false;
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
                ((Common.Rest.Requests.Matters.ResponsibleUser)request, onComplete);
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
                ((Common.Rest.Requests.Matters.ResponsibleUser)request, onComplete);
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
                ((Common.Rest.Requests.Matters.ResponsibleUser)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return ListItems<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
                ((Common.Rest.Requests.Matters.ResponsibleUser)request, onComplete);
        }
    }
}
