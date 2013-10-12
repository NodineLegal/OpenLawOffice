using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace OpenLawOffice.WinClient.Controls
{
    public interface IDockableWindow : IDisposable
    {
        Action<IDockableWindow> OnActivated { get; set; }
        Action<IDockableWindow> OnDeactivated { get; set; }
        Action<IDockableWindow> OnSelected { get; set; }
        Action<IDockableWindow> OnDeselected { get; set; }
        Action<IDockableWindow> OnClose { get; set; }
        Action<IDockableWindow> OnDispose { get; set; }

        bool IsSelected { get; set; }
        bool CanHaveMultipleInstances { get; }
        LayoutDocument DockingWindow { get; set; }
        string Title { get; }

        void Activate();
        void Load();
        void Refresh();
        void Close();
    }
}
