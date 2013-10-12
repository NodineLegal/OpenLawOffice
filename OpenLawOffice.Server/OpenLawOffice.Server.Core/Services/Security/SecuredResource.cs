using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class SecuredResource
        : ResourceBase<Common.Models.Security.SecuredResource, DBOs.Security.SecuredResource,
            Rest.Requests.Security.SecuredResource, Common.Rest.Responses.Security.SecuredResource>
    {
        public override List<DBOs.Security.SecuredResource> GetList(Rest.Requests.Security.SecuredResource request, System.Data.IDbConnection db)
        {
            string filterClause = " \"utc_disabled\" is null";
            return db.Query<DBOs.Security.SecuredResource>("SELECT * FROM \"secured_resource\" WHERE" + filterClause,
                new { });
        }
    }
}
