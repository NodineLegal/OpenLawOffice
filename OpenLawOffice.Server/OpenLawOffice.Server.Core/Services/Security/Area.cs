using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class Area
        : ResourceBase<Common.Models.Security.Area, DBOs.Security.Area,
            Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
    {
        public override List<DBOs.Security.Area> GetList(Rest.Requests.Security.Area request, IDbConnection db)
        {
            string filterClause = "";
            int parentid = 0;

            if (!string.IsNullOrEmpty(request.Name))
                filterClause += " LOWER(\"Name\") like LOWER('%@Name%') AND";

            if (!request.ShowAll.HasValue || !request.ShowAll.Value)
            { 
                // honor parent
                if (request.ParentId.HasValue && request.ParentId.Value > 0)
                {
                    filterClause += " \"ParentId\"=@ParentId AND";
                    parentid = request.ParentId.Value;
                }
                else
                    filterClause += " \"ParentId\" is null AND";
            }

            filterClause += " \"UtcDisabled\" is null";

            return db.Query<DBOs.Security.Area>("SELECT * FROM \"Area\" WHERE" + filterClause,
                new { Name = request.Name, ParentId = parentid });
        }
    }
}
