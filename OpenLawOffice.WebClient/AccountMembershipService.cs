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

    public class AccountMembershipService
    {
        private readonly MembershipProvider _provider;
        
        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "username");
            if (string.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(username, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipCreateStatus CreateUser(string username, string password, string email, bool isApproved = false)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(username, password, email, null, null, isApproved, null, out status);
            return status;
        }

        public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return _provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public MembershipUserCollection FindUsersByUsername(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return _provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return _provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public MembershipUser GetUser(string username, bool userIsOnline = false)
        {
            return _provider.GetUser(username, userIsOnline);
        }

        public MembershipUser GetUser(Guid userPId, bool userIsOnline = false)
        {
            return _provider.GetUser(userPId, userIsOnline);
        }

        public string GetUserNameByEmail(string email)
        {
            return _provider.GetUserNameByEmail(email);
        }

        public string ResetPassword(string username, string answer)
        {
            return _provider.ResetPassword(username, answer);
        }

        public bool UnlockUser(string username)
        {
            return _provider.UnlockUser(username);
        }

        public void UpdateUser(MembershipUser user)
        {
            _provider.UpdateUser(user);
        }

        public bool ValidateUser(string username, string password)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "username");
            //if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            
            return _provider.ValidateUser(username, password);
        }
    }
}