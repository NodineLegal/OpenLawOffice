using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ModelBase<TModel> : System.ComponentModel.INotifyPropertyChanged, IViewModel
        where TModel : Common.Models.ModelBase
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected TModel _model;

        public void AttachModel(TModel model)
        {
            _model = model;
        }

        public void AttachModel(Common.Models.ModelBase model)
        {
            _model = (TModel)model;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
