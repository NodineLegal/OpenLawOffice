using System;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;

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
    }
}
