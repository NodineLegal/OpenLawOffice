using System;

namespace OpenLawOffice.WinClient.Controllers
{
    public class HandleAttribute : Attribute
    {
        public Type ModelType { get; set; }

        public HandleAttribute(Type model)
        {
            ModelType = model;
        }
    }
}
