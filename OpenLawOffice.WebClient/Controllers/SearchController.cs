namespace OpenLawOffice.WebClient.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;

    public class SearchController : Controller
    {
        public ActionResult Tags()
        {
            return View(new ViewModels.Search.TagSearchViewModel()
                {
                    SearchMatters = true,
                    SearchTasks = true
                });
        }

        [HttpPost]
        public ActionResult Tags(ViewModels.Search.TagSearchViewModel viewModel)
        {
            List<Common.Models.Matters.MatterTag> matterTags = Data.Matters.MatterTag.Search(viewModel.Query.ToLower());
            List<Common.Models.Tasks.TaskTag> taskTags = Data.Tasks.TaskTag.Search(viewModel.Query.ToLower());

            if (viewModel.SearchMatters)
            {
                viewModel.MatterTags = new List<ViewModels.Matters.MatterTagViewModel>();

                matterTags.ForEach(x =>
                {
                    ViewModels.Matters.MatterTagViewModel mtvm = Mapper.Map<ViewModels.Matters.MatterTagViewModel>(x);
                    mtvm.Matter = Mapper.Map<ViewModels.Matters.MatterViewModel>(x.Matter);
                    viewModel.MatterTags.Add(mtvm);
                });
            }

            if (viewModel.SearchTasks)
            {
                viewModel.TaskTags = new List<ViewModels.Tasks.TaskTagViewModel>();

                taskTags.ForEach(x =>
                {
                    ViewModels.Tasks.TaskTagViewModel mtvm = Mapper.Map<ViewModels.Tasks.TaskTagViewModel>(x);
                    mtvm.Task = Mapper.Map<ViewModels.Tasks.TaskViewModel>(x.Task);
                    viewModel.TaskTags.Add(mtvm);
                });
            }

            return View(viewModel);
        }
    }
}