namespace OpenLawOffice.WebClient.ViewModels
{
    public class JqGridObject
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public object[] Rows { get; set; }
    }
}