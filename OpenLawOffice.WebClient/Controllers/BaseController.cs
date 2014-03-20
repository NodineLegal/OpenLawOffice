namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Data;
    using System.Web.Mvc;
    using AutoMapper;

    public class BaseController : Controller
    {
        public ViewModels.Security.UserViewModel GetUser(int id)
        {
            Common.Models.Security.User user = OpenLawOffice.Data.Security.User.Get(id);
            if (user == null) return null;
            return Mapper.Map<ViewModels.Security.UserViewModel>(user);
        }

        public ViewModels.Security.UserViewModel GetUser(Guid authToken)
        {
            Common.Models.Security.User user = OpenLawOffice.Data.Security.User.Get(authToken);
            if (user == null) return null;
            return Mapper.Map<ViewModels.Security.UserViewModel>(user);
        }

        public void PopulateCoreDetails(ViewModels.CoreViewModel model)
        {
            if (model.CreatedBy != null)
            {
                if (model.CreatedBy.Id.HasValue)
                    model.CreatedBy = GetUser(model.CreatedBy.Id.Value);
                else if (model.CreatedBy.UserAuthToken.HasValue)
                    model.CreatedBy = GetUser(model.CreatedBy.UserAuthToken.Value);
                else
                    model.CreatedBy = null;
            }
            if (model.ModifiedBy != null)
            {
                if (model.ModifiedBy.Id.HasValue)
                    model.ModifiedBy = GetUser(model.ModifiedBy.Id.Value);
                else if (model.ModifiedBy.UserAuthToken.HasValue)
                    model.ModifiedBy = GetUser(model.ModifiedBy.UserAuthToken.Value);
                else
                    model.ModifiedBy = null;
            }
            if (model.DisabledBy != null)
            {
                if (model.DisabledBy.Id.HasValue)
                    model.DisabledBy = GetUser(model.DisabledBy.Id.Value);
                else if (model.DisabledBy.UserAuthToken.HasValue)
                    model.DisabledBy = GetUser(model.DisabledBy.UserAuthToken.Value);
                else
                    model.DisabledBy = null;
            }
        }

        //public ViewModels.Security.AreaAclViewModel GetAreaAcl(int areaId, int userId)
        //{
        //    using (IDbConnection db = Database.Instance.OpenConnection())
        //    {
        //        return GetAreaAcl(areaId, userId, db);
        //    }
        //}

        //public ViewModels.Security.AreaAclViewModel GetAreaAcl(int areaId, int userId, IDbConnection db)
        //{
        //    DBOs.Security.AreaAcl dbo = db.Single<DBOs.Security.AreaAcl>(
        //        new { SecurityAreaId = areaId, UserId = userId });
        //    if (dbo == null) return null;
        //    return Mapper.Map<ViewModels.Security.AreaAclViewModel>(dbo);
        //}

        //public Common.Models.Security.SecuredResourceAcl GetSecuredResourceAcl(
        //    Guid securedResourceId, int userId)
        //{
        //    using (IDbConnection db = Database.Instance.OpenConnection())
        //    {
        //        return GetSecuredResourceAcl(securedResourceId, userId, db);
        //    }
        //}

        //public Common.Models.Security.SecuredResourceAcl GetSecuredResourceAcl(
        //    Guid securedResourceId, int userId, IDbConnection db)
        //{
        //    DBOs.Security.SecuredResourceAcl dbo = db.Single<DBOs.Security.SecuredResourceAcl>(
        //        new { SecuredResourceId = securedResourceId, UserId = userId });
        //    if (dbo == null) return null;
        //    return Mapper.Map<Common.Models.Security.SecuredResourceAcl>(dbo);
        //}

        public ActionResult InsufficientRights()
        {
            return View();
        }
    }
}