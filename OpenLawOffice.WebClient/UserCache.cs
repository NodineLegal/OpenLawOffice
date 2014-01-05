namespace OpenLawOffice.WebClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data;
    using ServiceStack.OrmLite;
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

            using (IDbConnection db = OpenLawOffice.Server.Core.Database.Instance.OpenConnection())
            {
                OpenLawOffice.Server.Core.DBOs.Security.User dboUser = 
                    db.QuerySingle<OpenLawOffice.Server.Core.DBOs.Security.User>(
                    new { UserAuthToken = authToken });

                if (dboUser == null) return null;

                Add(ret = Mapper.Map<Common.Models.Security.User>(dboUser));
            }

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