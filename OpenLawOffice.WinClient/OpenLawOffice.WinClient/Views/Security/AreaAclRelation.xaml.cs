using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AutoMapper;

namespace OpenLawOffice.WinClient.Views.Security
{
    /// <summary>
    /// Interaction logic for AreaAclRelation.xaml
    /// </summary>
    public partial class AreaAclRelation : UserControl, IRelationView, Controls.IDetail
    {
        private ViewModels.Security.Area _viewModel;

        public AreaAclRelation()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public Action<AreaAclRelation> OnClose { get; set; }

        public Action<AreaAclRelation> OnEdit { get; set; }

        public Action<AreaAclRelation> OnView { get; set; }
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
            _viewModel = (ViewModels.Security.Area)obj;
        }

        public void Load()
        {
            Refresh();
        }

        public void Refresh()
        {
            ObservableCollection<ViewModels.Security.AreaAcl> results = new ObservableCollection<ViewModels.Security.AreaAcl>();
            Consumers.Security.AreaAcl consumer = new Consumers.Security.AreaAcl();
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            IsBusy = true;

            Consumers.ListConsumerResult<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
                consumerResults =
                consumer.GetList<Common.Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>(
                new Common.Rest.Requests.Security.AreaAcl()
                {
                    SecurityAreaId = _viewModel.Id
                });

            foreach (Common.Rest.Responses.Security.AreaAcl item in consumerResults.Response)
            {
                Common.Models.Security.AreaAcl sysModel = Mapper.Map<Common.Models.Security.AreaAcl>(item);

                Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                    userResponse =
                    userConsumer.GetSingle<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>(
                    new Common.Rest.Requests.Security.User()
                    {
                        Id = item.User.Id
                    });

                Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(userResponse.Response);

                sysModel.User = sysUser;

                ViewModels.Security.AreaAcl viewModel = ViewModels.Creator.Create<ViewModels.Security.AreaAcl>(sysModel);

                results.Add(viewModel);
            }

            UIList.ItemsSource = results;

            IsBusy = false;
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