using System;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    public class ActionableError : ErrorBase
    {
        public Action<ActionableError, object, Action<ActionableError, object>> Recover { get; set; }
        public Action<ActionableError, object> Fail { get; set; }

        public bool CanRecover { get { return Recover != null; } }
        public bool CanFail { get { return Fail != null; } }

        public string Title { get; set; }

        public override void Throw(object data)
        {
            base.Throw(data);
            
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ActionableErrorWindow win = new ActionableErrorWindow()
                {
                    Title = Title,
                    DataContext = this
                };

                win.Yes += (sender, e) =>
                {
                    win.Close();
                    if (CanRecover) Recover(this, data, Fail);
                };

                win.No += (sender, e) =>
                {
                    win.Close();
                    if (CanFail) Fail(this, data);
                };

                win.ShowDialog();
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
