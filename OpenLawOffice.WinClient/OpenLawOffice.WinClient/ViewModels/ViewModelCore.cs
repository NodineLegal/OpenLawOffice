using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class ViewModelCore<TModel> : ViewModelWithDatesOnly<TModel>
        where TModel : Common.Models.Core, new()
    {
        private Security.User _createdBy;
        public Security.User CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                _createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        private Security.User _modifiedBy;
        public Security.User ModifiedBy
        {
            get { return _modifiedBy; }
            set
            {
                _modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        private Security.User _disabledBy;
        public Security.User DisabledBy
        {
            get { return _disabledBy; }
            set
            {
                _disabledBy = value;
                OnPropertyChanged("DisabledBy");
            }
        }

        public ViewModelCore()
            : base()
        {
        }

        public ViewModelCore(TModel model)
            : base(model)
        {
        }
    }
}
