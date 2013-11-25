using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Matters
{
    public class MatterTag : 
        ResourceBase<Common.Models.Matters.MatterTag, DBOs.Matters.MatterTag,
        Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>
    {
        public override List<DBOs.Matters.MatterTag> GetList(Rest.Requests.Matters.MatterTag request, System.Data.IDbConnection db)
        {
            string filterClause = "";
            int tagCatId = 0;

            // MatterId
            // TagCategory
            // Tag

            if (request.TagCategoryId.HasValue &&
                request.TagCategoryId.Value > 0)
            {
                tagCatId = request.TagCategoryId.Value;
                filterClause += " \"tag_category_id\" = @TagId AND ";
            }

            if (!string.IsNullOrEmpty(request.Tag))
                filterClause += " LOWER(\"tag\") like '%' || LOWER(@Tag) || '%' AND";

            filterClause += " \"utc_disabled\" is null";

            return db.Query<DBOs.Matters.MatterTag>("SELECT * FROM \"matter_tag\" WHERE" + filterClause,
                new { TagId = tagCatId, Tag = request.Tag });
        }
    }
}
