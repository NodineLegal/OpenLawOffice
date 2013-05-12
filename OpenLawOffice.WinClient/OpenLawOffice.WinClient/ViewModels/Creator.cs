using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public static class Creator
    {
        public static TViewModel Create<TViewModel>(Common.Models.ModelBase model)
            where TViewModel : ViewModelBase
        {
            return ViewModelBase.Create<TViewModel>(model);
        }

        public static IViewModel Create(Common.Models.ModelBase model, Type viewModelType)
        {
            return ViewModelBase.Create(model, viewModelType);
        }

        public static IViewModel Create(Type viewModelType)
        {
            return ViewModelBase.Create(viewModelType);
        }

        public static TViewModel CreateDummy<TViewModel>(Common.Models.ModelBase model)
            where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = ViewModelBase.Create<TViewModel>(model);
            viewModel.IsDummy = true;
            return (TViewModel)viewModel;
        }
    }
}
