using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AutoMapper;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for SecuredResourceAclRelation.xaml
    /// </summary>
    public partial class SecuredResourceAclRelation : UserControl, IRelationView, Controls.IDetail
    {
        private ViewModels.Matters.Matter _viewModel;

        public SecuredResourceAclRelation()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public Action<SecuredResourceAclRelation> OnAdd { get; set; }

        public Action<SecuredResourceAclRelation> OnClose { get; set; }

        public Action<SecuredResourceAclRelation> OnEdit { get; set; }

        public Action<SecuredResourceAclRelation> OnView { get; set; }

        public ViewModels.Matters.Matter ViewModel { get { return _viewModel; } }

        public void ClearSelected()
        {
            UIList.UnselectAll();
        }

        public object GetSelectedItem()
        {
            return UIList.SelectedItem;
        }

        public TViewModel GetSelectedItem<TViewModel>()
            where TViewModel : ViewModels.ViewModelBase
        {
            return (TViewModel)GetSelectedItem();
        }

        public void Initialize(object obj)
        {
            _viewModel = (ViewModels.Matters.Matter)obj;
        }

        public void Load()
        {
            Refresh();
        }

        public void Refresh()
        {
            ObservableCollection<ViewModels.Security.SecuredResourceAcl> results = new ObservableCollection<ViewModels.Security.SecuredResourceAcl>();
            Consumers.Security.SecuredResourceAcl consumer = new Consumers.Security.SecuredResourceAcl();
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            IsBusy = true;

            Consumers.ListConsumerResult<Common.Rest.Requests.Security.SecuredResourceAcl, Common.Rest.Responses.Security.SecuredResourceAcl>
                consumerResults =
                consumer.GetList<Common.Rest.Requests.Security.SecuredResourceAcl, Common.Rest.Responses.Security.SecuredResourceAcl>(
                new Common.Rest.Requests.Security.SecuredResourceAcl()
                {
                    SecuredResourceId = _viewModel.Id
                });

            foreach (Common.Rest.Responses.Security.SecuredResourceAcl item in consumerResults.Response)
            {
                Common.Models.Security.SecuredResourceAcl sysModel = Mapper.Map<Common.Models.Security.SecuredResourceAcl>(item);

                Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                    userResponse =
                    userConsumer.GetSingle<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>(
                    new Common.Rest.Requests.Security.User()
                    {
                        Id = item.User.Id
                    });

                Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(userResponse.Response);

                sysModel.User = sysUser;

                ViewModels.Security.SecuredResourceAcl viewModel = ViewModels.Creator.Create<ViewModels.Security.SecuredResourceAcl>(sysModel);

                results.Add(viewModel);
            }

            UIList.ItemsSource = results;

            IsBusy = false;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (OnAdd != null) OnAdd(this);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (OnClose != null) OnClose(this);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit != null) OnEdit(this);
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (OnView != null) OnView(this);
        }
    }
}