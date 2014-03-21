using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Data.DBOs.Tagging
{
    public abstract class TagBase : Core
    {
        //[Required]
        //public Guid Id { get; set; }

        [ColumnMapping(Name = "tag_category_id")]
        public int? TagCategoryId { get; set; }

        [ColumnMapping(Name = "tag")]
        public string Tag { get; set; }
    }
}