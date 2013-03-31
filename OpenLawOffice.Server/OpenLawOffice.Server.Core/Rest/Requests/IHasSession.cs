using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenLawOffice.Server.Core.Rest.Requests
{
    public interface IHasSession
    {
        Core.Security.Session Session { get; set; }
    }
}
