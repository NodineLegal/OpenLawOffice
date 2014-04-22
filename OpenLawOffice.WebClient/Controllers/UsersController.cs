namespace OpenLawOffice.WebClient.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    public class UsersController : BaseController
    {
        //
        // GET: /User/
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult Index()
        {
            List<ViewModels.Security.UserViewModel> viewModelList = new List<ViewModels.Security.UserViewModel>();
            List<Common.Models.Security.User> modelList = OpenLawOffice.Data.Security.User.List();

            modelList.ForEach(x =>
            {
                viewModelList.Add(Mapper.Map<ViewModels.Security.UserViewModel>(x));
            });

            return View(viewModelList);
        }

        //
        // GET: /User/Details/5
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(int id)
        {
            ViewModels.Security.UserViewModel viewModel = null;
            Common.Models.Security.User model = OpenLawOffice.Data.Security.User.Get(id);
            viewModel = Mapper.Map<ViewModels.Security.UserViewModel>(model);
            return View(viewModel);
        }

        //
        // GET: /User/Create
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Security.UserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User model = Mapper.Map<Common.Models.Security.User>(viewModel);
                model.PasswordSalt = GetRandomString(10);

                // TODO : This will eventually be done in javascript on the browser
                model.Password = WebClient.Security.ClientHashPassword("12345");
                model.Password = WebClient.Security.ServerHashPassword(
                    model.Password, model.PasswordSalt);
                model = OpenLawOffice.Data.Security.User.Create(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }

        //
        // GET: /User/Edit/5
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(int id)
        {
            ViewModels.Security.UserViewModel viewModel = null;
            Common.Models.Security.User model = OpenLawOffice.Data.Security.User.Get(id);
            viewModel = Mapper.Map<ViewModels.Security.UserViewModel>(model);
            viewModel.Password = null;
            return View(viewModel);
        }

        //
        // POST: /User/Edit/5
        [SecurityFilter(SecurityAreaName = "Security", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(int id, ViewModels.Security.UserViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentModel = OpenLawOffice.Data.Security.User.Get(id);
                Common.Models.Security.User model = Mapper.Map<Common.Models.Security.User>(viewModel);

                // TODO : This will eventually be done in javascript on the browser
                model.Password = WebClient.Security.ClientHashPassword(viewModel.Password);
                model.Password = WebClient.Security.ServerHashPassword(
                    model.Password, model.PasswordSalt);

                model = OpenLawOffice.Data.Security.User.Edit(model);
                model = OpenLawOffice.Data.Security.User.SetPassword(model);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
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