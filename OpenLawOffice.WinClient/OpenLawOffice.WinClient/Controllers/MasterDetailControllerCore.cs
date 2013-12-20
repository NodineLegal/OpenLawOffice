using System;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class MasterDetailControllerCore<TMasterView, TDetailView, TEditView, TCreateView>
        : MasterDetailController<TMasterView, TDetailView, TEditView, TCreateView>
        where TMasterView : UserControl, Controls.IMaster, new()
        where TDetailView : UserControl, new()
        where TEditView : UserControl, new()
        where TCreateView : UserControl, new()
    {
        public MasterDetailControllerCore(string title, RibbonTab ribbonTab, RibbonToggleButton editButton,
            RibbonButton createButton, RibbonButton disableButton, RibbonButton saveButton, RibbonButton cancelButton)
            : base(title, ribbonTab, editButton, createButton, disableButton, saveButton, cancelButton)
        {
        }

        public virtual Task PopulateCoreDetails<TModel>(ViewModels.ViewModelCore<TModel> viewModel, Action onComplete)
            where TModel : Common.Models.Core, new()
        {            
            return GetCoreDetails<TModel>(viewModel, (creator, modifier, disabler) =>
            {
                PushCreatingUserToUI<TModel>(viewModel, creator);
                PushModifyingUserToUI<TModel>(viewModel, modifier);
                PushDisablingUserToUI<TModel>(viewModel, disabler);
                if (onComplete != null) onComplete();
            });
        }
    }
}