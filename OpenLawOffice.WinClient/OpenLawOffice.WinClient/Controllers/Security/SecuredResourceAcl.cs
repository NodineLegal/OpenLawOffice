// -----------------------------------------------------------------------
// <copyright file="SecuredResourceAcl.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.WinClient.Controllers.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Handle(typeof(Common.Models.Security.SecuredResourceAcl))]
    public class SecuredResourceAcl
        : MasterDetailControllerCore<Controls.ListGridView, Views.Security.SecuredResourceAclDetail,
            Views.Security.SecuredResourceAclEdit, Views.Security.SecuredResourceAclCreate>
    {
        public SecuredResourceAcl()
            : base("Resource Permissions",
            Globals.Instance.MainWindow.SecuredResourceAclsTab,
            Globals.Instance.MainWindow.SecuredResourceAcls_Edit,
            Globals.Instance.MainWindow.SecuredResourceAcls_Create,
            null,
            Globals.Instance.MainWindow.SecuredResourceAcls_Save,
            Globals.Instance.MainWindow.SecuredResourceAcls_Cancel)
        {
            _consumer = new Consumers.Security.AreaAcl();

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

        public override Type ModelType { get { return typeof(Common.Models.Security.SecuredResourceAcl); } }

        public override Type RequestType { get { return typeof(Common.Rest.Requests.Security.SecuredResourceAcl); } }

        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Security.SecuredResourceAcl); } }

        public override Type ViewModelType { get { return typeof(ViewModels.Security.SecuredResourceAcl); } }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Security.SecuredResourceAcl,
                Common.Rest.Responses.Security.SecuredResourceAcl>
                ((Common.Rest.Requests.Security.SecuredResourceAcl)request, (result, error) =>
                {
                    if (error != null) return;
                    ViewModels.Security.SecuredResourceAcl castedResult = (ViewModels.Security.SecuredResourceAcl)result;
                    ICollection<ViewModels.IViewModel> collection = (ICollection<ViewModels.IViewModel>)MasterDetailWindow.MasterDataContext;
                    LoadUser(castedResult);
                    collection.Add(castedResult);
                    if (onComplete != null)
                        onComplete(castedResult, error);
                });
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Security.SecuredResourceAcl,
                Common.Rest.Responses.Security.SecuredResourceAcl>
                ((Common.Rest.Requests.Security.SecuredResourceAcl)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return ListItems<Common.Rest.Requests.Security.SecuredResourceAcl,
                Common.Rest.Responses.Security.SecuredResourceAcl>
                ((Common.Rest.Requests.Security.SecuredResourceAcl)request, onComplete);
        }

        public void LoadUser(ViewModels.Security.SecuredResourceAcl viewModel)
        {
            Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User> userResult = null;

            Consumers.Security.User userConsumer = new Consumers.Security.User();

            Common.Rest.Requests.Security.User userRequest = Mapper.Map<Common.Rest.Requests.Security.User>(
                new Common.Models.Security.User() { Id = viewModel.User.Id });

            userResult = userConsumer.GetSingle(userRequest);

            Common.Models.Security.User userModel = Mapper.Map<Common.Models.Security.User>(userResult.Response);

            viewModel.User = ViewModels.Creator.Create<ViewModels.Security.User>(userModel);
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            ViewModels.Security.SecuredResourceAcl castViewModel = (ViewModels.Security.SecuredResourceAcl)viewModel;

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            return PopulateCoreDetails<Common.Models.Security.SecuredResourceAcl>(castViewModel, () =>
            {
                MasterDetailWindow.DetailView.IsBusy = false;
                MasterDetailWindow.EditView.IsBusy = false;
            });
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
                    MainWindow.SecuredResourceAcls_List_User.ItemsSource = userList;
                    MainWindow.SecuredResourceAcls_List_User.DisplayMemberPath = "Username";
                }));
            });
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            MainWindow.SecuredResourceAcls_ClearUser.Click += (sender, e) =>
            {
                MainWindow.SecuredResourceAcls_List_User.SelectedIndex = -1;
            };

            // ribbon controls
            MainWindow.SecuredResourceAcls_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    foreach (ViewModels.Security.SecuredResourceAcl result in results)
                        LoadUser(result);

                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.SecuredResourceAcls_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Security.SecuredResourceAcl viewModel = 
                        ViewModels.Creator.Create<ViewModels.Security.SecuredResourceAcl>(new Common.Models.Security.SecuredResourceAcl());
                    viewModel.AllowFlags = Common.Models.PermissionType.None;
                    viewModel.DenyFlags = Common.Models.PermissionType.None;
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.SecuredResourceAcls_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.SecuredResourceAcls_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Security.SecuredResourceAcl)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Security.SecuredResourceAcl)MasterDetailWindow.CreateView.DataContext, null);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.SecuredResourceAcls_Cancel.Command = new Commands.DelegateCommand(x =>
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

            _consumer = new Consumers.Security.SecuredResourceAcl();

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

                        foreach (ViewModels.Security.SecuredResourceAcl viewModel in results)
                        {
                            LoadUser(viewModel);
                        }
                        if (selected != null) SelectItem(selected);
                        if (callback != null) callback();
                    });
                }));
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Security.SecuredResourceAcl,
                Common.Rest.Responses.Security.SecuredResourceAcl>
                ((Common.Rest.Requests.Security.SecuredResourceAcl)request, onComplete);
        }

        private ViewModels.Security.SecuredResourceAcl BuildFilter()
        {
            ViewModels.Security.SecuredResourceAcl filter = 
                ViewModels.Creator.Create<ViewModels.Security.SecuredResourceAcl>(
                new Common.Models.Security.SecuredResourceAcl());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                ViewModels.Security.User userFilter = null;

                if (MainWindow.SecurityAreaAcls_List_User.SelectedItem != null)
                    userFilter = (ViewModels.Security.User)MainWindow.SecurityAreaAcls_List_User.SelectedItem;

                if (userFilter != null)
                {
                    if (userFilter != null)
                        filter.User = userFilter;
                }
            }));

            return filter;
        }

    }
}
