using System;
using System.Collections.Generic;

namespace OpenLawOffice.Common.Models
{
    public interface IHierarchicalModel<T>
    {
        T Parent { get; set; }
        List<T> Children { get; set; }

        void AddChild(T model);
        void RemoveChild(T model);
    }
}
