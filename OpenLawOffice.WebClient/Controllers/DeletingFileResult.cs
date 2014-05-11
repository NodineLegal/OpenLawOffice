namespace OpenLawOffice.WebClient.Controllers
{
    using System.Web.Mvc;

    public class DeletingFileResult : FilePathResult
    {
        private string Path { get; set; }
        private string DownloadedFilename { get; set; }

        public DeletingFileResult(string path, string downloadedFilename)
            : base(path, "application/zip")
        {
            Path = path;
            DownloadedFilename = downloadedFilename;
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + DownloadedFilename);
            response.ContentType = this.ContentType;
            response.WriteFile(Path);
            response.Flush();
            System.IO.File.Delete(Path);
            response.End();
        }
    }
}