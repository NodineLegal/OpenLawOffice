using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace OpenLawOffice.WebClient.DBOs.Tagging
{
    public abstract class TagBase : Core
    {
        //[Required]
        //public Guid Id { get; set; }

        [References(typeof(TagCategory))]
        public int? TagCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Tag { get; set; }
    }
}