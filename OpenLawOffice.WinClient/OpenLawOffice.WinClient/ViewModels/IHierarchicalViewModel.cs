using System;
using DW.SharpTools;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IHierarchicalViewModel<T> : IViewModel
        where T : IViewModel
    {
        T Parent { get; set; }
        ObservableCollection<T> Children { get; set; }

        void AddChild(T child);
        void RemoveChild(T child);
    }
}
