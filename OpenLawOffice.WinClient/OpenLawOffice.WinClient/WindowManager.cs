using System;
using System.Collections.Generic;
using Xceed.Wpf.AvalonDock.Layout;

namespace OpenLawOffice.WinClient
{
    public class WindowManager : Common.Singleton<WindowManager>
    {
        public LinkedList<Controls.IDockableWindow> WindowHistory { get; set; }
        public List<Controls.IDockableWindow> Windows { get; set; }

        public Controls.IDockableWindow ActiveWindow
        {
            get
            {
                lock (WindowHistory)
                {
                    if (WindowHistory.Count > 0)
                        return WindowHistory.Last.Value;
                    else
                        return null;
                }
            }
        }

        public WindowManager()
        {
            Windows = new List<Controls.IDockableWindow>();
            WindowHistory = new LinkedList<Controls.IDockableWindow>();
        }

        public Controls.IDockableWindow Lookup(Xceed.Wpf.AvalonDock.Layout.LayoutDocument document)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].DockingWindow == document)
                    return Windows[i];
            }

            return null;
        }

        public Controls.IDockableWindow Lookup(Controls.IDockableWindow window)
        {
            return Windows.Find(delegate(Controls.IDockableWindow winToCompare)
            {
                return window.Title == winToCompare.Title;
            });
        }

        public void RegisterWindow(Controls.IDockableWindow window)
        {
            if (!window.CanHaveMultipleInstances)
            {
                if (Lookup(window) != null)
                    throw new Exception("Window with same title already instantiated and registered.");
            }

            Windows.Add(window);
            window.DockingWindow.Content = window;
            window.DockingWindow.Title = window.Title;

            lock (WindowHistory)
            {
                WindowHistory.AddLast(window);
            }

            // Fire before attaching event so as not to trigger event
            window.DockingWindow.IsActive = true;
            window.DockingWindow.IsSelected = true;
            window.IsSelected = true;

            window.DockingWindow.IsActiveChanged += delegate(object sender, EventArgs e)
            {
                window.IsSelected = window.DockingWindow.IsActive;
                if (window.DockingWindow.IsActive)
                {
                    //lock (WindowHistory)
                    //{
                    //    WindowHistory.Remove(window);
                    //    WindowHistory.AddLast(window);
                    //}

                    if (window.OnActivated != null)
                        window.OnActivated(window);
                }
                else
                {
                    if (window.OnDeactivated != null)
                        window.OnDeactivated(window);
                }
            };

            window.DockingWindow.IsSelectedChanged += delegate(object sender, EventArgs e)
            {
                window.IsSelected = window.DockingWindow.IsSelected;
                if (window.DockingWindow.IsSelected)
                {
                    lock (WindowHistory)
                    {
                        WindowHistory.Remove(window);
                        WindowHistory.AddLast(window);
                    }

                    if (window.OnSelected != null)
                        window.OnSelected(window);
                }
                else
                {
                    if (window.OnDeselected != null)
                        window.OnDeselected(window);
                }
            };

            Globals.Instance.MainWindow.AddDocumentWindow(window);

            // Since we did not fire our standard changed event, we need to trip our OnWindowActivated event
            // we do not need to worry about deactivation as it will still fire
            if (window.OnActivated != null) window.OnActivated(window);
        }

        public void UnregisterWindow(Controls.IDockableWindow window)
        {
            Windows.Remove(window);

            lock (WindowHistory)
            {
                WindowHistory.Remove(window);
                if (WindowHistory.Count > 0)
                    WindowHistory.Last.Value.Activate();
            }
        }
    }
}
