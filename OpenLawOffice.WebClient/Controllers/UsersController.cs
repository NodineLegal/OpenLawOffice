namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using ServiceStack.OrmLite;
    using AutoMapper;

    public class UsersController : BaseController
    {
        //
        // GET: /User/
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.UserViewModel> modelList = new List<ViewModels.Security.UserViewModel>();
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Security.User> list = db.SqlList<DBOs.Security.User>(
                    "SELECT * FROM \"user\" "+
                    "WHERE \"utc_disabled\" is null");

                list.ForEach(dbo =>
                {
                    modelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(dbo));
                });
            }

            return View(modelList);
        }

        //
        // GET: /User/Details/5
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Security.UserViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Security.User dbo = db.Single<DBOs.Security.User>(
                    "SELECT * FROM \"user\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Security.UserViewModel>(dbo);
            }

            return View(model);
        }

        //
        // GET: /User/Create
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /User/Create
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Security.UserViewModel model)
        {
            try
            {
                DBOs.Security.User dboUser = Mapper.Map<DBOs.Security.User>(model);
                dboUser.UtcCreated = dboUser.UtcModified = DateTime.UtcNow;
                dboUser.PasswordSalt = GetRandomString(10);
                // TODO : This will eventually be done in javascript on the browser
                dboUser.Password = WebClient.Security.ClientHashPassword("12345");
                dboUser.Password = WebClient.Security.ServerHashPassword(
                    dboUser.Password, dboUser.PasswordSalt);
                
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // Insert User
                    db.Insert<DBOs.Security.User>(dboUser);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        
        //
        // GET: /User/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.UserViewModel model = null;
            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                // Load base DBO
                DBOs.Security.User dbo = db.Single<DBOs.Security.User>(
                    "SELECT * FROM \"user\" WHERE \"id\"=@Id AND \"utc_disabled\" is null",
                    new { Id = id });

                model = Mapper.Map<ViewModels.Security.UserViewModel>(dbo);
            }

            model.Password = null;

            return View(model);
        }

        //
        // POST: /User/Edit/5
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Security.UserViewModel model)
        {
            try
            {
                DBOs.Security.User dbo = Mapper.Map<DBOs.Security.User>(model);
                dbo.UtcModified = DateTime.UtcNow;

                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    if (model.Password != null && model.Password.Length > 0)
                    {
                        DBOs.Security.User dboCurrent = db.SingleById<DBOs.Security.User>(id);

                        // TODO : This will eventually be done in javascript on the browser
                        dbo.Password = WebClient.Security.ClientHashPassword(model.Password);
                        dbo.Password = WebClient.Security.ServerHashPassword(
                        dbo.Password, dboCurrent.PasswordSalt);

                        db.UpdateOnly(dbo,
                            fields => new
                            {
                                fields.Username,
                                fields.Password,
                                fields.UtcModified
                            },
                            where => where.Id == dbo.Id);
                    }
                    else
                    {
                        db.UpdateOnly(dbo,
                            fields => new
                            {
                                fields.Username,
                                fields.UtcModified
                            },
                            where => where.Id == dbo.Id);
                    }
                }
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /User/Delete/5
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
            return View();
        }

        //
        // POST: /User/Delete/5
        [SecurityFilter(SecurityAreaName = "Security.User", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Disable)]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            throw new NotImplementedException();
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }

        private string GetRandomString(int length)
        {
            string[] array = new string[54]
	        {
		        "0","2","3","4","5","6","8","9",
		        "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
		        "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
	        };

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }
    }
}
