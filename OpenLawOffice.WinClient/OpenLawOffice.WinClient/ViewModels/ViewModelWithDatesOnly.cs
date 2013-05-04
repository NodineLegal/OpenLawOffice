using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ViewModelWithDatesOnly<TModel> 
        : ViewModelBase<TModel>
        where TModel : Common.Models.ModelWithDatesOnly
    {
        private DateTime? _utcCreated;
        public DateTime? UtcCreated
        {
            get 
            {
                return _utcCreated; 
            }
            set
            {
                _utcCreated = value;
                OnPropertyChanged("UtcCreated");
            }
        }

        private DateTime? _utcModified;
        public DateTime? UtcModified
        {
            get
            {
                return _utcModified;
            }
            set
            {
                _utcModified = value;
                OnPropertyChanged("UtcModified");
            }
        }

        private DateTime? _utcDisabled;
        public DateTime? UtcDisabled
        {
            get
            {
                return _utcDisabled;
            }
            set
            {
                _utcDisabled = value;
                OnPropertyChanged("UtcDisabled");
            }
        }

        public ViewModelWithDatesOnly()
            : base()
        {
        }

        public ViewModelWithDatesOnly(TModel model)
            : base(model)
        {
        }
    }
}
