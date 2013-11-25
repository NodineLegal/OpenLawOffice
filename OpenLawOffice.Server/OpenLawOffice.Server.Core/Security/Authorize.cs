using System;
using System.Data;
using ServiceStack.OrmLite;
using AutoMapper;

namespace OpenLawOffice.Server.Core.Security
{
    public static class Authorize
    {
        public static AuthorizeResult AreaAccess(Rest.Requests.Security.User userRequest, 
            Rest.Requests.Security.Area areaRequest, Common.Models.PermissionType permissionRequired)
        {
            DBOs.Security.User dbUser;
            DBOs.Security.Area dbArea;
            DBOs.Security.AreaAcl dbAreaAcl;
            Common.Models.Security.AreaAcl sysAreaAcl;

            // Validation
            if (!areaRequest.Id.HasValue && string.IsNullOrEmpty(areaRequest.Name)) // no id and no name
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Area must have either an Id or a Name."
                };
            if (!userRequest.Id.HasValue && // No id
                string.IsNullOrEmpty(userRequest.Username) && // No username
                (!userRequest.AuthToken.HasValue || // No authtoken
                userRequest.AuthToken.Value == Guid.Empty))
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "User must have an Id, Username or AuthToken."
                };

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // If we do not know the area id we need to get it.
                if (!areaRequest.Id.HasValue)
                {
                    // We know from validation that we have a name
                    dbArea = db.QuerySingle<DBOs.Security.Area>(new { areaRequest.Name });
                }
                else
                {
                    dbArea = new DBOs.Security.Area() { Id = areaRequest.Id.Value };
                }

                // Do the same for user
                if (!userRequest.Id.HasValue)
                {
                    if (userRequest.AuthToken.HasValue && userRequest.AuthToken.Value != Guid.Empty)
                    {
                        // Fetch by authtoken
                        dbUser = db.QuerySingle<DBOs.Security.User>(new { UserAuthToken = userRequest.AuthToken });
                    }
                    else if (!string.IsNullOrEmpty(userRequest.Username))
                    {
                        // We know from validation that we must now have a Username
                        dbUser = db.QuerySingle<DBOs.Security.User>(new { userRequest.Username });
                    }
                    else
                        return new AuthorizeResult()
                        {
                            HasError = true,
                            IsAuthorized = false,
                            ErrorMessage = "User must have an Id, Username or AuthToken."
                        };
                }
                else
                {
                    dbUser = new DBOs.Security.User() { Id = userRequest.Id.Value };
                }

                // Make sure we have something loaded
                if (dbArea == null)
                    return new AuthorizeResult()
                    {
                        HasError = true,
                        IsAuthorized = false,
                        ErrorMessage = "Could not find Area."
                    };
                if (dbUser == null)
                    return new AuthorizeResult()
                    {
                        HasError = true,
                        IsAuthorized = false,
                        ErrorMessage = "Could not find User."
                    };

                // Database models are now stuffed with at least the mandatory Id information

                // Load the Area Acl database model
                dbAreaAcl = db.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbArea.Id, UserId = dbUser.Id });
            }

            // Load the Area Acl system model
            sysAreaAcl = Mapper.Map<Common.Models.Security.AreaAcl>(dbAreaAcl);

            if (sysAreaAcl == null)
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission not given."
                };

            // Ensure flags have values - security precautions
            if (!sysAreaAcl.DenyFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "DenyFlags must have a value."
                };
            if (!sysAreaAcl.AllowFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "AllowFlags must have a value."
                };

            // Test deny flags
            if (sysAreaAcl.DenyFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission explicitly denied."
                };

            // Test allow flags
            if (sysAreaAcl.AllowFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = true,
                    RequestingUser = Mapper.Map<Common.Models.Security.User>(dbUser)
                };

            // Neither denied or allowed, deny = fail SECURE
            return new AuthorizeResult()
            {
                HasError = false,
                IsAuthorized = false,
                RequestingUser = Mapper.Map<Common.Models.Security.User>(dbUser)
            };
        }

        public static AuthorizeResult SecuredResourceAccess(Rest.Requests.Security.User userRequest,
            Rest.Requests.Security.SecuredResource securedResourceRequest, Common.Models.PermissionType permissionRequired)
        {
            DBOs.Security.User dbUser;
            DBOs.Security.SecuredResource dbSecuredResource;
            DBOs.Security.SecuredResourceAcl dbSecuredResourceAcl;
            Common.Models.Security.SecuredResourceAcl sysSecuredResourceAcl;

            // Validation
            if (!securedResourceRequest.Id.HasValue) // no id
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Secured Resource must have an Id."
                };
            if (!userRequest.Id.HasValue && string.IsNullOrEmpty(userRequest.Username) &&
                userRequest.AuthToken == Guid.Empty) // no id, no username and no authtoken
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "User must have an Id, Username or AuthToken."
                };


            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Validation requires Id
                dbSecuredResource = new DBOs.Security.SecuredResource() { Id = securedResourceRequest.Id.Value };

                // Load up the necessaries on user
                if (!userRequest.Id.HasValue)
                {
                    if (userRequest.AuthToken != Guid.Empty)
                    {
                        // Fetch by authtoken
                        dbUser = db.QuerySingle<DBOs.Security.User>(new { userRequest.AuthToken });
                    }
                    else
                    {
                        // We know from validation that we must now have a Username
                        dbUser = db.QuerySingle<DBOs.Security.User>(new { userRequest.Username });
                    }
                }
                else
                {
                    dbUser = new DBOs.Security.User() { Id = userRequest.Id.Value };
                }

                // Make sure we have something loaded
                if (dbSecuredResource == null)
                    return new AuthorizeResult()
                    {
                        HasError = true,
                        IsAuthorized = false,
                        ErrorMessage = "Could not find Secured Resource."
                    };
                if (dbUser == null)
                    return new AuthorizeResult()
                    {
                        HasError = true,
                        IsAuthorized = false,
                        ErrorMessage = "Could not find User."
                    };

                // Database models are now stuffed with at least the mandatory Id information

                // Load the Secured Resource Acl database model
                dbSecuredResourceAcl = db.QuerySingle<DBOs.Security.SecuredResourceAcl>(
                    new { SecuredResourceId = dbSecuredResource.Id, UserId = dbUser.Id }
                    );
            }

            // Load the Secured Resource Acl system model
            sysSecuredResourceAcl = Mapper.Map<Common.Models.Security.SecuredResourceAcl>(dbSecuredResourceAcl);

            // Ensure flags have values - security precautions
            if (!sysSecuredResourceAcl.DenyFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "DenyFlags must have a value."
                };
            if (!sysSecuredResourceAcl.AllowFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "AllowFlags must have a value."
                };

            // Test deny flags
            if (!sysSecuredResourceAcl.DenyFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission explicitly denied."
                };

            // Test allow flags
            if (sysSecuredResourceAcl.AllowFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = true,
                    RequestingUser = Mapper.Map<Common.Models.Security.User>(dbUser)
                };

            // Neither denied or allowed, deny = fail SECURE
            return new AuthorizeResult()
            {
                HasError = false,
                IsAuthorized = false,
                RequestingUser = Mapper.Map<Common.Models.Security.User>(dbUser)
            };
        }
    }
}