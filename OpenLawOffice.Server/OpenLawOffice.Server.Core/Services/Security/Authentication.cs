using System;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class Authentication
        : FunctionBase<Common.Models.Security.Authentication, DBOs.NullDbo,
            Rest.Requests.Security.Authentication, Common.Rest.Responses.Security.Authentication>
    {
        public override object Post(Rest.Requests.Security.Authentication request)
        {
            Common.Models.Security.Authentication sysAuth;
            DBOs.Security.User dbUser;
            Common.Models.Security.User sysUser;

            if (!CanExecute)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Execution not enabled."
                    }
                };
            }

            // Convert to our system model
            sysAuth = AutoMapper.Mapper.Map<Common.Models.Security.Authentication>(request);

            // Load user
            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    dbUser = db.QuerySingle<DBOs.Security.User>(new { sysAuth.Username });
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "Error loading user information.",
                        Exception = e
                    }
                };
            }

            // Was the username found?
            if (dbUser == null)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Error = new Common.Error()
                    {
                        Message = "Invalid username and/or password."
                    }
                };
            }


            // Check PW
            // Client enter "a" as password to get the client submitted pw of:
            // 1F40FC92DA241694750979EE6CF582F2D5D7D28E18335DE05ABC54D0560E0F5302860C652BF08D560252AA5E74210546F369FBBBCE8C12CFC7957B2652FE9A75
            // Server then appends a salt of "0123456789" to the client submitted pw and hashes it:
            // 85396556E29786C9BD4846462FD905DE3CC46C8ACB08EF7D952369B9792676ACB92FE5E01AE8D116BCB9870723CD84A62738604B2BE4F503AB23DEF4A6B30882
            // So...
            // Username = "TestUser"
            // Password = "85396556E29786C9BD4846462FD905DE3CC46C8ACB08EF7D952369B9792676ACB92FE5E01AE8D116BCB9870723CD84A62738604B2BE4F503AB23DEF4A6B30882"
            // PasswordSalt = "0123456789"
            if (ServerHashPassword(sysAuth.Password, dbUser.PasswordSalt) != dbUser.Password)
            {
                // != so fail out
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Error = new Common.Error()
                    {
                        Message = "Invalid username and/or password."
                    }
                };
            }

            // Import the database representation to a system model
            try
            {
                sysUser = AutoMapper.Mapper.Map<Common.Models.Security.User>(dbUser);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "Error converting database model to system model.",
                        Exception = e
                    }
                };
            }

            // Assign auth token and expiry
            sysUser.UserAuthToken = Guid.NewGuid();
            sysUser.UserAuthTokenExpiry = DateTime.Now.AddMinutes(15);


            // Update the database
            try
            {
                dbUser = AutoMapper.Mapper.Map<DBOs.Security.User>(sysUser);
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.Update<DBOs.Security.User>(
                        set: "\"user_auth_token\" = {0}, \"user_auth_token_expiry\" = {1}".Params(dbUser.UserAuthToken.Value, dbUser.UserAuthTokenExpiry.Value),
                        where: "\"id\" = {0}".Params(dbUser.Id));
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "Error updating user.",
                        Exception = e
                    }
                };
            }

            // Build successful auth result
            // Drop user and password as we have no need to put those out on the wire again.
            sysAuth.AuthToken = sysUser.UserAuthToken;
            sysAuth.Username = null;
            sysAuth.Password = null;

            // Return successful result
            try
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Data = AutoMapper.Mapper.Map<Common.Rest.Responses.Security.Authentication>(sysAuth)
                };
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<Common.Rest.Responses.Security.Authentication>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "Error converting system model to response model.",
                        Exception = e
                    }
                };
            }
        }

        public static string ClientHashPassword(string plainTextPassword)
        {
            return Hash(plainTextPassword);
        }

        public static string ServerHashPassword(string plainTextPassword, string salt)
        {
            return Hash(salt + plainTextPassword);
        }

        private static string Hash(string str)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = sha512.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
