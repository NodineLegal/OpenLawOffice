namespace OpenLawOffice.WinClient.ViewModels
{
    public static class Creator
    {
        public static TViewModel Create<TViewModel>(Common.Models.ModelBase model)
            where TViewModel : ViewModelBase
        {
            return ViewModelBase.Create<TViewModel>(model);
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
