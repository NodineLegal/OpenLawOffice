using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;
using AutoMapper;

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
                filterClause += " \"security_area_id\"=@Area AND";
                areaid = request.SecurityAreaId.Value;
            }
            if (request.UserId.HasValue)
            {
                filterClause += " \"user_id\"=@User AND";
                userid = request.UserId.Value;
            }

            filterClause += " \"utc_disabled\" is null";

            return db.Query<DBOs.Security.AreaAcl>("SELECT * FROM \"area_acl\" WHERE" + filterClause,
                new { Area = areaid, User = userid });
        }

        public override object Post(Rest.Requests.Security.AreaAcl request)
        {
            Common.Models.Security.AreaAcl sysModel;
            DBOs.Security.AreaAcl dbModel;
            Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl> response;

            if (!CanCreate)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Post verb not enabled."
                    }
                };
            }

            try
            {
                response = Authorize(request, Common.Models.PermissionType.Create);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An unexpected error occurred while attempting to authorize the request.",
                        Exception = e
                    }
                };
            }

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return response;

            sysModel = Mapper.Map<Common.Models.Security.AreaAcl>(request);
            sysModel.UtcCreated = DateTime.Now;
            sysModel.UtcModified = DateTime.Now;
            sysModel.UtcDisabled = null;
            sysModel.CreatedBy = request.Session.RequestingUser;
            sysModel.ModifiedBy = request.Session.RequestingUser;
            sysModel.DisabledBy = null;

            dbModel = Mapper.Map<DBOs.Security.AreaAcl>(sysModel);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    List<DBOs.Security.AreaAcl> existingAreaAclList =
                        db.Query<DBOs.Security.AreaAcl>("SELECT * FROM \"area_acl\" WHERE \"security_area_id\"=@Area AND \"user_id\"=@User",
                        new { Area = sysModel.Area.Id.Value, User = sysModel.User.Id.Value });
                    

                    if (existingAreaAclList.Count > 0)
                    {
                        return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl>()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.Conflict,
                            Error = new Common.Error()
                            {
                                Message = "The requested user and area combination is in conflict with an existing entry, you must update."
                            }
                        };
                    }

                    db.Insert<DBOs.Security.AreaAcl>(dbModel);
                    dbModel.Id = (int)db.GetLastInsertId();
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to create the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<Common.Models.Security.AreaAcl>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.AreaAcl>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<Common.Rest.Responses.Security.AreaAcl>(sysModel)
            };
        }
    }
}
