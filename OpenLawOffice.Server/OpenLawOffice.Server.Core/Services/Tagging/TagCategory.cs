using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Tagging
{
    public class TagCategory : ResourceBase<Common.Models.Tagging.TagCategory, DBOs.Tagging.TagCategory,
        Rest.Requests.Tagging.TagCategory, Common.Rest.Responses.Tagging.TagCategory>
    {
        public override List<DBOs.Tagging.TagCategory> GetList(Rest.Requests.Tagging.TagCategory request, System.Data.IDbConnection db)
        {
            string filterClause = "";

            if (!string.IsNullOrEmpty(request.Name))
                filterClause += " LOWER(\"name\") like '%' || LOWER(@Name) || '%' AND";

            filterClause += " \"utc_disabled\" is null";

            return db.Query<DBOs.Tagging.TagCategory>("SELECT * FROM \"tag_category\" WHERE" + filterClause,
                new { Name = request.Name});
        }
    }
}
