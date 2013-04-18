using System;

namespace OpenLawOffice.WinClient.Controls
{
    public interface IMaster
    {
        Action<IMaster, object> OnSelectionChanged { get; set; }
        object SelectedItem { get; }
        void ClearSelected();
    }
}
