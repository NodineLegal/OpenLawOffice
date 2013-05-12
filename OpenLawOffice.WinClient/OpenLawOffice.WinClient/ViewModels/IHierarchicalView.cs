using System;
using DW.SharpTools;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IHierarchicalView<T> : IViewModel
    {
        bool IsDummy { get; set; }
        T Parent { get; set; }
        ObservableCollection<T> Children { get; set; }

        void AddChild(T child);
        void RemoveChild(T child);
    }
}
