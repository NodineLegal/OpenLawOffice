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

    public class TagCategoriesController : BaseController
    {
        //
        // GET: /TagCategories/

        public ActionResult Index()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Tagging.TagCategory", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.List)]
        public ActionResult ListNameOnly(string term)
        {
            List<ViewModels.Tagging.TagCategoryViewModel> modelList = new List<ViewModels.Tagging.TagCategoryViewModel>();
            //List<string> nameList = new List<string>();

            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                List<DBOs.Tagging.TagCategory> list = db.SqlList<DBOs.Tagging.TagCategory>(
                    "SELECT * FROM \"tag_category\" WHERE LOWER(\"name\") LIKE '%' || LOWER(@Term) || '%' AND \"utc_disabled\" is null",
                    new { Term = term });

                if (list == null || list.Count == 0)
                    return null;

                list.ForEach(dbo =>
                {
                    ViewModels.Tagging.TagCategoryViewModel model = Mapper.Map<ViewModels.Tagging.TagCategoryViewModel>(dbo);
                    modelList.Add(model);
                    //nameList.Add(dbo.Name);
                });
            }

            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /TagCategories/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /TagCategories/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TagCategories/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /TagCategories/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /TagCategories/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /TagCategories/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TagCategories/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
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
    }
}
