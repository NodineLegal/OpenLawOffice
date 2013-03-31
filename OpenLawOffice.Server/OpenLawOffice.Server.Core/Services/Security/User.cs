using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class User
        : ResourceBase<Common.Models.Security.User, DBOs.Security.User,
            Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
    {
        public override List<DBOs.Security.User> GetList(Rest.Requests.Security.User request, IDbConnection db)
        {
            string filterClause = "";

            if (!string.IsNullOrEmpty(request.Username))
                filterClause += " LOWER(\"Username\") like LOWER('%@Username%') AND";

            filterClause += " \"UtcDisabled\" is null";

            return db.Query<DBOs.Security.User>("SELECT * FROM \"User\" WHERE" + filterClause,
                new { Username = request.Username });
        }
    }
}
