using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenLawOffice.WebClient.Controllers
{
    public class TagCategoriesController : BaseController
    {
        //
        // GET: /TagCategories/ListNameOnly

        public ActionResult ListNameOnly()
        {
            string term = Request["term"];
            List<Common.Models.Tagging.TagCategory> list = Data.Tagging.TagCategory.List(term.Trim());

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}
