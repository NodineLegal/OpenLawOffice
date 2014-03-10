using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

using System.Linq;
using System.Text;
using System.Data.SqlClient;


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
            
            //return db.SqlList<DBOs.Matters.Matter>(
            //    "SELECT * FROM \"matter\" WHERE" + filterClause,
            //    new { Title = request.Title });

            return db.SqlList<DBOs.Matters.Matter>("SELECT * FROM \"matter\" JOIN \"secured_resource_acl\" ON " +
                "\"matter\".\"id\"=\"secured_resource_acl\".\"secured_resource_id\" " +
                "WHERE " + filterClause +
                "\"secured_resource_acl\".\"allow_flags\" & 2 > 0 " +
                "AND NOT \"secured_resource_acl\".\"deny_flags\" & 2 > 0 " +
                "AND \"matter\".\"utc_disabled\" is null  " +
                "AND \"secured_resource_acl\".\"utc_disabled\" is null",
                new { Title = request.Title });
        }
    }
}
