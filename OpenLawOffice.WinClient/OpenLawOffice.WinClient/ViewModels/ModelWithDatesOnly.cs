using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ModelWithDatesOnly<TModel> : ModelBase<TModel>
        where TModel : Common.Models.ModelWithDatesOnly, new()
    {
        public DateTime? UtcCreated
        {
            get 
            {
                if (_model == null) return null;
                return _model.UtcCreated; 
            }
            set
            {
                if (_model == null) AttachModel(new TModel());
                _model.UtcCreated = value;
                OnPropertyChanged("UtcCreated");
            }
        }

        public DateTime? UtcModified
        {
            get 
            {
                if (_model == null) return null;
                return _model.UtcModified; 
            }
            set
            {
                if (_model == null) AttachModel(new TModel());
                _model.UtcModified = value;
                OnPropertyChanged("UtcModified");
            }
        }

        public DateTime? UtcDisabled
        {
            get 
            {
                if (_model == null) return null;
                return _model.UtcDisabled; 
            }
            set
            {
                if (_model == null) AttachModel(new TModel());
                _model.UtcDisabled = value;
                OnPropertyChanged("UtcDisabled");
            }
        }
    }
}
