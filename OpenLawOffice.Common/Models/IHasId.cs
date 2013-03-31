using System;

namespace OpenLawOffice.Common.Models
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
