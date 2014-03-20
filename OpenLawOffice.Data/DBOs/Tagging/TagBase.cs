using System.ComponentModel.DataAnnotations;

namespace OpenLawOffice.Data.DBOs.Tagging
{
    public abstract class TagBase : Core
    {
        //[Required]
        //public Guid Id { get; set; }

        public int? TagCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Tag { get; set; }
    }
}