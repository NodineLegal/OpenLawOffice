namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    public class UserTaskSettingsController : BaseController
    {
        public ActionResult Index()
        {
            ViewModels.Settings.UserTaskSettingsViewModel viewModel = new ViewModels.Settings.UserTaskSettingsViewModel();

            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            List<Common.Models.Settings.TagFilter> taskTagFilterList =
                Data.Settings.UserTaskSettings.ListTagFiltersFor(currentUser);

            viewModel.MyTasksFilter = new List<ViewModels.Settings.TagFilterViewModel>();

            taskTagFilterList.ForEach(x =>
            {
                viewModel.MyTasksFilter.Add(Mapper.Map<ViewModels.Settings.TagFilterViewModel>(x));
            });

            return View(viewModel);
        }

        public ActionResult DetailsFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel = null;
            Common.Models.Settings.TagFilter model = OpenLawOffice.Data.Settings.UserTaskSettings.GetTagFilter(id);
            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        public ActionResult CreateFilter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFilter(ViewModels.Settings.TagFilterViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };
                Common.Models.Settings.TagFilter model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
                model.User = currentUser;
                model = OpenLawOffice.Data.Settings.UserTaskSettings.CreateTagFilter(model, currentUser);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(viewModel);
            }
        }

        public ActionResult EditFilter(long id)
        {
            ViewModels.Settings.TagFilterViewModel viewModel = null;
            Common.Models.Settings.TagFilter model = OpenLawOffice.Data.Settings.UserTaskSettings.GetTagFilter(id);
            model.User = Data.Security.User.Get(model.User.Id.Value);
            viewModel = Mapper.Map<ViewModels.Settings.TagFilterViewModel>(model);
            viewModel.User = Mapper.Map<ViewModels.Security.UserViewModel>(model.User);
            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };
                Common.Models.Settings.TagFilter model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
                model.User = currentUser;
                model = OpenLawOffice.Data.Settings.UserTaskSettings.EditTagFilter(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }

        public ActionResult DeleteFilter(long id)
        {
            return DetailsFilter(id);
        }

        [HttpPost]
        public ActionResult DeleteFilter(long id, ViewModels.Settings.TagFilterViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                viewModel.User = new ViewModels.Security.UserViewModel() { Id = currentUser.Id };
                Common.Models.Settings.TagFilter model = Mapper.Map<Common.Models.Settings.TagFilter>(viewModel);
                OpenLawOffice.Data.Settings.UserTaskSettings.DeleteTagFilter(model, currentUser);
                return RedirectToAction("Index");
            }
            catch
            {
                return DetailsFilter(id);
            }
        }
    }
}