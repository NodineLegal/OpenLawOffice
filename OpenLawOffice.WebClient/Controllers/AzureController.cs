using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Profile;
using System.Web.Security;

namespace OpenLawOffice.WebClient.Controllers
{
    public class AzureController : Controller
    {
        //
        // GET: /Azure/


        //HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create("
        //https://login.windows.net/936e2aed-bca1-43a5-960c-40386f5c1c78/oauth2/authorize?response_type=code&client_id=c3e0daf6-61a6-4a50-90ee-fa8db8f11eac&redirect_uri=http://localhost:62914/Azure&resource=https:%2f%2foutlook.office365.com%2f&state=5fdfd60b-8457-4536-b20f-fcb658d19458&prompt=admin_consent
        //");
            
        public ActionResult Index()
        {
            string code = Request["code"];
            string state = Request["state"];
            string error = Request["error"];
            string errorDescription = Request["error_description"];

            if (string.IsNullOrEmpty(error))
            {
                string postData =
                    "grant_type=authorization_code" +
                    "&code=" + code +
                    "&redirect_uri=" +
                    Common.Settings.Manager.Instance.System.WebsiteUrl.ToString() + "Azure" +
                    "&client_id=" + Common.Settings.Manager.Instance.System.Office365ClientId +
                    "&client_secret=" + Common.Settings.Manager.Instance.System.Office365ClientKey;

                //postData = HttpUtility.UrlEncode(postData);
                
                byte[] postBytes = Encoding.UTF8.GetBytes(postData);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Common.Settings.Manager.Instance.System.Office365TokenEndpoint);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postBytes.Length;

                // add post data to request
                Stream postStream = req.GetRequestStream();
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Flush();
                postStream.Close();

                try
                {
                    using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        {
                            JToken jt = JToken.Parse(reader.ReadToEnd());
                            string access_token = jt["access_token"].Value<string>();
                            string token_type = jt["token_type"].Value<string>();
                            string expires_in = jt["expires_in"].Value<string>();
                            string expires_on = jt["expires_on"].Value<string>();
                            string resource = jt["resource"].Value<string>();
                            string refresh_token = jt["refresh_token"].Value<string>();
                            string scope = jt["scope"].Value<string>();
                            string id_token = jt["id_token"].Value<string>();

                            dynamic profile = ProfileBase.Create(Membership.GetUser().UserName);

                            profile.Office365AccessToken = access_token;
                            profile.Office365RefreshToken = refresh_token;

                            profile.Save();
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (HttpWebResponse resp = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        {
                            string str = reader.ReadToEnd();
                            ViewData["Error"] = str;
                        }
                    }
                }
            }
            else
            {
                ViewData["Error"] = true;
            }

            return View();
        }

        public ActionResult Connect()
        {
            if (Common.Settings.Manager.Instance.System.Office365AuthEndpoint == null ||
                string.IsNullOrEmpty(Common.Settings.Manager.Instance.System.Office365ClientId))
            {
                ViewData["NotSetup"] = true;
            }
            else
            {
                // TODO : State needs work, needs to be randomly generated and checked on return
                ViewData["Url"] = Common.Settings.Manager.Instance.System.Office365AuthEndpoint.ToString() +
                    "&response_type=code&client_id=" +
                    Common.Settings.Manager.Instance.System.Office365ClientId +
                    "&redirect_uri=" +
                    Common.Settings.Manager.Instance.System.WebsiteUrl + "Azure" +
                    "&resource=https:%2f%2foutlook.office365.com%2f&state=5fdfd60b-8457-4536-b20f-fcb658d19458" +
                    "&prompt=admin_consent";
            }
            return View();
        }

    }
}
