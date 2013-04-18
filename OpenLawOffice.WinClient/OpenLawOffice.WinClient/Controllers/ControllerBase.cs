using System;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class ControllerBase
    {
        public MainWindow MainWindow = Globals.Instance.MainWindow;

        public abstract void LoadUI();
        public abstract void GetData<TModel>(Action<object> onComplete, object obj)
            where TModel : Common.Models.ModelBase;
        public abstract void GetData(Action<object> onComplete, ViewModels.IViewModel obj);
        public abstract void GetDetailData(Action onComplete, ViewModels.IViewModel viewModel);
        public abstract void UpdateUI(ViewModels.IViewModel viewModel, object data, Controls.DisplayModeType? displayMode = null);
    }
}
