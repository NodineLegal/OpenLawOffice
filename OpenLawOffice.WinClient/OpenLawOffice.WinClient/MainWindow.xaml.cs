using System;
using System.Windows;
using Microsoft.Windows.Controls.Ribbon;
using AvalonDock.Layout;
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

            DockManager.DocumentClosed += delegate(object sender, AvalonDock.DocumentClosedEventArgs args)
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


            Home_Security_Areas.Command = new Commands.RelayCommand(x => 
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.Area>();
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

        public void EnableMatterRelationshipControls()
        {
            Matters_Contacts.IsEnabled = Matters_Tasks.IsEnabled = Matters_Documents.IsEnabled = 
                Matters_Notes.IsEnabled = true;
        }

        public void DisableMatterRelationshipControls()
        {
            Matters_Contacts.IsEnabled = Matters_Tasks.IsEnabled = Matters_Documents.IsEnabled =
                Matters_Notes.IsEnabled = false;
        }

        private void Home_Matters_Click(object sender, RoutedEventArgs e)
        {
            //if (_mattersListWindow == null)
            //{
            //    _mattersListWindow = new Windows.Matters();

            //    _mattersListWindow.OnActivated += iwin =>
            //        {
            //            MattersTab.Visibility = System.Windows.Visibility.Visible;
            //            MattersTab.IsSelected = true;
            //        };
            //    _mattersListWindow.OnDeactivated += iwin =>
            //        {
            //            MattersTab.Visibility = System.Windows.Visibility.Hidden;
            //        };
            //    _mattersListWindow.OnDispose += iwin =>
            //        {
            //            _mattersListWindow = null;
            //        };
            //    _mattersListWindow.Load();
            //}
            //else
            //{
            //    _mattersListWindow.Activate();
            //}
        }

        private void Home_Contacts_Click(object sender, RoutedEventArgs e)
        {
            //if (_contactsListWindow == null)
            //{
            //    _contactsListWindow = new Windows.Contacts();
            //    _contactsListWindow.OnActivated += iwin =>
            //    {
            //        ContactsTab.Visibility = System.Windows.Visibility.Visible;
            //        ContactsTab.IsSelected = true;
            //    };
            //    _contactsListWindow.OnDeactivated += iwin =>
            //    {
            //        ContactsTab.Visibility = System.Windows.Visibility.Hidden;
            //    };
            //    _contactsListWindow.OnDispose += iwin =>
            //    {
            //        _contactsListWindow = null;
            //    };
            //    _contactsListWindow.Load();
            //}
            //else
            //{
            //    _contactsListWindow.Activate();
            //}
        }
                
        private void Matters_List_Title_UserDataChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //if (_mattersListWindow != null)
            //    _mattersListWindow.UpdateList();
        }

        private void Matters_List_Tags_UserDataChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //if (_mattersListWindow != null)
            //    _mattersListWindow.UpdateList();
        }

        private void Matters_Contacts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Tasks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Documents_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Notes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_List_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Matters_Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
