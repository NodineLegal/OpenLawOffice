using System;

namespace OpenLawOffice.Common.Models
{
    [Flags]
    public enum PermissionType
    {
        None = 0,

        // Reading
        Read = 1,

        List = 2,
        AllRead = Read | List,

        // Writing
        Create = 16384,

        Modify = 32768,
        AllWrite = Create | Modify,

        // Adminstration
        Disable = 4194304,

        Delete = 8388608,
        AllAdmin = Disable | Delete,
    }
}