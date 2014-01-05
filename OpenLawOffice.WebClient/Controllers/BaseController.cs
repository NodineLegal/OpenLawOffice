namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using Database = OpenLawOffice.Server.Core.Database;
    using DBOs = OpenLawOffice.Server.Core.DBOs;
    using AutoMapper;

    public class BaseController : Controller
    {
        public Common.Models.Security.User GetUser(int id)
        {
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                return GetUser(id, db);
            }
        }

        public Common.Models.Security.User GetUser(Guid authToken)
        {
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                return GetUser(authToken, db);
            }
        }

        public Common.Models.Security.User GetUser(int id, IDbConnection db)
        {
            DBOs.Security.User dbo = db.GetById<DBOs.Security.User>(id);
            if (dbo == null) return null;
            return Mapper.Map<Common.Models.Security.User>(dbo);
        }

        public Common.Models.Security.User GetUser(Guid authToken, IDbConnection db)
        {
            DBOs.Security.User dbo = db.QuerySingle<DBOs.Security.User>(new { UserAuthToken = authToken });
            if (dbo == null) return null;
            return Mapper.Map<Common.Models.Security.User>(dbo);
        }

        public void PopulateCoreDetails(Common.Models.Core model)
        {
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                if (model.CreatedBy != null)
                {
                    if (model.CreatedBy.Id.HasValue)
                        model.CreatedBy = GetUser(model.CreatedBy.Id.Value, db);
                    else if (model.CreatedBy.UserAuthToken.HasValue)
                        model.CreatedBy = GetUser(model.CreatedBy.UserAuthToken.Value, db);
                    else
                        model.CreatedBy = null;
                }
                if (model.ModifiedBy != null)
                {
                    if (model.ModifiedBy.Id.HasValue)
                        model.ModifiedBy = GetUser(model.ModifiedBy.Id.Value, db);
                    else if (model.ModifiedBy.UserAuthToken.HasValue)
                        model.ModifiedBy = GetUser(model.ModifiedBy.UserAuthToken.Value, db);
                    else
                        model.ModifiedBy = null;
                }
                if (model.DisabledBy != null)
                {
                    if (model.DisabledBy.Id.HasValue)
                        model.DisabledBy = GetUser(model.DisabledBy.Id.Value, db);
                    else if (model.DisabledBy.UserAuthToken.HasValue)
                        model.DisabledBy = GetUser(model.DisabledBy.UserAuthToken.Value, db);
                    else
                        model.DisabledBy = null;
                }
            }
        }

        public Common.Models.Security.AreaAcl GetAreaAcl(int areaId, int userId)
        {
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                return GetAreaAcl(areaId, userId, db);
            }
        }

        public Common.Models.Security.AreaAcl GetAreaAcl(int areaId, int userId, IDbConnection db)
        {
            DBOs.Security.AreaAcl dbo = db.QuerySingle<DBOs.Security.AreaAcl>(
                new { SecurityAreaId = areaId, UserId = userId });
            if (dbo == null) return null;
            return Mapper.Map<Common.Models.Security.AreaAcl>(dbo);
        }

        public Common.Models.Security.SecuredResourceAcl GetSecuredResourceAcl(
            Guid securedResourceId, int userId)
        {
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                return GetSecuredResourceAcl(securedResourceId, userId, db);
            }
        }

        public Common.Models.Security.SecuredResourceAcl GetSecuredResourceAcl(
            Guid securedResourceId, int userId, IDbConnection db)
        {
            DBOs.Security.SecuredResourceAcl dbo = db.QuerySingle<DBOs.Security.SecuredResourceAcl>(
                new { SecuredResourceId = securedResourceId, UserId = userId });
            if (dbo == null) return null;
            return Mapper.Map<Common.Models.Security.SecuredResourceAcl>(dbo);
        }

        public ActionResult InsufficientRights()
        {
            return View();
        }
    }
}
