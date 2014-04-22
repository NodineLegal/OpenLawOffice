// -----------------------------------------------------------------------
// <copyright file="UserCache.cs" company="Nodine Legal, LLC">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data;
    using AutoMapper;

    public class UserCache : Common.Singleton<UserCache>
    {
        private Dictionary<Guid, Common.Models.Security.User> _table;

        public UserCache()
        {
            _table = new Dictionary<Guid, Common.Models.Security.User>();
        }

        public void Add(Common.Models.Security.User user)
        {
            // First, auto remove any old entries

            Dictionary<Guid, Common.Models.Security.User>.Enumerator en = _table.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current.Value.Id == user.Id)
                {
                    // Match -> Replace (by removing and falling thru
                    _table.Remove(en.Current.Key);
                    break;
                }
            }

            _table.Add(user.UserAuthToken.Value, user);
        }

        public void Remove(Common.Models.Security.User user)
        {
            _table.Remove(user.UserAuthToken.Value);
        }

        public Common.Models.Security.User Lookup(Guid authToken)
        {
            Common.Models.Security.User ret = null;

            if (_table.ContainsKey(authToken)) return _table[authToken];

            Add(ret = Data.Security.User.Get(authToken));

            return ret;
        }

        public Common.Models.Security.User Lookup(HttpRequestBase request)
        {
            Guid guid;
            HttpCookie cookie = request.Cookies["UserAuthToken"];
            if (cookie == null) return null;
            if (!Guid.TryParse(cookie.Value, out guid)) return null;
            return Lookup(guid);
        }
    }
}