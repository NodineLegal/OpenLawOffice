using System;

namespace OpenLawOffice.Common.Models
{
    public class MapMeAttribute : Attribute
    {
        public string MapMethodName { get; set; }

        public MapMeAttribute()
        {
            MapMethodName = "BuildMappings";
        }
    }
}