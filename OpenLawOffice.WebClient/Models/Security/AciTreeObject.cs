namespace OpenLawOffice.WebClient.Models.Security
{
    using System.Collections.Generic;

    public class AciTreeObject
    {
        public int id { get; set; }
        public string label { get; set; }
        public bool inode { get; set; }
        public bool open { get; set; }
        public string icon { get; set; }
        public List<AciTreeObject> branch { get; set; }

        public AciTreeObject()
        {
            branch = new List<AciTreeObject>();
        }
    }
}