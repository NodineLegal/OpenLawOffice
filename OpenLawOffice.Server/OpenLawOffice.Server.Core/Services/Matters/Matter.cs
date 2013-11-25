using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Matters
{
    public class Matter : ResourceBase<Common.Models.Matters.Matter, DBOs.Matters.Matter,
        Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
    {
        public override List<DBOs.Matters.Matter> GetList(Rest.Requests.Matters.Matter request, IDbConnection db)
        {
            string filterClause = "";

            if (!string.IsNullOrEmpty(request.Title))
                filterClause += " LOWER(\"title\") like '%' || LOWER(@Title) || '%' AND";

            filterClause += " \"utc_disabled\" is null";

            return db.Query<DBOs.Matters.Matter>("SELECT * FROM \"matter\" WHERE" + filterClause,
                new { Title = request.Title });
        }
    }
}
