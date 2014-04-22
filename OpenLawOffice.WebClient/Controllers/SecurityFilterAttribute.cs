namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class SecurityFilterAttribute : ActionFilterAttribute
    {
        public string SecurityAreaName { get; set; }

        public bool IsSecuredResource { get; set; }

        public Common.Models.PermissionType Permission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid authToken = Guid.Empty;
            Guid id = Guid.Empty;
            BaseController controller;
            HttpCookie cookie = null;
            Data.Authorization.AuthorizeResult authResult;

            cookie = filterContext.HttpContext.Request.Cookies["UserAuthToken"];

            if (cookie == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            controller = (BaseController)filterContext.Controller;

            if (!Guid.TryParse(cookie.Value, out authToken))
                throw new Exception("AuthToken could not be parsed.");

            if (string.IsNullOrEmpty(SecurityAreaName))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            authResult = Data.Authorization.AreaAccess(
                new Common.Models.Security.User(),
                new Common.Models.Security.Area() { Name = SecurityAreaName },
                authToken,
                Permission);

            if (!authResult.IsAuthorized)
            {
                filterContext.Result = new RedirectResult("~/Account/InsufficientRights");
                return;
            }

            if (!IsSecuredResource)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!Permission.HasFlag(Common.Models.PermissionType.List) &&
                !Permission.HasFlag(Common.Models.PermissionType.Create))
            {
                if (filterContext.RouteData.Values["id"] != null)
                {
                    if (!Guid.TryParse((string)filterContext.RouteData.Values["id"], out id))
                        throw new Exception("Resource id could not be parsed.");
                }

                authResult = Data.Authorization.SecuredResourceAccess(
                    new Common.Models.Security.User(),
                    new Common.Models.Security.SecuredResource() { Id = id },
                    authToken,
                    Permission);

                if (!authResult.IsAuthorized)
                {
                    filterContext.Result = new RedirectResult("~/Account/InsufficientRights");
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}