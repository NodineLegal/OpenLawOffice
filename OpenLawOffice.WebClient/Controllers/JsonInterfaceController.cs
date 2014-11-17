using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError(View = "Errors/Index", Order = 10)]
    public class JsonInterfaceController : Controller
    {
        public ActionResult Matters(bool Active = true)
        {
            Guid token;
            Common.Net.Response<List<Common.Models.Matters.Matter>> response 
                = new Common.Net.Response<List<Common.Models.Matters.Matter>>();

            response.RequestReceived = DateTime.Now;

            if ((token = GetToken(Request)) == Guid.Empty)
            {
                response.Successful = false;
                response.Error = "Invalid Token";
            }

            if (!VerifyToken(token))
            {
                response.Successful = false;
                response.Error = "Invalid Token";
            }

            response.Successful = true;
            response.Package = Data.Matters.Matter.List(Active);
            response.ResponseSent = DateTime.Now;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private Guid GetToken(HttpRequestBase request)
        {
            Guid token;

            if (request.Cookies["Token"] == null)
                return Guid.Empty;

            if (!Guid.TryParse(Request.Cookies["Token"].Value, out token))
                return Guid.Empty;

            return token;
        }

        private bool VerifyToken(Guid token, bool renewSession = true)
        {
            Common.Models.External.ExternalSession session = Data.External.ExternalSession.Get(token);

            if (session == null) return false;

            if (session.Expires < DateTime.Now) return false;

            if (renewSession)
                session = Data.External.ExternalSession.Renew(session);

            return true;
        }
    }
}
