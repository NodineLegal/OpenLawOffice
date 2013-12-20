using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using AutoMapper;

namespace OpenLawOffice.WinClient.Views.Matters
{
    /// <summary>
    /// Interaction logic for ResponsibleUserRelation.xaml
    /// </summary>
    public partial class ResponsibleUserRelation : UserControl, IRelationView, Controls.IDetail
    {
        public Action<ResponsibleUserRelation> OnClose { get; set; }
        public Action<ResponsibleUserRelation> OnEdit { get; set; }
        public Action<ResponsibleUserRelation> OnView { get; set; }
        public Action<ResponsibleUserRelation> OnAdd { get; set; }

        public ViewModels.Matters.Matter ViewModel { get { return _viewModel; } }
        private ViewModels.Matters.Matter _viewModel;

        public bool IsBusy
        {
            get { return UIBusyIndicator.IsBusy; }
            set { UIBusyIndicator.IsBusy = value; }
        }

        public ResponsibleUserRelation()
        {
            InitializeComponent();
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
            ObservableCollection<ViewModels.Matters.ResponsibleUser> results = new ObservableCollection<ViewModels.Matters.ResponsibleUser>();
            Consumers.Matters.ResponsibleUser consumer = new Consumers.Matters.ResponsibleUser();
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            IsBusy = true;

            Consumers.ListConsumerResult<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
                consumerResults =
                consumer.GetList<Common.Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>(
                new Common.Rest.Requests.Matters.ResponsibleUser()
                {
                    MatterId = _viewModel.Id
                });

            foreach (Common.Rest.Responses.Matters.ResponsibleUser item in consumerResults.Response)
            {
                Common.Models.Matters.ResponsibleUser sysModel = Mapper.Map<Common.Models.Matters.ResponsibleUser>(item);

                Consumers.ConsumerResult<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                    userResponse =
                    userConsumer.GetSingle<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>(
                    new Common.Rest.Requests.Security.User()
                    {
                        Id = item.User.Id
                    });

                Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(userResponse.Response);

                sysModel.User = sysUser;

                ViewModels.Matters.ResponsibleUser viewModel = ViewModels.Creator.Create<ViewModels.Matters.ResponsibleUser>(sysModel);

                results.Add(viewModel);
            }

            UIList.ItemsSource = results;

            IsBusy = false;
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

        public void ClearSelected()
        {
            UIList.UnselectAll();
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (OnAdd != null) OnAdd(this);
        }
    }
}
