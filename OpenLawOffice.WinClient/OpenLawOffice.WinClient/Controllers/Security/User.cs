﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.User))]
    public class User
        : MasterDetailController<Controls.ListGridView, Views.Security.UserDetail,
            Views.Security.UserEdit, Views.Security.UserCreate>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Security.User); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Security.User); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Security.User); } }
        public override Type ModelType { get { return typeof(Common.Models.Security.User); } }

        public User()
            : base("Users",
            Globals.Instance.MainWindow.SecurityUserTab,
            Globals.Instance.MainWindow.SecurityUsers_Edit,
            Globals.Instance.MainWindow.SecurityUsers_Create,
            Globals.Instance.MainWindow.SecurityUsers_Disable,
            Globals.Instance.MainWindow.SecurityUsers_Save,
            Globals.Instance.MainWindow.SecurityUsers_Cancel)
        {
            _consumer = new Consumers.Security.User();

            MasterDetailWindow.MasterView.AddColumn(new System.Windows.Controls.GridViewColumn()
            {
                Header = "Username",
                DisplayMemberBinding = new System.Windows.Data.Binding("Username")
                {
                    Mode = System.Windows.Data.BindingMode.TwoWay
                },
                Width = 200
            });
        }

        private ViewModels.Security.User BuildFilter()
        {
            ViewModels.Security.User filter = ViewModels.Creator.Create<ViewModels.Security.User>(new Common.Models.Security.User());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                string nameFilter = MainWindow.SecurityUsers_List_Name.Text.Trim();
                if (!string.IsNullOrEmpty(nameFilter))
                    filter.Username = nameFilter;
            }));

            return filter;
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            // ribbon controls
            MainWindow.SecurityUsers_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.SecurityUsers_Acls.Command = new Commands.DelegateCommand(x =>
            {
                MasterDetailWindow.ShowRelationView<Common.Models.Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.SecurityUsers_Acls.Command);

            MainWindow.SecurityUsers_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Security.User viewModel = ViewModels.Creator.Create<ViewModels.Security.User>(new Common.Models.Security.User());
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.SecurityUsers_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.SecurityUsers_Disable.Command = new Commands.DelegateCommand(x =>
            {
                System.Windows.MessageBoxResult mbResult = System.Windows.MessageBox.Show(MainWindow,
                    "Disabling the selected item will make it unusable and prevent it from showing up in searches.  Are you sure you wish to disable the selected item?",
                    "Confirm",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question,
                    System.Windows.MessageBoxResult.No);
                if (mbResult == System.Windows.MessageBoxResult.Yes)
                {
                    DisableItem((ViewModels.Security.User)MasterDetailWindow.MasterView.SelectedItem, null);
                }
            }, x => MasterDetailWindow.DisableEnabled);

            MainWindow.SecurityUsers_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Security.User)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Security.User)MasterDetailWindow.CreateView.DataContext, null);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.SecurityUsers_Cancel.Command = new Commands.DelegateCommand(x =>
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

            // load window
            MasterDetailWindow.Load();
            if (!MasterDetailWindow.IsSelected)
                MasterDetailWindow.SelectWindow();

            viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

            LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
            {
                MasterDetailWindow.MasterDataContext = results;
                if (selected != null) SelectItem(selected);
                if (callback != null) callback();
            });
        }

        public override Task LoadItems(ViewModels.IViewModel filter, ICollection<ViewModels.IViewModel> collection, Action<ICollection<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return base.LoadItems(filter, collection, (results, error) =>
            {
                if (onComplete != null)
                    onComplete(results, error);
            });
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            ViewModels.Security.User castViewModel = (ViewModels.Security.User)viewModel;

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            return Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.DetailView.IsBusy = false;
                    MasterDetailWindow.EditView.IsBusy = false;
                }));
            });
        }

        //public override Task UpdateItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        //{
        //    return base.UpdateItem<TRequest, TResponse>(request, onComplete);
        //}

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                ((Common.Rest.Requests.Security.User)request, onComplete);
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                ((Common.Rest.Requests.Security.User)request, onComplete);
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                ((Common.Rest.Requests.Security.User)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return ListItems<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                ((Common.Rest.Requests.Security.User)request, onComplete);
        }
    }
}
