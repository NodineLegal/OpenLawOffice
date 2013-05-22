using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Controls
{
    public class RelationCollection : Dictionary<Type, UserControl>
    {
        public void InitializeAllWith(object obj)
        {
            foreach (var item in this)
            {
                ((Views.IRelationView)item.Value).Initialize(obj);
            }
        }
    }
}
