using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IViewModel
    {
        bool IsDummy { get; set; }
        bool IsHierarchical { get; }
        StateType State { get; }

        void UpdateModel();
        void Bind(Common.Models.ModelBase model);
        TModel Export<TModel>();
        Common.Models.ModelBase GetModel();
        T GetModel<T>()
            where T : Common.Models.ModelBase;
    }
}
