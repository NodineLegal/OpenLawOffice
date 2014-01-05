namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web;
    using CoreSecurity = OpenLawOffice.Server.Core.Security;
    using System.Data;

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
            CoreSecurity.AuthorizeResult authResult;
            
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

            authResult = CoreSecurity.Authorize.AreaAccess(
                new Server.Core.Rest.Requests.Security.User()
                {
                    AuthToken = authToken
                },
                new Server.Core.Rest.Requests.Security.Area()
                {
                    Name = SecurityAreaName
                },
                Permission);

            if (!authResult.IsAuthorized)
            {
                filterContext.Result = controller.InsufficientRights();
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

                authResult = CoreSecurity.Authorize.SecuredResourceAccess(
                    new Server.Core.Rest.Requests.Security.User()
                    {
                        AuthToken = authToken
                    },
                    new Server.Core.Rest.Requests.Security.SecuredResource()
                    {
                        Id = id
                    },
                    Permission);

                if (!authResult.IsAuthorized)
                {
                    filterContext.Result = controller.InsufficientRights();
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}