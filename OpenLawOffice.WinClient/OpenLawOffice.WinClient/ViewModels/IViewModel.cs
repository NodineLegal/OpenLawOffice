using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public interface IViewModel
    {
        void AttachModel(Common.Models.ModelBase model);
    }
}
