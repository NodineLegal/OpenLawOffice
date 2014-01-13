namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Data;
    using System.Web;
    using System.Web.Mvc;
    using OpenLawOffice.Server.Core;
    using OpenLawOffice.WebClient.ViewModels.Account;
    using ServiceStack.OrmLite;
    using DBOs = OpenLawOffice.Server.Core.DBOs;
    using AutoMapper;

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
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    DBOs.Security.User userDbo = db.QuerySingle<DBOs.Security.User>(new { Username = model.Username });

                    if (userDbo == null)
                    {
                        ModelState.AddModelError("", "The username or password provided is incorrect.");
                        return View(model);
                    }

                    // Apply client hash (ideally this will be done on the client side in javascript eventually)
                    string hashedPassword = OpenLawOffice.Server.Core.Services.Security.Authentication.ClientHashPassword(model.Password);

                    // Apply server hash
                    hashedPassword = OpenLawOffice.Server.Core.Services.Security.Authentication.ServerHashPassword(hashedPassword, userDbo.PasswordSalt);

                    if (hashedPassword != userDbo.Password)
                    {
                        ModelState.AddModelError("", "The username or password provided is incorrect.");
                        return View(model);
                    }

                    Guid newAuthToken = Guid.NewGuid();

                    HttpCookie cookie = new HttpCookie("UserAuthToken", newAuthToken.ToString());
                    HttpContext.Response.AppendCookie(cookie);
                    HttpContext.Response.AppendCookie(new HttpCookie("Username", userDbo.Username));

                    userDbo.UserAuthToken = newAuthToken;
                    userDbo.UserAuthTokenExpiry = DateTime.Now.AddMinutes(15);

                    db.UpdateOnly(userDbo,
                        fields => new { fields.UserAuthToken, fields.UserAuthTokenExpiry },
                        where => where.Id == userDbo.Id);

                    UserCache.Instance.Add(Mapper.Map<Common.Models.Security.User>(userDbo));

                    Response.Redirect("~/Home", false);

                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}