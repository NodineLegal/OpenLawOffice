namespace OpenLawOffice.WebClient
{
    using System;
    using System.Data;
    using ServiceStack.OrmLite;
    using AutoMapper;

    public class Security
    {
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

        public static AuthorizeResult AreaAccess(ViewModels.Security.UserViewModel user,
            ViewModels.Security.AreaViewModel area, 
            Guid? authToken,
            Common.Models.PermissionType permissionRequired)
        {
            DBOs.Security.User dbUser;
            DBOs.Security.Area dbArea;
            DBOs.Security.AreaAcl dbAreaAcl;
            Common.Models.Security.AreaAcl sysAreaAcl;

            // Validation
            if (!area.Id.HasValue && string.IsNullOrEmpty(area.Name)) // no id and no name
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Area must have either an Id or a Name."
                };
            if (!user.Id.HasValue && // No id
                string.IsNullOrEmpty(user.Username) && // No username
                (!authToken.HasValue || // No authtoken
                authToken.Value == Guid.Empty))
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "User must have an Id, Username or AuthToken."
                };

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // If we do not know the area id we need to get it.
                if (!area.Id.HasValue)
                {
                    // We know from validation that we have a name
                    dbArea = db.Single<DBOs.Security.Area>(new { area.Name });
                }
                else
                {
                    dbArea = new DBOs.Security.Area() { Id = area.Id.Value };
                }

                // Do the same for user
                if (!user.Id.HasValue)
                {
                    if (authToken.HasValue && authToken.Value != Guid.Empty)
                    {
                        // Fetch by authtoken
                        dbUser = db.Single<DBOs.Security.User>(new { UserAuthToken = authToken });
                    }
                    else if (!string.IsNullOrEmpty(user.Username))
                    {
                        // We know from validation that we must now have a Username
                        dbUser = db.Single<DBOs.Security.User>(new { user.Username });
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
                    dbUser = new DBOs.Security.User() { Id = user.Id.Value };
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
                dbAreaAcl = db.Single<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbArea.Id, UserId = dbUser.Id });
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

        public static AuthorizeResult SecuredResourceAccess(ViewModels.Security.UserViewModel user,
            ViewModels.Security.SecuredResourceViewModel securedResource,
            Guid authToken, Common.Models.PermissionType permissionRequired)
        {
            DBOs.Security.User dbUser;
            DBOs.Security.SecuredResource dbSecuredResource;
            DBOs.Security.SecuredResourceAcl dbSecuredResourceAcl;
            Common.Models.Security.SecuredResourceAcl sysSecuredResourceAcl;

            // Validation
            if (!securedResource.Id.HasValue) // no id
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Secured Resource must have an Id."
                };
            if (!user.Id.HasValue && string.IsNullOrEmpty(user.Username) &&
                authToken == Guid.Empty) // no id, no username and no authtoken
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "User must have an Id, Username or AuthToken."
                };


            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Validation requires Id
                dbSecuredResource = new DBOs.Security.SecuredResource() { Id = securedResource.Id.Value };

                // Load up the necessaries on user
                if (!user.Id.HasValue)
                {
                    if (authToken != Guid.Empty)
                    {
                        // Fetch by authtoken
                        dbUser = db.Single<DBOs.Security.User>(new { UserAuthToken = authToken });
                    }
                    else
                    {
                        // We know from validation that we must now have a Username
                        dbUser = db.Single<DBOs.Security.User>(new { Username = user.Username });
                    }
                }
                else
                {
                    dbUser = new DBOs.Security.User() { Id = user.Id.Value };
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
                dbSecuredResourceAcl = db.Single<DBOs.Security.SecuredResourceAcl>(
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
            if (sysSecuredResourceAcl.DenyFlags.Value.HasFlag(permissionRequired))
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