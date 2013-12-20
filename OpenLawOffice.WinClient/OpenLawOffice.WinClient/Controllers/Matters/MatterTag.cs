using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenLawOffice.WinClient.Controllers.Matters
{
    [Handle(typeof(Common.Models.Matters.MatterTag))]
    public class MatterTag
        : MasterDetailControllerCore<Controls.NullView, Views.NullView,
            Views.NullView, Views.NullView>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Matters.MatterTag); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Matters.MatterTag); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Matters.MatterTag); } }
        public override Type ModelType { get { return typeof(Common.Models.Matters.MatterTag); } }

        public MatterTag()
            : base("Matter Tags",
            null,
            null,
            null,
            null,
            null,
            null)
        {
            _consumer = new Consumers.Matters.Matter();
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            if (callback != null) callback();
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
            }));
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
            }));
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
            }));
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
            }));
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
            }));
        }
    }
}
