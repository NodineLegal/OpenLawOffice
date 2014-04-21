// -----------------------------------------------------------------------
// <copyright file="TagFilter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Common.Models.Settings
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TagFilter : Core
    {
        public long? Id { get; set; }

        public Security.User User { get; set; }

        public string Category { get; set; }

        public string Tag { get; set; }
    }
}