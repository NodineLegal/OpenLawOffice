using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class AreaAcl : ResourceBase<Common.Models.Security.AreaAcl, DBOs.Security.AreaAcl,
        Rest.Requests.Security.AreaAcl, Common.Rest.Responses.Security.AreaAcl>
    {
        public override List<DBOs.Security.AreaAcl> GetList(Rest.Requests.Security.AreaAcl request, IDbConnection db)
        {
            string filterClause = "";
            int userid = 0;
            int areaid = 0;

            if (request.SecurityAreaId.HasValue && request.SecurityAreaId.Value > 0)
            {
                filterClause += " \"SecurityAreaId\"=@Area AND";
                areaid = request.SecurityAreaId.Value;
            }
            if (request.UserId.HasValue)
            {
                filterClause += " \"UserId\"=@User AND";
                userid = request.UserId.Value;
            }

            filterClause += " \"UtcDisabled\" is null";

            return db.Query<DBOs.Security.AreaAcl>("SELECT * FROM \"AreaAcl\" WHERE" + filterClause,
                new { Area = areaid, User = userid });
        }
    }
}
