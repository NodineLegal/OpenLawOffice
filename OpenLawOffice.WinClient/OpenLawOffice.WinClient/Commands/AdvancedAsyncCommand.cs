using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;

namespace OpenLawOffice.WinClient.Commands
{
    public class AdvancedAsyncCommand : ICommand
    {
        readonly Func<object, object> _execute;
        readonly Predicate<object> _canExecute;


        public AdvancedAsyncCommand(Func<object, object> execute)
        {
        }


        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
