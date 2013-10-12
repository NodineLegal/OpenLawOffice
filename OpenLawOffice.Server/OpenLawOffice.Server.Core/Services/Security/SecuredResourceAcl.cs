using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class SecuredResourceAcl : ResourceBase<Common.Models.Security.SecuredResourceAcl, DBOs.Security.SecuredResourceAcl,
        Rest.Requests.Security.SecuredResourceAcl, Common.Rest.Responses.Security.SecuredResourceAcl>
    {
        public override List<DBOs.Security.SecuredResourceAcl> GetList(Rest.Requests.Security.SecuredResourceAcl request, IDbConnection db)
        {
            string filterClause = "";
            int userid = 0;
            Guid resid = Guid.Empty;

            if (request.SecuredResourceId.HasValue && request.SecuredResourceId.Value != Guid.Empty)
            {
                filterClause += " \"secured_resource_id\"=@SecuredResource AND";
                resid = request.SecuredResourceId.Value;
            }
            if (request.UserId.HasValue)
            {
                filterClause += " \"user_id\"=@User AND";
                userid = request.UserId.Value;
            }

            filterClause += " \"utc_disabled\" is null";

            return db.Query<DBOs.Security.SecuredResourceAcl>("SELECT * FROM \"secured_resource_acl\" WHERE" + filterClause,
                new { Area = resid, User = userid });
        }
    }
}
