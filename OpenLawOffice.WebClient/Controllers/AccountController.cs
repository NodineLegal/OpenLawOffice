namespace OpenLawOffice.WebClient.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using OpenLawOffice.WebClient.ViewModels.Account;

    [HandleError]
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Apply client hash (ideally this will be done on the client side in javascript eventually)
                string hashedPassword = WebClient.Security.ClientHashPassword(model.Password);

                OpenLawOffice.Data.Authentication.LoginResult result =
                    OpenLawOffice.Data.Authentication.Login(model.Username, hashedPassword);

                HttpCookie cookie = new HttpCookie("UserAuthToken", result.UserAuthToken);
                HttpContext.Response.AppendCookie(cookie);
                HttpContext.Response.AppendCookie(new HttpCookie("Username", model.Username));

                UserCache.Instance.Add(result.User);

                Response.Redirect("~/Home", false);

                return View();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}