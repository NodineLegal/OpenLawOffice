namespace OpenLawOffice.WebClient.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using AutoMapper;

    public class NotesController : BaseController
    {
        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Read)]
        public ActionResult Details(Guid id)
        {
            ViewModels.Notes.NoteViewModel viewModel = null;
            Common.Models.Notes.Note model = Data.Notes.Note.Get(id);
            viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(model);

            Common.Models.Matters.Matter noteMatter = Data.Notes.NoteMatter.GetRelatedMatter(id);
            Common.Models.Tasks.Task noteTask = Data.Notes.NoteTask.GetRelatedTask(id);

            if (noteMatter != null)
            { // Note belongs to a matter
                ViewData["MatterId"] = noteMatter.Id.Value;
            }
            else if (noteTask != null)
            { // Note belongs to a task
                ViewData["TaskId"] = noteTask.Id.Value;
            }
            else
                throw new Exception("Note without relation to a matter or task, orphaned.");

            PopulateCoreDetails(viewModel);
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        public ActionResult Edit(Guid id)
        {
            ViewModels.Notes.NoteViewModel viewModel = null;
            Common.Models.Notes.Note model = Data.Notes.Note.Get(id);
            viewModel = Mapper.Map<ViewModels.Notes.NoteViewModel>(model);
            return View(viewModel);
        }

        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Modify)]
        [HttpPost]
        public ActionResult Edit(Guid id, ViewModels.Notes.NoteViewModel viewModel)
        {
            Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
            Common.Models.Notes.Note model = Mapper.Map<Common.Models.Notes.Note>(viewModel);
            model = Data.Notes.Note.Edit(model, currentUser);
            return RedirectToAction("Details", new { Id = id });
        }

        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [SecurityFilter(SecurityAreaName = "Notes.Note", IsSecuredResource = false,
            Permission = Common.Models.PermissionType.Create)]
        [HttpPost]
        public ActionResult Create(ViewModels.Notes.NoteViewModel viewModel)
        {
            try
            {
                Common.Models.Security.User currentUser = UserCache.Instance.Lookup(Request);
                Common.Models.Notes.Note model = Mapper.Map<Common.Models.Notes.Note>(viewModel);
                model = OpenLawOffice.Data.Notes.Note.Create(model, currentUser);

                if (Request["MatterId"] != null)
                {
                    Guid matterid = Guid.Parse(Request["MatterId"]);
                    Data.Notes.Note.RelateMatter(model, matterid, currentUser);
                }
                else if (Request["TaskId"] != null)
                {
                    long taskid = long.Parse(Request["TaskId"]);
                    Data.Notes.Note.RelateTask(model, taskid, currentUser);
                }
                else
                    throw new HttpRequestValidationException("Must specify a MatterId or a TaskId");

                return RedirectToAction("Details", new { Id = model.Id });
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
