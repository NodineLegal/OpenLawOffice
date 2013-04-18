//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Windows.Input;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OpenLawOffice.WinClient.Commands
//{
//    /// <summary>
//    /// ICommand implementation that runs the Execute in a Task 
//    /// and the CanExecute sequentially
//    /// Also prevents the command from firing at the same time
//    /// </summary>
//    public class DelegateCommand : ICommand
//    {
//        volatile bool _running;
//        object lock_obj = new object();

//        readonly Action<object> _execute;
//        readonly Predicate<object> _canExecute;

//        public DelegateCommand(Action<object> execute) : this(execute, null) { }

//        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
//        {
//            if (execute == null)
//                throw new ArgumentNullException("execute");

//            _running = false;
//            _execute = execute;
//            _canExecute = canExecute;
//        }

//        [DebuggerStepThrough]
//        public bool CanExecute(object parameter)
//        {
//            if (_running)
//                return false;

//            if (_canExecute == null)
//                return true;

//            return _canExecute(parameter);
//        }

//        public event EventHandler CanExecuteChanged
//        {
//            add { CommandManager.RequerySuggested += value; }
//            remove { CommandManager.RequerySuggested -= value; }
//        }

//        public void Execute(object parameter)
//        {
//            if (Monitor.TryEnter(lock_obj) == false)
//                return;

//            try
//            {
//                if (_running)
//                    return;

//                _running = true;

//                new Task(() =>
//                {
//                    _execute(parameter);
//                    _running = false;
//                }).Start();
//            }
//            finally
//            {
//                Monitor.Exit(lock_obj);
//            }
//        }
//    }
//}
