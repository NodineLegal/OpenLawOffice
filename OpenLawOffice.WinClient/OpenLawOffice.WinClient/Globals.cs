using System;

namespace OpenLawOffice.WinClient
{
    public class Globals : OpenLawOffice.Common.Singleton<Globals>
    {
        public Settings Settings { get; set; }
        public Guid AuthToken { get; set; }
        public MainWindow MainWindow { get; set; }

        public Globals()
        {
            Settings = new Settings();
        }
    }
}
