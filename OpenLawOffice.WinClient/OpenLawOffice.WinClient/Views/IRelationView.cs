using System;

namespace OpenLawOffice.WinClient.Views
{
    public interface IRelationView
    {
        void Initialize(object obj);
        void Load();
        void Refresh();
    }
}
