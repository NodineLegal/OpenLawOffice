// -----------------------------------------------------------------------
// <copyright file="AccountMembershipService.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient
{
    using System.Web.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AccountRoleService
    {
        private readonly RoleProvider _provider;

        public AccountRoleService()
            : this(null)
        {
        }

        public AccountRoleService(RoleProvider provider)
        {
            _provider = provider ?? Roles.Provider;
        }

        public void AddUserToRole(string username, string roleName)
        {
            _provider.AddUsersToRoles(new string[] { username }, new string[] { roleName });
        }

        public void CreateRole(string roleName)
        {
            _provider.CreateRole(roleName);
        }

        public bool DeleteRole(string roleName)
        {
            return _provider.DeleteRole(roleName, true);
        }

        public List<string> FindUsersInRole(string roleName, string usernameToMatch)
        {
            return _provider.FindUsersInRole(roleName, usernameToMatch).ToList<string>();
        }

        public List<string> GetAllRoles()
        {
            return _provider.GetAllRoles().ToList<string>();
        }

        public List<string> GetRolesForUser(string username)
        {
            return _provider.GetRolesForUser(username).ToList<string>();
        }

        public List<string> GetUsersInRole(string roleName)
        {
            return _provider.GetUsersInRole(roleName).ToList<string>();
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return _provider.IsUserInRole(username, roleName);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            _provider.RemoveUsersFromRoles(usernames, roleNames);
        }

        public bool RoleExists(string roleName)
        {
            return _provider.RoleExists(roleName);
        }
    }
}