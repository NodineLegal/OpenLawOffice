using System;
using System.Windows.Threading;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ModelBase<TModel> : System.ComponentModel.INotifyPropertyChanged, IViewModel
        where TModel : Common.Models.ModelBase
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        readonly Dispatcher _dispatcher;
        protected TModel _model;
        public virtual bool IsHierarchical { get { return false; } }

        protected ModelBase()
        {
            _dispatcher = App.Current.Dispatcher;
        }

        public IViewModel AttachModel(TModel model)
        {
            _model = model;
            return this;
        }

        public IViewModel AttachModel(Common.Models.ModelBase model)
        {
            _model = (TModel)model;
            return this;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public virtual void Synchronize(Action a)
        {
            _dispatcher.BeginInvoke(a, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
