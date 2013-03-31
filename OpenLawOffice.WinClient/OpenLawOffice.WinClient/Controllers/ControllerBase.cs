using System;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class ControllerBase
    {
        public abstract void LoadUI();
        public abstract void GetData(ViewModels.IViewModel obj);
    }
}
