using System;
using System.Windows;
using Microsoft.Windows.Controls.Ribbon;
using Xceed.Wpf.AvalonDock.Layout;
using System.Windows.Input;

namespace OpenLawOffice.WinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
            Globals.Instance.MainWindow = this;
            Common.ObjectMapper.MapAssembly(typeof(Common.ObjectMapper).Assembly);
            Common.ObjectMapper.MapAssembly(typeof(MainWindow).Assembly);
            ControllerManager.Instance.ScanAssembly(typeof(MainWindow).Assembly);

            DockManager.DocumentClosed += delegate(object sender, Xceed.Wpf.AvalonDock.DocumentClosedEventArgs args)
            {
                Controls.IDockableWindow iwin = WindowManager.Instance.Lookup(args.Document);
                if (iwin.OnDeactivated != null)
                    iwin.OnDeactivated(iwin);

                iwin.Close();
                if (iwin.OnClose != null)
                {
                    iwin.OnClose(iwin);
                }

                WindowManager.Instance.UnregisterWindow(iwin);

                iwin.Dispose();
                if (iwin.OnDispose != null) iwin.OnDispose(iwin);
            };
             
            Globals.Instance.MainWindow.DisableUserControl();
            Windows.LoginWindow loginWindow = new Windows.LoginWindow();
            loginWindow.Load();


            Home_Security_Areas.Command = new Commands.DelegateCommand(x => 
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.Area>();
            });
            Home_Security_AreaAcls.Command = new Commands.DelegateCommand(x =>
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.AreaAcl>();
            });
            Home_Security_Users.Command = new Commands.DelegateCommand(x =>
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.User>();
            });
            Home_Matters.Command = new Commands.DelegateCommand(x =>
            {
                ControllerManager.Instance.LoadUI<Common.Models.Matters.Matter>();
            });
        }

        public void DisableUserControl()
        {
            Ribbon.IsEnabled = false;
        }

        public void EnableUserControl()
        {
            Ribbon.IsEnabled = true;
        }

        public void AddDocumentWindow(Controls.IDockableWindow doc)
        {
            LayoutPanel.Children.Add(doc.DockingWindow);
        }
    }
}
