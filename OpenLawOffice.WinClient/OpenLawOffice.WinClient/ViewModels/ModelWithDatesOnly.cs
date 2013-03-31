using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ModelWithDatesOnly<TModel> : ModelBase<TModel>
        where TModel : Common.Models.ModelWithDatesOnly
    {
        public DateTime? UtcCreated
        {
            get { return _model.UtcCreated; }
            set
            {
                _model.UtcCreated = value;
                OnPropertyChanged("UtcCreated");
            }
        }

        public DateTime? UtcModified
        {
            get { return _model.UtcModified; }
            set
            {
                _model.UtcModified = value;
                OnPropertyChanged("UtcModified");
            }
        }

        public DateTime? UtcDisabled
        {
            get { return _model.UtcDisabled; }
            set
            {
                _model.UtcDisabled = value;
                OnPropertyChanged("UtcDisabled");
            }
        }
    }
}
