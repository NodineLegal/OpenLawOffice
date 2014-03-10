using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;
using AutoMapper;

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
                filterClause += " LOWER(\"username\") like '%' || LOWER(@Username) || '%' AND";

            filterClause += " \"utc_disabled\" is null";

            return db.SqlList<DBOs.Security.User>("SELECT * FROM \"user\" WHERE" + filterClause,
                new { Username = request.Username });
        }

        public override object Post(Rest.Requests.Security.User request)
        {
            Common.Models.Security.User sysModel;
            DBOs.Security.User dbModel;
            Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User> response;

            if (!CanCreate)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
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
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
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

            sysModel = Mapper.Map<Common.Models.Security.User>(request);
            sysModel.UtcCreated = DateTime.Now;
            sysModel.UtcModified = DateTime.Now;
            sysModel.UtcDisabled = null;
            sysModel.PasswordSalt = GetRandomString(10);
            sysModel.Password = Services.Security.Authentication.ServerHashPassword(request.Password, sysModel.PasswordSalt);

            dbModel = Mapper.Map<DBOs.Security.User>(sysModel);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // AutoIncrementing Ids
                    db.Insert<DBOs.Security.User>(dbModel);
                    dbModel.Id = (int)db.GetLastInsertId();
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to create the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<Common.Models.Security.User>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<Common.Rest.Responses.Security.User>(sysModel)
            };
        }

        public override object Put(Rest.Requests.Security.User request)
        {
            Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User> response;

            if (!CanUpdate)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Put verb not enabled."
                    }
                };
            }

            try
            {
                response = Authorize(request, Common.Models.PermissionType.Modify);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
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

            /* To do an update we need to
             * 1) Load the database model from the database using the Id of the request
             * 2) Convert to system model 1
             * 3) Load system model 2 from the request
             * 4) If assignable from date base -> copy date base properties from system model 1 to system model 2
             * 5) If assignable from core -> copy core properties from system model 1 to system model 2
             *  5a) Overwrite core's modified properties (this can be simply done by ignoring on copy)
             */

            object idValue = null;
            DBOs.Security.User dbModelCurrent;
            DBOs.Security.User dbModelNew;
            Common.Models.Security.User sysModel1;
            Common.Models.Security.User sysModel2;

            idValue = GetIdValue(request);

            // Send an error if idValue is null
            if (idValue == null)
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new Common.Error()
                    {
                        Message = "The request must specify an Id."
                    }
                };


            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                dbModelCurrent = db.GetByIdParam<DBOs.Security.User>(idValue);
                sysModel1 = Mapper.Map<Common.Models.Security.User>(dbModelCurrent);
                sysModel2 = Mapper.Map<Common.Models.Security.User>(request);
                sysModel2.UtcCreated = sysModel1.UtcCreated;
                sysModel2.UtcModified = DateTime.Now;
                sysModel2.UtcDisabled = null;

                sysModel2.Password = sysModel1.Password;
                sysModel2.PasswordSalt = sysModel1.PasswordSalt;
                sysModel2.UserAuthToken = sysModel1.UserAuthToken;
                sysModel2.UserAuthTokenExpiry = sysModel1.UserAuthTokenExpiry;

                // Set new password, if !null in request
                if (!string.IsNullOrEmpty(request.Password))
                    sysModel2.Password = Services.Security.Authentication.ServerHashPassword(request.Password, sysModel1.PasswordSalt);

                dbModelNew = Mapper.Map<DBOs.Security.User>(sysModel2);

                try
                {
                    db.Update<DBOs.Security.User>(dbModelNew);
                }
                catch (Exception e)
                {
                    return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Error = new Common.Error()
                        {
                            Message = "An error occurred while attempting to update the object in the database.",
                            Exception = e
                        }
                    };
                }
            }

            return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.User>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<Common.Rest.Responses.Security.User>(sysModel2)
            };
        }

        private int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }

        private string GetRandomString(int length)
        {
            string[] array = new string[54]
	        {
		        "0","2","3","4","5","6","8","9",
		        "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
		        "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
	        };

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }
    }
}
