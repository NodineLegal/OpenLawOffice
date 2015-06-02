using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError(View = "Errors/Index", Order = 10)]
    public class JsonInterfaceController : Controller
    {
        public AccountMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [HttpPost]
        public ActionResult Authenticate()
        {
            Common.Net.Request<Common.Net.AuthPackage> request;
            Common.Net.Response<Guid> response = new Common.Net.Response<Guid>();

            request = Request.InputStream.JsonDeserialize<Common.Net.Request<Common.Net.AuthPackage>>();

            response.RequestReceived = DateTime.Now;

            if (MembershipService.ValidateUser(request.Package.Username, request.Package.Password))
            {
                Common.Models.External.ExternalSession session =
                    Data.External.ExternalSession.Get(request.Package.AppName, request.Package.MachineId, request.Package.Username);
                Common.Models.Account.Users user =
                    Data.Account.Users.Get(request.Package.Username);

                if (session == null)
                { // create
                    session = Data.External.ExternalSession.Create(new Common.Models.External.ExternalSession()
                    {
                        MachineId = request.Package.MachineId,
                        User = user,
                        AppName = request.Package.AppName
                    });
                }
                else
                { // update
                    session = Data.External.ExternalSession.Update(new Common.Models.External.ExternalSession()
                    {
                        Id = session.Id,
                        MachineId = request.Package.MachineId,
                        User = user,
                        AppName = request.Package.AppName
                    });
                }

                response.Successful = true;
                response.Package = session.Id.Value;
            }
            else
            {
                response.Successful = false;
                response.Package = Guid.Empty;
            }

            response.ResponseSent = DateTime.Now;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CloseSession()
        {
            Guid token;
            Common.Net.Request<Common.Net.AuthPackage> request;
            Common.Models.External.ExternalSession session;
            Common.Net.Response<bool> response = new Common.Net.Response<bool>();
            
            request = Request.InputStream.JsonDeserialize<Common.Net.Request<Common.Net.AuthPackage>>();
            
            response.RequestReceived = DateTime.Now;

            if ((token = GetToken(Request)) == Guid.Empty)
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (!VerifyToken(token))
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            // Close the session here
            session = Data.External.ExternalSession.Get(request.Package.AppName, request.Package.MachineId, request.Package.Username);
            session = Data.External.ExternalSession.Delete(session);

            response.Successful = true;
            response.ResponseSent = DateTime.Now;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Matters(string contactFilter, string titleFilter, string caseNumberFilter, string jurisdictionFilter, bool activeFilter = true)
        {
            Guid token;
            Common.Net.Response<List<Common.Models.Matters.Matter>> response 
                = new Common.Net.Response<List<Common.Models.Matters.Matter>>();

            response.RequestReceived = DateTime.Now;

            if ((token = GetToken(Request)) == Guid.Empty)
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                response.ResponseSent = DateTime.Now;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (!VerifyToken(token))
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                response.ResponseSent = DateTime.Now;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            response.Successful = true;
            response.Package = Data.Matters.Matter.List(activeFilter, contactFilter, titleFilter, caseNumberFilter, jurisdictionFilter);
            response.ResponseSent = DateTime.Now;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListFormsForMatter(Guid matterId)
        {
            Guid token;
            Common.Net.Response<List<Common.Models.Forms.Form>> response
                = new Common.Net.Response<List<Common.Models.Forms.Form>>();

            response.RequestReceived = DateTime.Now;

            if ((token = GetToken(Request)) == Guid.Empty)
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                response.ResponseSent = DateTime.Now;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (!VerifyToken(token))
            {
                response.Successful = false;
                response.Error = "Invalid Token";
                response.ResponseSent = DateTime.Now;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            response.Successful = true;
            response.Package = Data.Forms.Form.ListForMatter(matterId);
            response.ResponseSent = DateTime.Now;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadForm(int id)
        {
            Guid token;
            string ext = "";
            Common.Models.Forms.Form model;

            if ((token = GetToken(Request)) == Guid.Empty)
            {
                return null;
            }

            if (!VerifyToken(token))
            {
                return null;
            }

            model = Data.Forms.Form.Get(id);

            if (Path.HasExtension(model.Path))
                ext = Path.GetExtension(model.Path);

            return File(model.Path, Common.Utilities.GetMimeType(ext), model.Title + ext);
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
