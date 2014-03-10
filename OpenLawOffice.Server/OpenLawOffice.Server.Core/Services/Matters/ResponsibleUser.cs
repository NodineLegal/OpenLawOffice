using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;
using AutoMapper;

namespace OpenLawOffice.Server.Core.Services.Matters
{
    public class ResponsibleUser :
        ResourceBase<Common.Models.Matters.ResponsibleUser, DBOs.Matters.ResponsibleUser,
        Rest.Requests.Matters.ResponsibleUser, Common.Rest.Responses.Matters.ResponsibleUser>
    {
        public override List<DBOs.Matters.ResponsibleUser> GetList(Rest.Requests.Matters.ResponsibleUser request, System.Data.IDbConnection db)
        {
            string filterClause = "";
            int userId = 0;
            Guid matterId = Guid.Empty;

            // MatterId
            // UserId - list of responsibilities for user
            // Responsibility

            if (request.MatterId.HasValue &&
                request.MatterId.Value != Guid.Empty)
            {
                matterId = request.MatterId.Value;
                filterClause += " \"matter_id\" = @MatterId AND ";
            }

            if (request.UserId.HasValue)
            {
                filterClause += " \"user_id\"=@User AND";
                userId = request.UserId.Value;
            }

            if (!string.IsNullOrEmpty(request.Responsibility))
                filterClause += " LOWER(\"responsibility\") like '%' || LOWER(@Responsibility) || '%' AND";

            filterClause += " \"utc_disabled\" is null";

            return db.SqlList<DBOs.Matters.ResponsibleUser>("SELECT * FROM \"responsible_user\" WHERE" + filterClause,
                new { MatterId = matterId, UserId = userId, Responsibility = request.Responsibility });
        }

        public override object Post(Rest.Requests.Matters.ResponsibleUser request)
        {
            Common.Models.Matters.ResponsibleUser sysModel;
            DBOs.Matters.ResponsibleUser dbModel;
            Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser> response;

            if (!CanCreate)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser>()
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
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser>()
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

            sysModel = Mapper.Map<Common.Models.Matters.ResponsibleUser>(request);

            sysModel.UtcCreated = DateTime.Now;
            sysModel.UtcModified = DateTime.Now;
            sysModel.UtcDisabled = null;
            sysModel.CreatedBy = request.Session.RequestingUser;
            sysModel.ModifiedBy = request.Session.RequestingUser;
            sysModel.DisabledBy = null;

            dbModel = Mapper.Map<DBOs.Matters.ResponsibleUser>(sysModel);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    List<DBOs.Matters.ResponsibleUser> results = 
                        db.SqlList<DBOs.Matters.ResponsibleUser>("SELECT * FROM \"responsible_user\" WHERE \"matter_id\"=@MatterId AND \"user_id\"=@UserId",
                        new { MatterId = request.MatterId, UserId = request.UserId });

                    if (results == null || results.Count <= 0)
                    {
                        db.Insert<DBOs.Matters.ResponsibleUser>(dbModel);
                        dbModel.Id = (int)db.GetLastInsertId();
                    }
                    else if (results.Count == 1)
                    {
                        DBOs.Matters.ResponsibleUser existingResult = results[0];
                        dbModel.Id = existingResult.Id;
                        dbModel.UtcCreated = existingResult.UtcCreated;
                        dbModel.CreatedByUserId = existingResult.CreatedByUserId;
                        db.Update<DBOs.Matters.ResponsibleUser>(dbModel);
                    }
                    else
                    {
                        return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser>()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                            Error = new Common.Error()
                            {
                                Message = "More than one record exists within the database where only a single record should exist."
                            }
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to create the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<Common.Models.Matters.ResponsibleUser>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Matters.ResponsibleUser>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<Common.Rest.Responses.Matters.ResponsibleUser>(sysModel)
            };
        }
    }
}
