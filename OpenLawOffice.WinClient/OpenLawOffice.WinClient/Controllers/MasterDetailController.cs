using System;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class MasterDetailController<TMasterView, TDetailView, TEditView, TCreateView>
        : ControllerBase
        where TMasterView : UserControl, Controls.IMaster, new()
        where TDetailView : UserControl, new()
        where TEditView : UserControl, new()
        where TCreateView : UserControl, new()
    {
        public Controls.TypedMasterDetailWindow<TMasterView, TDetailView, TEditView, TCreateView> MasterDetailWindow { get; set; }

        public MasterDetailController(string title, RibbonTab ribbonTab, RibbonToggleButton editButton,
            RibbonButton createButton, RibbonButton disableButton, RibbonButton saveButton, RibbonButton cancelButton)
        {
            MasterDetailWindow = new Controls.TypedMasterDetailWindow<TMasterView, TDetailView, TEditView, TCreateView>
                (title, ribbonTab, editButton, createButton, disableButton, saveButton, cancelButton, this);
        }

        public override Task UpdateItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            MasterDetailWindow.IsBusy = true;

            return base.UpdateItem<TRequest, TResponse>(request, (result, error) =>
            {
                if (onComplete != null)
                    onComplete(result, error);

                MasterDetailWindow.MasterView.ClearSelected();

                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                    MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);

                MasterDetailWindow.IsBusy = false;
            });
        }

        public override Task CreateItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            MasterDetailWindow.IsBusy = true;

            return base.CreateItem<TRequest, TResponse>(request, (viewModels, error) =>
            {
                if (onComplete != null)
                    onComplete(viewModels, error);

                MasterDetailWindow.MasterView.ClearSelected();

                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                    MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);

                MasterDetailWindow.IsBusy = false;
            });
        }

        public override Task DisableItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            MasterDetailWindow.IsBusy = true;

            return base.DisableItem<TRequest, TResponse>(request, (result, error) =>
            {
                if (onComplete != null) 
                    onComplete(result, error);
                MasterDetailWindow.IsBusy = false;
            });
        }

        public override Task ListItems<TRequest, TResponse>(TRequest request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            MasterDetailWindow.IsBusy = true;

            return base.ListItems<TRequest, TResponse>(request, (viewModels, error) =>
            {
                if (onComplete != null) 
                    onComplete(viewModels, error);
                MasterDetailWindow.IsBusy = false;
            });
        }

        public override void SelectItem(ViewModels.IViewModel viewModel)
        {
            MasterDetailWindow.MasterView.SelectItem(viewModel);
        }

        public override void SetDisplayMode(Controls.DisplayModeType mode)
        {
            MasterDetailWindow.SetDisplayMode(mode);
        }

        public override void GoIntoCreateMode(object obj)
        {
            MasterDetailWindow.GoIntoCreateMode(obj);
        }
    }
}
