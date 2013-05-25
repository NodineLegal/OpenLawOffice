using System;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controls
{
    public interface IMaster
    {
        Func<ViewModels.IViewModel, ViewModels.IViewModel> GetItemDetails { get; set; }
        Func<ViewModels.IViewModel, List<ViewModels.IViewModel>> GetItemChildren { get; set; }
        Action<IMaster, object> OnSelectionChanged { get; set; }
        object SelectedItem { get; }
        void ClearSelected();
        void SelectItem(object obj);
    }
}
