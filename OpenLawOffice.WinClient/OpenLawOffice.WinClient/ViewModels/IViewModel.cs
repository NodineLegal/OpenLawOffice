using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IViewModel
    {
        bool IsHierarchical { get; }
        IViewModel AttachModel(Common.Models.ModelBase model);
        void Synchronize(Action a);
    }
}
