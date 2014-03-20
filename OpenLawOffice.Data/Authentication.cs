// -----------------------------------------------------------------------
// <copyright file="Security.cs" company="Nodine Legal, LLC">
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
    using System.Data;
    using AutoMapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Authentication
    {
        public class LoginResult
        {
            public bool Success { get; set; }
            public string UserAuthToken { get; set; }        
            public DateTime Expiry { get; set; }
            public string FailReason { get; set; }
            public Common.Models.Security.User User { get; set; }
        }

        public static LoginResult Login(string username, string clientHashedPassword)
        {
            return null;
            //DbModels.User dbo;
            //string hashedPassword;

            //dbo = DbModels.User.FirstOrDefault("SELECT * FROM \"user\" WHERE \"username\"=@0 AND \"utc_disabled\" is null",
            //    username);

            //if (dbo == null)
            //    return new LoginResult() 
            //    { 
            //        Success = false,
            //        FailReason = "Invalid username and/or password."
            //    };

            //// Apply server hash
            //hashedPassword = ServerHashPassword(clientHashedPassword, dbo.PasswordSalt);

            //if (hashedPassword != dbo.Password)
            //    return new LoginResult()
            //    {
            //        Success = false,
            //        FailReason = "Invalid username and/or password."
            //    };

            //dbo.UserAuthToken = Guid.NewGuid().ToString();
            //dbo.UserAuthTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            //if (DbModels.User.Update("UPDATE \"user\" SET (\"user_auth_token\"=@0, \"user_auth_token_expiry\"=@1) WHERE \"id\"=@2",
            //    dbo.UserAuthToken, dbo.UserAuthTokenExpiry.Value, dbo.Id) < 1)
            //    return new LoginResult()
            //    {
            //        Success = false,
            //        FailReason = "Failed to update the user within the database."
            //    };

            //return new LoginResult()
            //{
            //    Success = true,
            //    UserAuthToken = dbo.UserAuthToken,
            //    Expiry = dbo.UserAuthTokenExpiry.Value,
            //    User = Mapper.Map<Common.Models.Security.User>(dbo)
            //};
        }

        private static string ServerHashPassword(string plainTextPassword, string salt)
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
