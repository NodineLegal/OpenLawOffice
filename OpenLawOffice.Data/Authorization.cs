// -----------------------------------------------------------------------
// <copyright file="Authorization.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Authorization
    {
        public class AuthorizeResult
        {
            public bool HasError { get; set; }

            public bool IsAuthorized { get; set; }

            public string ErrorMessage { get; set; }

            public Common.Models.Security.User RequestingUser { get; set; }
        }

        public static AuthorizeResult AreaAccess(Common.Models.Security.User user,
            Common.Models.Security.Area area, Guid? authToken,
            Common.Models.PermissionType permissionRequired)
        {
            Common.Models.Security.AreaAcl areaAcl = null;

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

            // If we do not know the area id we need to get it.
            if (!area.Id.HasValue)
            {
                // We know from validation that we have a name
                area = Data.Security.Area.Get(area.Name);
            }
            else
            {
                area = Data.Security.Area.Get(area.Id.Value);
            }

            // Do the same for user
            if (!user.Id.HasValue)
            {
                if (authToken.HasValue && authToken.Value != Guid.Empty)
                {
                    // Fetch by authtoken
                    user = Data.Security.User.Get(authToken.Value);
                }
                else if (!string.IsNullOrEmpty(user.Username))
                {
                    // We know from validation that we must now have a Username
                    user = Data.Security.User.Get(user.Username);
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
                user = Data.Security.User.Get(user.Id.Value);
            }

            // Make sure we have something loaded
            if (area == null)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Could not find Area."
                };
            if (user == null)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Could not find User."
                };

            // Database models are now stuffed with at least the mandatory Id information

            // Load the Area Acl database model
            areaAcl = Data.Security.AreaAcl.Get(user.Id.Value, area.Id.Value);

            if (areaAcl == null)
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission not given."
                };

            // Ensure flags have values - security precautions
            if (!areaAcl.DenyFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "DenyFlags must have a value."
                };
            if (!areaAcl.AllowFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "AllowFlags must have a value."
                };

            // Test deny flags
            if (areaAcl.DenyFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission explicitly denied."
                };

            // Test allow flags
            if (areaAcl.AllowFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = true,
                    RequestingUser = user
                };

            // Neither denied or allowed, deny = fail SECURE
            return new AuthorizeResult()
            {
                HasError = false,
                IsAuthorized = false,
                RequestingUser = user
            };
        }

        public static AuthorizeResult SecuredResourceAccess(Common.Models.Security.User user,
            Common.Models.Security.SecuredResource securedResource,
            Guid authToken, Common.Models.PermissionType permissionRequired)
        {
            Common.Models.Security.SecuredResourceAcl securedResourceAcl = null;

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

            // Validation requires Id
            securedResource = Data.Security.SecuredResource.Get(securedResource.Id.Value);

            // Load up the necessaries on user
            if (!user.Id.HasValue)
            {
                if (authToken != Guid.Empty)
                {
                    // Fetch by authtoken
                    user = Data.Security.User.Get(authToken);
                }
                else
                {
                    // We know from validation that we must now have a Username
                    user = Data.Security.User.Get(user.Username);
                }
            }
            else
            {
                user = Data.Security.User.Get(user.Id.Value);
            }

            // Make sure we have something loaded
            if (securedResource == null)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Could not find Secured Resource."
                };
            if (user == null)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "Could not find User."
                };

            // Database models are now stuffed with at least the mandatory Id information

            // Load the Secured Resource Acl model
            securedResourceAcl = Data.Security.SecuredResourceAcl.Get(user.Id.Value, securedResource.Id.Value);

            // Ensure flags have values - security precautions
            if (!securedResourceAcl.DenyFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "DenyFlags must have a value."
                };
            if (!securedResourceAcl.AllowFlags.HasValue)
                return new AuthorizeResult()
                {
                    HasError = true,
                    IsAuthorized = false,
                    ErrorMessage = "AllowFlags must have a value."
                };

            // Test deny flags
            if (securedResourceAcl.DenyFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = false,
                    ErrorMessage = "Permission explicitly denied."
                };

            // Test allow flags
            if (securedResourceAcl.AllowFlags.Value.HasFlag(permissionRequired))
                return new AuthorizeResult()
                {
                    HasError = false,
                    IsAuthorized = true,
                    RequestingUser = user
                };

            // Neither denied or allowed, deny = fail SECURE
            return new AuthorizeResult()
            {
                HasError = false,
                IsAuthorized = false,
                RequestingUser = user
            };
        }
    }
}