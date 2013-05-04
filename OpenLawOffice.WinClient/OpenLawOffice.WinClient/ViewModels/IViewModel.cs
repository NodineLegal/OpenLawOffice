using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IViewModel
    {
        bool IsHierarchical { get; }
        StateType State { get; }
    }
}
