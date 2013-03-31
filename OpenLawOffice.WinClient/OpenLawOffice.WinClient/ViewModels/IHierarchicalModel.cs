using System;
using DW.SharpTools;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IHierarchicalModel<T>
        where T : ModelBase<T>
    {
        bool IsDummy { get; set; }
        T Parent { get; set; }
        EnhancedObservableCollection<T> Children { get; set; }

        void AddChild(T child);
        void RemoveChild(T child);
    }
}
