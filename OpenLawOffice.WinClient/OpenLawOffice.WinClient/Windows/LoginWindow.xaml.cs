using System;
using System.Windows;
using System.Windows.Controls;
using RestSharp;

namespace OpenLawOffice.WinClient.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl, Controls.IDockableWindow
    {
        public Action<Controls.IDockableWindow> OnActivated { get; set; }
        public Action<Controls.IDockableWindow> OnDeactivated { get; set; }
        public Action<Controls.IDockableWindow> OnSelected { get; set; }
        public Action<Controls.IDockableWindow> OnDeselected { get; set; }
        public Action<Controls.IDockableWindow> OnClose { get; set; }
        public Action<Controls.IDockableWindow> OnDispose { get; set; }

        public bool IsSelected { get; set; }
        public virtual bool CanHaveMultipleInstances { get { return false; } }
        public AvalonDock.Layout.LayoutDocument DockingWindow { get; set; }
        public virtual string Title { get { return "Login"; } }

        public LoginWindow()
        {
            InitializeComponent();
            DockingWindow = new AvalonDock.Layout.LayoutDocument();
            DockingWindow.CanClose = false;
        }

        private void UILogin_Click(object sender, RoutedEventArgs e)
        {
            RestClient client = new RestClient(Globals.Instance.Settings.HostBaseUrl);
            RestRequest request = new RestRequest("Authentication", Method.POST);

            request.AddParameter("Username", UIUsername.Text.Trim(), ParameterType.GetOrPost);
            request.AddParameter("Password", ClientHashPassword(UIPassword.Text.Trim()), ParameterType.GetOrPost);

            client.ExecuteAsync(request, response =>
            {
                try
                {
                    ServiceStack.Text.JsonObject jobj = ServiceStack.Text.JsonObject.Parse(response.Content);
                    ServiceStack.Text.JsonObject jobjData = jobj.Object("Data");
                    string authTokenStr = jobjData["AuthToken"];
                    if (authTokenStr == null)
                    {
                        App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                        {
                            MessageBox.Show("Invalid Username or Password.");
                        }), System.Windows.Threading.DispatcherPriority.Normal);
                    }
                    else
                    {
                        Guid authToken = Guid.Parse(authTokenStr);
                        Globals.Instance.AuthToken = authToken;
                        App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                        {
                            DockingWindow.CanClose = true;
                            Close();
                            Globals.Instance.MainWindow.EnableUserControl();
                        }), System.Windows.Threading.DispatcherPriority.Normal);
                    }
                }
                catch (Exception ex)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                }
            });
        }

        private void UIClose_Click(object sender, RoutedEventArgs e)
        {
            // Kill me
            Application.Current.Shutdown();
        }

        public static string ClientHashPassword(string plainTextPassword)
        {
            return Hash(plainTextPassword);
        }

        private static string Hash(string str)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = sha512.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public void Activate()
        {
            DockingWindow.IsSelected = true;
        }

        public void Load()
        {
            WindowManager.Instance.RegisterWindow(this);
        }

        public void Refresh()
        {
        }

        public void Close()
        {
            if (DockingWindow.CanClose)
                DockingWindow.Close();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
